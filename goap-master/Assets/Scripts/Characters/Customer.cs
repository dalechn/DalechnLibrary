using BehaviorDesigner.Runtime;
using System;
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
    public FoodType foodType;       //要点的食物
    public int needEnvironmentScore;    //需要的环境分
    public int needFoodScore;           //需要的厨艺分

    public float dinnerTime;            //吃饭的时间
    public float patienceTime;          //忍耐度
}

public class Customer : PersonBase
{
    public CustomerProp customerProp;
    public bool Unused { get; set; }  //是否被回收
    public bool Served { get; set; }  //是否被服务了

    public bool IsRight { get; set; }            //是否从右边来

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

    // 进入对象池的时候会调用
    public void OnDisable()
    {
        //Debug.Log("test");
        currentGame = null;
        currentSite = null;

        currentOrder = null;
        Unused = false;
    }

    protected override void Start()
    {
        base.Start();

        waitTarget = GameObject.Find(CustomerBTVal.WaitTarget.ToString());

        behaviorTree.SetVariableValue(CustomerBTVal.Speed.ToString(), moveSpeed);
        behaviorTree.SetVariableValue(CustomerBTVal.WaitTarget.ToString(), waitTarget);
        behaviorTree.SetVariableValue(CustomerBTVal.DinnerTime.ToString(), customerProp.dinnerTime);
        behaviorTree.SetVariableValue(CustomerBTVal.PatienceTime.ToString(), customerProp.patienceTime);

        customerProp.foodType = Dalechn.GameUtils.RandomEnum<FoodType>();       //暂定随机
    }

    private bool judged = false;
    public void Emoji(MessageType emojiType, float possibility = 1.0f)
    {
        if (emojiType == MessageType.None)
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

        float p = UnityEngine.Random.value;
        if (p <= possibility)
        {
            MessageCenter.Instance.SendMessageByCustomer(gameObject, emojiType, currentOrder);
        }
        //发表情
    }

    public void HaveFun()
    {
        Debug.Log("HaveFun");
    }

    public void LeaveShop(MessageType emojiType, bool cancelOrder)
    {
        ShopInfo.Instance.RemoveWaitingCustomer(this);          //移除等待的人(没有进店铺的)

        if (!judged)//只能判断一次,因为havefun和leaveshop都会判断这个
        {
            Emoji(emojiType);
        }

        // 强行中止订单
        if (currentOrder != null && cancelOrder && currentOrder.staff != null)
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
        if (currentOrder != null)
        {
            currentOrder.orderFinished = true;         //需要标记订单已被取消,无法处理!!!
            currentOrder = null;      //把订单清除放到reset 不然会出现一些奇怪的bug?比如emoji判断需要订单,不行! 会导致强行取消订单的问题!!!
        }
        Unused = true;
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
        // 改成在customermanager里面判断了
        if (checkDistance && !Unused /*Vector3.Distance(tr.position, waitTarget.transform.position) > GlobalConfig.DistanceJudgeConst*/)
        {
            return null;
        }

        //return currentSite;
        if (!currentSite)
        {
            currentSite = ShopInfo.Instance.GetUeableTable();
            if (currentSite)
            {
                //ShopInfo.Instance.CurrentWaitNumber--; //减掉当前在等待的人数
                ShopInfo.Instance.RemoveWaitingCustomer(this);

                currentSite.SetActive(false);
                return currentSite;
            }
        }

        return currentSite;
    }

    public void GenOrder()
    {
        currentOrder = new Order();
        currentOrder.customer = this;
        currentOrder.orderFoodName = customerProp.foodType.ToString();
        //currentOrder.customerPosition = currentSite.transform; //暂时没用了
        currentOrder.date = DateTime.Now.TimeOfDay.ToString();        		// 17:16:40.8520884

        ShopInfo.Instance.GenOrder(currentSite, ref currentOrder);

        Emoji(MessageType.OrderName);
    }

    public void HavingDinner()
    {
        //吃饭动画
    }


    public void WaitingTable()
    {
        if (!currentSite)
        {
            //ShopInfo.Instance.orderState.currentWaitNumber++;

            ShopInfo.Instance.AddWaitingCustomer(this);
        }
    }


    public bool GetInto()
    {
        return ShopInfo.Instance.WillCustomerInto(customerProp.needEnvironmentScore);

    }

    //是否被对待,不是被服务
    public bool GetTreated()
    {
        if (currentOrder == null)
        {
            return false;
        }
        return currentOrder.staff != null || currentOrder.canBeAssigned == false;
    }

}
