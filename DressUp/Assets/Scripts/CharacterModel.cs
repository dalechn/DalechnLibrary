using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicaCloth;
using Newtonsoft.Json;

public class CharacterModel : MonoBehaviour
{
    public struct avatarState
    {
        public int groupHandle;     //不穿的时候为0
        public int currentIndex;    //不穿的时候为-1
        public bool canNull;

        public avatarState(avatarState state)
        {
            this.groupHandle = state.groupHandle;
            this.currentIndex = state.currentIndex;

            this.canNull = state.canNull;
        }

        public avatarState(int groupHandle, int currentIndex, bool canNull = false)
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
    public Dictionary<string, avatarState> avatarStatesDict = new Dictionary<string, avatarState>();

    public void Init(string partName)
    {
        int storedIndex = 0;    //需要用json加载,不然默认0
        ScrollViewTem tem = ScrollViewTem.Tem(partName + ":" + storedIndex);

        avatarStatesDict.Add(partName, new avatarState(0, -1, tem.CanNull));

        AddAvatarParts(tem, tem.CanNull ? -1 : storedIndex, tem.CanNull);
    }

    public int GetCurrentIndex(string partName)
    {
        return avatarStatesDict[partName].currentIndex;
    }

    //public void AddPart(string name, ScrollState state)
    //{

    //}

    //public void DetachPart(string name)
    //{

    //}

    public void AddAvatarParts(ScrollViewTem template, int index, bool canNull = false)
    {
        if (template == null)
        {
            return;
        }

        string[] splitParts = template.key.Split(':');
        string partName = splitParts[0];

        //套装清理其他部位的逻辑2. 这部分要先执行,防止套装也自动穿衣服
        Dictionary<string, avatarState> tempDict = new Dictionary<string, avatarState>();
        foreach (var val in avatarStatesDict)
        {
            ScrollViewTem item = ScrollViewTem.Tem(val.Key + ":0"); //判断第一组数据是否有默认位置

            if (!item.CanNull && val.Value.currentIndex == -1 && item.ClearOther != "")
            {
                int handle = LoadAsset(item.PrefabLocation);

                tempDict.Add(val.Key, new avatarState(handle, 0, item.CanNull));
                //avatarStatesDict[val.Key] = new avatarState(handle, 0, item.CanNull);
            }
        }
        foreach (var val in tempDict)
        {
            avatarStatesDict[val.Key] = val.Value;
        }

        //清理当前部位
        avatarState group = new avatarState();
        if (avatarStatesDict.TryGetValue(partName, out group) && group.groupHandle != 0)
        {
            avatar.DetachAvatarParts(group.groupHandle);
            group.Reset();
            avatarStatesDict[partName] = group;
        }

        if (!canNull)
        {
            // 套装清理其他部位的逻辑1.
            if (template.ClearOther != "")
            {
                string[] splitOther = template.ClearOther.Split(';');

                avatarState groupOther = new avatarState();
                List<string> deleteList = new List<string>();
                foreach (var val in splitOther)
                {
                    if (avatarStatesDict.TryGetValue(val, out groupOther) && groupOther.groupHandle != 0)
                    {
                        avatar.DetachAvatarParts(groupOther.groupHandle);
                        deleteList.Add(val);
                    }
                }

                groupOther.Reset();
                foreach (var val in deleteList)
                {
                    avatarStatesDict[val] = groupOther;
                }
            }

            //穿上当前部位
            avatarState state = new avatarState(0, index, template.CanNull);
            state.groupHandle = LoadAsset(template.PrefabLocation);
            avatarStatesDict[partName] = state;
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
