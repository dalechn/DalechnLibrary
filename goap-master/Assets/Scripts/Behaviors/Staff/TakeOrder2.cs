using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using BehaviorDesigner.Runtime;
using MyShop;
using UnityEngine;

[TaskCategory("Shop")]
public class TakeOrder2 : NavMeshMovement
{
    protected Staff staff;

    protected Transform pos;

    public override void OnStart()
    {
        base.OnStart();
        staff = Owner.GetVariable(GlobalConfig.SharedPerson).GetValue() as Staff;

        pos = Owner.GetVariable(StaffBTVal.CurrentFoodPos.ToString()).GetValue() as Transform;
        SetDestination(pos.position);

        MessageCenter.Instance?.CloseFood(staff.gameObject);
    }

    public override TaskStatus OnUpdate()
    {
        if (!staff.IsSlotDecorationMode()&&HasArrived())
        {
            staff.Emoji(false);     //���¼�ʱ,���ù���ǰ��ֱ�����¿�ʼ
            return TaskStatus.Success;
        }

        pos = Owner.GetVariable(StaffBTVal.CurrentFoodPos.ToString()).GetValue() as Transform;
        SetDestination(pos.position);

        return TaskStatus.Running;
    }
}
