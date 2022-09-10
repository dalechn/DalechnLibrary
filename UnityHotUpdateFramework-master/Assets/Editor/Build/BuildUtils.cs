using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using LitJson;
using UnityEngine.Networking;
using LuaFramework;

public class BuildUtils
{
    /// <summary>
    /// �ݹ������ȡĿ��Ŀ¼�е������ļ�
    /// </summary>
    /// <param name="sourceDir">Ŀ��Ŀ¼</param>
    /// <param name="splitAssetPath">�Ƿ�Ҫ�и�Ŀ¼����AssetsĿ¼Ϊ��</param>
    public static List<string> GetFiles(string sourceDir, bool splitAssetPath)
    {
        List<string> fileList = new List<string>();
        string[] fs = Directory.GetFiles(sourceDir);
        string[] ds = Directory.GetDirectories(sourceDir);
        for (int i = 0, len = fs.Length; i < len; ++i)
        {
            var index = splitAssetPath ? fs[i].IndexOf("Assets") : 0;
            fileList.Add(fs[i].Substring(index));
        }
        for (int i = 0, len = ds.Length; i < len; ++i)
        {
            fileList.AddRange(GetFiles(ds[i], splitAssetPath));
        }
        return fileList;
    }

    public static List<string> GetFiles(string[] sourceDirs, bool splitAssetPath)
    {
        List<string> fileList = new List<string>();
        foreach (var sourceDir in sourceDirs)
        {
            fileList.AddRange(GetFiles(sourceDir, splitAssetPath));
        }
        return fileList;
    }



    /// <summary>
    /// ���ݹ�ϣ����AssetBundleBuild�б�
    /// </summary>
    /// <param name="tb">��ϣ��keyΪassetBundleName��valueΪĿ¼</param>
    /// <returns></returns>
    public static AssetBundleBuild[] MakeAssetBundleBuildArray(Hashtable tb)
    {
        AssetBundleBuild[] buildArray = new AssetBundleBuild[tb.Count];
        int index = 0;
        foreach (string key in tb.Keys)
        {
            buildArray[index].assetBundleName = key;
            List<string> fileList = new List<string>();
            fileList = GetFiles(Application.dataPath + "/" + tb[key], true);
            buildArray[index].assetNames = fileList.ToArray();
            ++index;
        }

        return buildArray;
    }

    /// <summary>
    /// ����������ñ�AssetBundle
    /// </summary>
    public static void BuildNormalCfgBundle(string targetPath)
    {
        Hashtable tb = new Hashtable();
        tb["normal_cfg.bundle"] = "GameRes/Config";
        AssetBundleBuild[] buildArray = BuildUtils.MakeAssetBundleBuildArray(tb);
        BuildUtils.BuildBundles(buildArray, targetPath);
    }

    /// <summary>
    /// ���Lua��AssetBundle
    /// </summary>
    public static void BuildLuaBundle(string targetPath)
    {
        // ����Lua��Bundle��ʱĿ¼
        var luabundleDir = BuildUtils.CreateTmpDir("luabundle");
        // ��Lua���뿽����Bundle��ʱĿ¼�������ܴ���
        var luaFiles = BuildUtils.GetFiles(new string[] {
            Application.dataPath + "/LuaFramework/Lua",
            Application.dataPath + "/LuaFramework/ToLua/Lua",
        }, true);
        BuildUtils.CopyLuaToBundleDir(luaFiles, luabundleDir);
        // ����AssetBundleBuild�б�
        Hashtable tb = new Hashtable();
        tb["lua.bundle"] = "luabundle";
        AssetBundleBuild[] buildArray = MakeAssetBundleBuildArray(tb);
        // ���AssetBundle
        BuildBundles(buildArray, targetPath);

        // ɾ��Lua��Bundle��ʱĿ¼
        DeleteDir(luabundleDir);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// �����Ϸ��ԴAssetBundle
    /// </summary>
    public static void BuildGameResBundle(string targetPath)
    {
        Hashtable tb = new Hashtable();
        tb["baseres.bundle"] = "GameRes/BaseRes";
        tb["uiprefabs.bundle"] = "GameRes/UIPrefabs";
        AssetBundleBuild[] buildArray = MakeAssetBundleBuildArray(tb);
        BuildBundles(buildArray, targetPath);
    }

    /// <summary>
    /// ��AssetBundle
    /// </summary>
    /// <param name="buildArray">AssetBundleBuild�б�</param>
    public static void BuildBundles(AssetBundleBuild[] buildArray, string targetPath)
    {
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }
        BuildPipeline.BuildAssetBundles(targetPath, buildArray, BuildAssetBundleOptions.ChunkBasedCompression, GetBuildTarget());

    }

