using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ScrollViewTem
{
    protected static Dictionary<string, ScrollViewTem> msData = new Dictionary<string,ScrollViewTem>();
    protected static bool msIsInit = false;

    #region Get Template & Dictionary  & List
    public static List<ScrollViewTem> Lis(params object[] keys)
	{
		Dic();
        string key;
        key = "";

		foreach (object obj in keys)
        {
            key += obj.ToString() + ":";
        }  

        List<ScrollViewTem> list;
        list = new List<ScrollViewTem>();

        foreach (KeyValuePair<string, ScrollViewTem> pair in msData)
        {
            if ((pair.Key.ToString() + ":").StartsWith(key))
            {
                list.Add(pair.Value);
            }
        }
        return list;
	}
       
	 
    public static Dictionary<string, ScrollViewTem> Dic()
    {
        if (!msIsInit)
        {
            msIsInit = true;

            #region Init Data
            ScrollViewTem template;
			
			
//.......................................
template = new ScrollViewTem();
template.key = "Face:0";
template.PrefabLocation = "AvatarParts/Face/Face_1";
template.Location = "Textures/KartUI/Kart0";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Face:1";
template.PrefabLocation = "AvatarParts/Face/Face_2";
template.Location = "Textures/KartUI/Kart1";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Face:2";
template.PrefabLocation = "AvatarParts/Face/Face_3";
template.Location = "Textures/KartUI/Kart2";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Face:3";
template.PrefabLocation = "AvatarParts/Face/Face_4";
template.Location = "Textures/KartUI/Kart3";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Face:4";
template.PrefabLocation = "AvatarParts/Face/Face_5";
template.Location = "Textures/KartUI/Kart4";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Hair:0";
template.PrefabLocation = "AvatarParts/Hair/Hair_1";
template.Location = "Textures/KartUI/Kart0";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Hair:1";
template.PrefabLocation = "AvatarParts/Hair/Hair_2";
template.Location = "Textures/KartUI/Kart1";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Hair:2";
template.PrefabLocation = "AvatarParts/Hair/Hair_3";
template.Location = "Textures/KartUI/Kart2";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Hair:3";
template.PrefabLocation = "AvatarParts/Hair/Hair_4";
template.Location = "Textures/KartUI/Kart3";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Hair:4";
template.PrefabLocation = "AvatarParts/Pants/Hair_5";
template.Location = "Textures/KartUI/Kart4";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Pants:0";
template.PrefabLocation = "AvatarParts/Pants/Pants_1";
template.Location = "Textures/KartUI/Kart0";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "Suit;";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Pants:1";
template.PrefabLocation = "AvatarParts/Pants/Pants_2";
template.Location = "Textures/KartUI/Kart1";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "Suit;";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Pants:2";
template.PrefabLocation = "AvatarParts/Pants/Pants_3";
template.Location = "Textures/KartUI/Kart2";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "Suit;";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Pants:3";
template.PrefabLocation = "AvatarParts/Pants/Pants_4";
template.Location = "Textures/KartUI/Kart3";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "Suit;";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Pants:4";
template.PrefabLocation = "AvatarParts/Pants/Pants_5";
template.Location = "Textures/KartUI/Kart4";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "Suit;";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Shoes:0";
template.PrefabLocation = "AvatarParts/Shoes/Shoes_1";
template.Location = "Textures/KartUI/Kart0";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Shoes:1";
template.PrefabLocation = "AvatarParts/Shoes/Shoes_2";
template.Location = "Textures/KartUI/Kart1";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Shoes:2";
template.PrefabLocation = "AvatarParts/Shoes/Shoes_3";
template.Location = "Textures/KartUI/Kart2";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Shoes:3";
template.PrefabLocation = "AvatarParts/Shoes/Shoes_4";
template.Location = "Textures/KartUI/Kart3";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Shoes:4";
template.PrefabLocation = "AvatarParts/Shoes/Shoes_5";
template.Location = "Textures/KartUI/Kart4";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Top:0";
template.PrefabLocation = "AvatarParts/Skirt/Skirt_1";
template.Location = "Textures/KartUI/Kart0";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "Suit;";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Top:1";
template.PrefabLocation = "AvatarParts/Skirt/Skirt_2";
template.Location = "Textures/KartUI/Kart1";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "Suit;";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Top:2";
template.PrefabLocation = "AvatarParts/Skirt/Skirt_3";
template.Location = "Textures/KartUI/Kart2";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "Suit;";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Top:3";
template.PrefabLocation = "AvatarParts/Skirt/Skirt_4";
template.Location = "Textures/KartUI/Kart3";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "Suit;";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Top:4";
template.PrefabLocation = "AvatarParts/Skirt/Skirt_5";
template.Location = "Textures/KartUI/Kart4";
template.Owned = true;
template.CanNull = false;
template.ClearOther = "Suit;";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Coat:0";
template.PrefabLocation = "AvatarParts/Skirt2/Skirt2_1";
template.Location = "Textures/KartUI/Kart0";
template.Owned = true;
template.CanNull = true;
template.ClearOther = "Suit";
msData.Add(template.key,template);
//.......................................
template = new ScrollViewTem();
template.key = "Suit:0";
template.PrefabLocation = "AvatarParts/Suit/Suit_1";
template.Location = "Textures/KartUI/Kart0";
template.Owned = true;
template.CanNull = true;
template.ClearOther = "Pants;Top;Coat";
msData.Add(template.key,template);
            
            #endregion
        }
        return msData;
    }

    public static ScrollViewTem Tem(params object[] keys)
    {
        Dic();

        StringBuilder key = new StringBuilder(keys[0].ToString());
        if (keys.Length > 1)
        {
            for (int i = 1; i < keys.Length; i++)
            {
                key.Append(":").Append(keys[i].ToString());
            }
        }

        ScrollViewTem t;
        if (msData.TryGetValue(key.ToString(), out t))
        {
            return t;
        }
        return null;
    }

    #endregion

    #region member variable
    public string key;
    public string PrefabLocation;
public string Location;
public bool Owned;
public bool CanNull;
public string ClearOther;

    #endregion
}

