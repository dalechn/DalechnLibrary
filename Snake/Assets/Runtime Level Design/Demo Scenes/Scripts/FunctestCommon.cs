using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Events;

//[AddComponentMenu("Test/Functest")] //将脚本添加到Component菜单中。在Hierarchy中选中对象后可以快速将脚本添加到对象
//[HelpURL("https://carloswilkes.com/Documentation/LeanTouch#LeanTouch")]	//文档链接
//[System.Serializable] //序列化一个类，使其能在Inspector面板中显示
//[DefaultExecutionOrder(-100)] // 定义脚本执行顺序
//[DisallowMultipleComponent] //不允许添加多个组件
//[RequireComponent(typeof(Functest))] //依赖某个组件

//[ExecuteInEditMode] // 允许在编辑器模式也能运行
//[ExecuteAlways] // 改进版,支持Prefab Mode下的脚本调用

public partial class Functest : MonoBehaviour
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


    //  快捷键与字符串用空格分割，_w: 单一的快捷键     #w shift+w     %w: ctrl+w     &w: Alt+w
    //[UnityEditor.MenuItem("Test/TestFuncStatic")] //将static方法添加到菜单栏中
    //[UnityEditor.MenuItem("CONTEXT/Functest/TestFuncStatic")] //将static方法添加到脚本右侧的设置菜单中 (必须带上类名)
    public static void TestFuncStatic([DefaultValue("0")]int i) { } // defaultvalue好像只是起到提示作用
    //[ContextMenu("TestFunc")]  //将方法添加到脚本右侧的设置菜单中（不用带上类名,不用静态类）
    public void TestFunc() { }

    //[vHelpBox("Common", vHelpBoxAttribute.MessageType.Info)]
    [vSeparator("Common")]
    public LayerMask finalLayer;
    public LayerMask testLayer;

    //public GameObject prefab;
    public RotDebug prefab;

    public GenericInput horizontalInput;
    public GenericInput verticalInput;

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

    private void InputTest()
    {
        //// 0鼠标左键,1鼠标右键,2鼠标中键 
        //if (Input.GetMouseButton(0))
        //{
        //    Debug.Log("左键按住");
        //}
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    Debug.Log("Space按住");
        //}
        //if (Input.GetButtonDown("Horizontal"))
        //{
        //    Debug.Log("Horizontal按下");
        //}
        //if (Input.GetButtonUp("Fire3"))
        //{
        //    Debug.Log("Shift抬起");
        //}

        //自定义axis和GetButton需要在InputManager里配置
        //Horizontal和Vertical映射到w，a，s，d或者箭头键。
        //Fire1，Fire2，Fire3,Jump分别映射到Control(Ctrl),Option(Alt),Command(Shift),Space
        //MouseX和MouseY映射到鼠标移动的增量。
        //Debug.Log("Horizontal Axis,AxisRaw: " + Input.GetAxis("Horizontal") + " " + Input.GetAxisRaw("Horizontal")); //范围(-1,1), 范围-1,0,1(只有3个值)
        //Debug.Log("Mouse X,Y: " + Input.GetAxis("Mouse X") + " " + Input.GetAxis("Mouse Y")); //鼠标沿着屏幕X,Y移动时触发
        //Debug.Log("Mouse ScrollWheel: " + Input.GetAxis("Mouse ScrollWheel") + " " + Input.mouseScrollDelta); //当鼠标滚动轮滚动时触发
        //Debug.Log("Mouse mousePosition: " + Input.mousePosition + " " + Camera.main.ScreenToViewportPoint(Input.mousePosition));    //鼠标位置 ,位置归一化

        inputDir = new Vector3(horizontalInput.GetAxis(), 0, verticalInput.GetAxis());

    }

    private void FloatTest()
    {
        // Mathf.Epsilon = float.Epsilon = 1.401298E-45f
        // Quaternion.kEpsilon = 1E-06F;(10^-6, 0.000006)
        // Vector3.kEpsilon=1E - 05F;
        // Vector3.kEpsilonNormalSqrt = 1E-15F;

        Debug.Log(transform.position + " " + transform.rotation);

        // 低精度下Mathf.Approximately ,和0的判断,== ,<=, >= 都可以使用
        // 高精度下一般使用 Mathf.Abs(a - b) < 1E - 05F)
        Debug.Log(Mathf.Approximately(0, float.Epsilon)); //true

        float a = 0.01f, b = 0.01f;
        Debug.Log(Mathf.Abs(a - b) < 0.001f); //true
        Debug.Log(Mathf.Approximately(a, b)); //true

        a = 6.000025415f; b = 6.000004545f;
        Debug.Log(Mathf.Abs(a - b) < 0.001f); //true
        Debug.Log(Mathf.Approximately(a, b)); //false
        Debug.Log(Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)))); //6.000025E-06

        a = 8f; b = 8.0000004545f;
        Debug.Log(Mathf.Abs(a - b) < 0.001f); //true
        Debug.Log(Mathf.Approximately(a, b)); //true
        Debug.Log(Mathf.Max(Mathf.Abs(a), Mathf.Abs(b))); // 结果是8...离谱
        Debug.Log(Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)))); //8E-06

        a = 0.000001121f; b = 0;
        Debug.Log(Mathf.Abs(a) < 0.001f); //true
        Debug.Log(Mathf.Approximately(a, b)); //false
        Debug.Log(Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)))); //1.121E-12

        a = 0.00003f; b = 0;
        Debug.Log(Mathf.Abs(a) < 0.001f); //true
        Debug.Log(Mathf.Approximately(a, b)); //false
        Debug.Log(Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)))); //3E-11

    }

    private void MaskTest()
    {
        // 优先级 ！> && > ||

        string layerName = LayerMask.LayerToName(gameObject.layer);
        Debug.Log(layerName);
        int layer = LayerMask.NameToLayer(layerName);

        //testLayer = 1 << layer; // 开启某个layer
        testLayer = ~(1 << layer); // 开启除了layer
        Debug.Log(testLayer.value); //0代表nothing,-1代表everything,其他layer从1开始计数

        //finalLayer |= testLayer; // 开启testLayer ,需要 testLayer = 1 << layer
        finalLayer &= testLayer; // 关闭testLayer, 需要testLayer = ~(1 << layer)
        finalLayer &= testLayer; //常用于和0判断,射线检测原理,需要testLayer = 1 << layer
        //finalLayer ^= testLayer; // 开关testLayer , 需要testLayer = 1 << layer
    }

    IEnumerator CoroutineTest()
    {
        int f = 0;
        while (true)
        {
            // null 或者任意数字会让后续的代码在下一帧执行
            Debug.Log(Time.time);
            yield return null;
            //yield return 0;

            //   return WaitForEndOfFrame 会让后续的代码在 lateUpdtae  以后执行,不用等到下一帧
            Debug.Log(Time.time);
            yield return new WaitForEndOfFrame();

            //   return WaitForEndOfFrame 会让后续的代码在 fixedUpdate  以后执行,下一帧
            Debug.Log(Time.time);
            yield return new WaitForFixedUpdate();

            //   实时挂起时间等于给定时间除以 Time.timeScale
            Debug.Log(Time.time);
            yield return new WaitForSeconds(1f);

            //   无视 Time.timeScale
            Debug.Log(Time.time);
            yield return new WaitForSecondsRealtime(1f);

            f++;
            Debug.Log("【CorTest】Normal 运行一次协程，f值：" + f);
        }
    }

    // // 直接设定欧拉角(unity旋转顺序world: zxy  local: yxz)
    private void GimbalLockTest()
    {
        transform.eulerAngles = new Vector3(0, 90, 0); // y或者z旋转90度都会触发gimbal lock
        //transform.localEulerAngles = new Vector3(90f, 180f, -90f);
        //Log: (90,270,0),Inspector:(90,0,90)
        //Debug.Log(transform.localEulerAngles);

        StartCoroutine(ChangeRotation());
    }

    private IEnumerator ChangeRotation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            var original = transform.eulerAngles;
            original.x += 5f;
            transform.eulerAngles = original;
        }
    }

    [System.Serializable]
    public sealed class UnityEventInt : UnityEvent<int> { }

    public delegate void TestDelegate();

    public TestDelegate test;
    public event TestDelegate testEvent;
    public UnityAction<int> unityAction;

    public UnityEventInt unityEventInt;
    public UnityEvent unityEvent;

    private void TestDel()
    {
        //在类的外部，事件只能用“+=”和“-=”去订阅/取消订阅，如果是委托的话还可以使用“=”
        testEvent = () => { Debug.Log("testEvent1"); };
        testEvent += delegate () { Debug.Log("testEvent2"); };

        test = () => { Debug.Log("test1"); };
        test += delegate () { Debug.Log("test2"); };

        unityAction = (a) => { Debug.Log("unityAction1"); };
        unityAction += delegate (int a) { Debug.Log("unityAction2"); };

        unityEvent?.AddListener(() => { Debug.Log("unityEvent1"); });
        unityEvent?.AddListener(delegate () { Debug.Log("unityEvent2"); });

        unityEventInt?.AddListener(unityAction);

        test?.Invoke();
        testEvent?.Invoke();
        unityAction?.Invoke(1);
        unityEvent?.Invoke();
        unityEventInt?.Invoke(1);
    }

    private void DataStructTest()
    {
        // 有装箱,读取快增删慢
        ArrayList arrayList = new ArrayList() { 1, 2, 3, 4 };

        {
            // 无装箱,读取快增删慢
            List<People> intList = new List<People>();
            var fa = new People("Fa", 45, "man");
            intList.Add(new People("I", 26, "man"));
            intList.Add(fa);
            intList.Add(new People("Ma", 46, "woman"));
            intList.Add(new People("Sis", 12, "woman"));
            intList.Add(new People("Bro", 9, "man"));

            for (int i = intList.Count - 1; i >= 0; i--)
            {
                if (intList[i].age == 26)
                    intList.Remove(intList[i]);
            }

            intList[1].age = 100;

            //性能:Contains > Exists > Where > Any
            Debug.Log(intList.Contains(fa));
            Debug.Log(intList.Exists(e => e.age == 45));
            Debug.Log(intList.Where(e => e.age == 45).Any());
            Debug.Log(intList.Any(e => e.age == 45));

            People findVal = intList.Find(e => e.age == 45);
            List<People> findVals = intList.FindAll(e => e.age == 45);
            int findIndex = intList.FindIndex(e => e.age == 45);
            findVal = intList.FindLast(e => e.age == 45); // 找到最后一个
            findIndex = intList.FindLastIndex(e => e.age == 45);

            //intList.Sort(new TestComparer());
            //intList.Sort(delegate (People a, People b) { return a.age.CompareTo(b.age); });
            intList.Sort((a, b) => a.age.CompareTo(b.age));

            foreach (var val in intList)
            {
                Debug.Log(val.pName + " " + val.age);
            }

        }

        // 读取慢增删快
        LinkedList<int> linkedList = new LinkedList<int>();

        //先进先出
        Queue<string> queue = new Queue<string>();
        //先进后出
        Stack<string> stack = new Stack<string>();

        //确保唯一性
        HashSet<string> hashSet = new HashSet<string>();
        hashSet.Add("123");

        //有装箱操作
        Hashtable table = new Hashtable();
        table.Add(1, "table");
        Hashtable.Synchronized(table);//线程安全,只有一个线程写  多个线程读

        //字典不是线程安全 
        Dictionary<int, string> dic = new Dictionary<int, string>();
        dic.Add(1, "dic");

        // 确保唯一性 不允许重复
        SortedSet<string> sortedSet = new SortedSet<string>();
        sortedSet.Add("123");

        // 有装箱操作 不允许重复
        SortedList sortedList = new SortedList();//IComparer
        sortedList.Add(1, "sortedList");
        sortedList.Add(0, "sortedList");

        // 不允许重复
        SortedDictionary<int, string> sortedDic = new SortedDictionary<int, string>();
        sortedDic.Add(1, "sortedDic");
        sortedDic.Add(0, "sortedDic");

        //foreach (DictionaryEntry val in sortedList)
        //{
        //    Debug.Log(val.Key + " " + val.Value);
        //}
        //ConcurrentQueue 线程安全版本的Queue
        //ConcurrentStack 线程安全版本的Stack
        //ConcurrentDictionary 线程安全版本的Dictionary
    }


    // 当加载脚本或在检查器中更改值时调用此函数（仅在编辑器中调用）
    private void OnValidate()
    {
        //Debug.Log("OnValidate");
    }

    //private void Awake()
    //{

    //}

    //private void OnEnable()
    //{

    //}


    //编辑模式下，第一次给物体添加脚本或者手动点击reset脚本以后执行
    private void Reset()
    {
        Debug.Log("Reset");
    }

    //private void Start()
    //{

    //}

    //private void FixedUpdate()
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


    //private void OnTriggerEnter(Collider other)
    //{

    //}


    //private void OnCollisionEnter(Collision collision)
    //{

    //}

    //如果对象可见，则为每个相机调用一次此函数
    private void OnWillRenderObject()
    {
        //Debug.Log("OnWillRenderObject");
    }

    //在相机剔除场景之前调用此函数,需要挂在相机脚本下
    private void OnPreCull()
    {
        //Debug.Log("OnPreCull");
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


    //在摄像机渲染场景前调用,需要挂在相机脚本下
    private void OnPreRender()
    {
        //Debug.Log("OnPreRender");
    }


    //在摄像机渲染场景后调用
    private void OnRenderObject()
    {
        //Debug.Log("OnRenderObject");
    }


    //在摄像机渲染场景后调用,需要挂在相机脚本下
    private void OnPostRender()
    {
        //Debug.Log("OnPostRender");
    }

    //在完成场景渲染后调用此函数，以便对屏幕图像进行后处理
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Debug.Log("OnRenderImage");
    }

    //private void OnDrawGizmos()
    //{

    //}

    //private void OnDrawGizmosSelected()
    //{

    //}

    //private void OnGUI()
    //{

    //}

    //强制暂停时，先 OnApplicationPause，后 OnApplicationFocus；
    //重新“启动”手机时，先OnApplicationFocus，后 OnApplicationPause；
    //游戏进入后台时执行该方法 pause为true 切换回前台时pause为false
    void OnApplicationPause(bool pause)
    {
        //Debug.Log("OnApplicationPause");
    }

    //游戏失去焦点也就是进入后台时 focus为false 切换回前台时 focus为true
    void OnApplicationFocus(bool focus)
    {
        //Debug.Log("OnApplicationFocus");
    }

    private void OnApplicationQuit()
    {
        //Debug.Log("OnApplicationQuit");
    }


    //private void OnDisable()
    //{

    //}

    //// 物体被销毁时执行
    //private void OnDestroy()
    //{

    //}

}

public class People
{
    public string pName { get; set; }
    public int age { get; set; }
    public string sex { get; set; }

    public People(string name, int age, string sex)
    {
        this.pName = name;
        this.age = age;
        this.sex = sex;
    }
}

public class TestComparer : IComparer<People>
{
    public int Compare(People p1, People p2)
    {
        return p1.age.CompareTo(p2.age);
    }
}
