using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[TaskCategory("Shop")]

public class GetUeableTable : Conditional
{
    public Customer customer;

    public override void OnStart()
    {
        base.OnStart();

        customer = GetComponent<Customer>();
    }

    public override TaskStatus OnUpdate()
    {
        return customer.GetUeableTable(true) ? TaskStatus.Success : TaskStatus.Failure;
    }
}
