using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class BuildApp : EditorWindow
{
    [MenuItem("Build/���APP")]
    public static void ShowWin()
    {
        var win = GetWindow<BuildApp>();
        win.Show();
    }

    private void Awake()
    {
        m_versionGUI = new VersionGUI();
        m_versionGUI.Awake();
    }

    private void OnGUI()
    {
        m_versionGUI.DrawVersion();
        DrawBuildApp();
    }

    

    private void DrawBuildApp()
    {
        if(GUILayout.Button("Build APP"))
        {
            // ����ԭʼluaȫ���ļ���md5
            BuildUtils.GenOriginalLuaFrameworkMD5File();
            // ��AssetBundle
            BuildAssetBundle.Build();
            // ���APP
            BuildUtils.BuildApp();

            BuildUtils.GenUpdateList();
        }
    }

    private VersionGUI m_versionGUI;
}
