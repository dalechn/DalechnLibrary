using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//可操作的msg
public class HandleMSG : MessageBase
{
    protected Sprite emojiSprite;
    [Invector.vEditorToolbar("UI")]
    public LeanButton button;       //可操作按钮
    public Image buttonImage; //按钮的图标
    public Image plate;
    public LeanPulse pulse;

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
            Dalechn.bl_UpdateManager.RunActionOnce("",1.5f,() => {
                if (pulse)
                {
                    pulse.enabled = true;
                }
            });

            this.order = order;
            autoHide = false;
            EnableRaycast(true);

            emojiSprite = Resources.Load<Sprite>(order.foodSpriteLocation);

            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener(() =>
            {
                //EnableRaycast(false);
                //Toggle();

                //弹窗
                Debug.Log("operation");
                //移除订单
                ShopInfo.Instance.CurrentHandleOrder = order;
                UIManager.Instance.TogglePopUI(PopType.switchModal);
            });
            buttonImage.sprite = emojiSprite;
            if(plate!=null)
            {
                plate.enabled = order.havePlate;
            }

            Toggle();
        }
    }

    public override void Hide()
    {
        base.Hide();
        
        EnableRaycast(false);

        if (pulse)
        {
            pulse.enabled = false;
        }
    }


    Order order;
    bool entered = false;
    protected void Update()
    {
        if (!entered && order != null && (order.staff != null || order.orderFinished||order.customer.Served))//被系统分配,或者顾客提前走人,或者玩家手动结束
        {
            entered = true;
            TryHide();
        }
    }

    // 对象池调用
    public void OnDisable()
    {
        order = null;
        entered = false;
    }

}
