using UnityEngine;
using UnityEngine.UI;

namespace MyShop
{

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

        public void HandleMessage(MessageType emoji, Order order, string content, bool autoHide)
        {
            image.sprite = Resources.Load<Sprite>(content);

            this.autoHide = autoHide;

            Show();
        }

        public override void Show()
        {
            endOvercall.RemoveAllListeners();       //�ر�����/��ǰ�ļ�����
            if (isVisible)
            {
                Hide();
                endOvercall.AddListener(() =>
                {
                    base.Show();
                    endOvercall.RemoveAllListeners();       //���ע��ļ�����ֻ��һ��
                });
            }
            else
            {
                base.Show();
            }
        }

    }
}