using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[TaskCategory("Shop")]
public class LeaveShop: Seek
{
    public Customer customer;

    public override TaskStatus OnUpdate()
    {
        if (HasArrived())
        {
            return TaskStatus.Success;
        }

        SetDestination(Target());

        return TaskStatus.Running;
    }

    public override void OnStart()
    {
        base.OnStart();
        customer = GetComponent<Customer>();

        customer.LeaveShop();
    }
}