using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Shop")]
public class PasserBy : Seek
{
    protected Customer customer;
    public Transform midPosition;

    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {
        if (HasArrived())
        {
            return TaskStatus.Success;
        }

        const int distance = 5;
        if (Vector3.SqrMagnitude(midPosition.position - customer.tr.position) < Mathf.Pow(distance, 2))  //不开方会不会好点,,
        {
            if (!customer.Unused && !customer.GetInto())
            {
                customer.Emoji(MessageType.PasserBy, 0.3f);
            }
            customer.Unused = true;
        }

        SetDestination(Target());

        return TaskStatus.Running;
    

    }

}
