using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using System.Threading;


/// <summary>
/// ������
/// </summary>
public class Downloader
{
    private Stream m_fs, m_ns;
    private int m_readSize;
    private byte[] m_buff;
    private HotUpdater.PackInfo m_packInfo;


    /// <summary>
    /// ����״̬
    /// </summary>
    public DownloadState state { get; private set; }
    /// <summary>
    /// ��ǰ�����صĴ�С
    /// </summary>
    public long curDownloadSize { get; private set; }

    /// <summary>
    /// ֹͣ�߳�
    /// </summary>
    private bool m_stopThread = false;
    private Thread m_thread;


    public void Start(HotUpdater.PackInfo packInfo)
    {
        m_buff = new byte[1024*4];
        state = DownloadState.Ready;
        m_packInfo = packInfo;
        var httpReq = HttpWebRequest.Create(m_packInfo.url) as HttpWebRequest;
        httpReq.Timeout = 5000;
        // ��md5��Ϊ�ļ��������ļ�
        var savePath = Application.persistentDataPath + "/" + m_packInfo.md5;
        GameLogger.LogGreen("Downloader Start, savePath: " + savePath);
        m_fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write);
        curDownloadSize = m_fs.Length;
        if (curDownloadSize == m_packInfo.size)
        {
            state = DownloadState.End;
            Dispose();
            return;
        }
        else if (curDownloadSize > m_packInfo.size)
        {
            // ���ص��ļ�������ʵ�ʴ�С��ɾ����������
            curDownloadSize = 0;
            m_fs.Close();
            File.Delete(savePath);
            m_fs = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        }
        else if (curDownloadSize > 0)
        {
            GameLogger.LogGreen("��⵽�ϴ��ļ�����δ�������ϵ��������ϴ������ش�С: " + curDownloadSize);
            // ���ñ����ļ�������ʼλ��
            m_fs.Seek(curDownloadSize, SeekOrigin.Current);
            // ����Զ�̷����ļ�������ʼλ��
            httpReq.AddRange(curDownloadSize);
        }

        HttpWebResponse response;
        try
        {
            response = (HttpWebResponse)httpReq.GetResponse();
        }
        catch (System.Exception e)
        {
            GameLogger.LogError(e);
            state = DownloadState.ConnectionError;
            return;
        }

        GameLogger.Log("response.StatusCode: " + response.StatusCode);
        if (response.StatusCode != HttpStatusCode.PartialContent)
        {
            if (File.Exists(savePath))
            {
                m_fs.Close();
                m_fs = null;
                curDownloadSize = 0;
                File.Delete(savePath);
            }
            m_fs = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        }

        m_ns = response.GetResponseStream();

        // ����һ��������д�ļ��߳�
        if (null == m_thread)
        {
            m_stopThread = false;
            m_thread = new Thread(WriteThread);
            m_thread.Start();
        }
    }


    /// <summary>
    /// д�ļ��߳�
    /// </summary>
    private void WriteThread()
    {
        state = DownloadState.Ing;
        while (!m_stopThread)
        {
            try
            {
                var readSize = m_ns.Read(m_buff, 0, m_buff.Length);
                if (readSize > 0)
                {
                    m_fs.Write(m_buff, 0, readSize);
                    curDownloadSize += readSize;
                    Thread.Sleep(0);
                }
                else
                {
                    // ���
                    m_stopThread = true;
                    state = DownloadState.End;
                    Dispose();
                }
            }
            catch (System.Exception e)
            {
                // ���س���
                state = DownloadState.DataProcessingError;
                Dispose();
            }
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Dispose()
    {
        m_stopThread = true;
        if (null != m_fs)
        {
            m_fs.Close();
            m_fs = null;
        }
        if (null != m_ns)
        {
            m_ns.Close();
            m_ns = null;
        }
        m_packInfo = null;
        m_buff = null;
    }


    public enum DownloadState
    {
        Ready,
        Ing,
        ConnectionError,
        DataProcessingError,
        End,
    }
}
