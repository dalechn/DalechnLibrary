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

    // 需要动态获取staff送餐的时候站的位置,现在不需要了
    public GameObject GetUsableStaffPosition()
    {
        foreach (var val in staffPositionList)
        {
            if (val.activeInHierarchy)
            {
                return val;
            }
        }

        // 如果没有设置员工位置
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
