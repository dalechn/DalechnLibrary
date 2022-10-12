using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    //public Dictionary<string, RandomArea> areaDict = new Dictionary<string, RandomArea>();  //管理随机区域

    // 每个员工start的时候会向这里注册
    public List<Staff> staffList = new List<Staff>();
    //public System.Action<Staff> idleEvent;

    //public Dictionary<Order, Staff> orderDict = new Dictionary<Order, Staff>();

    void Start()
    {
        //Dalechn.bl_UpdateManager.RunActionOnce("", Time.deltaTime, () =>
        //{
        //    foreach (var val in staffList)
        //    {
        //        val.finishEvent += (Staff staff) =>
        //        {
        //            orderDict.Remove(staff.currentOrder);
        //            Debug.Log("remove"+ staff.currentOrder);
        //        };
        //    }
        //});
    }

    //public void RegistArea(RandomAreaName areaName, RandomArea area)
    //{
    //    areaDict.Add(areaName.ToString(),area);
    //}

    public void StaffGetOrder(Staff staff, Order order)
    {
        staff.TakeOrder(order);

        //orderDict.Add(order, staff);

    }

    // 感觉根本不需要这个dict?可以直接操作order的staff
    public void CancelOrder(Order order)
    {
        order.staff.OrderFinish(false);
        ////if(order!=null) //未知:不写这个的话有时候会key=null?           ->因为have fun的时候会调用2次leaveshop, 所以改成提前判断了
        //{
        //    if (orderDict.TryGetValue(order, out Staff staff))
        //    {
        //        staff.OrderFinish(false);
        //    }
        //}
    }

    public Staff GetFreeStaff()
    {
        staffList.OrderByDescending(p => p.staffProp.priority);
        foreach (var val in staffList)
        {
            if (val.idleState)
            {
                return val;
            }
        }
        return null;
    }


    void Update()
    {
        //staffList.OrderByDescending(p => p.staffProp.priority);
        //foreach (var val in staffList)
        //{
        //    if (val.idleState)
        //    {
        //        idleEvent?.Invoke(val); 
        //    }
        //}
    }
}
