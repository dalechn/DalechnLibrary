using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LuaFramework;
using System;
using LitJson;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.IO;
using Ionic.Zip;


/// <summary>
/// �ȸ����߼�
/// </summary>
public class HotUpdater
{
    /// <summary>
    /// app����ǿ�Ƹ���
    /// </summary>
    public Action actionForceFullAppUpdate;
    /// <summary>
    /// app������ǿ�Ƹ���
    /// </summary>
    public Action actionWeakFullAppUpdate;
    /// <summary>
    /// û���κθ���
    /// </summary>
    public Action actionNothongUpdate;
    /// <summary>
    /// ȫ���ȸ����������
    /// </summary>
    public Action actionAllDownloadDone;
    /// <summary>
    /// ������ʾ
    /// </summary>
    public Action<UnityWebRequest.Result> actionShowErrorTips;

    /// <summary>
    /// ������ʾ��
    /// </summary>
    public Action<string> actionUpdateTipsText;

    public Action<float> actionUpdateProgress;
    private DownloadingInfo m_downloadingInfo;


    /// <summary>
    /// ��Ҫ��������
    /// </summary>
    private bool m_needFullAppUpdate;
    /// <summary>
    /// �Ƿ�ǿ�Ƹ���
    /// </summary>
    private bool m_hasNextUpdateBtn;
    /// <summary>
    /// �������µ�URL
    /// </summary>
    private string m_fullAppUpdateUrl;
    /// <summary>
    /// ��ͬapp�汾�ŵ�res���£�res�汾�����ӣ�
    /// </summary>
    private bool m_sameAppVerResUpdate;
    private List<PackInfo> m_packList = new List<PackInfo>();
    /// <summary>
    /// ������
    /// </summary>
    private Downloader m_downloader;

    private IEnumerator m_onDownloadItr;

    private int m_curPackIndex = 0;

    public void Init()
    {
        // ���HTTPS֤������
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
    }

    public void Start()
    {
        m_needFullAppUpdate = false;
        m_hasNextUpdateBtn = true;
        m_fullAppUpdateUrl = null;
        m_sameAppVerResUpdate = false;
        m_packList.Clear();
        m_downloadingInfo = new DownloadingInfo();

        // ��������б�
        var updateInfos = ReqUpdateInfo();
        if (null != updateInfos)
        {
            // �Ը����б��������
            SortUpdateInfo(ref updateInfos);
            // ����ʵ����Ҫ���ص��������б�
            CalculateUpdatePackList(updateInfos);
            // ��Ҫ��������
            if (m_needFullAppUpdate)
            {
                if (m_hasNextUpdateBtn)
                {
                    // ���Բ�ǿ�Ƹ����������С��´Ρ���ť
                    actionWeakFullAppUpdate?.Invoke();
                }
                else
                {
                    // �����������£�ǿ�ƣ�û�С��´Ρ���ť
                    actionForceFullAppUpdate?.Invoke();
                }
            }
            else if (m_sameAppVerResUpdate)
            {
                // ��ͬapp�汾��res����������
                m_curPackIndex = 0;
                StartDownloadResPack();
            }
            else
            {
                // û���κθ���
                actionNothongUpdate?.Invoke();
            }
        }
    }

    /// <summary>
    /// ���������Ϣ
    /// </summary>
    private List<UpdateInfo> ReqUpdateInfo()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(AppConst.WebUrl + "update_list.json");
        var request = uwr.SendWebRequest();
        while (!request.isDone) { }
        if (!string.IsNullOrEmpty(uwr.error))
        {
            // Debug.LogError(uwr.error);
            actionShowErrorTips.Invoke(uwr.result);
            return null;
        }

