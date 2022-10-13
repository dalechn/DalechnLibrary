using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;

namespace MyShop
{

    public class SwitchModal : PopupWindow      //这个不能继承PopupWidow ，因为要call base.hide()
    {
        [Invector.vEditorToolbar("UI")]
        public LeanButton button;
        public LeanButton xButton;
        public LeanButton cancelButton;

        private LeanButton[] buttonList;

        protected override void Start()
        {
            base.Start();

            buttonList = GetComponentsInChildren<LeanButton>();

            button.OnClick.AddListener(() =>
            {
                UIManager.Instance.TogglePopUI(PopType.modeFrame, true);
            //base.Hide();        //不toggle main
                HideOnlySelf();

            });
            xButton.OnClick.AddListener(() =>
            {
                Hide();
            });
            cancelButton.OnClick.AddListener(() =>
            {
            //UIManager.Instance.TogglePop(ModalType.modeFrame, true);
                Hide();

            });
        }

        public void EnableButton(bool en)
        {
            foreach (var val in buttonList)
            {
                val.enabled = en;
            }
        }

        //public override void Show()
        //{
        //    base.Show();
        //    UIManager.Instance.ToggleMainPop(false);
        //}

        //public override void Hide()
        //{
        //    base.Hide();

        //    UIManager.Instance.ToggleMainPop(true);
        //}
    }
}