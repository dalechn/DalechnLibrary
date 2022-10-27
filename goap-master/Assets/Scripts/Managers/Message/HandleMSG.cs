using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyShop
{

    //�ɲ�����msg
    public class HandleMSG : MessageBase
    {
        protected Sprite emojiSprite;
        [Invector.vEditorToolbar("UI")]
        public LeanButton button;       //�ɲ�����ť
        public Image buttonImage; //��ť��ͼ��
        public Image plate;
        public LeanPulse pulse;

        public LeanPulse secondPulse;
        public GameObject switchObj;

        protected override void Start()
        {
            base.Start();

            if (person)
            {
                MessageCenter.Instance.RegistMSG(person.gameObject, this);
            }
        }

        public void HandleMessage(MessageType emoji, Order order)
        {
            if (order != null)
            {
                //Dalechn.bl_UpdateManager.RunActionOnce("", 1.5f, () =>
                //{          //�ӳ�ҡ��ҡ, ��leanpulse ����̫��д
                if (pulse)
                {
                    pulse.RemainingTime = pulse.TimeInterval;
                    pulse.enabled = true;
                }
                //});

                this.order = order;
                autoHide = false;
                EnableRaycast(true);

                plate.gameObject.SetActive(true);
                switchObj.SetActive(false);

                emojiSprite = Resources.Load<Sprite>(order.foodSpriteLocation);

                buttonImage.sprite = emojiSprite;
                if (plate != null)
                {
                    plate.enabled = order.havePlate;
                }

                button.OnClick.RemoveAllListeners();
                button.OnClick.AddListener(() =>
                {
                    //button.OnClick.RemoveAllListeners();

                //EnableRaycast(false);
                //Toggle();

                //����
                    Debug.Log("operation");
                //�Ƴ�����
                    ShopInfo.Instance.CurrentHandleOrder = order;
                    UIManager.Instance.TogglePopUI(PopType.switchModal);
                });

                Toggle();
            }
        }

        public override void Hide()
        {
            base.Hide();

            //endOvercall.RemoveAllListeners();

            if (autoHide)
            {
                ShopInfo.Instance.AddMoney(order);      //�Զ��رվ�ֱ�Ӽ�Ǯ
                return;
            }

            //if (order.customer.Served)           //�л����򹳵�ͼƬ
            //{
            //    autoHide = true;

            //    plate.gameObject.SetActive(false);
            //    switchObj.SetActive(true);

            //    endOvercall.AddListener(() => { base.Show(); });

            //    button.OnClick.RemoveAllListeners();
            //    button.OnClick.AddListener(() =>
            //    {
            //        Hide();
            //        //ShopInfo.Instance.AddMoney(order);

            //    });
            //}
            //else
            {
                EnableRaycast(false);

                if (pulse)
                {
                    pulse.enabled = false;
                }
                if (secondPulse)
                {
                    secondPulse.enabled = false;
                }
            }
        }

        public override void Show()
        {
            base.Show();
        }

        public override void ToggleCanvas(bool en)      //�л�����
        {
            base.ToggleCanvas(en);

        }

        Order order;
        bool entered = false;
        protected void Update()
        {
            if (!entered && order != null && (order.staff != null || order.orderFinished || order.customer.Served))//��ϵͳ����,���߹˿���ǰ����,��������ֶ�����
            //if (!entered && order != null && (order.state == OrderState.BySystem|| order.state == OrderState.Finished || order.customer.Served))
            {
                entered = true;

                if (order.customer.Served)           //�л����򹳵�ͼƬ
                {
                    autoHide = true;
                    if (autoHide && autoHideTime > 0)
                    {
                        StartCoroutine(CoroutineHide());
                    }

                    if (secondPulse)            //�л�����,���ǵ���д��,ʡ���ֶ�ȥsetactive��
                    {
                        pulse.RemainingTime = 0;        //��������
                        secondPulse.enabled = true;
                    }
                    if (pulse)
                    {
                        pulse.RemainingTime = 0;
                        pulse.enabled = false;
                    }

                    plate.gameObject.SetActive(false);
                    switchObj.SetActive(true);

                    button.OnClick.RemoveAllListeners();
                    button.OnClick.AddListener(() =>
                    {
                        Hide();
                        button.OnClick.RemoveAllListeners();
                    });
                }
                else
                {
                    TryHide();
                }
            }
        }

        // ����ص���
        public void OnDisable()
        {
            order = null;
            entered = false;
        }

    }
}