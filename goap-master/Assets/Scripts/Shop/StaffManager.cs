using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyShop
{

    public class StaffManager : MonoBehaviour
    {
        // ÿ��Ա��start��ʱ���������ע��
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

        // �о���������Ҫ���dict?����ֱ�Ӳ���order��staff
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