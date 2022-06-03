using System.Collections;
using UnityEngine;
using UnityEngine.Internal;
using System.Collections.Generic;

namespace Dalechn
{

    //[AddComponentMenu("Test/Functest")] //将脚本添加到Component菜单中。在Hierarchy中选中对象后可以快速将脚本添加到对象
    //[HelpURL("https://carloswilkes.com/Documentation/LeanTouch#LeanTouch")]	//文档链接
    //[System.Serializable] //序列化一个类，使其能在Inspector面板中显示
    //[DefaultExecutionOrder(-100)] // 定义脚本执行顺序
    //[DisallowMultipleComponent] //不允许添加多个组件
    //[RequireComponent(typeof(Functest))] //依赖某个组件

    //[ExecuteInEditMode] // 允许在编辑器模式也能运行
    //[ExecuteAlways] // 改进版,支持Prefab Mode下的脚本调用
    [Invector.vClassHeader("TansformTest")]
    public  class UnityTest : bl_MonoBehaviour
    {
        //[Tooltip("Functest")]  //鼠标悬停显示提示信息
        //[Header("Funtest")] //添加标题
        //[Space(10)]  //添加空行

        //[System.NonSerialized] // system.不序列化public
        //[SerializeField] // unityengine.序列化private
        //[HideInInspector] // unityengine.不序列化public
        //[Multiline] // string设置区域变大
        //[TextAreaAttribute] //string设置区域变大且换行
        //[Range(0, 10)]  //数值滑动条

        //[vCheckProperty] //未测试
        //[Invector.vButton] //添加按钮在inspector调用方法

        //[Invector.vReadOnly(false)] // 让inspector无法编辑
        //public float testRead;

        //[vMinMax(minLimit = -45, maxLimit = 80)]
        //public Vector2 testScroll;

        //[vToggleOption] //对bool类型加提示
        //public bool hide = true;
        //[Invector.vHideInInspector("hide",true)] // 隐藏inspector属性
        //[vBarDisplay("health")] // 状态栏
        //public float health = 0;

        [Invector.vEditorToolbar("Common")]
        //[vHelpBox("Common", vHelpBoxAttribute.MessageType.Info)]
        //[vSeparator("Common")]
        public LayerMask finalLayer;
        public LayerMask testLayer;

        public RotDebug prefab;

        //  快捷键与字符串用空格分割，_w: 单一的快捷键     #w shift+w     %w: ctrl+w     &w: Alt+w
        //[UnityEditor.MenuItem("Test/TestFuncStatic")] //将static方法添加到菜单栏中
        //[UnityEditor.MenuItem("CONTEXT/Functest/TestFuncStatic")] //将static方法添加到脚本右侧的设置菜单中 (必须带上类名)
        public static void TestFuncStatic([DefaultValue("0")]int i) { } // defaultvalue好像只是起到提示作用

        //[ContextMenu("TestFunc")]  //将方法添加到脚本右侧的设置菜单中（不用带上类名,不用静态类）
        public void TestFunc() { }

        private void ObjTest()
        {
            GameObject obj = new GameObject();
            RotDebug rotDebug = Instantiate(prefab, transform);
            if (rotDebug)
            {
                rotDebug.target = transform;
                //rotDebug.enabled = false;
                //rotDebug.gameObject.SetActive(false);
            }

            // 不返回gameObject.active = false,可返回transform.enable = false
            RotDebug rotDebugTest = FindObjectOfType<RotDebug>();
            Debug.Log(rotDebugTest);
            rotDebugTest = GetComponentInChildren<RotDebug>();
            Debug.Log(rotDebugTest);
            RotDebug[] rotDebugTests = GetComponentsInChildren<RotDebug>();
            foreach (var val in rotDebugTests)
            {
                Debug.Log(val);
            }

            obj.hideFlags = HideFlags.DontSave;        //需手动清理对象
            obj.hideFlags |= HideFlags.NotEditable;  // Inspector不可编辑
                                                     //obj.hideFlags |= HideFlags.HideInHierarchy; //Hierarchy不可见
            obj.hideFlags |= HideFlags.HideInInspector; //Inspector不可见

            //Destroy(obj);

            //gameObject 或者component都可以执行
            //当前gameobject messaga+ 子gameobject message
            gameObject.BroadcastMessage("BroadcastMessageTest", SendMessageOptions.DontRequireReceiver);

            // 当前gameobject messaga
            gameObject.SendMessage("SendMessageTest");

            // 当前gameobject messaga+ 父gameobject message
            SendMessageUpwards("SendMessageUpwardsTest");

            //Debug.Log(transform.hierarchyCapacity+" "+transform.hierarchyCount); //未知
            transform.SetSiblingIndex(0); // 设置hierarchy位置

        }

        private void SendMessageUpwardsTest()
        {
            Debug.Log("SendMessageUpwardsTest");
        }

        private void SendMessageTest()
        {
            Debug.Log("SendMessageTest");
        }

        private void BroadcastMessageTest()
        {
            Debug.Log("BroadcastMessageTest");
        }

