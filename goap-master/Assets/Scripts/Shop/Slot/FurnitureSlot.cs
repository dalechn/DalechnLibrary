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
        //public LeanPlayer player;       //不用这个了效果不好
        //public GameObject dampObject;

        protected override void Start()
        {
            base.Start();

        }

        protected override void OnLeanSelected(LeanSelect s)
        {
            //player.Begin();

            //const float waitDuration = 0.5f;
            //if (CanClick(gameObject, waitDuration))
            //{
            //    Dalechn.GameUtils.DampAnimation(gameObject, waitDuration);
            //}
            base.OnLeanSelected(s);

            //用标记是否被打开来判断是否进入玩家操作模式
            if (foodList[0].activeInHierarchy)
            {
                ModeFrame top = UIManager.Instance.GetObj<ModeFrame>(PopType.modeFrame.ToString());
                top.ShowMask(foodList[0].transform);

                foodList[0].SetActive(false);
            }
        }

        //public Transform GetTagPosition(GameObject obj)
        //{
        //    int index = slotList.FindIndex(e => { return e == obj; });

        //    if (index != -1)
        //    {
        //        return foodList[index].transform;
        //    }

        //    return null;
        //}


        // Update is called once per frame
        void Update()
        {

        }
    }
}