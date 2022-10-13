using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lean.Gui;

namespace MyShop
{

    public class SettingWindow : PopupWindow
    {
        [Invector.vEditorToolbar("UI")]
        public LeanButton closeBtn;

        protected override void Start()
        {
            base.Start();

            closeBtn.OnClick.AddListener(() =>
            {
                Hide();
            });
        }

    }
}