    public static void BuildApp()
    {
        string[] scenes = new string[] { "Assets/Scenes/Main.unity" };
        string appName = PlayerSettings.productName + "_" + VersionMgr.instance.appVersion + GetTargetPlatfromAppPostfix();
        string outputPath = Application.dataPath + "/../Bin/";
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        string appPath = Path.Combine(outputPath, appName);

        // ��������������ø��ְ汾��
        // PlayerSettings.Android.bundleVersionCode
        // PlayerSettings.bundleVersion
        // PlayerSettings.iOS.buildNumber

        BuildPipeline.BuildPlayer(scenes, appPath, GetBuildTarget(), BuildOptions.None);
        GameLogger.Log("Build APP Done");
    }

    /// <summary>
    /// ����ԭʼlua�����md5
    /// </summary>
    public static void GenOriginalLuaFrameworkMD5File()
    {
        VersionMgr.instance.Init();
        JsonData jd = GetOriginalLuaframeworkMD5Json();
        var jsonStr = JsonMapper.ToJson(jd);
        jsonStr = jsonStr.Replace(",", ",\n");
        if (!Directory.Exists(BIN_PATH))
        {
            Directory.CreateDirectory(BIN_PATH);
        }
        using (StreamWriter sw = new StreamWriter(BIN_PATH + "LuaFrameworkFiles_" + VersionMgr.instance.appVersion + ".json"))
        {
            sw.Write(jsonStr);
        }
        GameLogger.Log("GenLuaframeworkMd5 Done");
    }

    public static JsonData GetOriginalLuaframeworkMD5Json()
    {
        var sourceDirs = new string[] {
            Application.dataPath + "/LuaFramework/Lua",
            Application.dataPath + "/LuaFramework/ToLua/Lua",
        };
        JsonData jd = new JsonData();
        foreach (var sourceDir in sourceDirs)
        {
            List<string> fileList = new List<string>();
            fileList = GetFiles(sourceDir, false);
            foreach (var luaFile in fileList)
            {
                if (!luaFile.EndsWith(".lua")) continue;

                var md5 = LuaFramework.Util.md5file(luaFile);
                var key = luaFile.Substring(luaFile.IndexOf("Assets/"));
                jd[key] = md5;
            }
        }

        return jd;
    }

    /// <summary>
    /// ������ʱĿ¼
    /// </summary>
    /// <returns></returns>
    public static string CreateTmpDir(string dirName)
    {
        var tmpDir = string.Format(Application.dataPath + "/{0}/", dirName);
        if (Directory.Exists(tmpDir))
        {
            Directory.Delete(tmpDir, true);

        }
        Directory.CreateDirectory(tmpDir);
        return tmpDir;
    }

    /// <summary>
    /// ɾ��Ŀ¼
    /// </summary>
    public static void DeleteDir(string targetDir)
    {
        if (Directory.Exists(targetDir))
        {
            Directory.Delete(targetDir, true);
        }
        AssetDatabase.Refresh();
    }


