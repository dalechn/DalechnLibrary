using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct StaffProp
{
    public int priority;        //�ݶ��Ǽ�?
    public float cookingTime;   //ֻ���ڼ���ÿ��ʱ��İٷֱ�
    public float serveInterval; //ÿ�η�����ʱ��
    public int cookingScore;    //���շ�
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
        //finishEvent?.Invoke(this);  //staff manager �Ƴ�����

        if(currentOrder!=null)  //��ʵҲû��Ҫ�ж�
        {
            ShopInfo.Instance.FinishOrder(currentOrder, served);
        }

        currentOrder = null;    //�������Ҫֱ���Ƴ�,��Ȼ�ᵼ�¶���״̬û����(��Ϊ״̬���ж���currentOrder==null)
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
            food.foodTime *= staffProp.cookingTime;                                                                             //��̬����cookingtime
            behaviorTree.SetVariableValue(StaffBTVal.CookingTime.ToString(), food.foodTime+1);             // ��̬����cookingtime,+1�Ǹ�ʱ�仺��һ��,��wait 1��

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
