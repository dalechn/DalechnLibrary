using UnityEngine;
using UnityEngine.UI;

namespace MyShop
{

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

        public void HandleMessage(MessageType emoji, Order order, string content, bool autoHide)
        {
            image.sprite = Resources.Load<Sprite>(content);

            this.autoHide = autoHide;

            Show();
        }

        public override void Show()
        {
            endOvercall.RemoveAllListeners();       //关闭其他/以前的监听器
            if (isVisible)
            {
                Hide();
                endOvercall.AddListener(() =>
                {
                    base.Show();
                    endOvercall.RemoveAllListeners();       //这次注册的监听器只用一次
                });
            }
            else
            {
                base.Show();
            }
        }

    }
}