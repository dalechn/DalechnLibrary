using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using MyShop;

    [TaskCategory("Shop")]
public class WaitingTable : NavMeshMovement
{
    protected Customer customer;

    public RandomAreaName areaName;
    private RandomArea area;

    private Vector3 pos;

    public override void OnStart()
    {
        base.OnStart();
        //customer = GetComponent<Customer>();
        customer = Owner.GetVariable(GlobalConfig.SharedPerson).GetValue() as Customer;

        //area = ShopInfo.Instance.GetFloor(areaName);
        area = RandomArea.areaDict[areaName.ToString()];

        customer.WaitingTable();            //还没进入判断圈子的时候用的是customerManager 的midpoint

        pos= ShopInfo.Instance.GetWaitingPoint(customer);

        //pos = area.GetPosition();     //如果是用这种的话就不要waitingtable2了
        SetDestination(pos);
    }

    public override TaskStatus OnUpdate()
    {
        if (HasArrived())
        {
            return TaskStatus.Success;
        }

        //customer.WaitingTable(out pos);
        pos = ShopInfo.Instance.GetWaitingPoint(customer);

        SetDestination(pos);

        return TaskStatus.Running;
    }
}
