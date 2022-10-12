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
    public FoodType foodType;       //Ҫ���ʳ��
    public int needEnvironmentScore;    //��Ҫ�Ļ�����
    public int needFoodScore;           //��Ҫ�ĳ��շ�

    public float dinnerTime;            //�Է���ʱ��
    public float patienceTime;          //���Ͷ�
}

public class Customer : PersonBase
{
    public CustomerProp customerProp;
    public bool Unused { get; set; }  //�Ƿ񱻻���
    public bool Served { get; set; }  //�Ƿ񱻷�����

    public bool IsRight { get; set; }            //�Ƿ���ұ���

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

    // �������ص�ʱ������
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

        customerProp.foodType = Dalechn.GameUtils.RandomEnum<FoodType>();       //�ݶ����
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
        //������
    }

    public void HaveFun()
    {
        Debug.Log("HaveFun");
    }

    public void LeaveShop(MessageType emojiType, bool cancelOrder)
    {
        ShopInfo.Instance.RemoveWaitingCustomer(this);          //�Ƴ��ȴ�����(û�н����̵�)

        if (!judged)//ֻ���ж�һ��,��Ϊhavefun��leaveshop�����ж����
        {
            Emoji(emojiType);
        }

        // ǿ����ֹ����
        if (currentOrder != null && cancelOrder && currentOrder.staff != null)
        {
            ShopInfo.Instance.CancelOrder(currentOrder);
        }
        //ͳ��׼������ ��͵Ĺ˿�
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
            currentOrder.orderFinished = true;         //��Ҫ��Ƕ����ѱ�ȡ��,�޷�����!!!
            currentOrder = null;      //�Ѷ�������ŵ�reset ��Ȼ�����һЩ��ֵ�bug?����emoji�ж���Ҫ����,����! �ᵼ��ǿ��ȡ������������!!!
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
        // ȷ������һ������Ԥ������
        // �ĳ���customermanager�����ж���
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
                //ShopInfo.Instance.CurrentWaitNumber--; //������ǰ�ڵȴ�������
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
        //currentOrder.customerPosition = currentSite.transform; //��ʱû����
        currentOrder.date = DateTime.Now.TimeOfDay.ToString();        		// 17:16:40.8520884

        ShopInfo.Instance.GenOrder(currentSite, ref currentOrder);

        Emoji(MessageType.OrderName);
    }

    public void HavingDinner()
    {
        //�Է�����
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

    //�Ƿ񱻶Դ�,���Ǳ�����
    public bool GetTreated()
    {
        if (currentOrder == null)
        {
            return false;
        }
        return currentOrder.staff != null || currentOrder.canBeAssigned == false;
    }

}
