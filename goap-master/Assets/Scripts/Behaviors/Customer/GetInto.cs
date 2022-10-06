using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Shop")]

public class GetInto : Conditional
{
    public Customer customer;

    public override void OnStart()
    {
        base.OnStart();

        customer = GetComponent<Customer>();
    }

    public override TaskStatus OnUpdate()
    {
        return customer.GetInto()? TaskStatus.Success : TaskStatus.Failure;
    }
}
