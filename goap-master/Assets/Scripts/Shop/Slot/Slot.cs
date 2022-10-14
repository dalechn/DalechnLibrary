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
        public CircleUI ui;
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
            ui = GetComponentInChildren<CircleUI>();

            if (translateAlong)
            {
                translateAlong.ScreenDepth.Object = ShopInfo.Instance.plane;
            }

            if (select)
            {
                select.OnSelected.AddListener(OnLeanSelected);
            }

            if (multiHeld)
            {
                //ToggleClick(true);
                multiHeld.OnFingersDown.RemoveAllListeners();   //移除在编辑器注册的,我靠这样居然无法移除???
                multiHeld.OnFingersDown.AddListener(OnMultiHeld);
            }

            if (ui)
            {
                //ui关闭的事件
                ui.endOvercall.AddListener(() =>
                {
                    translateAlong.enabled = false;
                    select.OnSelected.AddListener(OnLeanSelected);

                    ShopInfo.Instance.TogglePerson(true, true);
                });

                ui.RegistEvent((index) =>
                {
                    switch (index)
                    {
                        case 0:
                            ui.Hide();
                            break;
                        case 1:
                            ResetPosition();
                            //ui.Hide();
                            break;
                        case 2:
                            Rotate();
                            break;
                        case 3:
                            Recycle();
                            break;
                        default:
                            break;
                    }
                });
            }
        }

        public void ToggleClick(bool en)
        {
            if (!multiHeld)
            {
                return;
            }
            multiHeld.enabled = en;

            //if (en)
            //{
            //    multiHeld.OnFingersDown.RemoveAllListeners();
            //    multiHeld.OnFingersDown.AddListener(OnMultiHeld);
            //}
            //else
            //{
            //    multiHeld.OnFingersDown.RemoveAllListeners();
            //}
        }

        public void Rotate()
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 90, 0));
        }


        private Quaternion originRotation;
        private Vector3 originPosition;

        public void ResetPosition()
        {
            transform.position = originPosition;
            transform.rotation = originRotation;
        }

        public void Recycle()
        {
            //回收
            //UIManager.Instance.TogglePopUI(PopType.recycleModal);
            Debug.Log("Recycle");
        }

        protected Dictionary<GameObject, float> canClickLastTimeMap = new Dictionary<GameObject, float>();

        //这应该叫节流?
        protected bool CanClick(GameObject key, float t = 0.5f)
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

        protected virtual void OnLeanSelected(LeanSelect s)
        {
            const float waitDuration = 0.5f;
            if (CanClick(gameObject, waitDuration))
            {
                Dalechn.GameUtils.DampAnimation(gameObject, waitDuration);
            }
        }


        protected virtual void OnMultiHeld(List<LeanFinger> f)
        {
            originRotation = transform.rotation;
            originPosition = transform.position;

            ui?.Show();
            translateAlong.enabled = true;

            select.OnSelected.RemoveListener(OnLeanSelected);

            ShopInfo.Instance.TogglePerson(false, true);
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

        //给家具用的时候获取的tag的位置,给桌子的时候获取的是桌子上的食物的位置
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