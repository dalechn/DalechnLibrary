using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//����msg
public class TextMSG : MessageBase
{
    [Invector.vEditorToolbar("UI")]
    public Text text;
    public Color color;

    //protected string msg;

    protected override void Start()
    {
        base.Start();

        if(person)
        {
            MessageCenter.Instance.RegistMSG(person.gameObject, this);
        }

    }

    public  void HandleMessage(MessageType emoji, Order order,string content)
    {
        Toggle();

        if (emoji == MessageType.OrderName)
        {
            content += ColorUtils.HtmlColor(order.orderFoodName, color) ;

            //����ͼƬ��Ϣ,ͼƬ��Ϣ�����order�Ƿ�null���жϼ���
            Dalechn.bl_UpdateManager.RunActionOnce("", TotalTime(), () =>
            {
                MessageCenter.Instance.SendMessageHandle(person.gameObject, emoji, order);
            });
        }
        else if(emoji == MessageType.OrderNameStaff)
        {
            content += ColorUtils.HtmlColor(order.orderFoodName, color);
        }

        text.text = content;
    }
}
