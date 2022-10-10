using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[TaskCategory("Shop")]
public class WaitingTable : Seek
{
    public Customer customer;
  
    public override void OnStart()
    {
        base.OnStart();
        customer = GetComponent<Customer>();

        customer.WaitingTable();
    }
}
