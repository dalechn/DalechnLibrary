using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using MyShop;
using BehaviorDesigner.Runtime.Tasks.Movement;
using BehaviorDesigner.Runtime;

[TaskCategory("Shop")]
public class HavingDinner2 : NavMeshMovement
{
    protected Customer customer;

    protected Transform pos;

    public override void OnStart()
    {
        base.OnStart();
        customer = Owner.GetVariable(GlobalConfig.SharedPerson).GetValue() as Customer;

        pos = customer.GetUeableTable().transform;

        SetDestination(pos.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (!customer.IsSlotDecorationMode() && HasArrived())
        {
            return TaskStatus.Success;
        }

        SetDestination(pos.position);

        return TaskStatus.Running;
    }
}
