using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class UpdatePanel : BasePanel
{
    private Text m_versionText;
    private Slider m_progressSlider;
    private Text m_progressText;
    private Text m_tipsText;
    private GameObject m_appUpdateDlg;
    private Button m_nextBtn;
    private Button m_fullAppUpdateBtn;
    private GameObject m_errorTipsDlg;
    private Text m_errorText;
    private Button m_retryBtn;

    private HotUpdater m_hotUpdater;
    //public Action m_cb;

    //public static void Create(Action cb)
    //{
    //    var panel = PanelMgr.instance.ShowPanel<UpdatePanel>("UpdatePanel", "BaseRes/UpdatePanel.prefab");
    //    panel.m_cb = cb;
    //}

    void Awake()
    {
        var uiBinder = gameObject.GetComponent<PrefabObjBinder>();
        m_versionText = uiBinder.GetObj<Text>("versionText");
        m_progressSlider = uiBinder.GetObj<Slider>("progressSlider");
        m_progressText = uiBinder.GetObj<Text>("progressText");
        m_tipsText = uiBinder.GetObj<Text>("tipsText");
        m_appUpdateDlg = uiBinder.GetObj<GameObject>("appUpdateDlg");
        m_nextBtn = uiBinder.GetObj<Button>("nextBtn");
        m_fullAppUpdateBtn = uiBinder.GetObj<Button>("updateBtn");
        m_errorTipsDlg = uiBinder.GetObj<GameObject>("errorTipsDlg");
        m_errorText = uiBinder.GetObj<Text>("errorText");
        m_retryBtn = uiBinder.GetObj<Button>("retryBtn");

        m_nextBtn.onClick.AddListener(OnNextBtnClick);
        m_fullAppUpdateBtn.onClick.AddListener(OnFullAppUpdateBtnClick);
        m_retryBtn.onClick.AddListener(OnRetryBtnClick);
    }

    /// <summary>
    /// �ȸ��º�
    /// </summary>
    private void AfterHotUpdate()
    {
        Debug.Log("AfterHotUpdate");

        // Ԥ����Lua��AssetBundle
        ResourceMgr.instance.PreloadLuaBundles();

        // ����lua���
        AppFacade.Instance.StartUp();

    }

    void Start()
    {
        // �����ȸ���
        m_versionText.text = string.Format("app: {0} res: {1}", VersionMgr.instance.appVersion, VersionMgr.instance.resVersion);
        m_progressSlider.value = 0;
        m_progressText.text = "0%";
        m_tipsText.text = "����������£����Ե�...";

        m_hotUpdater = new HotUpdater();
        m_hotUpdater.actionForceFullAppUpdate = ShowForceAppUpdateDlg;
        m_hotUpdater.actionWeakFullAppUpdate = ShowUnForceAppUpdateDlg;
        m_hotUpdater.actionShowErrorTips = ShowErrorTips;
        m_hotUpdater.actionUpdateTipsText = UpdateTipsText;
        m_hotUpdater.actionNothongUpdate = NothingUpdate;
        m_hotUpdater.actionUpdateProgress = UpdateProgress;
        m_hotUpdater.actionAllDownloadDone = OnAllDownloadDone;
        m_hotUpdater.Init();
        m_hotUpdater.Start();

        //m_cb = AfterHotUpdate;
    }

    private void ShowForceAppUpdateDlg()
    {
        m_appUpdateDlg.SetActive(true);
        m_nextBtn.gameObject.SetActive(false);
    }

    private void ShowUnForceAppUpdateDlg()
    {
        m_appUpdateDlg.SetActive(true);
        m_nextBtn.gameObject.SetActive(true);
    }

    private void ShowErrorTips(UnityWebRequest.Result error)
    {
        m_errorTipsDlg.SetActive(true);
        switch (error)
        {
            case UnityWebRequest.Result.ConnectionError:
                {
                    m_errorText.text = "�������·�����ʧ�ܣ������ԣ�";
                }
                break;
            case UnityWebRequest.Result.DataProcessingError:
                {
                    m_errorText.text = "�����쳣���������粢���ԣ�";
                }
                break;
            default:
                {
                    m_errorText.text = "������·�����ʧ�ܣ������ԣ�";
                }
                break;
        }
    }

    private void UpdateTipsText(string tipsStr)
    {
        m_tipsText.text = tipsStr;
    }

    private void NothingUpdate()
    {
        Finish();
    }

    private void OnAllDownloadDone()
    {
        Finish();
    }

    private void OnNextBtnClick()
    {
        m_appUpdateDlg.SetActive(false);
        m_hotUpdater.DoNextTime();
    }

    private void OnFullAppUpdateBtnClick()
    {
        m_hotUpdater.DoFullAppUpdate();
    }

    private void OnRetryBtnClick()
    {
        m_errorTipsDlg.SetActive(false);
        m_hotUpdater.Start();
    }

    private void Update()
    {
        m_hotUpdater.Update();
    }

    /// <summary>
    /// ���½�����
    /// </summary>
    /// <param name="value"></param>
    private void UpdateProgress(float value)
    {
        m_progressSlider.value = value < 0.05f ? 0.05f : value;
        m_progressText.text = (100 * value).ToString("0.00") + "%";
    }

    private void Finish()
    {
        Destroy(gameObject);
        //m_cb?.Invoke();

        AfterHotUpdate();
    }

    private void OnApplicationQuit()
    {
        m_hotUpdater.Dispose();
    }
}