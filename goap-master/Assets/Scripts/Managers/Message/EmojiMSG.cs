using UnityEngine;
using UnityEngine.UI;


//单纯的表情msg
public class EmojiMSG : MessageBase
{
    [Invector.vEditorToolbar("UI")]
    public Image image;         //表情

    protected override void Start()
    {
        base.Start();

        if (person)
        {
            MessageCenter.Instance.RegistMSG(person.gameObject, this);
        }
    }

    public void HandleMessage(MessageType emoji, Order order,string content)
    {
        image.sprite = Resources.Load<Sprite>(content);

        Toggle();
    }
}
