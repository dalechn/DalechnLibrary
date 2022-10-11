using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWindow : PopupUI
{
    public override void Show()
    {
        base.Show();
        UIManager.Instance.ToggleMainPop(false);
    }

    public override void Hide()
    {
        base.Hide();

        UIManager.Instance.ToggleMainPop(true);
    }
}
