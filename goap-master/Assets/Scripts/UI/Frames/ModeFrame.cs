using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeFrame : PopupWindow
{
    [Invector.vEditorToolbar("UI")]
    public List<Image> slotImage;       //确保size要>=最大的小食物数量
    public List<GameObject> imageOBJ;

    protected override void Start()
    {
        base.Start();

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
                imageOBJ[i].SetActive(false);           //超出的部分不显示,因为总量固定
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
