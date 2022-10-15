using Lean.Common;
using PathologicalGames;
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

    //模式1:手动处理订单模式(handle mode)
    //模式2:装饰小店模式(decoration mode)
    //模式3:装修模式(fix mode)
    //模式4:对话模式(dialog mode)
    public class ShopInfo : MonoBehaviour
    {
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
            if (currentScore.genRate > 0)
            {
                customerManager.StartGen(currentScore.genRate);
            }
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
            order.foodPrefabLocation.gameObject.SetActive(true);            //显示食物
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

        public void CustomerLeave()
        {

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
                    food.foodPosition = furniture.GetUsableStaffPosition();     //获取做饭的时候站的位置
                    food.foodTime = tem2.Time;                                              //每个小食物做饭的时间
                    food.subFoodSpriteLocation = tem2.Location;                     //每个小食物loading的时间
                    food.tagPosition = furniture.GetFoodPosition(furniture.slotList[0]);        // 手动操作的时候tag的位置

                    order.foodQueue.Enqueue(food);
                    order.foodList.Add(food);
                }
            }

            Slot slot = slotManager.tableSlotDict[table];
            Transform pos = slot.GetUsableStaffPosition();
            order.staffPosition = pos;                                                           //获取送餐的时候站的位置

            order.tableFoodPosition = slot.GetFoodPosition(table);     //桌子上食物的位置
            //order.foodPrefabLocation = tem.PrefabLocation;     
            order.foodPrefabLocation = PoolManager.Pools["FoodPool"].Spawn(tem.PrefabLocation);         //产生订单的同时直接生成prefab.
            order.foodPrefabLocation.position = order.tableFoodPosition.position;
            order.foodPrefabLocation.gameObject.SetActive(false);

            order.foodSpriteLocation = tem.Location;                                            //服务员手里的最终的食物位置

            order.price = tem.Price;                                                                    //订单价格
            order.havePlate = tem.HavePlate;                                                        //图片是否有盘子

            orderQueue.Enqueue(order);
            //Debug.Log(orderQueue.Count);
            //Debug.Log(task.foodSprite);
        }

        public void RegistSlot(string objName, Slot table, RegistName registName)
        {
            slotManager.RegistSlot(objName, table, registName);
        }

        public void RegistStaff(Staff staff)
        {
            staffManager.staffList.Add(staff);
        }

        public GameObject GetUeableGame()
        {
            return slotManager.GetUeableGame();
        }

        public GameObject GetUeableTable()
        {
            return slotManager.GetUeableTable();
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