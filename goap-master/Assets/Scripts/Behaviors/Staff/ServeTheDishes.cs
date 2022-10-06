using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using BehaviorDesigner.Runtime;

[TaskCategory("Shop")]
public class ServeTheDishes : NavMeshMovement
{
    public Staff staff;

    public SharedFloat waitTime = 1;

    protected float waitDuration;
    protected float startTime;
    protected float pauseTime;

    protected bool firstArrive = true;

    public override void OnStart()
    {
        waitDuration = waitTime.Value;

        staff = GetComponent<Staff>();
    }

    public override TaskStatus OnUpdate()
    {
        if (!staff.HaveOrder())
        {
            return TaskStatus.Failure;
        }

        staff.Serve();
        SetDestination(staff.GetStaffPosition());

        if (HasArrived())
        {
            if(firstArrive)
            {
                startTime = Time.time;
                firstArrive = false;
            }

            if (startTime + waitDuration < Time.time)
            {
                staff.OrderFinish();

                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        return TaskStatus.Running;
    }

    public override void OnPause(bool paused)
    {
        if (paused)
        {
            pauseTime = Time.time;
        }
        else
        {
            startTime += (Time.time - pauseTime);
        }
    }
}
