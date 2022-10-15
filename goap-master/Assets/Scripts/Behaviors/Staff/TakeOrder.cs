using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyShop;

[TaskCategory("Shop")]
public class TakeOrder : NavMeshMovement
{
    protected Staff staff;

    private Transform currentPos;
    private bool haveFood;

    public override void OnStart()
    {
        base.OnStart();

        //staff = GetComponent<Staff>();
        staff = Owner.GetVariable(GlobalConfig.SharedPerson).GetValue() as Staff;

        haveFood = staff.DequeFoodPosition(out currentPos);

        Owner.SetVariableValue(StaffBTVal.CurrentFoodPos.ToString(),currentPos);

        //SetDestination(currentPos.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (!staff.HaveOrder()|| !haveFood)     //强行被取消订单和小食物取完
        {
            //staff.OrderFinish();

            return TaskStatus.Failure;
        }
        //Debug.Log(navMeshAgent.velocity.magnitude);
        //Debug.Log(currentPos);

        SetDestination(currentPos.position);
        if (HasArrived())
        {
            staff.Emoji(false);

            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnReset()
    {
        base.OnReset();
    }
}
