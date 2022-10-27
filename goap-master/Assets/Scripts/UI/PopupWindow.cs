using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyShop
{

    public class PopupWindow : PopupUI
    {
        [Invector.vEditorToolbar("UI")]
        public bool selfMode;       //ֻ�����Լ�
        public Image dimed;

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

            if(dimed)
            {
                dimed.enabled = true;
            }
        }

        public override void Hide()
        {
            base.Hide();
            if (!selfMode)
            {
                UIManager.Instance.ToggleMainPop(true);
            }

            if (dimed)
            {
                dimed.enabled = false;
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

        public virtual void ShowOnlySelf()
        {
            base.Show();

            if (dimed)
            {
                dimed.enabled = true;
            }
        }

        public virtual void HideOnlySelf()
        {
            base.Hide();


            if (dimed)
            {
                dimed.enabled = false;
            }
        }
    }
}
