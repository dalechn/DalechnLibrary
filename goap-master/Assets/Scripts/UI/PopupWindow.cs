using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWindow : PopupUI
{
    [Invector.vEditorToolbar("UI")]
    public bool selfMode;       //只开关自己

    protected override void Start()
    {
        base.Start();

        //startOvercall.AddListener(() => { selfMode = false; });
        endOvercall.AddListener(() => { selfMode = false; });   //关闭的时候才退出selfmode
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
