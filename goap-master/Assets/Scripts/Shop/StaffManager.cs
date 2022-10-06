using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    // ÿ��Ա��start��ʱ���������ע��
    public List<Staff> staffList = new List<Staff>();
    public System.Action<Staff> idleEvent;

    public Dictionary<Order, Staff> orderDict = new Dictionary<Order, Staff>();

    void Start()
    {
        Dalechn.bl_UpdateManager.RunActionOnce("", Time.deltaTime, () =>
        {
            foreach (var val in staffList)
            {
                val.finishEvent += (Staff staff) =>
                {
                    orderDict.Remove(staff.currentOrder);
                    Debug.Log("remove"+ staff.currentOrder);
                };
            }
        });
    }

    public void StaffGetOrder(Staff staff, Order order)
    {
        staff.TakeOrder(order);

        orderDict.Add(order, staff);
    }

    public void CancelOrder(Order order)
    {
        //if(order!=null) //δ֪:��д����Ļ���ʱ���key=null?           ->��Ϊhave fun��ʱ������2��leaveshop, ���Ըĳ���ǰ�ж���
        {
            if (orderDict.TryGetValue(order, out Staff staff))
            {
                staff.OrderFinish(false);
            }
        }
    }

    void Update()
    {
        staffList.OrderByDescending(p => p.staffProp.priority);
        foreach (var val in staffList)
        {
            if (val.idleState)
            {
                idleEvent?.Invoke(val);
            }
        }
    }
}
