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
        public List<GameObject> slotList;           //      插槽位置
        public List<GameObject> staffPositionList;      //服务员站的位置

        public List<GameObject> foodList;           // 标记和 桌子上的食物食物要和每一个插槽一一对应

        public RegistName registName;

        protected LeanSelectableByFinger select;
        protected LeanDragTranslateAlong translateAlong;
        protected LeanMultiHeld multiHeld;

        protected virtual void Start()
        {
            ShopInfo.Instance.RegistSlot(name, this, registName);        //要改,不是start就add

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

        //这应该叫节流?
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

        //给家具用的时候获取的tag,给桌子的时候获取的是桌子上的食物
        public Transform GetFoodPosition(GameObject obj)
        {
            int index = slotList.FindIndex(e => { return e == obj; });

            if (index != -1)
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
}