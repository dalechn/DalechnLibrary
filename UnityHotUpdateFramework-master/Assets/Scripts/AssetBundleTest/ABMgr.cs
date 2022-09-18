using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

/// <summary>
/// 知识点
/// 1.AB包相关的API
/// 2.单例模式
/// 3.委托 --->Lambda表达式
/// 4.协程
/// 5.字典
/// </summary>
/// 
public class ABMgr :  MonoBehaviour
{
    public static ABMgr Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this;
    }

    //AB包管理器 目的是
    //让外部更方便的进行资源加载

        //主包
    private AssetBundle mainAB = null;
    //依赖包获取用的配置文件
    private AssetBundleManifest manifest = null;

    //AB包不能够重复加载 重复加载会报错
    //字典 用字典来存储 加载过的AB包
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// 这个AB包存放路径 方便修改
    /// </summary>
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    private string MainABName
    {
        get
        {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#elif UNITY_WEBGL
            return "WebGL";
#else
            return "PC";
#endif
        }
    }

    private string UpdatePathUrl
    {
        get
        {
            return Application.persistentDataPath + "/";
        }
    }

    public string CheckPath(string abName)
    {
        if (File.Exists(UpdatePathUrl+ abName))
        {
            // 优先从update目录（热更新目录）中查找资源
            return UpdatePathUrl;
        }
       else
        {
            return PathUrl;
        }
    }

    //同步加载 不指定类型
    public Object LoadRes(string abName, string resName)
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便 在加载资源时 判断一下 资源是不是Gameobject
        //如果是 直接实例化了 再返回给外部
        Object obj = abDic[abName].LoadAsset(resName);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }

    //同步加载 根据type指定类型
    public Object LoadRes(string abName, string resName, System.Type type)
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便 在加载资源时 判断一下 资源是不是Gameobject
        //如果是 直接实例化了 再返回给外部
        Object obj = abDic[abName].LoadAsset(resName, type);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }

    //同步加载 根据泛型指定类型
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便 在加载资源时 判断一下 资源是不是Gameobject
        //如果是 直接实例化了 再返回给外部
        T obj = abDic[abName].LoadAsset<T>(resName);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }

    //异步加载的方法
    //这里的异步加载 AB包并没有使用异步加载
    //只是从AB包中 加载资源时 使用异步
    //根据名字异步加载资源
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAsyncAB(abName, () => {
            StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
        });
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        //为了外面方便 在加载资源时 判断一下 资源是不是Gameobject
        //如果是 直接实例化了 再返回给外部
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;
        //异步加载结束后 通过委托 传递给外部 外部来使用
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else
            callBack(abr.asset);
    }

    //根据Type异步加载资源
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAsyncAB(abName, () => {
            StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack));
        });
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        //为了外面方便 在加载资源时 判断一下 资源是不是Gameobject
        //如果是 直接实例化了 再返回给外部
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;
        //异步加载结束后 通过委托 传递给外部 外部来使用
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else
            callBack(abr.asset);
    }

    //根据泛型 异步加载资源
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        //加载AB包
        LoadAsyncAB(abName, () =>
        {
            StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
        });
    }

    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        //为了外面方便 在加载资源时 判断一下 资源是不是Gameobject
        //如果是 直接实例化了 再返回给外部
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        //异步加载结束后 通过委托 传递给外部 外部来使用
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset) as T);
        else
            callBack(abr.asset as T);
    }

    //单个包卸载
    public void Unload(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }

    //所有包的卸载
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }

    /// <summary>
    /// 加载AB包
    /// </summary>
    /// <param name="abName"></param>
    public void LoadAB(string abName)
    {
        //加载AB包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //获取依赖包相关信息
        AssetBundle ab;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        //加载资源来源包   
        //如果没有加载过 再加载
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    /// <summary>
    /// 异步加载AB包
    /// </summary>
    /// <param name="abName"></param>
    public void LoadAsyncAB(string abName, UnityAction callback)
    {
        StartCoroutine(ReallyLoadAsyncAB(abName, callback));
    }

    private bool isRealed = false;

    private IEnumerator ReallyLoadAsyncAB(string abName, UnityAction callback)
    {
        //加载AB包
        AssetBundleCreateRequest abcr = null;

        //判断如果为null并且某有人在加载它 才会去加载
        if (mainAB == null && !isRealed)
        {
            isRealed = true;
            abcr = AssetBundle.LoadFromFileAsync(PathUrl + MainABName);
            yield return abcr;
            mainAB = abcr.assetBundle;
            AssetBundleRequest abr = mainAB.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
            yield return abr;
            manifest = (AssetBundleManifest)abr.asset;
        }
        //如果有人在加载 等别人加载完了 再说
        else
        {
            //每帧都检测别人加载完没有
            while (true)
            {
                //如果这两个不为空了 证明别人已经把他们加载好了 我们就可以跳出循环 继续做下面的逻辑了
                if (mainAB != null && manifest != null)
                    break;
                yield return null;
            }
        }

        //获取依赖包相关信息
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过 如果从来没有加载过 那么我们去加载
            if (!abDic.ContainsKey(strs[i]))
            {
                //先占个位置
                abDic.Add(strs[i], null);
                abcr = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                yield return abcr;
                //真正加载结束后 记录加载完成的ab包
                abDic[strs[i]] = abcr.assetBundle;
            }
            //字典里面有记录 那么可能字典里面也是空的
            else
            {
                //每帧都判断 这个包真正加载好没有 如果为null 表示没有加载好 我们就继续等 等到不为null循环自然就停了
                while (abDic[strs[i]] == null)
                {
                    yield return null;
                }
            }
        }

        //这下面的逻辑 同理
        //加载资源来源包   
        //如果没有加载过 再加载
        if (!abDic.ContainsKey(abName))
        {
            abDic.Add(abName, null);
            abcr = AssetBundle.LoadFromFileAsync(PathUrl + abName);
            yield return abcr;
            abDic[abName] = abcr.assetBundle;
        }
        //字典里面有记录 那么可能字典里面也是空的
        else
        {
            //每帧都判断 这个包真正加载好没有 如果为null 表示没有加载好 我们就继续等 等到不为null循环自然就停了
            while (abDic[abName] == null)
            {
                yield return null;
            }
        }

        callback();
    }
}

