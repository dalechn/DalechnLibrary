using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWindow : PopupUI
{
    [Invector.vEditorToolbar("UI")]
    public bool selfMode;       //ֻ�����Լ�

    protected override void Start()
    {
        base.Start();

        //startOvercall.AddListener(() => { selfMode = false; });
        endOvercall.AddListener(() => { selfMode = false; });   //�رյ�ʱ����˳�selfmode
    }

    public override void Show()
    {
        base.Show();
        if (!selfMode)
        {
            UIManager.Instance.ToggleMainPop(false);
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (!selfMode)
        {
            UIManager.Instance.ToggleMainPop(true);
        }
    }

    //public override void Toggle()
    //{
    //    if(selfMode)
    //    {
    //        ToggleOnlySelf();
    //    }
    //    else
    //    {
    //        base.Toggle();
    //    }
    //}

    public void ToggleOnlySelf()
    {
        if (isBusy)
            return;

        isBusy = true;

        if (isVisible)
            base.Hide();
        else
            base.Show();
    }

    public  void ShowOnlySelf()
    {
        base.Show();
    }

    public  void HideOnlySelf()
    {
        base.Hide();
    }
}
