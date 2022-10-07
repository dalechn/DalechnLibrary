using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct StaffProp
{
    public int priority;        //�ݶ��Ǽ�?
    public float cookingTime;
    public float serveInterval; //������ʱ��
    public int cookingScore;
}

public enum StaffBTVal
{
    CookingTime, Speed, ServeInterval
}

public class Staff : PersonBase
{
    public StaffProp staffProp;
    public Order currentOrder;
    public bool idleState = true;
    //public int switchMask;

    public System.Action<Staff> finishEvent;
    //private int originMask;
    //private int switchMask;

    protected override void Start()
    {
        base.Start();

        ShopInfo.Instance.RegistStaff(this);

        behaviorTree.SetVariableValue(StaffBTVal.Speed.ToString(), moveSpeed);
        behaviorTree.SetVariableValue(StaffBTVal.CookingTime.ToString(), staffProp.cookingTime);
        behaviorTree.SetVariableValue(StaffBTVal.ServeInterval.ToString(), staffProp.serveInterval);

        //originMask = ai.areaMask;
        //switchMask = ai.areaMask | 8;   //8Ϊ���Ĳ�"Out", 2^3;
    }

    public void Serve()
    {

    }

    public void TakeOrder(Order order)
    {
        idleState = false;
        currentOrder = order;

        order.cookingScore = staffProp.cookingScore;
    }

    public void OrderFinish(bool served = true)
    {
        finishEvent?.Invoke(this);

        currentOrder.customer.served = served;
        //��Ҫ�ȴ�һ�²��ܽӵ�
        Dalechn.bl_UpdateManager.RunActionOnce("", staffProp.serveInterval, () => 
        {
            idleState = true;
            currentOrder = null;
        });

        if (!served)    
        {
            ShopInfo.Instance.orderState.canceledOrder++;
        }
        else
        {
            ShopInfo.Instance.orderState.totalOrder++;
        }
    }

    public bool HaveOrder()
    {
        return currentOrder != null;
    }

    //public bool PeekFoodPosition(out Vector3 pos)
    //{
    //    //Debug.Log(currentOrder.foodPositionList.Count);
    //    return currentOrder.foodPositionList.TryPeek(out pos);
    //}

    public bool DequeFoodPosition(out Vector3 pos)
    {
        if (currentOrder != null)
        {
            return currentOrder.foodPositionList.TryDequeue(out pos);
        }

        pos = default(Vector3);
        return false;
    }

    public Vector3 GetStaffPosition()
    {
        return currentOrder.staffPosition;
    }

    protected override void Update()
    {
        base.Update();
        //ai.areaMask = HaveOrder() ? originMask : switchMask;
    }
}
