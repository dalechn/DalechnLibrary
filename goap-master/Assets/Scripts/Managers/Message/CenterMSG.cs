using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyShop
{

    //ºá·ùmsg
    public class CenterMSG : MessageBase
    {
        [Invector.vEditorToolbar("UI")]
        public Text text;

        protected override void Start()
        {
            base.Start();

            MessageCenter.Instance.RegistMSG(gameObject, this);
        }

        public void HandleMessage(MessageType emoji, string content)
        {
            Toggle();

            text.text = content;
        }
    }
}