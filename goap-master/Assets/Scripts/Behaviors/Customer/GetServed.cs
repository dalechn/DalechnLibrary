using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Shop")]

public class GetServed : Conditional
{
    public Customer customer;

    public override void OnStart()
    {
        base.OnStart();

        customer = GetComponent<Customer>();
    }

    public override TaskStatus OnUpdate()
    {
        return customer.served ? TaskStatus.Success : TaskStatus.Failure;
    }
}
