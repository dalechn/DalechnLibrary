using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;
using UObject = UnityEngine.Object;

public enum ModalType
{
    switchModal, modeFrame, topFrame
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
    //private PopupUI top;

    void Start()
    {
        binder = GetComponent<PrefabObjBinder>();

        foreach (var val in binder.items)
        {
            PopupUI ui = val.obj as PopupUI;
            if (ui)
            {
                ui.gameObject.SetActive(true);
            }
        }

        RegistMainPop();
        RegistButton();

        Dalechn.bl_UpdateManager.RunActionOnce("", Time.deltaTime, () => { ToggleMainPop(); });
    }

    private void RegistMainPop()
    {
        up = binder.GetObj<PopupUI>("up");
        left = binder.GetObj<PopupUI>("left");
        right = binder.GetObj<PopupUI>("right");
        bottom = binder.GetObj<PopupUI>("bottom");
    }

    private void RegistButton()
    {
        LeanButton gold = binder.GetObj<LeanButton>("reward");
        LeanButton reward = binder.GetObj<LeanButton>("gold");
        LeanButton gem = binder.GetObj<LeanButton>("gem");
        LeanButton staff = binder.GetObj<LeanButton>("staff");
        LeanButton furniture = binder.GetObj<LeanButton>("furniture");
        LeanButton info = binder.SetLeanButtonClick("info", () =>
        {
            binder.GetObj<PopupUI>("infoWindow").Toggle();
        });
        LeanButton setting = binder.SetLeanButtonClick("setting", () =>
        {
            binder.GetObj<PopupUI>("settingWindow").Toggle();
        });
        LeanButton chat = binder.SetLeanButtonClick("chat", () =>
        {
            binder.GetObj<PopupUI>("chatWindow").Toggle();
        });
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

    }

    public void ToggleMainPop()
    {
        up.Toggle();
        left.Toggle();
        right.Toggle();
        bottom.Toggle();
    }

    public void TogglePop(ModalType modalType)
    {
        PopupUI switchPop = binder.GetObj<PopupUI>(modalType.ToString());

        if (switchPop)
        {
            switchPop.Toggle();
        }
    }

    public void TogglePop(ModalType modalType, bool enable)
    {
        PopupUI switchPop = binder.GetObj<PopupUI>(modalType.ToString());

        if (switchPop)
        {
            if (enable)
            {
                switchPop.Show();
            }
            else
            {
                switchPop.Hide();
            }
        }
    }

    public T GetObj<T>(string name) where T : UObject
    {
        return binder.GetObj(name) as T;
    }

    void Update()
    {

    }
}