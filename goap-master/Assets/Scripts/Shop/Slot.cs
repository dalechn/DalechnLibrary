using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RegistName
{
    Table,Furniture,Game
}

public class Slot : MonoBehaviour
{
    public List<GameObject> slotList;           //      ���λ��
    public List<GameObject> staffPositionList;      //����Ավ��λ��

    public List<GameObject> foodList;           // ��Ǻ� �����ϵ�ʳ��ʳ��Ҫ��ÿһ�����һһ��Ӧ

    public RegistName registName;

   protected virtual void Start()
    {
        ShopInfo.Instance.RegistSlot(name,this, registName);        //Ҫ��,����start��add
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

    //���Ҿ��õ�ʱ���ȡ��tag,�����ӵ�ʱ���ȡ���������ϵ�ʳ��
    public Transform GetFoodPosition(GameObject obj)
    {
        int index = slotList.FindIndex(e => { return e == obj; });

        if(index!=-1)
        {
            return foodList[index].transform;
        }

        return null;
    }

    // ��Ҫ��̬��ȡstaff�Ͳ͵�ʱ��վ��λ��,���ڲ���Ҫ��
    public Transform GetUsableStaffPosition()
    {
        foreach (var val in staffPositionList)
        {
            if (val.activeInHierarchy)
            {
                return val.transform;
            }
        }

        // ���û������Ա��λ��
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
