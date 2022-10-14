using Lean.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyShop
{

    [System.Serializable]
    public struct Score
    {
        public int environmentScore;    //环境分
        public int maxWaitNumber;       //当前最大可等待人数
        public int cookingScore;            //厨艺分,所有工作中的员工相加
        public int thumbsNumber;        //点赞数

        public float genRate;          //来客速度
        public float goodRate;      //好评率
        public float enthusiasm;    //客人热情
    }

    public struct OrderState
    {
        public int totalOrder;          //所有产生的订单
        public int canceledOrder; //强行取消的订单(太慢了)
        public int hateOrder;   //评价不喜欢的客人
        public int leavedCustomer;  //没有进来的客人(已经在等待)
        public float totalPrice;
    }

    public enum FoodType
    {
        hotdog, burger, popcorn, taco, cola, beer, wine,
        hotdogBurger, hotdogBurgerpoPcorn
    }


    public class ShopInfo : MonoBehaviour
    {
        //private static int orderID = 0;

        // 每个家具/桌子start的时候会向这里注册
        //public List<Slot> tableList = new List<Slot>();
        //public List<Slot> gameList = new List<Slot>();

        //private Dictionary<GameObject, Slot> gameSlotList = new Dictionary<GameObject, Slot>();
        //private Dictionary<GameObject, Slot> tableSlotDict = new Dictionary<GameObject, Slot>();
        //private Dictionary<string, Slot> furnitureSlotDict = new Dictionary<string, Slot>();

        public CustomerManager customerManager;
        public StaffManager staffManager;
        //public OrderManager orderManager;
        public SlotManager slotManager;
        public Score currentScore;

        public OrderState orderState;
        public Queue<Order> orderQueue = new Queue<Order>();

        public LeanPlane plane;         //家具拖拽时候的平面

        public int CurrentWaitNumber { get { return customerManager.leftWaitCustomer.Count + customerManager.rightWaitCustomer.Count; } }     //当前等待的人数
        public Order CurrentHandleOrder { get; set; }   //当前手动处理的订单,由handle msg发出

        public static ShopInfo Instance { get; private set; }
        protected virtual void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            //staffManager.idleEvent += (Staff staff) =>
            //{
            //    if (orderQueue.TryDequeue(out Order order))
            //    {
            //        if (order.canBeAssigned)//确保没被手动操作过
            //        {
            //            staffManager.StaffGetOrder(staff, order);

            //        }
            //    }
            //};

            if (currentScore.genRate > 0)
            {
                customerManager.StartGen(currentScore.genRate);
            }

            //GenOrder("burger");
            //GenOrder("burger");
            //GenOrder("burger");
            //GenOrder("burger");
        }

        public void FinishOrder(Order order, bool served, bool addMoney = true)
        {
            order.customer.Served = served;

            if (!served)
            {
                orderState.canceledOrder++;
            }
            else
            {
                orderState.totalOrder++;

                if (addMoney)
                {
                    AddMoney(order);
                }
            }
        }

        //手动处理订单
        public void HandleOrder(bool canBeAssigned, bool finishOrder = false)
        {
            if (CurrentHandleOrder == null)
            {
                return;
            }

            // 显示/关闭每个小食物的tag
            foreach (var val in CurrentHandleOrder.foodList)
            {
                val.tagPosition.gameObject.SetActive(!canBeAssigned);
            }

            //当前手动操作的顾客显示outline
            CurrentHandleOrder.customer.ToggleOutline(!canBeAssigned);

            //手动结束订单
            if (finishOrder)
            {
                CurrentHandleOrder.cookingScore = int.MaxValue;     //直接拉满,,,
                FinishOrder(CurrentHandleOrder, true, false);
                TogglePerson(true, false);
                ToggleFurnitureEvent(true);

                CurrentHandleOrder = null;

                return;
            }

            // 打开/关闭frame(关闭是中途结束)
            if (CurrentHandleOrder.staff == null)        //一定要确保没被系统分配过
            {
                TogglePerson(canBeAssigned, false);
                ToggleFurnitureEvent(canBeAssigned);

                CurrentHandleOrder.canBeAssigned = canBeAssigned;

                //重新加入队列
                if (canBeAssigned && !CurrentHandleOrder.orderFinished)    //再次确认订单没被顾客提前结束
                {
                    orderQueue.Enqueue(CurrentHandleOrder);         //应该不用插队把,因为手动操作也要时间的,直接放到新订单处理了
                                                                    //Debug.Log("enqeue");
                    CurrentHandleOrder = null;
                }
            }
        }

        public void AddMoney(Order order)
        {
            orderState.totalPrice += order.price;

            Debug.Log(orderState.totalPrice);
        }

        public void CancelOrder(Order orderID)
        {
            staffManager.CancelOrder(orderID);

            //orderState.canceledOrder++;
        }

        public void GenOrder(GameObject table, ref Order order)
        {
            RemoveWaitingCustomer(order.customer);        //多更新几次,,管他呢

            FoodTem tem = FoodTem.Tem(order.orderFoodName);
            string[] splitParts = tem.Need.Split(';');
            foreach (var val in splitParts)
            {
                FoodTem tem2 = FoodTem.Tem(val);

                if (slotManager.furnitureSlotDict.TryGetValue(tem2.NeedFurniture, out Slot furniture))
                {
                    Food food = new Food();
                    food.foodPosition = furniture.transform;
                    food.foodTime = tem2.Time;
                    food.subFoodSpriteLocation = tem2.Location;
                    food.tagPosition = furniture.GetFoodPosition(furniture.slotList[0]);

                    order.foodQueue.Enqueue(food);
                    order.foodList.Add(food);
                }
            }

            Slot slot = slotManager.tableSlotDict[table];
            Transform pos = slot.GetUsableStaffPosition();
            order.staffPosition = pos;
            order.tableFoodPosition = slot.GetFoodPosition(pos.gameObject);

            order.foodSpriteLocation = tem.Location;
            order.price = tem.Price;
            order.havePlate = tem.HavePlate;

            orderQueue.Enqueue(order);
            //Debug.Log(orderQueue.Count);
            //Debug.Log(task.foodSprite);
        }

        public void RegistSlot(string objName, Slot table, RegistName registName)
        {
            slotManager.RegistSlot(objName, table, registName);
            //switch (registName)
            //{
            //    case RegistName.Table:
            //        {
            //            tableList.Add(table);
            //            foreach (var val in table.slotList)
            //            {
            //                tableSlotDict.Add(val, table);
            //            }
            //        }
            //        break;

            //    case RegistName.Furniture:
            //        {
            //            //furnitureDict.Add(objName, table);
            //            furnitureSlotDict.Add(objName, table);
            //        }
            //        break;

            //    case RegistName.Game:
            //        {
            //            gameList.Add(table);
            //            foreach (var val in table.slotList)
            //            {
            //                gameSlotList.Add(val, table);
            //            }
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }

        public void RegistStaff(Staff staff)
        {
            staffManager.staffList.Add(staff);
        }

        public GameObject GetUeableGame()
        {
            return slotManager.GetUeableGame();

            //List<GameObject> usableList = new List<GameObject>();
            //foreach (var val in gameSlotList)
            //{
            //    if (val.Key.activeInHierarchy)
            //    {
            //        usableList.Add(val.Key);
            //    }
            //}
            //if (usableList.Count > 0)
            //{
            //    int index = UnityEngine.Random.Range(0, usableList.Count);

            //    return usableList[index];
            //}
            //return null;
        }

        public GameObject GetUeableTable()
        {
            return slotManager.GetUeableTable();
            //List<GameObject> usableList = new List<GameObject>();

            //foreach (var val in tableSlotDict)
            //{
            //    if (val.Key.activeInHierarchy)
            //    {
            //        usableList.Add(val.Key);
            //    }
            //}
            //if (usableList.Count > 0)
            //{
            //    int index = UnityEngine.Random.Range(0, usableList.Count);

            //    return usableList[index];
            //}
            //return null;
        }

        public bool WillCustomerInto(float needScore)
        {
            return needScore <= currentScore.environmentScore
              && CurrentWaitNumber < currentScore.maxWaitNumber;    //不能等于会再加一个,只能小于
        }

        public void AddWaitingCustomer(Customer customer)
        {
            customerManager.AddWaitingCustomer(customer);
        }

        public void RemoveWaitingCustomer(Customer customer)
        {
            customerManager.RemoveWaitingCustomer(customer);
        }

        public Vector3 GetWaitingPoint(Customer customer)
        {
            return customerManager.GetWaitingPoint(customer);
        }

        public void TogglePerson(bool en, bool all)
        {
            staffManager.ToggleStaff(en);
            customerManager.ToggleCustomer(en, all);
        }

        public void ToggleFurnitureEvent(bool en)
        {
            slotManager.ToggleFurnitureEvent(en);
            //foreach (var val in gameList)
            //{
            //    val.ToggleClick(en);
            //}
            //foreach (var val in tableList)
            //{
            //    val.ToggleClick(en);
            //}
            //foreach (var val in furnitureSlotDict)
            //{
            //    val.Value.ToggleClick(en);
            //}
        }


        void Update()
        {
            Staff staff = staffManager.GetFreeStaff();
            if (staff && orderQueue.TryDequeue(out Order order))
            {
                if (order.canBeAssigned && !order.orderFinished)//确保没被手动操作过,或者顾客提前走人的
                {
                    staffManager.StaffGetOrder(staff, order);
                }
            }
        }
    }
}