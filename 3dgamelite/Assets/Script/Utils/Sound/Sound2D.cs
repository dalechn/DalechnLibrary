using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Play 2d background music and effect.
/// </summary>
public class Sound2D : MonoBehaviour
{
    public static Sound2D Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    public delegate void OnMuteDelegate(bool isMute);

    /// <summary>
    /// Mute Event Callback (Static)
    /// </summary>
    public static event OnMuteDelegate OnMuteCallback;

    private const int EFFECT_SOURCE_POOL_SIZE = 2;      // Pool Size
    private const float EFFECT_MIN_PLAY_TIME = 0.1f;    // Min Time

    private AudioListener m_audioListener;
    private AudioSource m_bgAudioSource;

    private int m_lastEffectId = 1024;  // effect id start no
    private Dictionary<int, PlayingEffectInfo> m_allEffectDict = new Dictionary<int, PlayingEffectInfo>();

    protected  void Init()
    {
        // Audio Listener
        InitAudioListener();

        // Listener Volume
        AudioListener.volume = Mute ? 0 : 1;

        // Audio Source (Background)
        m_bgAudioSource = gameObject.AddComponent<AudioSource>();
        m_bgAudioSource.loop = true;
        m_bgAudioSource.playOnAwake = false;
    }

    void Start()
    {
        InvokeRepeating("CheckEffectAudioSource", 1.0f, 1.0f);
    }

    #region Background Music
    private static string s_lastPlayingBgMusicName = string.Empty;
    public static void PlayBackgroundMusic(string audioClip, float volume = 1.0f)
    {
        if (audioClip == s_lastPlayingBgMusicName)
            return;

        AudioClip clip = Resources.Load<AudioClip>(audioClip);
        if (clip != null)
        {
            s_lastPlayingBgMusicName = audioClip;
            PlayBackgroundMusic(clip, volume);
        }
        else
        {
            Debug.Log("WJSound2D: not found audio clip " + audioClip);
        }
    }

    public static void PlayBackgroundMusic(AudioClip audioClip, float volume = 1.0f)
    {
        //NGUITools.PlaySound();
        if (Instance == null)
            return;

        Instance.m_bgAudioSource.clip = audioClip;
        Instance.m_bgAudioSource.volume = volume;
        Instance.m_bgAudioSource.Play();
    }

    public static void PauseBackgroundMusic()
    {
        if (Instance == null)
            return;

        Instance.m_bgAudioSource.Pause();
    }

    public static void ResumeBackgroundMusic()
    {
        if (Instance == null)
            return;

        Instance.m_bgAudioSource.Play();
    }
    #endregion

    #region Effect

    public static int PlayEffect(string audioClip, bool loop = false, float volume = 1.0f, float delay = 0.0f)
    {
        AudioClip clip = Resources.Load<AudioClip>(audioClip);
        if (clip != null)
        {
            return PlayEffect(clip, loop, volume, delay);
        }
        else
        {
            Debug.LogWarning("WJSound2D: not found audio clip " + audioClip);
            return -1;
        }
    }

    public static int PlayEffect(AudioClip audioClip, bool loop = false, float volume = 1.0f, float delay = 0.0f)
    {
        if (Instance == null)
            return -1;

        return Instance.PlayNewEffect(audioClip, loop, volume, delay);
    }

    public static void PlayEffectOneShot(string audioClip, float volume = 1.0f)
    {
        if (Instance == null)
            return;

        AudioClip clip = Resources.Load<AudioClip>(audioClip);
        if (clip != null)
        {
            Instance.m_bgAudioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning("WJSound2D: not found audio clip " + audioClip);
        }
    }

    public static void PlayEffectOneShot(AudioClip audioClip, float volume = 1.0f)
    {
        if (Instance == null)
            return;

        Instance.m_bgAudioSource.PlayOneShot(audioClip, volume);
    }

    private int PlayNewEffect(AudioClip clip, bool loop, float volume, float delay)
    {
        PlayingEffectInfo effectInfo = null;
        float t = Time.realtimeSinceStartup;

        // check pool
        Dictionary<int, PlayingEffectInfo>.ValueCollection values = m_allEffectDict.Values;
        foreach (PlayingEffectInfo info in values)
        {
            if (!info.Source.isPlaying && !info.IsPause && t - info.StartTime > EFFECT_MIN_PLAY_TIME)
            {
                // use this audiosource
                effectInfo = info;

                // remove info from dict
                m_allEffectDict.Remove(info.KeyEffectId);

                break;
            }
        }

        // not found, new
        if (effectInfo == null)
        {
            effectInfo = new PlayingEffectInfo();

            effectInfo.Source = gameObject.AddComponent<AudioSource>();
            effectInfo.Source.playOnAwake = false;

            effectInfo.IsInPool = m_allEffectDict.Count < EFFECT_SOURCE_POOL_SIZE;
        }

        effectInfo.StartTime = t;   // current time
        effectInfo.KeyEffectId = ++m_lastEffectId;  // set new effectid

        // play
        effectInfo.Source.loop = loop;
        effectInfo.Source.clip = clip;
        effectInfo.Source.volume = volume;
        effectInfo.Source.PlayDelayed(delay);

        // add to dict
        m_allEffectDict.Add(effectInfo.KeyEffectId, effectInfo);

        return effectInfo.KeyEffectId;
    }

