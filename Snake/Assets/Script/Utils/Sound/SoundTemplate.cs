using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SoundTemplate
{
    protected static Dictionary<string, SoundTemplate> msData = new Dictionary<string,SoundTemplate>();
    protected static bool msIsInit = false;

    #region Get Template & Dictionary  & List
    public static List<SoundTemplate> Lis(params object[] keys)
	{
		Dic();
        string key;
        key = "";

		foreach (object obj in keys)
        {
            key += obj.ToString() + ":";
        }  

        List<SoundTemplate> list;
        list = new List<SoundTemplate>();

        foreach (KeyValuePair<string, SoundTemplate> pair in msData)
        {
            if ((pair.Key.ToString() + ":").StartsWith(key))
            {
                list.Add(pair.Value);
            }
        }
        return list;
	}
       
	 
    public static Dictionary<string, SoundTemplate> Dic()
    {
        if (!msIsInit)
        {
            msIsInit = true;

            #region Init Data
            SoundTemplate template;
			
			
//.......................................
template = new SoundTemplate();
template.key = "Common:rolling";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "car-engine-rolling";
template.Delay = 0.0f;
template.IsLoop = true;
template.Volume = 1.0f;
template.MinDis = 1.0f;
template.MaxDis = 500.0f;
msData.Add(template.key,template);
            
            #endregion
        }
        return msData;
    }

    public static SoundTemplate Tem(params object[] keys)
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

        SoundTemplate t;
        if (msData.TryGetValue(key.ToString(), out t))
        {
            return t;
        }
        return null;
    }

    #endregion

    #region member variable
    public string key;
    public string MutexSoundID;
public string MutexAndStopSoundID;
public string Name;
public float Delay;
public bool IsLoop;
public float Volume;
public float MinDis;
public float MaxDis;

    #endregion
}

