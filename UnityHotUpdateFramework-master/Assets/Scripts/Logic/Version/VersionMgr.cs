using UnityEngine;
using LitJson;
using System.IO;
public class VersionMgr
{

    public void Init()
    {
        // ���ļ��ж�ȡ�汾��
        var versionText = Resources.Load<TextAsset>("version").text;
        var jsonData = JsonMapper.ToObject(versionText);
        appVersion = jsonData["app_version"].ToString();
        resVersion = jsonData["res_version"].ToString();

        // �ӻ����ж�ȡ��Դ�汾��
        var cacheResVersion = ReadCacheResVersion();

        // �������İ汾�ű��ļ��еİ汾�Ŵ��Ի����Ϊ׼����Ϊ�ȸ��»����ӻ������Դ�汾�ţ�
        if (CompareVersion(cacheResVersion, resVersion) > 0)
        {
            resVersion = cacheResVersion;
        }
        GameLogger.Log("appVersion: " + appVersion + ", resVersion: " + resVersion);
    }

    /// <summary>
    /// ��ȡ�������Դ�汾��
    /// </summary>
    private string ReadCacheResVersion()
    {
        if (File.Exists(cacheResVersionFile))
        {
            using (var f = File.OpenRead(cacheResVersionFile))
            {
                using (var sr = new StreamReader(f))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        return "0.0.0.0";
    }

    /// <summary>
    /// ������Դ�汾��
    /// </summary>
    public void UpdateResVersion(string resVersion)
    {
        this.resVersion = resVersion;
        var dir = Path.GetDirectoryName(cacheResVersionFile);
        if (Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        using (var f = File.OpenWrite(cacheResVersionFile))
        {
            using(StreamWriter sw = new StreamWriter(f))
            {
                sw.Write(resVersion);
            }
        }
        GameLogger.LogGreen("���±��ػ���汾�ţ�" + resVersion);
    }

    /// <summary>
    /// ɾ���������Դ�汾��
    /// </summary>
    public void DeleteCacheResVersion()
    {
        if(File.Exists(cacheResVersionFile))
        {
            File.Delete(cacheResVersionFile);
        }
    }

    /// <summary>
    /// �Ա������汾�ŵĴ�С
    /// </summary>
    /// <param name="v1">�汾��1</param>
    /// <param name="v2">�汾��2</param>
    /// <returns>1��v1��v2��0��v1����v2��-1��v1С��v2</returns>
    public static int CompareVersion(string v1, string v2)
    {
        if (v1 == v2) return 0;
        string[] v1Array = v1.Split('.');
        string[] v2Array = v2.Split('.');
        for (int i = 0, len = v1Array.Length; i < len; ++i)
        {
            if (int.Parse(v1Array[i]) < int.Parse(v2Array[i]))
                return -1;
            else if (int.Parse(v1Array[i]) > int.Parse(v2Array[i]))
                return 1;
        }
        return 0;
    }

    /// <summary>
    /// ��Ϸ�汾��
    /// </summary>
    public string appVersion { get; private set; }
    /// <summary>
    /// ��Դ�汾�ţ��ȸ�����������汾�ţ�
    /// </summary>
    public string resVersion { get; private set; }

    /// <summary>
    /// �ȸ��°汾��
    /// </summary>
    private string cacheResVersionFile
    {
        get
        {
            return Application.persistentDataPath + "/update/version.txt";
        }
    }

    private static VersionMgr s_instance;
    public static VersionMgr instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new VersionMgr();
            return s_instance;
        }
    }
}
