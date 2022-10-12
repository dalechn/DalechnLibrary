using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[TaskCategory("Shop")]

public class TakeASeat : NavMeshMovement
{
    protected Customer customer;

    //private bool first = true;
    public override void OnStart()
    {
        base.OnStart();

        customer = GetComponent<Customer>();

        //first = true;
    }

    public override TaskStatus OnUpdate()
    {
        GameObject pos = customer.GetUeableTable();
        if (pos)
        {
            //if(first)
            //{
            //    //ShopInfo.Instance.CurrentWaitNumber--; //减掉当前在等待的人数
            //    ShopInfo.Instance.RemoveWaitingCustomer(customer);
            //    first = false;
            //}

            SetDestination(pos.transform.position);

            if (HasArrived())
            {
                // todu: 播放动画
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
