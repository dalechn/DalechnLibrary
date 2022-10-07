using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[TaskCategory("Shop")]
public class HaveFun : NavMeshMovement
{
    public Customer customer;
    public MessageType emojiType;

    public override TaskStatus OnUpdate()
    {
        GameObject pos = customer.GetUsableGame();

        if (customer.Debuff()|| pos== null)
        {
            return TaskStatus.Failure;
        }

        SetDestination(pos.transform.position);

        if (HasArrived())
        {
            // todu: ���Ŷ���
            //customer.GenOrder();
            customer.HaveFun();

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnStart()
    {
        base.OnStart();
        customer = GetComponent<Customer>();

        customer.LeaveShop(emojiType);
    }
}
