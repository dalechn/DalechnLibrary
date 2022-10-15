using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyShop;

[TaskCategory("Shop")]
public class HaveOrder : Conditional
{
    protected Staff staff;

    public override void OnStart()
    {
        base.OnStart();

        //staff = GetComponent<Staff>();
        staff = Owner.GetVariable(GlobalConfig.SharedPerson).GetValue() as Staff;
    }

    public override TaskStatus OnUpdate()
    {
        return staff.HaveOrder()? TaskStatus.Success:TaskStatus.Failure;
    }
}
