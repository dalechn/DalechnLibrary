using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.ScrollRectItemsAdapter.Classic;
using MagicaCloth;

public struct ScrollState
{
    public int groupHandle;
    public int currentIndex;

    public ScrollState(int groupHandle, int currentIndex)
    {
        this.groupHandle = groupHandle;
        this.currentIndex = currentIndex;
    }
}

public class SingleScrollView : ClassicSRIA<CellViewsHolder>
{
    public RectTransform itemPrefab;
    public string currentListName = "Face";

    public MagicaAvatar avatar;

    protected List<ScrollViewTem> kartList;

    //[HideInInspector]
    public Dictionary<string, ScrollState> avatarStatesList = new Dictionary<string, ScrollState>();

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();

        kartList = ScrollViewTem.Lis(currentListName);
        ResetItems(kartList.Count);

        LightBackground();
    }

    protected override CellViewsHolder CreateViewsHolder(int itemIndex)
    {
        var instance = new CellViewsHolder();
        instance.Init(itemPrefab, itemIndex);

        return instance;
    }

    protected override void UpdateViewsHolder(CellViewsHolder vh)
    {
        ScrollViewTem template = kartList[vh.ItemIndex];

        //vh.titleText.text = template.Name;
        vh.image.sprite = Resources.Load<Sprite>(template.Location );
        vh.btn.onClick.AddListener(() =>
        {
            //Debug.Log(avatarStatesList[currentListName].currentIndex);
            if(avatarStatesList[currentListName].currentIndex !=vh.ItemIndex)
            {
                AddAvatarParts(template, vh.ItemIndex);

                LightBackground();
            }
            else if(template.CanNull)
            {
                AddAvatarParts(template, -1,true);

                LightBackground();
            }
        });
    }

    private void LightBackground()
    {
        ScrollState state = avatarStatesList[currentListName];
        foreach (var val in viewsHolders)
        {
            val.backGround.enabled = false;
        }
        if(state.currentIndex>=0)
        {
            viewsHolders[state.currentIndex].backGround.enabled = true;
        }
    }

    public void Init(string valName)
    {
        ScrollState state = new ScrollState();
        state.groupHandle = 0;
        state.currentIndex = 0;

        avatarStatesList.Add(valName, state);

        kartList = ScrollViewTem.Lis(valName);

        AddAvatarParts(kartList[state.currentIndex], state.currentIndex);
    }

    public void AddAvatarParts(ScrollViewTem template,int index,bool remove = false)
    {
        string[] splitdirs = template.key.Split(':');
        string middir = splitdirs[0];

        var group = avatarStatesList[middir];

        ScrollState state = new ScrollState();
        state.currentIndex = index;

        if (group.groupHandle != 0)
        {
            avatar.DetachAvatarParts(group.groupHandle);
            state.groupHandle = 0;

            //if (template.ClearOther!=null)
            //{
            //    string[] splitOther = template.ClearOther.Split(':');

            //    foreach (var val in splitOther)
            //    {
            //        var groupOther = avatarStatesList[val];
            //        avatar.DetachAvatarParts(groupOther.groupHandle);
            //    }
            //}
        }

        if(!remove)
        {
            GameObject Prefab = (GameObject)Resources.Load(template.PrefabLocation);
            var avatarPartsObject = Instantiate(Prefab);

            state.groupHandle = avatar.Runtime.AddAvatarParts(avatarPartsObject.GetComponent<MagicaAvatarParts>());
        }
        avatarStatesList[middir] = state;

        //Debug.Log(middir);
        //Debug.Log(avatarStatesList[middir]);
    }

    public void ChangeList(string valName)
    {
    
        Vector3 originTop = viewport.position ;
        Vector3 destTop = viewport.position + new Vector3(0, -400, 0);
        Dalechn.bl_UpdateManager.RunAction("", 0.3f, (t,r) => {

            viewport.position= Vector3.Lerp(originTop, destTop, t);

            //viewport.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 100, ViewportSize);

            ScrollRectComponent.enabled = false;
        }, () => {

            currentListName = valName;
            kartList = ScrollViewTem.Lis(valName);
            ResetItems(kartList.Count);

            //Refresh();

            LightBackground();

            Dalechn.bl_UpdateManager.RunAction("", 0.3f, (t, r) => {

                viewport.position= Vector3.Lerp(destTop, originTop, t);

                ScrollRectComponent.enabled = true;
            },null, Dalechn.EaseType.SineInOut);
        }, Dalechn.EaseType.SineInOut);
    }

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
