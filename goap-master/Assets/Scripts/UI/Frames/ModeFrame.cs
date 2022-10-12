using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeFrame : PopupWindow
{
    [Invector.vEditorToolbar("UI")]
    public List<Image> slotImage;       //ȷ��sizeҪ>=����Сʳ������
    public List<GameObject> imageOBJ;

    protected override void Start()
    {
        base.Start();

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
        });
        
        ShopInfo.Instance?.HandleOrder( false);

        int count = ShopInfo.Instance.CurrentHandleOrder.foodList.Count;

        for (int i=0;i< slotImage.Count;i++)
        {
            if (i< count)
            {
                string location = ShopInfo.Instance.CurrentHandleOrder.foodList[i].subFoodSpriteLocation;
                slotImage[i].sprite = Resources.Load<Sprite>(location);
            }
            else
            {
                imageOBJ[i].SetActive(false);           //�����Ĳ��ֲ���ʾ,��Ϊ�����̶�
            }
        }
    }

    public override void Hide()
    {
        base.Hide();
        //UIManager.Instance.ToggleMainPop(true);

        UIManager.Instance.TogglePopUI(PopType.topFrame, false);

        ShopInfo.Instance?.HandleOrder( true);
    }
}
