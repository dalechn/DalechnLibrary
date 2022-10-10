using UnityEngine;
using UnityEngine.UI;


//�����ı���msg
public class EmojiMSG : MessageBase
{
    [Invector.vEditorToolbar("UI")]
    public Image image;         //����

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
