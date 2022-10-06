
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using BehaviorDesigner.Runtime;

[TaskCategory("Shop")]

public class Emoji : Wait
{
    public Customer customer;
    public EmojiType startEmoji;
    public EmojiType overTimeEmoji;

    public override TaskStatus OnUpdate()
    {
        if (startTime + waitDuration < Time.time)
        {
            customer.Emoji(overTimeEmoji);
            return TaskStatus.Success;
        }

        return TaskStatus.Running;

    }

    public override void OnStart()
    {
        base.OnStart();
        customer = GetComponent<Customer>();

        customer.Emoji(startEmoji);
    }
}
