using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[TaskCategory("Shop")]
public class HaveFun : NavMeshMovement
{
    public Customer customer;

    public override TaskStatus OnUpdate()
    {
        GameObject pos = customer.GetUsableGame();

        if (pos)
        {
            SetDestination(pos.transform.position);

            if (HasArrived())
            {
                // todu: ²¥·Å¶¯»­
                //customer.GenOrder();
                customer.HaveFun();

                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    public override void OnStart()
    {
        base.OnStart();
        customer = GetComponent<Customer>();

        customer.LeaveShop();
    }
}
