using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskCategory("Shop")]
public class HaveOrder : Conditional
{
    public Staff staff;

    public override void OnStart()
    {
        base.OnStart();

        staff = GetComponent<Staff>();
    }

    public override TaskStatus OnUpdate()
    {
        return staff.HaveOrder()? TaskStatus.Success:TaskStatus.Failure;
    }
}
