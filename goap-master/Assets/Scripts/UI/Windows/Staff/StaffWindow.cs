using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lean.Gui;

namespace MyShop
{

    public class StaffWindow : PopupWindow
    {
        [Invector.vEditorToolbar("UI")]
        public StaffListWindow list;
        public LeanButton home;
        public LeanButton back;
        public LeanButton chat;

        protected override void Start()
        {
            base.Start();

            home.OnClick.AddListener(() =>
            {

                Hide();
                list.Hide();

                UIManager.Instance.ToggleMainPop(true);
            });


            back.OnClick.AddListener(() =>
            {

                Hide();
                list.Hide();

                UIManager.Instance.ToggleMainPop(true);
            });

            chat.OnClick.AddListener(() =>
            {

                //只关自己
                PopupWindow window = UIManager.Instance.GetObj<PopupWindow>(PopType.chatWindow.ToString());
                //window.ToggleOnlySelf();
                UIManager.Instance.TogglePopWindow(PopType.chatWindow, true);
            });
        }
    }
}