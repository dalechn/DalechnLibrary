using Lean.Common;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Lean.Transition;

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
        public GameObject dampObj;

        protected LeanSelectableByFinger select;
        protected LeanDragTranslateAlong translateAlong;
        protected LeanMultiHeld multiHeld;
        protected NavMeshObstacle obs;
        //protected LeanCtrl ctrl;

        protected RingCtrl ring;
        protected CircleUI ui;

        public bool InDecoration { get; set; }

        //protected List<PersonBase> customerList;            //������ע��Ĺ˿�

        //public void RegistCustomer(PersonBase p)
        //{
        //    customerList.Add(p);
        //}

        //public void RemoveCustomer(PersonBase p)
        //{
        //    customerList.Remove(p);
        //}

        protected virtual void Start()
        {
            ShopInfo.Instance.RegistSlot(name, this, registName);        //Ҫ��,����start��add

            select = gameObject.AddComponent<LeanSelectableByFinger>();
            translateAlong = gameObject.AddComponent<LeanDragTranslateAlong>();
            multiHeld = gameObject.AddComponent<LeanMultiHeld>();

            ui = GetComponentInChildren<CircleUI>();
            obs = GetComponentInChildren<NavMeshObstacle>();
            ring = GetComponentInChildren<RingCtrl>();

            //ctrl = FindObjectOfType<LeanCtrl>();       

            if (ring)
            {
                ring.box = GetComponentsInChildren<BoxCollider>();
            }

            if (translateAlong)
            {
                translateAlong.ScreenDepth.Conversion = LeanScreenDepth.ConversionType.PlaneIntercept;
                translateAlong.ScreenDepth.Object = ShopInfo.Instance.plane;
                translateAlong.Damping = 10;
                translateAlong.enabled = false;
            }

            if (select)
            {
                //select.enabled = true;          //����toggle enable �ᵼ�� ���̧���޷���ק��
                select.OnSelected.AddListener(OnLeanSelected);
                select.OnDeselected.AddListener(OnLeanDeSelected);
            }

            if (multiHeld)
            {
                multiHeld.RequiredCount = 1;
                multiHeld.MaximumMovement = 30;
                //ToggleClick(true);
                multiHeld.OnFingersDown.RemoveAllListeners();   //�Ƴ��ڱ༭��ע���,�ҿ�������Ȼ�޷��Ƴ�???
                multiHeld.OnFingersDown.AddListener(OnMultiHeld);       // UI�������¼�
            }

            if (ui)
            {
                // UI�رյ��¼�
                ui.endOvercall.AddListener(() =>
                {
                    InDecoration = false;

                    obs.enabled = true;
                    ring.Toggle(false);
                    translateAlong.enabled = false;
                    select.OnSelected.AddListener(OnLeanSelected);
                    select.OnDeselected.AddListener(OnLeanDeSelected);
                    //select.enabled = true;

                    ShopInfo.Instance.TogglePerson(true, true);
                    UIManager.Instance.ToggleMainPop(true);
                });

                ui.RegistEvent((index) =>
                {
                    switch (index)
                    {
                        case 0:
                            if(ring.CanPlace)
                            {
                                ui.Hide();
                            }
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
            //����
            //UIManager.Instance.TogglePopUI(PopType.recycleModal);
            Debug.Log("Recycle");
        }

        protected Dictionary<GameObject, float> canClickLastTimeMap = new Dictionary<GameObject, float>();

        //��Ӧ�ýн���?
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
            if (dampObj&&CanClick(dampObj, waitDuration))
            {
                Dalechn.GameUtils.DampAnimation(dampObj, waitDuration);
            }
            if(multiHeld.enabled)           //�ֶ�������ģʽ�Ὺ��multiheld���¼�,���Ը������ж��Ƿ���loading����
            {
                ui.ToggleLoading(true);
            }

            //ctrl?.Toggle(false);
        }

        protected virtual void OnLeanDeSelected(LeanSelect s)
        {
            if (multiHeld.enabled)
                ui.ToggleLoading(false);

            //ctrl?.Toggle(true);
        }

        protected virtual void OnMultiHeld(List<LeanFinger> f)
        {
            originRotation = transform.rotation;
            originPosition = transform.position;

            ui?.Show();
            translateAlong.enabled = true;


            select.OnSelected.RemoveListener(OnLeanSelected);
            select.OnDeselected.RemoveListener(OnLeanDeSelected);
            //select.enabled = false;

            ShopInfo.Instance.TogglePerson(false, true);
            UIManager.Instance.ToggleMainPop(false);

            obs.enabled = false;
            //ring.enabled = true;
            ring.Toggle(true);

            InDecoration = true;
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

        //��furniture�õ�ʱ���ȡ��tag��λ��,��table��ʱ���ȡ���������ϵ�ʳ���λ��
        public virtual Transform GetFoodPosition(GameObject obj)
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

            // ���û������staffPositionList,��ֱ��ȡslot��ֵ(game��furniture)
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