using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;
using Lean.Transition;
using System;

namespace MyShop
{
    public class CircleUI : PopupUI
    {
        [Invector.vEditorToolbar("UI")]
        public LeanButton[] imageArray;
        public float radius = 300;

        //protected Slot target;

        protected override void Start()
        {
            base.Start();

            //target = GetComponentInParent<Slot>();
        }

        public override void Show()
        {
            base.Show();

            CreateCubeAngle30();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void RegistEvent(Action<int> action)
        {
            for (int i = 0; i < imageArray.Length; i++)
            {
                imageArray[i].OnClick.RemoveAllListeners();
                int index = i;  //需要保存i的值
                imageArray[i].OnClick.AddListener(() =>
                {
                    action.Invoke(index);
                });
            }
        }

        public void CreateCubeAngle30()
        {

            //小item的scale ,不是PopUI
            foreach (var val in imageArray)
            {
                val.transform.localScale = Vector3.zero;
            }

            int i = 0;
            int interval = 180 / (imageArray.Length - 1);

            const float duration = 0.5f;
            const float delay = 0.05f;
            for (float angle = 0; angle <= 180; angle += interval, i++)
            {
                float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                if (i < imageArray.Length)
                {
                    imageArray[i].transform.localPosition = new Vector3(x, y, 0);
                    //imageArr[i].enabled = false;

                    int index = i;
                    imageArray[index].DelayTransition(i * delay).JoinTransition().EventTransition(() =>
                    {
                        imageArray[index].transform.localScale = Vector3.one;
                        Dalechn.GameUtils.DampAnimation(imageArray[index].gameObject, duration);
                    });
                }
            }
        }

    }
}
