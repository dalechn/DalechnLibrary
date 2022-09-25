using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicaCloth;
using Newtonsoft.Json;

public class CharacterModel : MonoBehaviour
{
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

    public MagicaAvatar avatar;

    //[HideInInspector]
    public Dictionary<string, ScrollState> avatarStatesList = new Dictionary<string, ScrollState>();
    //public string currentPart= "Face";    // 最后一次选择的列表名字

    public void Init(string partName)
    {
        ScrollState state = new ScrollState();
        state.groupHandle = 0;
        state.currentIndex = 0;

        avatarStatesList.Add(partName, state);
    }

    public int GetCurrentIndex(string partName)
    {
        return avatarStatesList[partName].currentIndex;
    }

    public void AddAvatarParts(ScrollViewTem template, int index, bool remove = false)
    {
        string[] splitParts = template.key.Split(':');
        string partName = splitParts[0];

        var group = avatarStatesList[partName];

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

        if (!remove)
        {
            GameObject Prefab = (GameObject)Resources.Load(template.PrefabLocation);
            var avatarPartsObject = Instantiate(Prefab);

            state.groupHandle = avatar.Runtime.AddAvatarParts(avatarPartsObject.GetComponent<MagicaAvatarParts>());
        }
        avatarStatesList[partName] = state;

        //Debug.Log(middir);
        //Debug.Log(avatarStatesList[middir]);
    }

    private void OnDestroy()
    {
       string jsonType =  JsonConvert.SerializeObject(avatarStatesList);

    }

    void Start()
    {
        //string jsonType="";
        //avatarStatesList = JsonConvert.DeserializeObject<Dictionary<string, ScrollState>>(jsonType);
    }

}
