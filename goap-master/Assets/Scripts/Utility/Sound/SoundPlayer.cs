using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundPlayer 
{
    #region Play Sound3D

    public static void PlaySoundEffect3D(GameObject go, string soundKey, bool isloop,
                                         float minDis, float maxDis, float volume,
                                         float delay, int audioSourceIndex = 0)
    {
        AudioSource source;
        AudioSource[] sourceArray = go.GetComponents<AudioSource>();
        if (sourceArray.Length <= audioSourceIndex)
        {
            source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }
        else
        {
            source = sourceArray[audioSourceIndex];
        }

        SoundTemplate st;
        if (!SoundTemplate.Dic().TryGetValue(soundKey, out st))
        {
            Debug.Log("not found SoundEffect key:" + soundKey);
            return;
        }

        // random
        string[] nameArray = st.Name.Split(':');
        source.clip = Resources.Load<AudioClip>("Audio/" + nameArray[UnityEngine.Random.Range(0, nameArray.Length)]);

        source.loop = isloop;
        source.volume = volume;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = minDis;
        source.maxDistance = maxDis;

        source.PlayDelayed(delay);
    }

    public static void PlaySoundEffect3D(GameObject go, string soundKey, int audioSourceIndex = 0)
    {
        SoundTemplate st;
        if (!SoundTemplate.Dic().TryGetValue(soundKey, out st))
        {
            Debug.Log("not found SoundEffect key:" + soundKey);
            return;
        }
        //PlaySoundEffect3D(go, soundKey, st.IsLoop,st.MinDis,st.MaxDis,st.Volume,st.Delay,audioSourceIndex);
    }

    public static void StopSoundEffect3D(GameObject go, int audioSourceIndex = -1)
    {
        AudioSource[] sourceArray = go.GetComponents<AudioSource>();
        if (audioSourceIndex == -1)
        {
            foreach (AudioSource source in sourceArray)
            {
                source.Stop();
            }
        }
        else if (audioSourceIndex < sourceArray.Length)
        {
            sourceArray[audioSourceIndex].Stop();
        }
    }
    #endregion

    #region Play Sound2D

    private static Dictionary<string, int> s_soundIds = new Dictionary<string, int>();

    /// <summary>
    /// soundKey
    /// sample:
    /// PlaySoundEffect("P001:1001");
    /// </summary>
    public static int PlaySoundEffect(string soundKey)
    {

        SoundTemplate st;
        if (!SoundTemplate.Dic().TryGetValue(soundKey, out st))
        {
            Debug.LogWarning("not found SoundEffect key:" + soundKey);
            return -1;
        }

        return PlaySoundEffect(soundKey, st.Delay, st.IsLoop, st.Volume, false, st);
    }

    /// <summary>
    /// play the sound effect one shot.
    /// </summary>
    public static void PlaySoundEffectOneShot(string soundKey)
    {
        SoundTemplate st;
        if (!SoundTemplate.Dic().TryGetValue(soundKey, out st))
        {
            Debug.LogWarning("not found SoundEffect key:" + soundKey);
            return;
        }

        PlaySoundEffect(soundKey, st.Delay, st.IsLoop, st.Volume, true, st);
    }

    public static int PlaySoundEffect(string soundKey, float delay, bool isLoop, float volume, bool bOneShot = false, SoundTemplate st = null)
    {
        int id = -1;
        if (st == null && !SoundTemplate.Dic().TryGetValue(soundKey, out st))
        {
            Debug.LogWarning("not found SoundEffect key:" + soundKey);
            return -1;
        }

        string[] str;
        string name;
        str = st.Name.Split(':');
        if (str.Length == 0)
        {
            Debug.LogWarning("播放声音失败: " + soundKey);
            return -1;
        }

        name = str[UnityEngine.Random.Range(0, str.Length)];
        if (name == "")
        {
            Debug.LogWarning("播放声音失败: " + soundKey);
            return -1;
        }

        // 互斥不播放
        if (st.MutexSoundID != "")
        {
            str = st.MutexSoundID.Split(',');
            for (int i = 0; i < str.Length; ++i)
            {
                if (IsPlaySoundEffect(str[i]))
                {
                    Debug.Log("播放声音失败[被互斥掉了!]: " + soundKey);
                    return -1;
                }
            }
        }

        // 互斥停掉其它音效
        if (st.MutexAndStopSoundID != "")
        {
            str = st.MutexAndStopSoundID.Split(',');
            for (int i = 0; i < str.Length; ++i)
                StopSoundEffect(str[i]);
        }

        //Debug.Log("成功播放声音: " + soundKey);
        if (bOneShot)
        {
            Sound2D.PlayEffectOneShot("Audio/" + name, volume);
        }
        else
        {
            id = Sound2D.PlayEffect("Audio/" + name, isLoop, volume, delay);
            if (id != -1)
            {
                if (s_soundIds.ContainsKey(soundKey))
                {
                    s_soundIds[soundKey] = id;
                }
                else
                {
                    s_soundIds.Add(soundKey, id);
                }
            }
        }

        return id;
    }

    private static Dictionary<string, float> mSoundLastPlayTime = new Dictionary<string, float>();
    public static int PlaySoundEffectByInterval(string soundKey, float interval)
    {
        SoundTemplate st;
        if (!SoundTemplate.Dic().TryGetValue(soundKey, out st))
        {
            Debug.Log("not found SoundEffect key:" + soundKey);
            return -1;
        }
        return PlaySoundEffectByInterval(soundKey, interval, st.Delay, st.IsLoop, st.Volume);
    }

    public static int PlaySoundEffectByInterval(string soundKey, float interval, float delay, bool isLoop, float volume)
    {
        if (mSoundLastPlayTime.ContainsKey(soundKey))
        {
            if (mSoundLastPlayTime[soundKey] + interval < Time.realtimeSinceStartup)
            {
                mSoundLastPlayTime[soundKey] = Time.realtimeSinceStartup;
                return PlaySoundEffect(soundKey, delay, isLoop, volume);
            }
        }
        else
        {
            mSoundLastPlayTime.Add(soundKey, Time.realtimeSinceStartup);
            return PlaySoundEffect(soundKey, delay, isLoop, volume);
        }
        return -1;
    }

    public static void StopSoundEffect(string soundKey)
    {
        if (s_soundIds.ContainsKey(soundKey))
        {
            StopSoundEffect(s_soundIds[soundKey]);
        }
    }

    public static void StopSoundEffect(int soundId)
    {
        Sound2D.StopEffect(soundId);
    }

    public static Sound2D.PlayingEffectInfo FindSound(string soundKey)
    {
        Sound2D.PlayingEffectInfo ret = null;
        if (s_soundIds.ContainsKey(soundKey))
        {
            ret = FindSound(s_soundIds[soundKey]);
        }
        return ret;
    }

    public static Sound2D.PlayingEffectInfo FindSound(int soundId)
    {
        return Sound2D.FindEffect(soundId);
    }

    public static bool IsPlaySoundEffect(string soundKey)
    {
        bool ret = false;
        if (s_soundIds.ContainsKey(soundKey))
        {
            ret = IsPlaySoundEffect(s_soundIds[soundKey]);
        }
        return ret;
    }

    public static bool IsPlaySoundEffect(int soundId)
    {
        return Sound2D.IsPlaying(soundId);
    }

    public static void PauseEffect(int soundId)
    {
        Sound2D.PauseEffect(soundId);
    }

    public static void PauseEffect(string soundKey)
    {
        if (s_soundIds.ContainsKey(soundKey))
        {
            PauseEffect(s_soundIds[soundKey]);
        }
    }

    public static void ResumeEffect(int soundId)
    {
        Sound2D.ResumeEffect(soundId);
    }

    public static void ResumeEffect(string soundKey)
    {
        if (s_soundIds.ContainsKey(soundKey))
        {
            ResumeEffect(s_soundIds[soundKey]);
        }
    }

    #endregion
}
