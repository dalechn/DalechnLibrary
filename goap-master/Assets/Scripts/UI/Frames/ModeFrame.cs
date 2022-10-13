using Lean.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;

namespace MyShop
{

    public class ModeFrame : PopupWindow
    {
        [Invector.vEditorToolbar("UI")]
        public List<Image> slotImage;       //确保size要>=最大的小食物数量
        public List<GameObject> imageOBJ;
        public List<Image> slotMask;

        private LeanButton[] buttonList;

        public LeanChase chese;

        protected override void Start()
        {
            base.Start();

            buttonList = GetComponentsInChildren<LeanButton>();
        }

        public override void Show()
        {
            base.Show();
            //UIManager.Instance.ToggleMainPop(false);

            // 这是上面那个关闭按钮.
            UIManager.Instance.TogglePopUI(PopType.topFrame, true);

            // 动态注册关闭事件,因为可能被多个frame使用,每个frame不一样
            TopFrame top = UIManager.Instance.GetObj<TopFrame>(PopType.topFrame.ToString());

            top.RegistClick(() =>
            {
                Hide();

                chese.enabled = false;
                ShopInfo.Instance?.HandleOrder(true);       //重新加入队列,
            });

            if (ShopInfo.Instance)
            {

                ShopInfo.Instance?.HandleOrder(false);      //手动操作

                int count = ShopInfo.Instance.CurrentHandleOrder.foodList.Count;

                for (int i = 0; i < slotImage.Count; i++)
                {
                    if (i < count)
                    {
                        imageOBJ[i].SetActive(true);

                        string location = ShopInfo.Instance.CurrentHandleOrder.foodList[i].subFoodSpriteLocation;
                        slotImage[i].sprite = Resources.Load<Sprite>(location);
                        slotMask[i].sprite = Resources.Load<Sprite>(location);
                        slotMask[i].fillAmount = 0;

                        buttonList[i].OnClick.RemoveAllListeners();
                        Transform pos = ShopInfo.Instance.CurrentHandleOrder.foodList[i].foodPosition;
                        buttonList[i].OnClick.AddListener(() =>         //注册镜头的事件
                        {
                            chese.enabled = true;
                            chese.Destination = pos;
                        });
                    }
                    else
                    {
                        imageOBJ[i].SetActive(false);           //超出的部分不显示,因为总量固定
                                                                //imageOBJ[i].transform.localScale = Vector3.zero         //..这样写的话就不能自动resize了
                    }

                }

                chese.enabled = true;
                chese.Destination = ShopInfo.Instance.CurrentHandleOrder.foodList[0].foodPosition;  //镜头移动到第一个位置,就不做中心点计算了
            }
        }

        int currentTimes = 0;
        public void ShowMask(Transform tr)
        {
            if (ShopInfo.Instance && ShopInfo.Instance.CurrentHandleOrder != null)
            {
                Order order = ShopInfo.Instance.CurrentHandleOrder;
                if (slotMask.Count > 0)
                {
                    int index = order.foodList.FindIndex(e => { return e.tagPosition == tr; });

                    //int times = currentTimes;
                    Dalechn.bl_UpdateManager.RunAction("", 1.0f, (t, r) =>
                    {
                        slotMask[index].fillAmount = t;
                    }, () =>
                    {
                        currentTimes++;
                        if (currentTimes == order.foodList.Count)
                        {
                            Hide();

                            chese.enabled = false;
                            ShopInfo.Instance.HandleOrder(true, true);      //订单结束

                            currentTimes = 0;   //重置点击次数
                        }
                    });
                }
            }
        }

        protected void Update()
        {
        }

        public override void Hide()
        {
            base.Hide();
            //UIManager.Instance.ToggleMainPop(true);

            UIManager.Instance.TogglePopUI(PopType.topFrame, false);

        }
    }
}