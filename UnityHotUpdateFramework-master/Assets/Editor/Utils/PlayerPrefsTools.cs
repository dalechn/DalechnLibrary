using UnityEngine;
using UnityEditor;

public class PlayerPrefsTools 
{
    [MenuItem("Tools/ClearCache")]
    private static void ClearCache()
    {
        PlayerPrefs.DeleteAll();
    }

    // *新增打开文件夹MenuItem
    [MenuItem("Tools/打开persistentDataPath")]
    static void MenueClick()
    {
        Application.OpenURL("file://" + Application.persistentDataPath);
        Debug.Log("file://" + Application.persistentDataPath);
    }
}
