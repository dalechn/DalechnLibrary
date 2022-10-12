using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lean.Gui;

public class StaffListWindow : PopupWindow
{
    [Invector.vEditorToolbar("UI")]
    public LeanButton home;
    public LeanButton back;

    private PopupUI parentPop;

    protected override void Start()
    {
        base.Start();

        home.OnClick.AddListener(() => {

            //if (parentPop != null)
            //{
            //    parentPop.Show();
            //}
            Hide();

            UIManager.Instance.ToggleMainPop(true);
        });


        back.OnClick.AddListener(() => {

            //UIManager.Instance.ToggleMainPop(true);

            if (parentPop!=null)
            {
                parentPop.Show();
            }
            Hide();
        });
    }

    public void RegistParent(PopupUI pop)
    {
        parentPop = pop;
    }
}
