
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using BehaviorDesigner.Runtime;
using MyShop;

[TaskCategory("Shop")]

public class Emoji : Wait
{
    protected Customer customer;
    public MessageType startEmoji;
    public MessageType overTimeEmoji;

    private bool emoed;

    public override TaskStatus OnUpdate()
    {
        if (startTime + waitDuration < Time.time)
        {
            return TaskStatus.Success;
        }

        //����2������patience time��һ��
        if (startTime + waitDuration/2 < Time.time&&!emoed)
        {
            emoed = true;
            customer.Emoji(overTimeEmoji);
        }

        return TaskStatus.Running;

    }

    public override void OnStart()
    {
        base.OnStart();
        //customer = GetComponent<Customer>();
        customer = Owner.GetVariable(GlobalConfig.SharedPersonBase).GetValue() as Customer;

        customer.Emoji(startEmoji);

        emoed = false;  //����س�ʼ��
    }
}
