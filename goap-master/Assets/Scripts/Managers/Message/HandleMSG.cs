using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�ɲ�����msg
public class HandleMSG : MessageBase
{
    protected Sprite emojiSprite;
    [Invector.vEditorToolbar("UI")]
    public LeanButton button;       //�ɲ�����ť
    public Image buttonImage; //��ť��ͼ��
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

                //����
                Debug.Log("operation");
                //�Ƴ�����
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
        if (!entered && order != null && (order.staff != null || order.orderFinished))//��ϵͳ����,���߹˿���ǰ����
        {
            entered = true;
            TryHide();
        }
    }

    // ����ص���
    public void OnDisable()
    {
        order = null;
        entered = false;
    }

}
