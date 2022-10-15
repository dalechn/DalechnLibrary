﻿using System;
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
template.Location = "Textures/FoodIcon/512/hotdog";
template.PrefabLocation = "hotdog";
template.Owned = true;
template.Need = "hotdog";
template.NeedFurniture = "Furniture1";
template.Time = 4.0f;
template.Price = 400.0f;
template.HavePlate = true;
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "burger";
template.Location = "Textures/FoodIcon/512/burger";
template.PrefabLocation = "burger";
template.Owned = true;
template.Need = "burger";
template.NeedFurniture = "Furniture2";
template.Time = 5.0f;
template.Price = 500.0f;
template.HavePlate = true;
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "popcorn";
template.Location = "Textures/FoodIcon/512/popcorn";
template.PrefabLocation = "popcorn";
template.Owned = true;
template.Need = "popcorn";
template.NeedFurniture = "Furniture3";
template.Time = 2.0f;
template.Price = 200.0f;
template.HavePlate = false;
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "taco";
template.Location = "Textures/FoodIcon/512/taco";
template.PrefabLocation = "taco";
template.Owned = true;
template.Need = "taco";
template.NeedFurniture = "Furniture4";
template.Time = 2.0f;
template.Price = 200.0f;
template.HavePlate = false;
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "cola";
template.Location = "Textures/FoodIcon/512/cola";
template.PrefabLocation = "cola";
template.Owned = true;
template.Need = "cola";
template.NeedFurniture = "Furniture5";
template.Time = 4.0f;
template.Price = 400.0f;
template.HavePlate = false;
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "beer";
template.Location = "Textures/FoodIcon/512/beer";
template.PrefabLocation = "beer";
template.Owned = true;
template.Need = "beer";
template.NeedFurniture = "Furniture6";
template.Time = 3.0f;
template.Price = 300.0f;
template.HavePlate = false;
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "wine";
template.Location = "Textures/FoodIcon/512/wine";
template.PrefabLocation = "wine";
template.Owned = true;
template.Need = "wine";
template.NeedFurniture = "Furniture7";
template.Time = 2.0f;
template.Price = 200.0f;
template.HavePlate = false;
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "hotdogBurger";
template.Location = "Textures/FoodIcon/512/burger";
template.PrefabLocation = "burger";
template.Owned = true;
template.Need = "hotdog;burger";
template.NeedFurniture = "";
template.Time = 0.0f;
template.Price = 0.0f;
template.HavePlate = true;
msData.Add(template.key,template);
//.......................................
template = new FoodTem();
template.key = "hotdogBurgerpoPcorn";
template.Location = "Textures/FoodIcon/512/popcorn";
template.PrefabLocation = "popcorn";
template.Owned = true;
template.Need = "hotdog;burger;popcorn";
template.NeedFurniture = "";
template.Time = 0.0f;
template.Price = 0.0f;
template.HavePlate = false;
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
public string PrefabLocation;
public bool Owned;
public string Need;
public string NeedFurniture;
public float Time;
public float Price;
public bool HavePlate;

    #endregion
}

