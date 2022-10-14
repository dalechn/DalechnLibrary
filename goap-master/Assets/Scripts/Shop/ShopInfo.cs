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
        public int environmentScore;    //������
        public int maxWaitNumber;       //��ǰ���ɵȴ�����
        public int cookingScore;            //���շ�,���й����е�Ա�����
        public int thumbsNumber;        //������

        public float genRate;          //�����ٶ�
        public float goodRate;      //������
        public float enthusiasm;    //��������
    }

    public struct OrderState
    {
        public int totalOrder;          //���в����Ķ���
        public int canceledOrder; //ǿ��ȡ���Ķ���(̫����)
        public int hateOrder;   //���۲�ϲ���Ŀ���
        public int leavedCustomer;  //û�н����Ŀ���(�Ѿ��ڵȴ�)
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

        // ÿ���Ҿ�/����start��ʱ���������ע��
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

        public LeanPlane plane;         //�Ҿ���קʱ���ƽ��

        public int CurrentWaitNumber { get { return customerManager.leftWaitCustomer.Count + customerManager.rightWaitCustomer.Count; } }     //��ǰ�ȴ�������
        public Order CurrentHandleOrder { get; set; }   //��ǰ�ֶ�����Ķ���,��handle msg����

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
            //        if (order.canBeAssigned)//ȷ��û���ֶ�������
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

        //�ֶ�������
        public void HandleOrder(bool canBeAssigned, bool finishOrder = false)
        {
            if (CurrentHandleOrder == null)
            {
                return;
            }

            // ��ʾ/�ر�ÿ��Сʳ���tag
            foreach (var val in CurrentHandleOrder.foodList)
            {
                val.tagPosition.gameObject.SetActive(!canBeAssigned);
            }

            //��ǰ�ֶ������Ĺ˿���ʾoutline
            CurrentHandleOrder.customer.ToggleOutline(!canBeAssigned);

            //�ֶ���������
            if (finishOrder)
            {
                CurrentHandleOrder.cookingScore = int.MaxValue;     //ֱ������,,,
                FinishOrder(CurrentHandleOrder, true, false);
                TogglePerson(true, false);
                ToggleFurnitureEvent(true);

                CurrentHandleOrder = null;

                return;
            }

            // ��/�ر�frame(�ر�����;����)
            if (CurrentHandleOrder.staff == null)        //һ��Ҫȷ��û��ϵͳ�����
            {
                TogglePerson(canBeAssigned, false);
                ToggleFurnitureEvent(canBeAssigned);

                CurrentHandleOrder.canBeAssigned = canBeAssigned;

                //���¼������
                if (canBeAssigned && !CurrentHandleOrder.orderFinished)    //�ٴ�ȷ�϶���û���˿���ǰ����
                {
                    orderQueue.Enqueue(CurrentHandleOrder);         //Ӧ�ò��ò�Ӱ�,��Ϊ�ֶ�����ҲҪʱ���,ֱ�ӷŵ��¶���������
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
            RemoveWaitingCustomer(order.customer);        //����¼���,,������

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
              && CurrentWaitNumber < currentScore.maxWaitNumber;    //���ܵ��ڻ��ټ�һ��,ֻ��С��
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
                if (order.canBeAssigned && !order.orderFinished)//ȷ��û���ֶ�������,���߹˿���ǰ���˵�
                {
                    staffManager.StaffGetOrder(staff, order);
                }
            }
        }
    }
}