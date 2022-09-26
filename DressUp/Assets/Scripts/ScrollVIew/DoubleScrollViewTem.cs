using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DoubleScrollViewTem
{
    protected static Dictionary<string, DoubleScrollViewTem> msData = new Dictionary<string,DoubleScrollViewTem>();
    protected static bool msIsInit = false;

    #region Get Template & Dictionary  & List
    public static List<DoubleScrollViewTem> Lis(params object[] keys)
	{
		Dic();
        string key;
        key = "";

		foreach (object obj in keys)
        {
            key += obj.ToString() + ":";
        }  

        List<DoubleScrollViewTem> list;
        list = new List<DoubleScrollViewTem>();

        foreach (KeyValuePair<string, DoubleScrollViewTem> pair in msData)
        {
            if ((pair.Key.ToString() + ":").StartsWith(key))
            {
                list.Add(pair.Value);
            }
        }
        return list;
	}
       
	 
    public static Dictionary<string, DoubleScrollViewTem> Dic()
    {
        if (!msIsInit)
        {
            msIsInit = true;

            #region Init Data
            DoubleScrollViewTem template;
			
			
//.......................................
template = new DoubleScrollViewTem();
template.key = "Dress:0";
template.Name = "Face";
template.Location = "Textures/KartUI/Kart0";
template.Owned = true;
msData.Add(template.key,template);
//.......................................
template = new DoubleScrollViewTem();
template.key = "Dress:1";
template.Name = "Hair";
template.Location = "Textures/KartUI/Kart1";
template.Owned = true;
msData.Add(template.key,template);
//.......................................
template = new DoubleScrollViewTem();
template.key = "Dress:2";
template.Name = "Pants";
template.Location = "Textures/KartUI/Kart2";
template.Owned = true;
msData.Add(template.key,template);
//.......................................
template = new DoubleScrollViewTem();
template.key = "Dress:3";
template.Name = "Top";
template.Location = "Textures/KartUI/Kart3";
template.Owned = true;
msData.Add(template.key,template);
//.......................................
template = new DoubleScrollViewTem();
template.key = "Dress:5";
template.Name = "Coat";
template.Location = "Textures/KartUI/Kart4";
template.Owned = true;
msData.Add(template.key,template);
//.......................................
template = new DoubleScrollViewTem();
template.key = "Dress:6";
template.Name = "Suit";
template.Location = "Textures/KartUI/Kart4";
template.Owned = true;
msData.Add(template.key,template);
//.......................................
template = new DoubleScrollViewTem();
template.key = "Dress:4";
template.Name = "Shoes";
template.Location = "Textures/KartUI/Kart4";
template.Owned = true;
msData.Add(template.key,template);
            
            #endregion
        }
        return msData;
    }

    public static DoubleScrollViewTem Tem(params object[] keys)
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

        DoubleScrollViewTem t;
        if (msData.TryGetValue(key.ToString(), out t))
        {
            return t;
        }
        return null;
    }

    #endregion

    #region member variable
    public string key;
    public string Name;
public string Location;
public bool Owned;

    #endregion
}

