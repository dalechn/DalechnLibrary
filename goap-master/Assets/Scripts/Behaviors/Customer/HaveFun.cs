using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using MyShop;

[TaskCategory("Shop")]
public class HaveFun : NavMeshMovement
{
    protected Customer customer;
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
            // todu: ²¥·Å¶¯»­
            //customer.GenOrder();
            customer.HaveFun();

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnStart()
    {
        base.OnStart();
        //customer = GetComponent<Customer>();
        customer = Owner.GetVariable(GlobalConfig.SharedPersonBase).GetValue() as Customer;

        customer.LeaveShop(emojiType,false);
    }
}
