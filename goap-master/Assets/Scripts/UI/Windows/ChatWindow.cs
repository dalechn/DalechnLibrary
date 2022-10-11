using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lean.Gui;

public class ChatWindow : PopupWindow
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
