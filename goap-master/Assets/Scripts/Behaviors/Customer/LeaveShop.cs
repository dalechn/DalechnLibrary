using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using MyShop;

[TaskCategory("Shop")]
public class LeaveShop: Seek
{
    protected Customer customer;
    public MessageType emojiType;
    public bool cancelOrder;

    //public override TaskStatus OnUpdate()
    //{
    //    if (HasArrived())
    //    {
    //        return TaskStatus.Success;
    //    }

    //    SetDestination(Target());

    //    return TaskStatus.Running;
    //}

    public override void OnStart()
    {
        base.OnStart();
        //customer = GetComponent<Customer>();
        customer = Owner.GetVariable(GlobalConfig.SharedPersonBase).GetValue() as Customer;


        customer.LeaveShop(emojiType, cancelOrder);
    }
}
