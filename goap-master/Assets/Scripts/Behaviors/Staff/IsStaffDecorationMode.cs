using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using MyShop;

[TaskCategory("Shop")]

public class IsStaffDecorationMode : Conditional
{
    protected Staff staff;

    public override void OnStart()
    {
        base.OnStart();

        staff = Owner.GetVariable(GlobalConfig.SharedPerson).GetValue() as Staff;

    }

    public override TaskStatus OnUpdate()
    {
        return staff.IsSlotDecorationMode() ? TaskStatus.Success : TaskStatus.Failure;
    }
}
