using Lean.Common;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyShop
{

    public enum RegistName
    {
        Table, Furniture, Game
    }

    public class Slot : MonoBehaviour
    {
        public List<GameObject> slotList;           //      ���λ��
        public List<GameObject> staffPositionList;      //����Ավ��λ��

        public List<GameObject> foodList;           // ��Ǻ� �����ϵ�ʳ��ʳ��Ҫ��ÿһ�����һһ��Ӧ

        public RegistName registName;

        protected LeanSelectableByFinger select;
        protected LeanDragTranslateAlong translateAlong;
        protected LeanMultiHeld multiHeld;

        protected virtual void Start()
        {
            ShopInfo.Instance.RegistSlot(name, this, registName);        //Ҫ��,����start��add

            select = GetComponentInChildren<LeanSelectableByFinger>();
            translateAlong = GetComponentInChildren<LeanDragTranslateAlong>();
            multiHeld = GetComponentInChildren<LeanMultiHeld>();

            if (translateAlong)
            {
                translateAlong.ScreenDepth.Object = ShopInfo.Instance.plane;
            }

            if (select)
            {
                select.OnSelected.AddListener(OnLeanSelected);
            }
        }

        protected Dictionary<GameObject, float> canClickLastTimeMap = new Dictionary<GameObject, float>();

        //��Ӧ�ýн���?
        public bool CanClick(GameObject key, float t = 0.5f)
        {
            float now = Time.realtimeSinceStartup;

            float ptime;
            if (!canClickLastTimeMap.TryGetValue(key, out ptime))
            {
                canClickLastTimeMap.Add(key, now);
                return true;
            }

            if (now - ptime > t)
            {
                canClickLastTimeMap[key] = now;
                return true;
            }

            return false;
        }

        protected float startTime;
        protected virtual void OnLeanSelected(LeanSelect s)
        {
            const float waitDuration = 0.5f;
            if (CanClick(gameObject, waitDuration))
            {
                Dalechn.GameUtils.DampAnimation(gameObject, waitDuration);
            }
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

            if (index != -1)
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
}