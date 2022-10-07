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
template.Location = "Textures/Arlan Trindade/Free emojis pixel art/emojis-x4-128x128/E2";
template.IsText = false;
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "Angry";
template.Location = "Textures/Arlan Trindade/Free emojis pixel art/emojis-x4-128x128/E27";
template.IsText = false;
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "Hate";
template.Location = "Textures/Arlan Trindade/Free emojis pixel art/emojis-x4-128x128/E47";
template.IsText = false;
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "Complain";
template.Location = "老板你真的不考虑加张桌子吗?";
template.IsText = true;
msData.Add(template.key,template);
//.......................................
template = new EmojiTem();
template.key = "OrderName";
template.Location = "我需要";
template.IsText = true;
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
    public string Location;
public bool IsText;

    #endregion
}

