using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    hotdog, burger, popcorn, taco, cola, beer, wine
}

public enum EmojiType
{
    None,
    Happy, Angry, Complain,
    BaseOnOrder
}

public enum CustomerBTVal
{
    LeaveTarget, WaitTarget, Speed, DinnerTime, PatienceTime
}

[System.Serializable]
public struct CustomerProp
{
    public FoodType foodType;
    public int needEnvironmentScore;

    public float dinnerTime;
    public float patienceTime;
}

public class Customer : PersonBase
{
    public CustomerProp customerProp;
    internal bool unused = false;   //是否被回收
    internal bool served = false;   //是否被服务了

    public GameObject currentSite;
    public GameObject currentGame;
    private GameObject waitTarget;


    private Order currentOrder;
    public GameObject LeaveTarget
    {
        set
        {
            behaviorTree.SetVariableValue(CustomerBTVal.LeaveTarget.ToString(), value);
        }
    }

    public void Reset()
    {
        currentGame = null;
        currentSite = null;

        currentOrder = null;
        unused = false;
    }

    protected override void Start()
    {
        base.Start();

        waitTarget = GameObject.Find(CustomerBTVal.WaitTarget.ToString());

        behaviorTree.SetVariableValue(CustomerBTVal.Speed.ToString(), moveSpeed);
        behaviorTree.SetVariableValue(CustomerBTVal.WaitTarget.ToString(), waitTarget);
        behaviorTree.SetVariableValue(CustomerBTVal.DinnerTime.ToString(), customerProp.dinnerTime);
        behaviorTree.SetVariableValue(CustomerBTVal.PatienceTime.ToString(), customerProp.patienceTime);
    }


    public void Emoji(EmojiType emojiType)
    {
        //发表情
        Debug.Log("emoji: " + emojiType);
    }

    public void HaveFun()
    {
        Debug.Log("HaveFun");
    }

    public void LeaveShop()
    {
        // 强行中止订单
        if(currentOrder!=null)
        {
            ShopInfo.Instance.CancelOrder(currentOrder);
        }
        //统计准备进店 点餐的顾客
        if(currentSite==null&&currentOrder==null)
        {
            ShopInfo.Instance.orderState.leavedCustomer++;
        }

        //Debug.Log("Leave");
        if (currentSite)
        {
            currentSite.SetActive(true);
            currentSite = null;
        }

        if (currentGame)
        {
            currentGame.SetActive(true);
            currentGame = null;
        }

        currentOrder = null;
        unused = true;
    }

    public GameObject GetUsableGame()
    {
        //return currentGame;
        if (!currentGame)
        {
            currentGame = ShopInfo.Instance.GetUeableGame();
            if (currentGame)
            {
                currentGame.SetActive(false);
                return currentGame;
            }
        }
        return currentGame;
    }

    public GameObject GetUeableTable(bool checkDistance = false)
    {
        // 确保不会一出生就预定桌子
        if (checkDistance && Vector3.Distance(tr.position, waitTarget.transform.position) > 5)
        {
            return null;
        }

        //return currentSite;
        if (!currentSite)
        {
            currentSite = ShopInfo.Instance.GetUeableTable();
            if (currentSite)
            {
                currentSite.SetActive(false);
                return currentSite;
            }
        }

        return currentSite;
    }

    public Order GenOrder()
    {
        currentOrder = ShopInfo.Instance.GenOrder(customerProp.foodType.ToString(), currentSite, this);
        return currentOrder;
    }

    public void HavingDinner()
    {
        //吃饭动画
    }

    public bool GetInto()
    {
        return customerProp.needEnvironmentScore <= ShopInfo.Instance.currentScore.environmentScore;
    }

}
