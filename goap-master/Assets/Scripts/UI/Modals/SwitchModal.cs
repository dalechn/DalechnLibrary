using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;

public class SwitchModal : PopupUI
{
    public LeanButton button;

    protected override void Start()
    {
        base.Start();

        button.OnClick.AddListener(() => {

            UIManager.Instance.TogglePop(ModalType.modeFrame,true);
        });
    }
}
