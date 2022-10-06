using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FoodTem
{
    protected static Dictionary<string, FoodTem> msData = new Dictionary<string,FoodTem>();
    protected static bool msIsInit = false;

    #region Get Template & Dictionary  & List
    public static List<FoodTem> Lis(params object[] keys)
	{
		Dic();
        string key;
        key = "";

		foreach (object obj in keys)
        {
            key += obj.ToString() + ":";
        }  

        List<FoodTem> list;
        list = new List<FoodTem>();

        foreach (KeyValuePair<string, FoodTem> pair in msData)
        {
            if ((pair.Key.ToString() + ":").StartsWith(key))
            {
                list.Add(pair.Value);
            }
        }
        return list;
	}
       
	 
    public static Dictionary<string, FoodTem> Dic()
    {
        if (!msIsInit)
        {
            msIsInit = true;

            #region Init Data
            FoodTem template;
			
			
//.......................................
template = new FoodTem();
template.key = "hotdog";
template.Location = "FoodIcon/512/hotdog";
template.Owned = true;
template.Need = "Furniture1";
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "burger";
template.Location = "FoodIcon/512/burger";
template.Owned = true;
template.Need = "Furniture1;Furniture2";
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "popcorn";
template.Location = "FoodIcon/512/popcorn";
template.Owned = true;
template.Need = "Furniture1;Furniture2;Furniture3";
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "taco";
template.Location = "FoodIcon/512/taco";
template.Owned = true;
template.Need = "Furniture1;Furniture4";
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "cola";
template.Location = "FoodIcon/512/cola";
template.Owned = true;
template.Need = "Furniture2;Furniture5";
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "beer";
template.Location = "FoodIcon/512/beer";
template.Owned = true;
template.Need = "Furniture3;Furniture6";
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "wine";
template.Location = "FoodIcon/512/wine";
template.Owned = true;
template.Need = "Furniture4;Furniture7";
msData.Add(template.key,template);
            
            #endregion
        }
        return msData;
    }

    public static FoodTem Tem(params object[] keys)
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

        FoodTem t;
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
public bool Owned;
public string Need;

    #endregion
}

