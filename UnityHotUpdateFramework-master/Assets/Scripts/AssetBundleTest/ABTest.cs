using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABTest : MonoBehaviour
{
    public Image img;

    void Start()
    {
        //同步测试
        //GameObject obj0 = ABMgr.Instance.LoadRes("model", "Cube") as GameObject;
        //obj0.transform.position = -Vector3.up;

        //GameObject obj = ABMgr.Instance.LoadRes("model", "Cube", typeof(GameObject)) as GameObject;
        //obj.transform.position = -Vector3.up;

        //GameObject obj2 = ABMgr.Instance.LoadRes<GameObject>("model", "Cube");
        //obj2.transform.position = Vector3.up;

        //异步测试
        ABMgr.Instance.LoadResAsync<GameObject>("model", "Cube", (obj) =>
        {
            obj.transform.position = -Vector3.up;
        });

        //ABMgr.Instance.LoadResAsync("model", "Cube", typeof(GameObject), (obj) =>
        //{
        //    (obj as GameObject).transform.position = Vector3.up;
        //});

        //ABMgr.Instance.LoadResAsync("model", "Cube",  (obj) =>
        //{
        //    (obj as GameObject).transform.position = Vector3.up;
        //});

        //----------------------------------------------------------------------------------------------------------

        //关于AB包的依赖 --- 一个资源身上用到了别的AB包中的资源
        //通过它创建对象 会出现资源丢失的情况
        //这种时候 需要把依赖包 一起加载了 才能正常

        //第一步 加载AB包
        //AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/model");

        ////依赖包的关键知识点 --- 利用主包 获取依赖信息
        ////1.加载主包
        //AssetBundle abMain = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/PC");
        ////2.加载主包中的固定文件
        //AssetBundleManifest abManiFest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        ////3.从固定文件中 得到依赖信息
        //string[] strs = abManiFest.GetAllDependencies("model");
        ////得到了 依赖包的名字
        //for (int i = 0; i < strs.Length; i++)
        //{
        //    AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + strs[i]);
        //}

        ////第二步 加载 AB包中的资源
        ////只使用名字加载 会出现 同名不同类型资源 分不清
        ////建议大家用 泛型加载 或者 时 Type指定类型
        //GameObject obj = ab.LoadAsset<GameObject>("Cube");
        ////GameObject obj = ab.LoadAsset("Cube", typeof(GameObject)) as GameObject;
        //Instantiate(obj);

        //ab.Unload(false);

        //AB包不能重复加载 否则报错
        //AssetBundle ab2 = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/model");
        //加载一个圆
        //GameObject obj2 = ab.LoadAsset("Sphere", typeof(GameObject)) as GameObject;
        //Instantiate(obj2);

        //异步加载--->协程
        //StartCoroutine(LoadABRes("head", "body14601_stand_3_0"));
    }

    IEnumerator LoadABRes(string ABName, string resName)
    {
        //第一步 加载AB包
        AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + ABName);
        yield return abcr;
        //第二步 加载资源
        AssetBundleRequest abq = abcr.assetBundle.LoadAssetAsync(resName, typeof(Sprite));
        yield return abq;
        //abq.asset as Sprite;
        img.sprite = abq.asset as Sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //卸载所有加载的AB包 参数为true 会把通过AB包加载的资源也卸载
            AssetBundle.UnloadAllAssetBundles(false);
        }
    }
}
