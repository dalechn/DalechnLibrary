using LuaFramework;
using LuaInterface;
using UnityEngine;

/// <summary>
/// 游戏入口脚本
/// </summary>
public class Main : MonoBehaviour
{
    private void Awake()
    {
        // 初始化一些必要的管理器
        GameLogger.Init();
        VersionMgr.instance.Init();
        PanelMgr.instance.Init();

        // *加载配置文件
        ResourcesCfg.instance.LoadCfg();
        // 预加载基础的Bundle
        ResourceMgr.instance.PreloadBaseBundle();

        // *加载UpdatePanel
        PanelMgr.instance.ShowPanel("UpdatePanel", 4);


    }

}