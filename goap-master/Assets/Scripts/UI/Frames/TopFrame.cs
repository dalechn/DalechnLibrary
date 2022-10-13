using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lean.Gui;

namespace MyShop
{

    public class TopFrame : PopupUI
    {
        [Invector.vEditorToolbar("UI")]
        public LeanButton btn;

        public void RegistClick(UnityAction action)
        {
            btn.OnClick.RemoveAllListeners();
            btn.OnClick.AddListener(action);
        }
    }
}