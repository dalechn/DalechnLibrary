using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.ScrollRectItemsAdapter.Classic;

public class DoubleScrollView : ClassicSRIA<CellViewsHolder>
{
    public SingleScrollView firstScrollView;

    public RectTransform itemPrefab;
    public string currentListName = "Dress";

    protected List<DoubleScrollViewTem> kartList;
    protected int currentIndex = 0;

    protected override void Awake()
    {
        base.Awake();

        kartList = DoubleScrollViewTem.Lis(currentListName);

        // 需要在子滚动条之前初始化
        foreach (var val in kartList)
        {
            firstScrollView.Init(val.Name);
        }
    }


    protected override void Start()
    {
        base.Start();

        ResetItems(kartList.Count);

        LightBackground();
    }

    protected override CellViewsHolder CreateViewsHolder(int itemIndex)
    {
        var instance = new CellViewsHolder();
        instance.Init(itemPrefab, itemIndex);

        return instance;
    }

    private void LightBackground()
    {
        foreach (var val in viewsHolders)
        {
            val.backGround.enabled = false;
        }

        viewsHolders[currentIndex].backGround.enabled = true;
    }

    protected override void UpdateViewsHolder(CellViewsHolder vh)
    {
        DoubleScrollViewTem template = kartList[vh.ItemIndex];

        vh.titleText.text = template.Name;
        //vh.image.sprite = Resources.Load<Sprite>(template.Location + vh.ItemIndex);
        vh.btn.onClick.AddListener(() =>
        {
            if(currentIndex !=vh.ItemIndex)
            {
                //Debug.Log("click");

                firstScrollView.ChangeList(template.Name);

                currentIndex = vh.ItemIndex;

                LightBackground();
            }
         
        });
    }

}

