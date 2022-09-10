using UnityEngine;
using UnityEditor;

public class PlayerPrefsTools 
{
    [MenuItem("Tools/ClearCache")]
    private static void ClearCache()
    {
        PlayerPrefs.DeleteAll();
    }

    // *�������ļ���MenuItem
    [MenuItem("Tools/��persistentDataPath")]
    static void MenueClick()
    {
        Application.OpenURL("file://" + Application.persistentDataPath);
        Debug.Log("file://" + Application.persistentDataPath);
    }
}
