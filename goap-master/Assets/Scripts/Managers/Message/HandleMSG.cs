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

                //����
                Debug.Log("operation");
                //�Ƴ�����
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
        if (!entered && order != null && (order.staff != null || order.orderFinished||order.customer.Served))//��ϵͳ����,���߹˿���ǰ����,��������ֶ�����
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
