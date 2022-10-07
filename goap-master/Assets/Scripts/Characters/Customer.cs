using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum CustomerBTVal
{
    LeaveTarget, WaitTarget, Speed, DinnerTime, PatienceTime
}

[System.Serializable]
public struct CustomerProp
{
    public FoodType foodType;
    public int needEnvironmentScore;
    public int needFoodScore;

    public float dinnerTime;
    public float patienceTime;
}

public class Customer : PersonBase
{
    public CustomerProp customerProp;
    internal bool unused = false;   //是否被回收
    internal bool served = false;   //是否被服务了

    private GameObject currentSite;
    private GameObject currentGame;
    private GameObject waitTarget;

    private MessageType currentEmoji;

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

    private bool judged = false;
    public void Emoji(MessageType emojiType)
    {
        if (emojiType==MessageType.None)
        {
            return;
        }

        if (emojiType == MessageType.BaseOnOrder)  
        {
            judged = true;
            if (currentOrder != null && currentOrder.cookingScore < customerProp.needFoodScore)
            {
                emojiType = MessageType.Hate;

                ShopInfo.Instance.orderState.hateOrder++;
            }
            else
            {
                emojiType = MessageType.Happy;
            }
        }

        currentEmoji = emojiType;
        //发表情
        MessageCenter.Instance.SendMessageImage(gameObject, emojiType, currentOrder);
    }

    public void HaveFun()
    {
        Debug.Log("HaveFun");
    }

    public void LeaveShop(MessageType emojiType)
    {
        if(!judged)//只能判断一次,因为havefun和leaveshop都会判断这个
        {
            Emoji(emojiType);
        }

        // 强行中止订单
        if (currentOrder != null)
        {
            ShopInfo.Instance.CancelOrder(currentOrder);
        }
        //统计准备进店 点餐的顾客
        if (currentSite == null && currentOrder == null)
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

        //currentOrder = null;      //把订单清除放到reset 不然会出现一些奇怪的bug?比如emoji判断需要订单
        unused = true;
    }

    public bool Debuff()
    {
        //Debug.Log(currentEmoji);

        return currentEmoji == MessageType.Angry || currentEmoji == MessageType.Complain || currentEmoji == MessageType.Hate;
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

    public void  GenOrder()
    {
        currentOrder = new Order();
        currentOrder.customer = this;
        currentOrder.orderFoodName = customerProp.foodType.ToString();
        currentOrder.customerPosition = currentSite.transform.position; //暂时没用了

        ShopInfo.Instance.GenOrder(currentSite,ref currentOrder);

        Emoji(MessageType.OrderName);
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
