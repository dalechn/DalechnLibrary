using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using MyShop;

[TaskCategory("Shop")]

public class IsCustomerDecorationMode : Conditional
{
    protected Customer customer;

    public override void OnStart()
    {
        base.OnStart();

        customer = Owner.GetVariable(GlobalConfig.SharedPerson).GetValue() as Customer;

    }

    public override TaskStatus OnUpdate()
    {
        return customer.IsSlotDecorationMode() ? TaskStatus.Success : TaskStatus.Failure;
    }
}
