using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.ScrollRectItemsAdapter.Classic;

public class SingleScrollView : ScrollViewBase<ScrollViewTem>
{
    public CharacterModel model;

    public string CheckLastSelected(bool load)
    {
        if (load)
        {
            string lastSelectedPart = PlayerPrefs.GetString(AppConst.LAST_SELECTED_PART, currentListName);
            currentListName = lastSelectedPart;

            return lastSelectedPart;
        }
        else
        {
            PlayerPrefs.SetString(AppConst.LAST_SELECTED_PART, currentListName);
        }
        return null;
    }

    protected override void Start()
    {
        base.Start();

        //Debug.Log(currentListName);

        ResetItem(currentListName);
    }

    protected override void UpdateViewsHolder(CellViewsHolder vh)
    {
        ScrollViewTem template = temList[vh.ItemIndex];

        //vh.titleText.text = template.Name;
        vh.image.sprite = Resources.Load<Sprite>(template.Location); //todo:改用assetbundle?
        vh.btn.onClick.AddListener(() =>
        {
            if (model.GetCurrentIndex(currentListName) != vh.ItemIndex)
            {
                model.AddAvatarParts(template, vh.ItemIndex);
            }
            else if (template.CanNull)
            {
                model.AddAvatarParts(template, -1, true);
            }

            LightBackground();
        });
    }

    public void Init(string partName)
    {
        model.Init(partName);
    }

    public void ChangeList(string partName)
    {
        const int dis = -500;

        Dalechn.bl_UpdateManager.RunAction("", 0.3f, (t, r) =>
        {
            viewport.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, Mathf.Lerp(0, dis, t), ViewportSize);
            ScrollRectComponent.enabled = false;

        }, () =>
        {
            currentListName = partName;
            ResetItem(partName);

            int index = model.GetCurrentIndex(currentListName);
            if (index < 0)
                index = 0;      //没有装扮的时候的处理
            SmoothScrollTo(index, 0);

            Dalechn.bl_UpdateManager.RunAction("", 0.3f, (t, r) =>
            {
                viewport.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, Mathf.Lerp(dis, 0, t), ViewportSize);
                ScrollRectComponent.enabled = true;

            }, null, Dalechn.EaseType.SineInOut);
        }, Dalechn.EaseType.SineInOut);
    }


    public override void LightBackground()
    {
        foreach (var val in viewsHolders)
        {
            val.backGround.enabled = false;
        }

        int currentIndex = model.GetCurrentIndex(currentListName);
        if (currentIndex >= 0)
        {
            viewsHolders[currentIndex].backGround.enabled = true;
        }
        else
        {
            Debug.Log("没有装扮 index :" + currentListName);
        }
    }


    public override void RunAnimation()
    {
        SmoothScrollTo(model.GetCurrentIndex(currentListName), 0.5f);
    }

    protected void ResetItem(string partName)
    {
        temList = ScrollViewTem.Lis(partName);
        ResetItems(temList.Count);

        LightBackground();
    }
}

public abstract class ScrollViewBase<Tem> : ClassicSRIA<CellViewsHolder>
{
    public RectTransform itemPrefab;
    public string currentListName = "Dress";

    protected List<Tem> temList;

    protected override CellViewsHolder CreateViewsHolder(int itemIndex)
    {
        var instance = new CellViewsHolder();
        instance.Init(itemPrefab, itemIndex);

        return instance;
    }

    public virtual void RunAnimation() { }
    public virtual void LightBackground() { }
    //protected virtual void LightBackground() { }
}

public class CellViewsHolder : CAbstractViewsHolder
{
    public Text titleText;
    public Image image;
    public Button btn;
    public Image backGround;

    public override void CollectViews()
    {
        base.CollectViews();

        titleText = root.Find("TitleText").GetComponent<Text>();
        image = root.Find("Image").GetComponent<Image>();
        btn = image.gameObject.GetComponent<Button>();

        backGround = root.Find("BackGround").GetComponent<Image>();
    }
}
