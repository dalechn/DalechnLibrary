using Lean.Common;
using Lean.Touch;
using Lean.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyShop
{

    public class FurnitureSlot : Slot
    {
        //public LeanPlayer player;       //���������Ч������
        //public GameObject dampObject;

        protected override void Start()
        {
            base.Start();

        }

        protected override void OnLeanSelected(LeanSelect s)
        {
            //player.Begin();

            const float waitDuration = 0.5f;
            if (CanClick(gameObject, waitDuration))
            {
                Dalechn.GameUtils.DampAnimation(gameObject, waitDuration);
            }

            if (foodList[0].activeInHierarchy)
            {
                ModeFrame top = UIManager.Instance.GetObj<ModeFrame>(PopType.modeFrame.ToString());
                top.ShowMask(foodList[0].transform);

                foodList[0].SetActive(false);
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}