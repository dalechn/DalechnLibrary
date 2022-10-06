using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RegistName
{
    Table,Furniture,Game
}

public class Slot : MonoBehaviour
{
    public List<GameObject> slotList;
    public List<GameObject> staffPositionList;
    public RegistName registName;

    void Start()
    {
        ShopInfo.Instance.RegistSlot(name,this, registName);
    }

    //public GameObject GetUsableSlot()
    //{
    //    foreach(var val in slotList)
    //    {
    //        if(val.activeInHierarchy)
    //        {
    //            //val.SetActive(false);
    //            return val;
    //        }
    //    }
    //    return null;
    //}

    // ��Ҫ��̬��ȡstaff�Ͳ͵�ʱ��վ��λ��,���ڲ���Ҫ��
    public GameObject GetUsableStaffPosition()
    {
        foreach (var val in staffPositionList)
        {
            if (val.activeInHierarchy)
            {
                return val;
            }
        }

        // ���û������Ա��λ��
        foreach (var val in slotList)
        {
            if (val.activeInHierarchy)
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
