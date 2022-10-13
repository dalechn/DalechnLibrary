using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using BehaviorDesigner.Runtime;
using MyShop;

[TaskCategory("Shop")]
public class ServeTheDishes : NavMeshMovement
{
    protected Staff staff;

    //public SharedFloat waitTime = 1;

    //protected float waitDuration;
    //protected float startTime;
    //protected float pauseTime;

    //protected bool firstArrive = true;

    public override void OnStart()
    {
        //waitDuration = waitTime.Value;

        //staff = GetComponent<Staff>();
        staff = Owner.GetVariable(GlobalConfig.SharedPersonBase).GetValue() as Staff;
    }

    public override TaskStatus OnUpdate()
    {
        if (!staff.HaveOrder())
        {
            //staff.OrderFinish();   

            return TaskStatus.Failure;
        }

        staff.Serve();
        SetDestination(staff.GetStaffPosition());

        if (HasArrived())
        {
            staff.OrderFinish();    //如果是放在waitDuration之后还需要改一些staff的东西?

            //if(firstArrive)
            //{
            //    startTime = Time.time;
            //    firstArrive = false;
            //}

            //if (startTime + waitDuration < Time.time)
            //{
            //    return TaskStatus.Success;
            //}
            //return TaskStatus.Running;
        }

        return TaskStatus.Running;
    }

    //public override void OnPause(bool paused)
    //{
    //    if (paused)
    //    {
    //        pauseTime = Time.time;
    //    }
    //    else
    //    {
    //        startTime += (Time.time - pauseTime);
    //    }
    //}
}
