using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using MyShop;

[TaskCategory("Shop")]

public class GetServed : Conditional
{
    protected Customer customer;

    public override void OnStart()
    {
        base.OnStart();

        //customer = GetComponent<Customer>();
        customer = Owner.GetVariable(GlobalConfig.SharedPersonBase).GetValue() as Customer;
    }

    public override TaskStatus OnUpdate()
    {
        return customer.Served ? TaskStatus.Success : TaskStatus.Running;   
    }
}
