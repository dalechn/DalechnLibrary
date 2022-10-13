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
        public List<Image> slotImage;       //ȷ��sizeҪ>=����Сʳ������
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

            // ���������Ǹ��رհ�ť.
            UIManager.Instance.TogglePopUI(PopType.topFrame, true);

            // ��̬ע��ر��¼�,��Ϊ���ܱ����frameʹ��,ÿ��frame��һ��
            TopFrame top = UIManager.Instance.GetObj<TopFrame>(PopType.topFrame.ToString());

            top.RegistClick(() =>
            {
                Hide();

                chese.enabled = false;
                ShopInfo.Instance?.HandleOrder(true);       //���¼������,
            });

            if (ShopInfo.Instance)
            {

                ShopInfo.Instance?.HandleOrder(false);      //�ֶ�����

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
                        buttonList[i].OnClick.AddListener(() =>         //ע�᾵ͷ���¼�
                        {
                            chese.enabled = true;
                            chese.Destination = pos;
                        });
                    }
                    else
                    {
                        imageOBJ[i].SetActive(false);           //�����Ĳ��ֲ���ʾ,��Ϊ�����̶�
                                                                //imageOBJ[i].transform.localScale = Vector3.zero         //..����д�Ļ��Ͳ����Զ�resize��
                    }

                }

                chese.enabled = true;
                chese.Destination = ShopInfo.Instance.CurrentHandleOrder.foodList[0].foodPosition;  //��ͷ�ƶ�����һ��λ��,�Ͳ������ĵ������
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
                            ShopInfo.Instance.HandleOrder(true, true);      //��������

                            currentTimes = 0;   //���õ������
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