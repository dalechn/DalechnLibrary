using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMSG : MessageBase
{
    public Image image;
    public Text text;

    protected string msg;

    protected override void Start()
    {
        base.Start();

        MessageCenter.Instance.RegistText(person.gameObject, this);

        //text = GetComponent<Text>();
    }

    public override void HandleMessage(MessageType emoji, Order messageText)
    {
        base.HandleMessage(emoji, messageText);

        EmojiTem tem = EmojiTem.Tem(emoji.ToString());
        msg = tem.Location;

        if (emoji == MessageType.OrderName)
        {
            msg += messageText.orderFoodName;

            //Dalechn.bl_UpdateManager.RunActionOnce("", 1.0f, () =>
            //{
            //    MessageCenter.Instance.SendMessageImage(person.gameObject, emoji, messageText);
            //});
        }

        text.text = msg;
    }
}
