using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Dictionary<string, Slot> furnitureDict = new Dictionary<string, Slot>();
    public List<Slot> tableList = new List<Slot>();
    public List<Slot> gameList = new List<Slot>();

    private List<GameObject> gameSlotList = new List<GameObject>();
    private Dictionary<GameObject, Slot> tableSlotDict = new Dictionary<GameObject, Slot>();
    private Dictionary<string, GameObject> furnitureSlotDict = new Dictionary<string, GameObject>();

    public CustomerManager customerManager;
    public StaffManager staffManager;
    //public OrderManager orderManager;
    public Score currentScore;


    public OrderState orderState;
    public Queue<Order> orderQueue = new Queue<Order>();

    public int CurrentWaitNumber { get { return customerManager.leftWaitCustomer.Count+customerManager.rightWaitCustomer.Count; } }

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

    public void FinishOrder(Order order, bool served)
    {
        order.customer.Served = served;

        if (!served)
        {
            orderState.canceledOrder++;
        }
        else
        {
            orderState.totalOrder++;
            orderState.totalPrice += order.price;
        }
    }

    //手动处理订单
    public void HandleOrder(Order order, bool canBeAssigned)
    {
        if (order.staff == null)        //一定要确保没被系统分配过
        {
            if (canBeAssigned && !order.orderFinished)    //再次确认订单没被顾客提前结束
            {
                order.canBeAssigned = canBeAssigned;
                orderQueue.Enqueue(order);
            }
        }
    }

    public void CancelOrder(Order orderID)
    {
        staffManager.CancelOrder(orderID);

        //orderState.canceledOrder++;
    }

    public void GenOrder(GameObject table, ref Order order)
    {

        FoodTem tem = FoodTem.Tem(order.orderFoodName);
        string[] splitParts = tem.Need.Split(';');
        foreach (var val in splitParts)
        {
            FoodTem tem2 = FoodTem.Tem(val);

            if (furnitureSlotDict.TryGetValue(tem2.NeedFurniture, out GameObject furniture))
            {
                Food food = new Food();
                food.foodPosition = furniture.transform.position;
                food.foodTime = tem2.Time;
                food.subFoodSpriteLocation = tem2.Location;

                order.foodQueue.Enqueue(food);
            }
        }

        Slot slot = tableSlotDict[table];
        Vector3 pos = slot.GetUsableStaffPosition().transform.position;
        order.staffPosition = pos;

        order.foodSpriteLocation = tem.Location;
        order.price = tem.Price;
        order.havePlate = tem.HavePlate;

        orderQueue.Enqueue(order);
        //Debug.Log(orderQueue.Count);
        //Debug.Log(task.foodSprite);
    }

    //public RandomArea GetFloor(RandomAreaName areaName)
    //{
    //    return staffManager.areaDict[areaName.ToString()];
    //}

    //public void RegistFloor(RandomAreaName areaName, RandomArea area)
    //{
    //    //areaDict.Add(areaName, area);
    //    staffManager.areaDict.Add(areaName.ToString(), area);
    //}

    public void RegistSlot(string objName, Slot table, RegistName registName)
    {
        switch (registName)
        {
            case RegistName.Table:
                {
                    tableList.Add(table);
                    foreach (var val in table.slotList)
                    {
                        tableSlotDict.Add(val, table);
                    }
                }
                break;

            case RegistName.Furniture:
                {
                    furnitureDict.Add(objName, table);
                    if (table.slotList.Count > 0)
                    {
                        furnitureSlotDict.Add(objName, table.slotList[0]);
                    }
                }
                break;

            case RegistName.Game:
                {
                    gameList.Add(table);
                    foreach (var val in table.slotList)
                    {
                        gameSlotList.Add(val);
                    }
                }
                break;
            default:
                break;
        }
    }

    public void RegistStaff(Staff staff)
    {
        staffManager.staffList.Add(staff);
    }

    public GameObject GetUeableGame()
    {
        List<GameObject> usableList = new List<GameObject>();
        foreach (var val in gameSlotList)
        {
            if (val.activeInHierarchy)
            {
                usableList.Add(val);
            }
        }
        if (usableList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, usableList.Count);

            return usableList[index];
        }
        return null;
    }

    public GameObject GetUeableTable()
    {
        List<GameObject> usableList = new List<GameObject>();

        foreach (var val in tableSlotDict)
        {
            if (val.Key.activeInHierarchy)
            {
                usableList.Add(val.Key);
            }
        }
        if (usableList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, usableList.Count);

            return usableList[index];
        }
        return null;
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

    void Update()
    {
        Staff staff = staffManager.GetFreeStaff();
        if (staff && orderQueue.TryDequeue(out Order order))
        {
            if (order.canBeAssigned && !order.orderFinished)//确保没被手动操作过,或者顾客提前走人的
            {
                //Debug.Log(111);
                staffManager.StaffGetOrder(staff, order);
            }
        }
    }
}
