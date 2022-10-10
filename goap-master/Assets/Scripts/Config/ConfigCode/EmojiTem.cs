using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EmojiTem
{
    protected static Dictionary<string, EmojiTem> msData = new Dictionary<string,EmojiTem>();
    protected static bool msIsInit = false;

    #region Get Template & Dictionary  & List
    public static List<EmojiTem> Lis(params object[] keys)
	{
		Dic();
        string key;
        key = "";

		foreach (object obj in keys)
        {
            key += obj.ToString() + ":";
        }  

        List<EmojiTem> list;
        list = new List<EmojiTem>();

        foreach (KeyValuePair<string, EmojiTem> pair in msData)
        {
            if ((pair.Key.ToString() + ":").StartsWith(key))
            {
                list.Add(pair.Value);
            }
        }
        return list;
	}
       
	 
    public static Dictionary<string, EmojiTem> Dic()
    {
        if (!msIsInit)
        {
            msIsInit = true;

            #region Init Data
            EmojiTem template;
			
			
//.......................................
template = new EmojiTem();
template.key = "Happy";
template.EmojiMSG = "Textures/Arlan Trindade/Free emojis pixel art/emojis-x4-128x128/E2";
template.TextMSG = "挺好吃的;下次还来";
template.CenterMSG = "";
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "Angry";
template.EmojiMSG = "Textures/Arlan Trindade/Free emojis pixel art/emojis-x4-128x128/E27";
template.TextMSG = "等这么久?吃个毛!";
template.CenterMSG = "客人因为长时间等待离开了";
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "Hate";
template.EmojiMSG = "Textures/Arlan Trindade/Free emojis pixel art/emojis-x4-128x128/E47";
template.TextMSG = "真难吃;";
template.CenterMSG = "客人因为难吃而离开";
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "Complain";
template.EmojiMSG = "Textures/Arlan Trindade/Free emojis pixel art/emojis-x4-128x128/E10";
template.TextMSG = "老板你真的不考虑加张桌子吗?;没座位啊;";
template.CenterMSG = "";
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "OrderName";
template.EmojiMSG = "";
template.TextMSG = "我需要";
template.CenterMSG = "";
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "OrderNameStaff";
template.EmojiMSG = "";
template.TextMSG = "好的,";
template.CenterMSG = "";
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "HaveFun";
template.EmojiMSG = "Textures/Arlan Trindade/Free emojis pixel art/emojis-x4-128x128/E15";
template.TextMSG = "";
template.CenterMSG = "";
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "WaitTooLong";
template.EmojiMSG = "Textures/Arlan Trindade/Free emojis pixel art/emojis-x4-128x128/E16";
template.TextMSG = "好多人啊,下次吧";
template.CenterMSG = "";
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "ComplainSlow";
template.EmojiMSG = "";
template.TextMSG = "老板上菜快点啊艹;能快点吗";
template.CenterMSG = "";
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "PasserBy";
template.EmojiMSG = "Textures/Arlan Trindade/Free emojis pixel art/emojis-x4-128x128/E8";
template.TextMSG = "路过~路过";
template.CenterMSG = "";
msData.Add(template.key,template);
            
            #endregion
        }
        return msData;
    }

    public static EmojiTem Tem(params object[] keys)
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

        EmojiTem t;
        if (msData.TryGetValue(key.ToString(), out t))
        {
            return t;
        }
        return null;
    }

    #endregion

    #region member variable
    public string key;
    public string EmojiMSG;
public string TextMSG;
public string CenterMSG;

    #endregion
}

