using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Score
{
    public int environmentScore;
    public int cookingScore;
    public int thumbsNumber;
}

public struct OrderState
{
    public int totalOrder;
    public int canceledOrder; //强行取消的订单
    public int leavedCustomer;
}

public class ShopInfo : MonoBehaviour
{
    //private static int orderID = 0;

    public Dictionary<string, RandomArea> areaDict = new Dictionary<string, RandomArea>();  //管理随机区域

    // 每个家具/桌子start的时候会向这里注册
    public Dictionary<string, Slot> furnitureDict = new Dictionary<string, Slot>();
    public List<Slot> tableList = new List<Slot>();
    public List<Slot> gameList = new List<Slot>();

    private List<GameObject> gameSlotList = new List<GameObject>();
    private Dictionary< GameObject, Slot> tableSlotDict = new Dictionary<GameObject, Slot>();
    private Dictionary<string, GameObject> furnitureSlotDict = new Dictionary<string, GameObject>();


    public Queue<Order> orderQueue = new Queue<Order>();

    public StaffManager staffManager;
    //public OrderManager orderManager;
    public MessageCenter messageCenter;
    public Score currentScore;
    public OrderState orderState;

    public static ShopInfo Instance { get; private set; }
    protected virtual void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        staffManager.idleEvent += (Staff staff) =>
        {
            if(orderQueue.TryDequeue(out Order order))
            {
                staffManager.StaffGetOrder(staff,order);
            }
        };

        //GenOrder("burger");
        //GenOrder("burger");
        //GenOrder("burger");
        //GenOrder("burger");
    }

    public void CancelOrder(Order orderID)
    {
        staffManager.CancelOrder(orderID);

        //orderState.canceledOrder++;
    }

    public void SendMessageCenter()
    {
        messageCenter.SendMessageCenter();
    }

    public void RegistFloor(string areaName,RandomArea area)
    {
        areaDict.Add(areaName,area);
    }

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

    public Order GenOrder(string orderName, GameObject table,Customer customer)
    {
        if (table)
        {
            Order order = new Order();

            FoodTem tem = FoodTem.Tem(orderName);
            string[] splitParts = tem.Need.Split(';');
            foreach (var val in splitParts)
            {
                if (furnitureSlotDict.TryGetValue(val, out GameObject furniture))
                {
                    order.foodPositionList.Enqueue(furniture.transform.position);
                }
            }

            order.customer = customer;

            Slot slot = tableSlotDict[table];
            Vector3 pos = slot.GetUsableStaffPosition().transform.position;
            order.staffPosition = pos;

            order.customerPosition = table.transform.position;
            order.foodSprite = Resources.Load<Sprite>(tem.Location);

            orderQueue.Enqueue(order);
            //Debug.Log(orderQueue.Count);
            //Debug.Log(task.foodSprite);

            return order;
        }

        return null;
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
            if(val.Key.activeInHierarchy)
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

    void Update()
    {

    }
}