    /// <summary>
    /// ����Lua��Ŀ��Ŀ¼���������ܴ���
    /// </summary>
    /// <param name="sourceDirs">ԴĿ¼�б�</param>
    /// <param name="luabundleDir">ĸ��Ŀ¼</param>
    public static void CopyLuaToBundleDir(List<string> luaFiles, string luabundleDir)
    {
        foreach (var luaFile in luaFiles)
        {
            if (luaFile.EndsWith(".meta")) continue;
            var luaFileFullPath = Application.dataPath + "/../" + luaFile;
            // ����Build AssetBundle��ʶ��.lua�ļ������Կ���һ�ݵ���ʱĿ¼��ͳһ����.bytes��β
            var targetFile = luaFile.Replace("Assets/LuaFramework/Lua", "");
            targetFile = targetFile.Replace("Assets/LuaFramework/ToLua/Lua", "");
            targetFile = luabundleDir + targetFile + ".bytes";
            var targetDir = Path.GetDirectoryName(targetFile);
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            // ���¼���
            byte[] bytes = File.ReadAllBytes(luaFileFullPath);
            byte[] encryptBytes = AESEncrypt.Encrypt(bytes);
            File.WriteAllBytes(targetFile, encryptBytes);
        }
        AssetDatabase.Refresh();
    }


    public void BuildLuaUpdateBundle(List<string> luaFileList)
    {

    }

    /// <summary>
    /// ��ȡ��ǰƽ̨
    /// </summary>
    public static BuildTarget GetBuildTarget()
    {

#if UNITY_STANDALONE
        return BuildTarget.StandaloneWindows;
#elif UNITY_ANDROID
        return BuildTarget.Android;
#else
        return BuildTarget.iOS;
#endif
    }

    /// <summary>
    /// ��ȡĿ��ƽ̨APP��׺
    /// </summary>
    /// <returns></returns>
    public static string GetTargetPlatfromAppPostfix(bool useAAB = false)
    {

#if UNITY_STANDALONE
        return ".exe";
#elif UNITY_ANDROID
        if(useAAB)
        {
            return ".aab";
        }
        else
        {
            return ".apk";
        }
#else
        return ".ipa";
#endif
    }

    public static string BIN_PATH
    {
        get { return Application.dataPath + "/../Bin/"; }
    }

    private static List<HotUpdater.UpdateInfo> ReqUpdateInfo()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(AppConst.WebUrl + "update_list.json");
        var request = uwr.SendWebRequest();
        while (!request.isDone) { }
        if (!string.IsNullOrEmpty(uwr.error))
        {
            return null;
        }

        Debug.Log(uwr.downloadHandler.text);
        return JsonMapper.ToObject<List<HotUpdater.UpdateInfo>>(uwr.downloadHandler.text);
    }

    // *�Զ�����update_list.json�ļ�
    public static void GenUpdateList(string zipFilePath = null)
    {
        VersionMgr.instance.Init();

        // ��������б�
        var updateInfos = ReqUpdateInfo();

        HotUpdater.UpdateInfo findVal = updateInfos.Find(e => e.appVersion == VersionMgr.instance.appVersion);
        if (findVal != null)
        {
           bool exit = findVal.updateList.Exists(e => e.resVersion == VersionMgr.instance.resVersion);

            // �ȸ���ʱ����Ҫд����Ϣ
            if (!exit&&zipFilePath != null)
            {
                HotUpdater.PackInfo packInfo = new HotUpdater.PackInfo();
                packInfo.resVersion = VersionMgr.instance.resVersion;
                packInfo.size = (int)new System.IO.FileInfo(zipFilePath).Length;
                packInfo.md5 = LuaFramework.Util.md5file(zipFilePath);
                packInfo.url = AppConst.WebUrl + zipFilePath.Replace(BuildUtils.BIN_PATH + "/", "");//��·����ȡ���ļ�������

                if (packInfo != null)
                {
                    findVal.updateList.Add(packInfo);
                }
            }
        }
        else
        {
            HotUpdater.UpdateInfo addAppVersion = new HotUpdater.UpdateInfo();
            addAppVersion.appVersion = VersionMgr.instance.appVersion;
            addAppVersion.appUrl = "https://blog.csdn.net/linxinfa";
            updateInfos.Add(addAppVersion);
        }

        var jsonStr = JsonMapper.ToJson(updateInfos);
        jsonStr = jsonStr.Replace(",", ",\n");

        if (!Directory.Exists(BIN_PATH))
        {
            Directory.CreateDirectory(BIN_PATH);
        }
        using (StreamWriter sw = new StreamWriter(BIN_PATH + "update_list.json"))
        {
            sw.Write(jsonStr);
        }
    }
}