    public static void StopAllEffect()
    {
        if (Instance != null)
            Instance.StopAllEffectImmediate();
    }

    public static void StopEffect(int effectId)
    {
        if (effectId > -1 && Instance != null)
            Instance.StopEffectImmediate(effectId);
    }

    public static bool IsPlaying(int effectId)
    {
        bool ret = false;
        if (Instance != null)
        {
            PlayingEffectInfo info;
            info = Instance.QueryEffect(effectId);
            if (info != null)
            {
                ret = info.Source.isPlaying;
            }
        }
        return ret;
    }

    public static void PauseEffect(int effectId)
    {
        if (Instance != null)
        {
            PlayingEffectInfo info;
            info = Instance.QueryEffect(effectId);
            if (info != null)
            {
                info.Source.Pause();
                info.IsPause = true;
            }
        }
    }

    public static void ResumeEffect(int effectId)
    {
        if (Instance != null)
        {
            PlayingEffectInfo info;
            info = Instance.QueryEffect(effectId);
            if (info != null)
            {
                info.Source.Play();
                info.IsPause = false;
            }
        }
    }

    public static PlayingEffectInfo FindEffect(int effectId)
    {
        return Instance.QueryEffect(effectId);
    }

    private PlayingEffectInfo QueryEffect(int effectId)
    {
        PlayingEffectInfo info = null;
        if (m_allEffectDict.ContainsKey(effectId))
        {
            info = m_allEffectDict[effectId];
        }
        return info;
    }

    private void StopAllEffectImmediate()
    {
        LinkedList<PlayingEffectInfo> infoDelete = new LinkedList<PlayingEffectInfo>();

        Dictionary<int, PlayingEffectInfo>.ValueCollection values = m_allEffectDict.Values;
        foreach (PlayingEffectInfo info in values)
        {
            info.IsPause = false;
            info.Source.Stop();
            info.Source.clip = null;

            if (!info.IsInPool)
            {
                infoDelete.AddLast(info);
            }
        }

        foreach (PlayingEffectInfo info in infoDelete)
        {
            Object.Destroy(info.Source);
            m_allEffectDict.Remove(info.KeyEffectId);
        }
    }

    private void StopEffectImmediate(int effectId)
    {
        PlayingEffectInfo info;
        if (m_allEffectDict.TryGetValue(effectId, out info))
        {
            info.IsPause = false;
            info.Source.Stop();
            info.Source.clip = null;

            if (!info.IsInPool)
            {
                Object.Destroy(info.Source);
                m_allEffectDict.Remove(effectId);
            }
        }
    }

    private void CheckEffectAudioSource()
    {
        if (m_allEffectDict.Count <= EFFECT_SOURCE_POOL_SIZE)
            return;  // immediate exit

        LinkedList<int> keysDelete = new LinkedList<int>();
        Dictionary<int, PlayingEffectInfo>.ValueCollection values = m_allEffectDict.Values;
        float t = Time.realtimeSinceStartup;

        foreach (PlayingEffectInfo info in values)
        {
            if (!info.IsInPool && !info.Source.isPlaying && !info.IsPause && t - info.StartTime > EFFECT_MIN_PLAY_TIME)
            {
                Object.Destroy(info.Source);
                keysDelete.AddLast(info.KeyEffectId);
            }
        }

        foreach (int key in keysDelete)
        {
            m_allEffectDict.Remove(key);
        }
    }

    #endregion

    #region Audio Listener
    public static bool Mute
    {
        get
        {
            return PlayerPrefs.GetInt("isSound2DMute", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("isSound2DMute", value ? 1 : 0);
            PlayerPrefs.Save();

            AudioListener.volume = value ? 0 : 1;

            if (OnMuteCallback != null)
            {
                OnMuteCallback.Invoke(value);
            }
        }
    }

    public static void RemoveAudioListener()
    {
        if (Instance != null)
        {
            Instance.DeleteAudioListener();
        }
    }

    private void DeleteAudioListener()
    {
        if (m_audioListener != null)
        {
            GameObject.Destroy(m_audioListener);
            m_audioListener = null;
        }
    }

    public static void CreateAudioListener()
    {
        if (Instance != null)
        {
            Instance.InitAudioListener();
        }
    }

    private void InitAudioListener()
    {
        if (m_audioListener == null)
        {
            m_audioListener = gameObject.AddComponent<AudioListener>();
        }
    }

    #endregion

    #region PlayingEffectInfo
    public class PlayingEffectInfo
    {
        public AudioSource Source = null;
        public float StartTime;
        public bool IsInPool;
        public int KeyEffectId;
        public bool IsPause;

        public PlayingEffectInfo()
        {
            IsPause = false;
        }

        public PlayingEffectInfo(AudioSource source, float startTime, bool isInPool, int effectId)
        {
            this.Source = source;
            this.StartTime = startTime;
            this.IsInPool = isInPool;
            this.KeyEffectId = effectId;
        }
    }
    #endregion
}
