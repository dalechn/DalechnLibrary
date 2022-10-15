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

    //ģʽ1:�ֶ�������ģʽ(handle mode)
    //ģʽ2:װ��С��ģʽ(decoration mode)
    //ģʽ3:װ��ģʽ(fix mode)
    //ģʽ4:�Ի�ģʽ(dialog mode)
    public class ShopInfo : MonoBehaviour
    {
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
            order.foodPrefabLocation.gameObject.SetActive(true);            //��ʾʳ��
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
            RemoveWaitingCustomer(order.customer);        //����¼���,,������

            FoodTem tem = FoodTem.Tem(order.orderFoodName);
            string[] splitParts = tem.Need.Split(';');
            foreach (var val in splitParts)
            {
                FoodTem tem2 = FoodTem.Tem(val);

                if (slotManager.furnitureSlotDict.TryGetValue(tem2.NeedFurniture, out Slot furniture))
                {
                    Food food = new Food();
                    food.foodPosition = furniture.GetUsableStaffPosition();     //��ȡ������ʱ��վ��λ��
                    food.foodTime = tem2.Time;                                              //ÿ��Сʳ��������ʱ��
                    food.subFoodSpriteLocation = tem2.Location;                     //ÿ��Сʳ��loading��ʱ��
                    food.tagPosition = furniture.GetFoodPosition(furniture.slotList[0]);        // �ֶ�������ʱ��tag��λ��

                    order.foodQueue.Enqueue(food);
                    order.foodList.Add(food);
                }
            }

            Slot slot = slotManager.tableSlotDict[table];
            Transform pos = slot.GetUsableStaffPosition();
            order.staffPosition = pos;                                                           //��ȡ�Ͳ͵�ʱ��վ��λ��

            order.tableFoodPosition = slot.GetFoodPosition(table);     //������ʳ���λ��
            //order.foodPrefabLocation = tem.PrefabLocation;     
            order.foodPrefabLocation = PoolManager.Pools["FoodPool"].Spawn(tem.PrefabLocation);         //����������ͬʱֱ������prefab.
            order.foodPrefabLocation.position = order.tableFoodPosition.position;
            order.foodPrefabLocation.gameObject.SetActive(false);

            order.foodSpriteLocation = tem.Location;                                            //����Ա��������յ�ʳ��λ��

            order.price = tem.Price;                                                                    //�����۸�
            order.havePlate = tem.HavePlate;                                                        //ͼƬ�Ƿ�������

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