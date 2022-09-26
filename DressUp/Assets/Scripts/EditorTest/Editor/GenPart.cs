﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MagicaCloth;

public class GenPart : EditorWindow
{
    public const string DECORATE_PATH = "Assets/Resources/AvatarParts/Decorates/";
    public const string FACE_PATH = "Assets/Resources/AvatarParts/Face/";
    public const string HAIR_PATH = "Assets/Resources/AvatarParts/Hair/";
    public const string PANTS_PATH = "Assets/Resources/AvatarParts/Pants/";
    public const string SHOES_PATH = "Assets/Resources/AvatarParts/Shoes/";
    public const string TOP_PATH = "Assets/Resources/AvatarParts/Skirt/";
    public const string COAT_PATH = "Assets/Resources/AvatarParts/Skirt2/";
    public const string SUIT_PATH = "Assets/Resources/AvatarParts/Suit/";
    public const string AVATARROOT_PATH = "Assets/Resources/AvatarParts/";

    private const string FBX_SUFFIX = ".fbx";
    private const string ANIM_SUFFIX = ".anim";
    private const string PREAFAB_SUFFIX = ".prefab";

    private string decoratesSuffix = "1";
    private string faceSuffix = "1";
    private string hairSuffix = "1";
    private string pantsSuffix = "1";
    private string shoesSuffix = "1";
    private string topSuffix = "1";
    private string coatSuffix = "1";
    private string suitSuffix = "1";

    private const string decoratesPrefix = "Deco_";
    private const string facePrefix = "Face_";
    private const string hairPrefix = "Hair_";
    private const string pantsPrefix = "Pants_";
    private const string shoesPrefix = "Shoes_";
    private const string topPrefix = "Skirt_";
    private const string coatPrefix = "Skirt2_";
    private const string suitPrefix = "Suit_";
    private const string rootName = "root";

    [MenuItem("CustomTools/GenPart")]
    static void Create()
    {
        EditorWindow.GetWindow<GenPart>("GenPart");
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        decoratesSuffix = EditorGUILayout.TextField("decoratesSuffix", decoratesSuffix);
        faceSuffix = EditorGUILayout.TextField("faceSuffix", faceSuffix);
        hairSuffix = EditorGUILayout.TextField("hairSuffix", hairSuffix);
        pantsSuffix = EditorGUILayout.TextField("pantsSuffix", pantsSuffix);
        shoesSuffix = EditorGUILayout.TextField("shoesSuffix", shoesSuffix);
        topSuffix = EditorGUILayout.TextField("topSuffix", topSuffix);
        coatSuffix = EditorGUILayout.TextField("coatSuffix", coatSuffix);
        suitSuffix = EditorGUILayout.TextField("suitSuffix", suitSuffix);

        if (GUILayout.Button(decoratesPrefix))
        {
            Generate<MagicaAvatarParts, BaseCloth>(DECORATE_PATH + decoratesPrefix + decoratesSuffix);
        }
        else if (GUILayout.Button(decoratesPrefix))
        {
            Generate<MagicaAvatarParts, BaseCloth>(FACE_PATH + facePrefix + faceSuffix);
        }
        else if (GUILayout.Button(hairPrefix))
        {
            // 没处理好 还需要手动清理 MagicaVirtualDeformer
            Generate<MagicaAvatarParts, MagicaMeshCloth>(HAIR_PATH + hairPrefix + hairSuffix);
        }
        else if (GUILayout.Button(pantsPrefix))
        {
            Generate<MagicaAvatarParts, MagicaBoneCloth>(PANTS_PATH + pantsPrefix + pantsSuffix);
        }
        else if (GUILayout.Button(shoesPrefix))
        {
            Generate<MagicaAvatarParts, BaseCloth>(SHOES_PATH + shoesPrefix + shoesSuffix);
        }
        else if (GUILayout.Button(topPrefix))
        {
            Generate<MagicaAvatarParts, MagicaBoneCloth>(TOP_PATH + topPrefix + topSuffix);
        }
        else if (GUILayout.Button(coatPrefix))
        {
            Generate<MagicaAvatarParts, MagicaBoneCloth>(COAT_PATH + coatPrefix + coatSuffix);
        }
        else if (GUILayout.Button(suitPrefix))
        {
            Generate<MagicaAvatarParts, MagicaBoneCloth>(SUIT_PATH + suitPrefix + suitSuffix);
        }
        else if (GUILayout.Button(rootName))
        {
            Generate<MagicaAvatar,BaseCloth>(AVATARROOT_PATH + rootName, false);
        }
     

        GUILayout.EndVertical();
    }

    private static GameObject Generate<T,K>(string path, bool destroyAni = true) where T : UnityEngine.Component where K: BaseCloth
    {
        List<GameObject> objs = CollectObjs();

        if (objs.Count > 0)
        {
            SkinnedMeshRenderer renderer = objs[0].GetComponent<SkinnedMeshRenderer>();

            GameObject rendererParent = GameObject.Instantiate(objs[0].transform.parent.gameObject);

            foreach (SkinnedMeshRenderer tempsmr in rendererParent.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                bool exit = objs.Exists(e => e.name == tempsmr.name);
                if (!exit)
                {
                    GameObject.DestroyImmediate(tempsmr.gameObject);
                }
                else
                {
                    //从新命名下防止重复
                    tempsmr.name += Time.time;
                }
            }

            foreach (BaseCloth tempsmr in rendererParent.GetComponentsInChildren<K>())
            {
                GameObject.DestroyImmediate(tempsmr.gameObject);
            }

            if (destroyAni)
            {
                Animator anim = rendererParent.GetComponent<Animator>();
                GameObject.DestroyImmediate(anim);
            }

            rendererParent.AddComponent<T>();

            string dstpath = path + PREAFAB_SUFFIX;
            GameObject obj = PrefabUtility.SaveAsPrefabAsset(rendererParent, dstpath);

            GameObject.DestroyImmediate(rendererParent);

            Debug.LogWarning("写入成功:" + dstpath);

            return obj;
        }
        else
        {
            Debug.LogError("必须在hierarchy选择一个以上的物体!");
        }

        return null;
    }

    private static List<GameObject> CollectObjs()
    {
        List<GameObject> objs = new List<GameObject>();
        foreach (UnityEngine.Object o in Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Unfiltered))
        {
            GameObject obj = o as GameObject;
            if (!obj)
                continue;

            if (obj.name.Contains("@"))
                continue;

            objs.Add(obj);
        }

        return objs;
    }
}
