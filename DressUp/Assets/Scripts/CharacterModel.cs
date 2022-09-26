using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicaCloth;
using Newtonsoft.Json;

public class CharacterModel : MonoBehaviour
{
    public struct ScrollState
    {
        public int groupHandle;     //不穿的时候为0
        public int currentIndex;    //不穿的时候为-1
        public bool canNull;

        public ScrollState(int groupHandle, int currentIndex, bool canNull = false)
        {
            this.groupHandle = groupHandle;
            this.currentIndex = currentIndex;

            this.canNull = canNull;
        }

        public void Reset()
        {
            groupHandle = 0;
            currentIndex = -1;
        }
    }

    public MagicaAvatar avatar;

    //[HideInInspector]
    public Dictionary<string, ScrollState> avatarStatesList = new Dictionary<string, ScrollState>();

    public void Init(string  partName)
    {
        int storedIndex = 0;    //需要用json加载,不然默认0
        ScrollViewTem tem = ScrollViewTem.Tem(partName+":"+ storedIndex);

        avatarStatesList.Add(partName, new ScrollState(0, -1, tem.CanNull));

        AddAvatarParts(tem, tem.CanNull?-1:storedIndex, tem.CanNull);
    }

    public int GetCurrentIndex(string partName)
    {
        return avatarStatesList[partName].currentIndex;
    }


    public void AddAvatarParts(ScrollViewTem template, int index, bool canNull = false)
    {
        if(template == null)
        {
            return;
        }

        string[] splitParts = template.key.Split(':');
        string partName = splitParts[0];

        var group = avatarStatesList[partName];

        ScrollState state = new ScrollState();
        state.currentIndex = index;
        state.canNull = template.CanNull;

        if (group.groupHandle != 0)
        {
            avatar.DetachAvatarParts(group.groupHandle);
            state.groupHandle = 0;
        }

        if (!canNull)
        {
            // 套装清理其他部位的逻辑1.
            if (template.ClearOther != "")
            {
                string[] splitOther = template.ClearOther.Split(';');

                foreach (var val in splitOther)
                {
                    var groupOther = avatarStatesList[val];
                    avatar.DetachAvatarParts(groupOther.groupHandle);
                    groupOther.Reset();
                }
            }

            state.groupHandle = LoadAsset(template.PrefabLocation);
        }

        avatarStatesList[partName] = state;

        //套装清理其他部位的逻辑2.
        foreach (var val in avatarStatesList)
        {
            ScrollViewTem item = ScrollViewTem.Tem(val.Key + ":0"); //判断第一组数据是否有默认位置

            if (!item.CanNull  && val.Value.currentIndex == -1)
            {
                int handle = LoadAsset(item.PrefabLocation);

                avatarStatesList[val.Key] = new ScrollState(handle, 0, item.CanNull);
            }
        }

    }

    // 需要改成assetbundle+对象池
    private int LoadAsset(string location)
    {
        GameObject Prefab = (GameObject)Resources.Load(location);
        var avatarPartsObject = Instantiate(Prefab);

        return avatar.Runtime.AddAvatarParts(avatarPartsObject.GetComponent<MagicaAvatarParts>());
    }

    private void OnDestroy()
    {
        //string jsonType =  JsonConvert.SerializeObject(avatarStatesList);

    }

    void Start()
    {
        //string jsonType="";
        //avatarStatesList = JsonConvert.DeserializeObject<Dictionary<string, ScrollState>>(jsonType);
    }

}