        Debug.Log(uwr.downloadHandler.text);
        return JsonMapper.ToObject<List<UpdateInfo>>(uwr.downloadHandler.text);
    }

    private void SortUpdateInfo(ref List<UpdateInfo> updateInfos)
    {
        // �������򣬰汾�Ŵ����ǰ��
        updateInfos.Sort((a, b) =>
        {
            return VersionMgr.CompareVersion(b.appVersion, a.appVersion);
        });
    }

    /// <summary>
    /// ������������
    /// </summary>
    public void CalculateUpdatePackList(List<UpdateInfo> updateInfos)
    {
        string bigestAppVer = "0.0.0.0";
        m_hasNextUpdateBtn = false;
        foreach (var info in updateInfos)
        {
            if (VersionMgr.CompareVersion(info.appVersion, VersionMgr.instance.appVersion) > 0)
            {
                m_needFullAppUpdate = true;
                if (VersionMgr.CompareVersion(info.appVersion, bigestAppVer) > 0)
                {
                    bigestAppVer = info.appVersion;
                    m_fullAppUpdateUrl = info.appUrl;
                }
            }
            if (VersionMgr.CompareVersion(info.appVersion, VersionMgr.instance.appVersion) == 0)
            {
                m_hasNextUpdateBtn = true;
                foreach (var pack in info.updateList)
                {
                    if (VersionMgr.CompareVersion(pack.resVersion, VersionMgr.instance.resVersion) > 0)
                    {
                        m_sameAppVerResUpdate = true;
                        m_packList.Add(pack);
                    }
                }
            }
        }
        GameLogger.Log("m_packList.Count: " + m_packList.Count);
    }


    public void DoFullAppUpdate()
    {
        // ����Ӧ���̵�
        Application.OpenURL(m_fullAppUpdateUrl);
    }

    /// <summary>
    /// ������汾����
    /// </summary>
    public void DoNextTime()
    {
        // ������������ִ��res�ȸ�
        if (m_sameAppVerResUpdate)
        {
            m_curPackIndex = 0;
            StartDownloadResPack();
        }
        else
        {
            actionNothongUpdate?.Invoke();
        }
    }

    private void StartDownloadResPack(bool next = false)
    {
        actionUpdateTipsText?.Invoke("�������ظ��°������Ե�...");
        if (next)
            ++m_curPackIndex;
        if (m_curPackIndex > m_packList.Count - 1)
        {
            GameLogger.Log("ȫ���ȸ����������");
            actionAllDownloadDone?.Invoke();
            return;
        }
        
        // ����ȷ���汾��С��������
        m_packList.Sort((a, b) =>
        {
            return VersionMgr.CompareVersion(a.resVersion, b.resVersion);
        });
        var packInfo = m_packList[m_curPackIndex];
        GameLogger.Log("��ʼ����: " + packInfo.ToString());
        m_downloadingInfo.totalPackCount = m_packList.Count;
        m_downloadingInfo.packIndex = m_curPackIndex;
        m_downloadingInfo.targetDownloadSize = packInfo.size;
        m_downloader = new Downloader();
        m_downloader.Start(packInfo);
    }

    public void Update()
    {
        if (null != m_downloader)
        {
            switch (m_downloader.state)
            {
                case Downloader.DownloadState.End:
                    {
                        m_downloader = null;
                        m_onDownloadItr = OnDownloadEnd();
                    }
                    break;
                case Downloader.DownloadState.Ing:
                    {
                        OnDownloading();
                    }
                    break;
                case Downloader.DownloadState.ConnectionError:
                    {
                        m_downloader = null;
                        OnDownloadError(UnityWebRequest.Result.ConnectionError);
                    }
                    break;
                case Downloader.DownloadState.DataProcessingError:

                    {
                        m_downloader = null;
                        OnDownloadError(UnityWebRequest.Result.DataProcessingError);
                    }
                    break;
            }
        }
        RunCoroutine();
    }

    private void RunCoroutine()
    {
        if (null != m_onDownloadItr && !m_onDownloadItr.MoveNext())
        {
            m_onDownloadItr = null;
        }
    }

    private IEnumerator OnDownloadEnd()
    {
        var packInfo = m_packList[m_curPackIndex];
        GameLogger.Log("�������: " + packInfo.url);
        actionUpdateTipsText?.Invoke("����У���ļ������Ե�...");
        yield return null;

        var filePath = Application.persistentDataPath + "/" + packInfo.md5;
        var md5Ok = CheckMd5(filePath, packInfo.md5);
        if (!md5Ok)
        {
            DeleteFile(filePath);
            Debug.LogError("MD5У�鲻ͨ������������");
            m_downloader = new Downloader();
            m_downloader.Start(m_packList[m_curPackIndex]);
        }
        else
        {
            // ��ѹzip
            actionUpdateTipsText?.Invoke("���ڽ�ѹ�ļ������Ե�...");
            yield return null;
            GameLogger.LogGreen("��ѹ�ļ���" + filePath);
            int index = 0;
            using (ZipFile zip = new ZipFile(filePath))
            {
                var cnt = zip.Count;
                foreach (var entity in zip)
                {
                    ++index;
                    entity.Extract(Application.persistentDataPath + "/update/", ExtractExistingFileAction.OverwriteSilently);
                    actionUpdateProgress?.Invoke((float)index / cnt);
                    yield return null;
                }
            }
            // ɾ��zip
            DeleteFile(filePath);
            // ���°汾��
            VersionMgr.instance.UpdateResVersion(packInfo.resVersion);
            // ����������һ��zip
            StartDownloadResPack(true);
        }
    }


    /// <summary>
    /// ������
    /// </summary>
    private void OnDownloading()
    {
        // ���½�����
        m_downloadingInfo.curDownloadSize = m_downloader.curDownloadSize;
        var value = (float)m_downloadingInfo.curDownloadSize / m_downloadingInfo.targetDownloadSize;
        actionUpdateProgress?.Invoke(value);
    }

    /// <summary>
    /// �����쳣
    /// </summary>
    private void OnDownloadError(UnityWebRequest.Result result)
    {
        actionShowErrorTips?.Invoke(result);
    }

    /// <summary>
    /// ɾ���ļ�
    /// </summary>
    private void DeleteFile(string filePath)
    {
        GameLogger.LogGreen("ɾ���ļ���" + filePath);
        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    /// <summary>
    /// ���MD5
    /// </summary>
    /// <param name="filePath">Ҫ�����ļ�·��</param>
    /// <param name="md5">Ŀ��MD5</param>
    /// <returns></returns>
    private bool CheckMd5(string filePath, string md5)
    {
        var localMd5 = LuaFramework.Util.md5file(filePath);
        var ok = localMd5.Equals(md5);
        GameLogger.LogGreen("MD5У�飬ok: " + ok);
        return ok;
    }

    /// <summary>
    /// ���HTTPS֤������
    /// </summary>
    private bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None) return true;
        bool isOk = true;
        for (int i = 0; i < chain.ChainStatus.Length; i++)
        {
            if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
            {
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                bool chainIsValid = chain.Build((X509Certificate2)certificate);
                if (!chainIsValid)
                {
                    isOk = false;
                    break;
                }
            }
        }
        return isOk;
    }

    public void Dispose()
    {
        if (null != m_downloader)
            m_downloader.Dispose();
    }

    public class UpdateInfo
    {
        public string appVersion;
        public string appUrl;
        public List<PackInfo> updateList;
    }

    public class PackInfo
    {
        public string resVersion;
        public string md5;
        public int size;
        public string url;

        public override string ToString()
        {
            return string.Format("resVersion: {0}, md5: {1}, size: {2}, url: {3}", resVersion, md5, size, url);
        }
    }

    public struct DownloadingInfo
    {
        public long curDownloadSize;
        public long targetDownloadSize;
        public int packIndex;
        public int totalPackCount;
    }
}