        private void MaskTest()
        {
            string layerName = LayerMask.LayerToName(gameObject.layer);
            Debug.Log(layerName);
            int layer = LayerMask.NameToLayer(layerName);

            //testLayer = 1 << layer; // 开启某个layer
            testLayer = ~(1 << layer); // 开启除了layer
            Debug.Log(testLayer.value); //0代表nothing,-1代表everything,其他layer从1开始计数

            //finalLayer |= testLayer; // 开启testLayer ,需要 testLayer = 1 << layer
            //finalLayer ^= testLayer; // 开关testLayer , 需要testLayer = 1 << layer
            finalLayer &= testLayer; // 关闭testLayer, 需要testLayer = ~(1 << layer)
            if ((finalLayer.value & (1 << testLayer)) != 0)// 射线检测原理
            {
                Debug.Log(true);
            }
        }

        //未知 :具体执行顺序还需要参考官方文档?
        // 即使enable=false,也会执行
        //private void Awake()
        //{
        //    Debug.Log("OnAwake");
        //}

        // SetActive 和 enable = true的时候会执行
        //private void OnEnable()
        //{
        //    Debug.Log("OnEnable");
        //}


        //编辑模式下，第一次给物体添加脚本或者手动点击reset脚本以后执行
        //private void Reset()
        //{
        //    Debug.Log("Reset");
        //}

        private void Start()
        {
            //gameObject.AddComponent<UnityCSharpTest>();
            //gameObject.AddComponent<GimbalLockTest>();
            //gameObject.AddComponent<FloatTest>();
            //gameObject.AddComponent<UnityCoroutineTest>();
            gameObject.AddComponent<RandomTest>();
        }

        //private void FixedUpdate()
        //{

        //}

        //private void OnTriggerEnter(Collider other)
        //{

        //}


        //private void OnCollisionEnter(Collision collision)
        //{

        //}

        //private void Update()
        //{
        //}

        //private void LateUpdate()
        //{

        //}

        private void OnAnimatorMove()
        {
            //Debug.Log("OnAnimatorMove");
        }

        private void OnAnimatorIK(int layerIndex)
        {
            //Debug.Log("OnAnimatorIK");
        }


        //public static CameraCallback onPreCull;
        //public static CameraCallback onPreRender;
        //public static CameraCallback onPostRender;

        //在相机剔除场景之前调用此函数,需要挂在相机脚本下
        private void OnPreCull()
        {
            //Debug.Log("OnPreCull");
        }

        //在摄像机渲染场景前调用,需要挂在相机脚本下
        private void OnPreRender()
        {
            //Debug.Log("OnPreRender");
        }

        //在摄像机渲染场景后调用,需要挂在相机脚本下
        private void OnPostRender()
        {
            //Debug.Log("OnPostRender");
        }


        //如果对象可见，则为每个相机调用一次此函数
        private void OnWillRenderObject()
        {
            //Debug.Log("OnWillRenderObject");
        }

        //在对象对于相机可见调用此函数
        private void OnBecameVisible()
        {
            //Debug.Log("OnBecameVisible");
        }

        //在对象对于相机不可见调用此函数
        private void OnBecameInvisible()
        {
            //Debug.Log("OnBecameInvisible");
        }

        //在摄像机渲染场景后调用
        private void OnRenderObject()
        {
            //Debug.Log("OnRenderObject");
        }

        //在完成场景渲染后调用此函数，以便对屏幕图像进行后处理
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            //Debug.Log("OnRenderImage");
        }


        //强制暂停时，先 OnApplicationPause，后 OnApplicationFocus；
        //重新“启动”手机时，先OnApplicationFocus，后 OnApplicationPause；
        //游戏进入后台时执行该方法 pause为true 切换回前台时pause为false
        private void OnApplicationPause(bool pause)
        {
            //Debug.Log("OnApplicationPause");
        }

        //游戏失去焦点也就是进入后台时 focus为false 切换回前台时 focus为true
        private void OnApplicationFocus(bool focus)
        {
            //Debug.Log("OnApplicationFocus");
        }

        private void OnApplicationQuit()
        {
            //Debug.Log("OnApplicationQuit");
        }

        //private void OnDisable()
        //{
        //    Debug.Log("OnDisable");
        //}

        ////物体被销毁时执行
        //private void OnDestroy()
        //{
        //    Debug.Log("OnDestroy");
        //}


        // 当加载脚本或在检查器中更改值时调用此函数（仅在编辑器中调用）
        private void OnValidate()
        {
            //Debug.Log("OnValidate");
        }
        private void OnDrawGizmos()
        {

        }

        private void OnDrawGizmosSelected()
        {

        }

        private void OnGUI()
        {

        }

    }


    //帧率
    //case CustomFixedTimeStep.FPS30:
    //    Time.fixedDeltaTime = 0.03333334f;
    //    break;
    //case CustomFixedTimeStep.FPS60:
    //    Time.fixedDeltaTime = 0.01666667f;
    //    break;
    //case CustomFixedTimeStep.FPS75:
    //    Time.fixedDeltaTime = 0.01333333f;
    //    break;
    //case CustomFixedTimeStep.FPS90:
    //    Time.fixedDeltaTime = 0.01111111f;
    //    break;
    //case CustomFixedTimeStep.FPS120:
    //    Time.fixedDeltaTime = 0.008333334f;
    //    break;
    //case CustomFixedTimeStep.FPS144:
    //    Time.fixedDeltaTime = 0.006944444f;
    //    break;


    //          Application.dataPath;  //resource只读目录
    //        Application.persistentDataPath; // 数据文件存储目录
    //        Application.streamingAssetsPath; //streamingAssets只读目录

}
