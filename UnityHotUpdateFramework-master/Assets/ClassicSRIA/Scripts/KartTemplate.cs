using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KartTemplate
{
    protected static Dictionary<string, KartTemplate> msData = new Dictionary<string, KartTemplate>();
    protected static bool msIsInit = false;

    #region Get Template & Dictionary  & List
    public static List<KartTemplate> Lis(params object[] keys)
    {
        Dic();
        string key;
        key = "";

        foreach (object obj in keys)
        {
            key += obj.ToString() + ":";
        }

        List<KartTemplate> list;
        list = new List<KartTemplate>();

        foreach (KeyValuePair<string, KartTemplate> pair in msData)
        {
            if ((pair.Key.ToString() + ":").StartsWith(key))
            {
                list.Add(pair.Value);
            }
        }
        return list;
    }


    public static Dictionary<string, KartTemplate> Dic()
    {
        if (!msIsInit)
        {
            msIsInit = true;

            #region Init Data
            KartTemplate template;


            //.......................................
            template = new KartTemplate();
            template.key = "Kart:0";
            template.Name = "motuo";
            template.Type = 0;
            template.Location = "Textures/KartUI/Kart";
            template.Owned = true;
            msData.Add(template.key, template);
            //.......................................
            template = new KartTemplate();
            template.key = "Kart:1";
            template.Name = "kart";
            template.Type = 0;
            template.Location = "Textures/KartUI/Kart";
            template.Owned = true;
            msData.Add(template.key, template);
            //.......................................
            template = new KartTemplate();
            template.key = "Kart:2";
            template.Name = "hover";
            template.Type = 0;
            template.Location = "Textures/KartUI/Kart";
            template.Owned = true;
            msData.Add(template.key, template);
            //.......................................
            template = new KartTemplate();
            template.key = "Kart:3";
            template.Name = "anhei";
            template.Type = 0;
            template.Location = "Textures/KartUI/Kart";
            template.Owned = true;
            msData.Add(template.key, template);

            #endregion
        }
        return msData;
    }

    public static KartTemplate Tem(params object[] keys)
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

        KartTemplate t;
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
    public int Type;
    public string Location;
    public bool Owned;

    #endregion
}

