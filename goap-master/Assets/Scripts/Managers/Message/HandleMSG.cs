using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyShop
{

    //可操作的msg
    public class HandleMSG : MessageBase
    {
        protected Sprite emojiSprite;
        [Invector.vEditorToolbar("UI")]
        public LeanButton button;       //可操作按钮
        public Image buttonImage; //按钮的图标
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
                //{          //延迟摇啊摇, 用leanpulse 好像不太好写
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

                //弹窗
                    Debug.Log("operation");
                //移除订单
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
                ShopInfo.Instance.AddMoney(order);      //自动关闭就直接加钱
                return;
            }

            //if (order.customer.Served)           //切换到打钩的图片
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

        public override void ToggleCanvas(bool en)      //切换动画
        {
            base.ToggleCanvas(en);

        }

        Order order;
        bool entered = false;
        protected void Update()
        {
            if (!entered && order != null && (order.staff != null || order.orderFinished || order.customer.Served))//被系统分配,或者顾客提前走人,或者玩家手动结束
            //if (!entered && order != null && (order.state == OrderState.BySystem|| order.state == OrderState.Finished || order.customer.Served))
            {
                entered = true;

                if (order.customer.Served)           //切换到打钩的图片
                {
                    autoHide = true;
                    if (autoHide && autoHideTime > 0)
                    {
                        StartCoroutine(CoroutineHide());
                    }

                    if (secondPulse)            //切换动画,还是单独写吧,省的手动去setactive了
                    {
                        pulse.RemainingTime = 0;        //立即重置
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

        // 对象池调用
        public void OnDisable()
        {
            order = null;
            entered = false;
        }

    }
}