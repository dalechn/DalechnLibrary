using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    //public Dictionary<string, RandomArea> areaDict = new Dictionary<string, RandomArea>();  //�����������

    // ÿ��Ա��start��ʱ���������ע��
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

    // �о���������Ҫ���dict?����ֱ�Ӳ���order��staff
    public void CancelOrder(Order order)
    {
        order.staff.OrderFinish(false);
        ////if(order!=null) //δ֪:��д����Ļ���ʱ���key=null?           ->��Ϊhave fun��ʱ������2��leaveshop, ���Ըĳ���ǰ�ж���
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
