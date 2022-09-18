using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.ScrollRectItemsAdapter.Classic;
using LuaFramework;
using LuaInterface;

public class SwitchCar : ClassicSRIA<CellViewsHolder>
{
    public RectTransform itemPrefab;
    public Button closeBtn;

    //private List<KartTemplate> kartList;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        LuaManager luaMgr = LuaHelper.GetLuaManager();
        if (null != luaMgr)
        {
            LuaFunction func = luaMgr.GetFunction("ScrollViewTest.GetConfigLength", false);
            func.BeginPCall();
            func.Push(this);
            func.PCall();
            double val = func.CheckNumber();

            func.EndPCall();

            ResetItems((int)val);
        }

        //kartList = KartTemplate.Lis("Kart");
        //ResetItems(kartList.Count);
    }

    protected override CellViewsHolder CreateViewsHolder(int itemIndex)
    {
        var instance = new CellViewsHolder();
        instance.Init(itemPrefab, itemIndex);

        return instance;
    }


    protected override void UpdateViewsHolder(CellViewsHolder vh)
    {
        LuaManager luaMgr = LuaHelper.GetLuaManager();
        if (null != luaMgr)
        {
            LuaFunction func = luaMgr.GetFunction("ScrollViewTest.GetConfig", false);
            func.BeginPCall();
            func.Push(vh.ItemIndex+1);
            func.PCall();
            LuaTable tab = func.CheckLuaTable();

            func.EndPCall();

            vh.titleText.text = (string)tab["name"];
            vh.image.sprite = Resources.Load<Sprite>((string)tab["location"] + vh.ItemIndex);
            vh.btn.onClick.AddListener(() =>
            {

                Debug.Log("click");

                closeBtn.onClick.Invoke();
            });

            tab.Dispose();
        }

        //KartTemplate template = kartList[vh.ItemIndex];

        //vh.titleText.text = template.Name;
        //vh.image.sprite = Resources.Load<Sprite>(template.Location + vh.ItemIndex);
        //vh.btn.onClick.AddListener(() =>
        //{

        //    Debug.Log("click");

        //    closeBtn.onClick.Invoke();
        //});
    }

}


public class CellViewsHolder : CAbstractViewsHolder
{
    public Text titleText;
    public Image image;
    public Button btn;

    public override void CollectViews()
    {
        base.CollectViews();

        titleText = root.Find("TitleText").GetComponent<Text>();
        image = root.Find("Image").GetComponent<Image>();
        btn = image.gameObject.GetComponent<Button>();
    }
}
