using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[TaskCategory("Shop")]

public class TakeASeat : NavMeshMovement
{
    public Customer customer;

    public override void OnStart()
    {
        base.OnStart();

        customer = GetComponent<Customer>();
    }

    public override TaskStatus OnUpdate()
    {
        GameObject pos = customer.GetUeableTable();

        if (pos)
        {
            SetDestination(pos.transform.position);

            if (HasArrived())
            {
                // todu: ²¥·Å¶¯»­
                customer.GenOrder();
                navMeshAgent.enabled = false;

                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Failure;
        }

    }
}
