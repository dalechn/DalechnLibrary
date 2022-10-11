using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using BehaviorDesigner.Runtime;

[TaskCategory("Shop")]
public class WaitingTable2 : NavMeshMovement    //边emo边动态设置位置,,,
{
    public Customer customer;

    public MessageType overTimeEmoji;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The amount of time to wait")]
    public SharedFloat waitTime = 1;

    protected float waitDuration;
    protected float startTime;

    protected bool emoed;

    protected Vector3 pos;

    public override void OnStart()
    {
        base.OnStart();
        customer = GetComponent<Customer>();

        // Remember the start time.
        startTime = Time.time;
        waitDuration = waitTime.Value;
    }

    public override TaskStatus OnUpdate()
    {
        if (startTime + waitDuration < Time.time)
        {
            return TaskStatus.Success;
        }

        //customer.WaitingTable(out pos);
        pos = ShopInfo.Instance.GetWaitingPoint(customer);

        SetDestination(pos);

        //除以2是设置patience time的一半
        if (startTime + waitDuration / 2 < Time.time && !emoed)
        {
            emoed = true;
            customer.Emoji(overTimeEmoji);
        }

        return TaskStatus.Running;
    }
}
