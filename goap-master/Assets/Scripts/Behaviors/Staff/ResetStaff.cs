using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskCategory("Shop")]
public class ResetStaff : Wait
{
    protected Staff staff;

    public override TaskStatus OnUpdate()
    {
        if (startTime + waitDuration < Time.time)
        {
            staff.idleState = true;

            return TaskStatus.Success;
        }

        return TaskStatus.Running;

    }

    public override void OnStart()
    {
        base.OnStart();
        staff = GetComponent<Staff>();
    }
}
