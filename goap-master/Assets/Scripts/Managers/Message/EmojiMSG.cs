using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiMSG : MessageBase
{
    protected Sprite emojiSprite;
    //public Image image;
    public Button button;

    protected override void Start()
    {
        base.Start();
        
        MessageCenter.Instance.RegistEmoji(person.gameObject, this);
        //image = GetComponent<Image>();
    }

    public override void HandleMessage( MessageType emoji, Order messageText)
    {
        base.HandleMessage(emoji, messageText);

        if(messageText!=null)
        {
            emojiSprite = Resources.Load<Sprite>(messageText.foodSpriteLocation);

            button.enabled = true;

            button.onClick.AddListener(() =>{

                button.enabled = false;
                //µ¯´°
            });
        }
        else
        {
            EmojiTem tem = EmojiTem.Tem(emoji.ToString());
            emojiSprite = Resources.Load<Sprite>(tem.Location);
        }

        button.image.sprite = emojiSprite;
    }
}
