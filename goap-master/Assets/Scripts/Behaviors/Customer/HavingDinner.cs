using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Shop")]
public class HavingDinner : Wait
{
    public Customer customer;

    public override void OnStart()
    {
        base.OnStart();

        customer = GetComponent<Customer>();

        customer.HavingDinner();
    }
}
