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
            this.order = order;
            //autoHideTime = order.customer.customerProp.patienceTime;
            autoHide = false;
            EnableRaycast(true);

            emojiSprite = Resources.Load<Sprite>(order.foodSpriteLocation);

            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener(() =>
            {
                EnableRaycast(false);
                //Toggle();

                //弹窗
                Debug.Log("operation");
                //移除订单
                ShopInfo.Instance.HandleOrder(order, false);
            });
            buttonImage.sprite = emojiSprite;
            if(plate!=null)
            {
                plate.enabled = order.havePlate;
            }

            Toggle();
        }
    }


    Order order;
    bool entered = false;
    protected void Update()
    {
        if (!entered && order != null && (order.staff != null || order.orderFinished))//被系统分配,或者顾客提前走人
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
