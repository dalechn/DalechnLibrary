using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeFrame : PopupWindow
{
    protected override void Start()
    {
        base.Start();

    }

    public override void Show()
    {
        base.Show();
        //UIManager.Instance.ToggleMainPop(false);

        UIManager.Instance.TogglePop(ModalType.topFrame, true);

        TopFrame top = UIManager.Instance.GetObj<TopFrame>(ModalType.topFrame.ToString());

        top.RegistClick(() =>
        {
            Hide();
        });
    }

    public override void Hide()
    {
        base.Hide();
        //UIManager.Instance.ToggleMainPop(true);

        UIManager.Instance.TogglePop(ModalType.topFrame, false);
    }
}
