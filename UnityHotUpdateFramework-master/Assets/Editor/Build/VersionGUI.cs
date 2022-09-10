using UnityEngine;
using UnityEditor;
using LitJson;
using System.IO;

public class VersionGUI 
{
    public void Awake()
    {
        VersionMgr.instance.Init();
        m_appVersion = VersionMgr.instance.appVersion;
        m_resVersion = VersionMgr.instance.resVersion;
    }

    public void DrawVersion(bool update=false)
    {
        GUILayout.BeginHorizontal();
        // * ÐÞ¸ÄÐ´Èë´íÎó
        if(update)
        {
            m_resVersion = EditorGUILayout.TextField("version", m_resVersion);
        }
        else
        {
            m_appVersion = EditorGUILayout.TextField("version", m_appVersion);
        }
        JsonData jd = new JsonData();
        jd["app_version"] = m_appVersion;
        jd["res_version"] = m_resVersion;
        if (GUILayout.Button("Save"))
        {
            using (StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/version.bytes"))
            {
                sw.Write(jd.ToJson());
            }
            AssetDatabase.Refresh();
            Debug.Log("Save Version OK: " + m_appVersion+" "+ m_resVersion);
            VersionMgr.instance.DeleteCacheResVersion();
            VersionMgr.instance.Init();
        }
        GUILayout.EndHorizontal();
    }

    private string m_resVersion;
    private string m_appVersion;
}
