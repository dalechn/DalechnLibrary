using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.ScrollRectItemsAdapter.Classic;

public class DoubleScrollView : ScrollViewBase<DoubleScrollViewTem>
{
    public SingleScrollView firstScrollView;

    protected int currentPartIndex = 0;

    protected override void Awake()
    {
        base.Awake();

        temList = DoubleScrollViewTem.Lis(currentListName);

        // 需要在子滚动条之前初始化
        foreach (var val in temList)
        {
            firstScrollView.Init(val.Name);
        }
    }

    protected override void Start()
    {
        base.Start();

        InitScrollView();

        ResetItems(temList.Count);
        LightBackground();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        firstScrollView.CheckLastSelected(false);
    }

    public override void RunAnimation()
    {
        SmoothScrollTo(currentPartIndex, 0.5f);
    }

    public override void LightBackground()
    {
        foreach (var val in viewsHolders)
        {
            val.backGround.enabled = false;
        }
        if (currentPartIndex >= 0)
        {
            viewsHolders[currentPartIndex].backGround.enabled = true;
        }
        else
        {
            Debug.LogWarning("index 超了");
        }
    }

    protected override void UpdateViewsHolder(CellViewsHolder vh)
    {
        DoubleScrollViewTem template = temList[vh.ItemIndex];

        vh.titleText.text = template.Name;
        //vh.image.sprite = Resources.Load<Sprite>(template.Location + vh.ItemIndex);
        vh.btn.onClick.AddListener(() =>
        {
            if (currentPartIndex != vh.ItemIndex)
            {
                //Debug.Log("click");

                firstScrollView.ChangeList(template.Name);

                currentPartIndex = vh.ItemIndex;

                LightBackground();
            }
        });
    }

    private void InitScrollView()
    {
        string lastSelectedPart = firstScrollView.CheckLastSelected(true);
        currentPartIndex = temList.FindIndex((e) => { return e.Name == lastSelectedPart; });
        if (currentPartIndex < 0)
            currentPartIndex = 0;           //默认为第一个
                                            //Debug.Log(currentPartIndex);

        Dalechn.bl_UpdateManager.RunActionOnce("", Time.deltaTime, () =>
        {
            RunAnimation();
            firstScrollView.RunAnimation();
        });
    }

}

