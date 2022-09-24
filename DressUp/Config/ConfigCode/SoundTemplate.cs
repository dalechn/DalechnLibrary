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
template.key = "Common:button";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "button";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:unlock";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "unlock";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:coin";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "coin";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:over";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "over";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:digger";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "digger";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:die";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "die";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:release";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "bubble";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:releaseLight";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "releaseLight";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:popup";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "popup";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:close";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "close";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:select";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "select";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:skillBubble";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "skillBubble";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:skillCoin";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "skillCoin";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
msData.Add(template.key,template);
//.......................................
template = new SoundTemplate();
template.key = "Common:skillFire";
template.MutexSoundID = "";
template.MutexAndStopSoundID = "";
template.Name = "skillFire";
template.Delay = 0.0f;
template.IsLoop = false;
template.Volume = 1.0f;
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

    #endregion
}

