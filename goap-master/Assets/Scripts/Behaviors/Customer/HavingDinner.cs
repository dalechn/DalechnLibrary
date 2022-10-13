using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using MyShop;

[TaskCategory("Shop")]
public class HavingDinner : Wait
{
    protected Customer customer;

    public override void OnStart()
    {
        base.OnStart();

        //customer = GetComponent<Customer>();
        customer = Owner.GetVariable(GlobalConfig.SharedPersonBase).GetValue() as Customer;

        customer.HavingDinner();
    }
}
