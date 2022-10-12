using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RegistName
{
    Table,Furniture,Game
}

public class Slot : MonoBehaviour
{
    public List<GameObject> slotList;           //      插槽位置
    public List<GameObject> staffPositionList;      //服务员站的位置

    public List<GameObject> foodList;           // 标记和 桌子上的食物食物要和每一个插槽一一对应

    public RegistName registName;

   protected virtual void Start()
    {
        ShopInfo.Instance.RegistSlot(name,this, registName);        //要改,不是start就add
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

    //给家具用的时候获取的tag,给桌子的时候获取的是桌子上的食物
    public Transform GetFoodPosition(GameObject obj)
    {
        int index = slotList.FindIndex(e => { return e == obj; });

        if(index!=-1)
        {
            return foodList[index].transform;
        }

        return null;
    }

    // 需要动态获取staff送餐的时候站的位置,现在不需要了
    public Transform GetUsableStaffPosition()
    {
        foreach (var val in staffPositionList)
        {
            if (val.activeInHierarchy)
            {
                return val.transform;
            }
        }

        // 如果没有设置员工位置
        foreach (var val in slotList)
        {
            if (val.activeInHierarchy)
            {
                return val.transform;
            }
        }

        return null;
    }

    void Update()
    {
        
    }
}
