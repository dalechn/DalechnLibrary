using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;

public enum ModalType
{
    switchModal,modeFrame
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    protected virtual void Awake()
    {
        Instance = this;
    }

    private PrefabObjBinder binder;

    private PopupUI up;
    private PopupUI left;
    private PopupUI right;
    private PopupUI bottom;

    private LeanButton reward;
    private LeanButton gold;
    private LeanButton gem;
    private LeanButton chat;
    private LeanButton staff;
    private LeanButton furniture;
    private LeanButton setting;
    private LeanButton info;

    void Start()
    {
        binder = GetComponent<PrefabObjBinder>();

        up = binder.GetObj<PopupUI>("up");
        left = binder.GetObj<PopupUI>("left");
        right = binder.GetObj<PopupUI>("right");
        bottom = binder.GetObj<PopupUI>("bottom");

        gold = binder.GetObj<LeanButton>("gold");
        reward = binder.GetObj<LeanButton>("reward");
        gem = binder.GetObj<LeanButton>("gem");
        chat = binder.GetObj<LeanButton>("chat");
        staff = binder.GetObj<LeanButton>("staff");
        furniture = binder.GetObj<LeanButton>("furniture");
        setting = binder.GetObj<LeanButton>("setting");
        info = binder.GetObj<LeanButton>("info");

    }

    private void RegistButton()
    {

    }

    public void ToggleMainPop(bool enable)
    {
        if (enable)
        {
            up.Show();
            left.Show();
            right.Show();
            bottom.Show();
        }
        else
        {
            up.Hide();
            left.Hide();
            right.Hide();
            bottom.Hide();
        }

        //up.Toggle();
        //left.Toggle();
        //right.Toggle();
        //bottom.Toggle();
    }

    public void TogglePop(ModalType modalType, bool closeMainPanel)
    {
        PopupUI switchPop = binder.GetObj<PopupUI>(modalType.ToString());

        if (switchPop)
        {
            switchPop.Toggle();

            if (closeMainPanel)
            {
                ToggleMainPop(closeMainPanel);
            }
        }
    }

    void Update()
    {

    }
}
