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

    private Vector3 currentPos;
    private bool haveFood;

    public override void OnStart()
    {
        base.OnStart();

        //staff = GetComponent<Staff>();
        staff = Owner.GetVariable(GlobalConfig.SharedPersonBase).GetValue() as Staff;

        haveFood = staff.DequeFoodPosition(out currentPos);
    }

    public override TaskStatus OnUpdate()
    {
        if (!staff.HaveOrder()|| !haveFood)
        {
            //staff.OrderFinish();

            return TaskStatus.Failure;
        }
        //Debug.Log(navMeshAgent.velocity.magnitude);
        //Debug.Log(currentPos);

        SetDestination(currentPos);
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
