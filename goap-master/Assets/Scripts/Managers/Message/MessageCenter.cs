using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MessageType
{
    None,
    Happy, Angry, WaitTooLong, Hate, BaseOnOrder, HaveFun,
    Complain, ComplainSlow, OrderName, OrderNameStaff, PasserBy
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
    public Dictionary<GameObject, HandleMSG> handleDict = new Dictionary<GameObject, HandleMSG>();
    public Dictionary<GameObject, FoodMSG> foodDict = new Dictionary<GameObject, FoodMSG>();

    public CenterMSG messageCenter;

    void Start()
    {
        //messageCenter = FindObjectOfType<CenterMSG>();
    }

    public void SendMessageCenter(MessageType messageType, string content)
    {
        if (messageCenter)
        {
            messageCenter.HandleMessage(messageType, content);
        }
    }

    public void SendMessageHandle(GameObject person, MessageType emoji, Order order)
    {
        if (handleDict.TryGetValue(person, out HandleMSG e1))
        {
            //Debug.Log("emoji: " + emoji);

            e1.HandleMessage(emoji, order);
        }
    }

    public void SendMessageEmoji(GameObject person, MessageType emoji, Order order, string content)
    {
        if (emojiDict.TryGetValue(person, out EmojiMSG e))
        {
            //Debug.Log("emoji: " + emoji);

            e.HandleMessage(emoji, order, content);
        }
    }

    public void SendMessageText(GameObject person, MessageType emoji, Order order, string content)
    {
        if (textDict.TryGetValue(person, out TextMSG e))
        {
            //Debug.Log("emoji: " + emoji);

            e.HandleMessage(emoji, order, content);
        }
    }

    private string SplitMSG(string content)
    {
        string[] split = content.Split(";");
        if (split.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, split.Length);
            content = split[index];
        }
        return content;
    }

    public void SendMessageByCustomer(GameObject person, MessageType emoji, Order messageText)
    {
        EmojiTem tem = EmojiTem.Tem(emoji.ToString());

        int r = UnityEngine.Random.Range(0, 2);
        string contentEmoji = SplitMSG(tem.EmojiMSG);
        string contentText = SplitMSG(tem.TextMSG);

        if (r < 1 && contentEmoji != "" || contentText=="")
        {
            SendMessageEmoji(person, emoji, null, contentEmoji);
        }
        else
        {
            SendMessageText(person, emoji, messageText, contentText);
        }

        if (tem.CenterMSG != "")
        {
            SendMessageCenter(emoji, tem.CenterMSG);
        }
    }

    public void SendMessageFood(GameObject person, MessageType emoji, Order order)
    {
        if (foodDict.TryGetValue(person, out FoodMSG e))
        {
            //Debug.Log("emoji: " + emoji);

            e.HandleMessage(emoji, order);
        }
    }

    public void SendMessageByStaff(GameObject person, bool isText, Order order)
    {
        if (isText)
        {
            Debug.Log(order.date);

            EmojiTem tem = EmojiTem.Tem(MessageType.OrderNameStaff.ToString());
            SendMessageText(person, MessageType.OrderNameStaff, order, tem.TextMSG);
        }
        else
        {
            SendMessageFood(person, MessageType.OrderNameStaff, order);
        }
    }

    public void RegistMSG(GameObject person, MessageBase msg)
    {
        if (msg is HandleMSG)
        {
            handleDict.Add(person, msg as HandleMSG);
        }
        else if (msg is EmojiMSG)
        {
            emojiDict.Add(person, msg as EmojiMSG);
        }
        else if (msg is TextMSG)
        {
            textDict.Add(person, msg as TextMSG);
        }
        else if (msg is CenterMSG)
        {
            messageCenter = msg as CenterMSG;
        }
        else if (msg is FoodMSG)
        {
            foodDict.Add(person, msg as FoodMSG);
        }
    }

}
