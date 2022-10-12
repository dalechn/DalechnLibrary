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

        // ��̬ע��ر��¼�,��Ϊ���ܱ����frameʹ��,ÿ��frame��һ��
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
