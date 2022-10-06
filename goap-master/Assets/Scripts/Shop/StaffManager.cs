using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    // 每个员工start的时候会向这里注册
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
        //if(order!=null) //未知:不写这个的话有时候会key=null?           ->因为have fun的时候会调用2次leaveshop, 所以改成提前判断了
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
