using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABTest : MonoBehaviour
{
    public Image img;

    void Start()
    {
        //ͬ������
        //GameObject obj0 = ABMgr.Instance.LoadRes("model", "Cube") as GameObject;
        //obj0.transform.position = -Vector3.up;

        //GameObject obj = ABMgr.Instance.LoadRes("model", "Cube", typeof(GameObject)) as GameObject;
        //obj.transform.position = -Vector3.up;

        //GameObject obj2 = ABMgr.Instance.LoadRes<GameObject>("model", "Cube");
        //obj2.transform.position = Vector3.up;

        //�첽����
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

        //����AB�������� --- һ����Դ�����õ��˱��AB���е���Դ
        //ͨ������������ �������Դ��ʧ�����
        //����ʱ�� ��Ҫ�������� һ������� ��������

        //��һ�� ����AB��
        //AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/model");

        ////�������Ĺؼ�֪ʶ�� --- �������� ��ȡ������Ϣ
        ////1.��������
        //AssetBundle abMain = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/PC");
        ////2.���������еĹ̶��ļ�
        //AssetBundleManifest abManiFest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        ////3.�ӹ̶��ļ��� �õ�������Ϣ
        //string[] strs = abManiFest.GetAllDependencies("model");
        ////�õ��� ������������
        //for (int i = 0; i < strs.Length; i++)
        //{
        //    AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + strs[i]);
        //}

        ////�ڶ��� ���� AB���е���Դ
        ////ֻʹ�����ּ��� ����� ͬ����ͬ������Դ �ֲ���
        ////�������� ���ͼ��� ���� ʱ Typeָ������
        //GameObject obj = ab.LoadAsset<GameObject>("Cube");
        ////GameObject obj = ab.LoadAsset("Cube", typeof(GameObject)) as GameObject;
        //Instantiate(obj);

        //ab.Unload(false);

        //AB�������ظ����� ���򱨴�
        //AssetBundle ab2 = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/model");
        //����һ��Բ
        //GameObject obj2 = ab.LoadAsset("Sphere", typeof(GameObject)) as GameObject;
        //Instantiate(obj2);

        //�첽����--->Э��
        //StartCoroutine(LoadABRes("head", "body14601_stand_3_0"));
    }

    IEnumerator LoadABRes(string ABName, string resName)
    {
        //��һ�� ����AB��
        AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + ABName);
        yield return abcr;
        //�ڶ��� ������Դ
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
            //ж�����м��ص�AB�� ����Ϊtrue ���ͨ��AB�����ص���ԴҲж��
            AssetBundle.UnloadAllAssetBundles(false);
        }
    }
}
