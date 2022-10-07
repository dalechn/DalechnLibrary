using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Score
{
    public int environmentScore;    //������
    public int cookingScore;            //���շ�,���й����е�Ա�����
    public int thumbsNumber;        //������

    public float genRate;          //�����ٶ�
    public float goodRate;      //������
    public float enthusiasm;    //��������
}

public struct OrderState
{
    public int totalOrder;          //���в����Ķ���
    public int canceledOrder; //ǿ��ȡ���Ķ���
    public int hateOrder;   //���۲�ϲ���Ŀ���
    public int leavedCustomer;  //û�н����Ŀ���(�Ѿ��ڵȴ�)
}

public enum FoodType
{
    hotdog, burger, popcorn, taco, cola, beer, wine
}


public class ShopInfo : MonoBehaviour
{
    //private static int orderID = 0;

    public Dictionary<string, RandomArea> areaDict = new Dictionary<string, RandomArea>();  //�����������

    // ÿ���Ҿ�/����start��ʱ���������ע��
    public Dictionary<string, Slot> furnitureDict = new Dictionary<string, Slot>();
    public List<Slot> tableList = new List<Slot>();
    public List<Slot> gameList = new List<Slot>();

    private List<GameObject> gameSlotList = new List<GameObject>();
    private Dictionary<GameObject, Slot> tableSlotDict = new Dictionary<GameObject, Slot>();
    private Dictionary<string, GameObject> furnitureSlotDict = new Dictionary<string, GameObject>();

    public CustomerManager customerManager;
    public StaffManager staffManager;
    //public OrderManager orderManager;
    //public MessageCenter messageCenter;
    public Score currentScore;


    public OrderState orderState;
    public Queue<Order> orderQueue = new Queue<Order>();

    public static ShopInfo Instance { get; private set; }
    protected virtual void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        staffManager.idleEvent += (Staff staff) =>
        {
            if (orderQueue.TryDequeue(out Order order))
            {
                staffManager.StaffGetOrder(staff, order);
            }
        };

        if(currentScore.genRate>0)
        {
            customerManager.StartGen(currentScore.genRate);
        }

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

    //public void SendMessageCenter()
    //{
    //    messageCenter.SendMessageCenter();
    //}

    public void RegistFloor(string areaName, RandomArea area)
    {
        areaDict.Add(areaName, area);
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

    public void GenOrder(GameObject table,ref  Order order)
    {
        FoodTem tem = FoodTem.Tem(order.orderFoodName);
        string[] splitParts = tem.Need.Split(';');
        foreach (var val in splitParts)
        {
            if (furnitureSlotDict.TryGetValue(val, out GameObject furniture))
            {
                order.foodPositionList.Enqueue(furniture.transform.position);
            }
        }

        Slot slot = tableSlotDict[table];
        Vector3 pos = slot.GetUsableStaffPosition().transform.position;
        order.staffPosition = pos;

        order.foodSpriteLocation = tem.Location;

        orderQueue.Enqueue(order);
        //Debug.Log(orderQueue.Count);
        //Debug.Log(task.foodSprite);
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

    void Update()
    {

    }
}
