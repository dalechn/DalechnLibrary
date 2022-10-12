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

        UIManager.Instance.TogglePopUI(ModalType.topFrame, true);

        // 动态注册关闭事件,因为可能被多个frame使用,每个frame不一样
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

        UIManager.Instance.TogglePopUI(ModalType.topFrame, false);
    }
}
