using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[TaskCategory("Shop")]
public class WaitingTable : NavMeshMovement
{
    public Customer customer;

    public RandomAreaName areaName;
    private RandomArea area;

    private Vector3 pos;

    public override void OnStart()
    {
        base.OnStart();
        customer = GetComponent<Customer>();
        area = ShopInfo.Instance.GetFloor(areaName);

        pos = area.GetPosition();
        SetDestination(pos);
        customer.WaitingTable();
    }

    public override TaskStatus OnUpdate()
    {
        if (HasArrived())
        {
            return TaskStatus.Success;
        }

        SetDestination(pos);

        return TaskStatus.Running;
    }
}
