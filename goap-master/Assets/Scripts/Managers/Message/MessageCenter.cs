using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MessageType
{
    None,
    Happy, Angry, Hate, BaseOnOrder,
    Complain, OrderName
}

public class MessageCenter : MonoBehaviour
{
    public static MessageCenter Instance { get; private set; }
    protected virtual void Awake()
    {
        Instance = this;
    }

    public Dictionary<GameObject, EmojiMSG> emojiDict = new Dictionary<GameObject, EmojiMSG>();
    public Dictionary<GameObject, TextMSG> textDict = new Dictionary<GameObject, TextMSG>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SendMessageImage(GameObject person,MessageType emoji,Order messageText =null)
    {
        EmojiTem tem = EmojiTem.Tem(emoji.ToString());

        if (!tem.IsText)
        {
            if (emojiDict.TryGetValue(person, out EmojiMSG e))
            {
                Debug.Log("emoji: " + emoji);

                e.HandleMessage(emoji, null);
            }
        }
        else
        {
            if (textDict.TryGetValue(person, out TextMSG e))
            {
                Debug.Log("emoji: " + emoji);

                e.HandleMessage(emoji, messageText);
            }
        }
    }

    public void RegistEmoji(GameObject person,EmojiMSG emoji)
    {
        emojiDict.Add(person,emoji);
    }

    public void RegistText(GameObject person, TextMSG emoji)
    {
        textDict.Add(person, emoji);
    }

    public void SendMessageCenter()
    {

    }
}
