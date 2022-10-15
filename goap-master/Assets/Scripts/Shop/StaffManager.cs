using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyShop
{

    public class StaffManager : MonoBehaviour
    {
        // 每个员工start的时候会向这里注册
        public List<Staff> staffList = new List<Staff>();

        public ShopKeeper keeper;


        void Start()
        {
        }

        public void ToggleStaff(bool en)
        {
            foreach (var val in staffList)
            {
                val.TogglePerson(en);

                MessageCenter.Instance.ToggleAllCanvas(val.gameObject, en);
            }

            keeper.TogglePerson(en);
        }

        public void StaffGetOrder(Staff staff, Order order)
        {
            staff.TakeOrder(order);

        }

        // 感觉根本不需要这个dict?可以直接操作order的staff
        public void CancelOrder(Order order)
        {
            order.staff.OrderFinish(false);
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
        }
    }
}