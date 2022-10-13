using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyShop
{

    public class MessageBase : PopupUI
    {
        protected PersonBase person;

        protected override void Start()
        {
            base.Start();
            person = GetComponentInParent<PersonBase>();
        }

        public virtual void ToggleCanvas(bool en)
        {
            canvas.enabled = en;
        }
    }
}