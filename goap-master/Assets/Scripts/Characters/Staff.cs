using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct StaffProp
{
    public int priority;        //暂定星级?
    public float cookingTime;   //只用于减少每个时间的百分比
    public float serveInterval; //每次服务间隔时间
    public int cookingScore;    //厨艺分
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

    //public System.Action<Staff> finishEvent;

    protected override void Start()
    {
        base.Start();

        ShopInfo.Instance.RegistStaff(this);

        behaviorTree.SetVariableValue(StaffBTVal.Speed.ToString(), moveSpeed);
        behaviorTree.SetVariableValue(StaffBTVal.CookingTime.ToString(), staffProp.cookingTime);
        behaviorTree.SetVariableValue(StaffBTVal.ServeInterval.ToString(), staffProp.serveInterval);

    }

    public void Serve()
    {

    }

    public void TakeOrder(Order order)
    {
        idleState = false;
        currentOrder = order;

        order.cookingScore = staffProp.cookingScore;
        order.staff = this;

        Emoji(true);
    }

    public void OrderFinish(bool served = true)
    {
        //finishEvent?.Invoke(this);  //staff manager 移除引用

        if(currentOrder!=null)  //其实也没必要判断
        {
            ShopInfo.Instance.FinishOrder(currentOrder, served);
        }

        currentOrder = null;    //这个还是要直接移除,不然会导致订单状态没更新(因为状态机判断是currentOrder==null)
    }

    public bool HaveOrder()
    {
        return currentOrder != null;
    }

    public void Emoji(bool emoji)
    {
        MessageCenter.Instance.SendMessageByStaff(gameObject, emoji, currentOrder);
    }

    public bool DequeFoodPosition(out Vector3 pos)
    {
        if (currentOrder != null && currentOrder.GetCurrentFood(out Food food))
        {
            food.foodTime *= staffProp.cookingTime;                                                                             //动态减少cookingtime
            behaviorTree.SetVariableValue(StaffBTVal.CookingTime.ToString(), food.foodTime+1);             // 动态设置cookingtime,+1是给时间缓冲一下,多wait 1秒

            pos = food.foodPosition;

            return true;
        }

        pos = default(Vector3);
        return false;
    }

    public Vector3 GetStaffPosition()
    {
        return currentOrder.staffPosition;
    }

}
