using UnityEngine;
using System.Collections.Generic;
using LuaInterface;
using System;

public class ToLuaTest : MonoBehaviour
{
    private string script =
        @"
print('hello world')
        ";

    public TextAsset luaFile = null;

    private LuaLooper looper = null;
    private LuaState lua;

    void Start()
    {
        new LuaResLoader();

        lua = new LuaState();
        //lua.LogGC = true;
        lua.Start();

        //未知:LuaLooper是干什么的?
        looper = gameObject.AddComponent<LuaLooper>();
        looper.luaState = lua;

        // 未知:绑定c#的类?
        DelegateFactory.Init();
        LuaBinder.Bind(lua);
        LuaCoroutine.Register(lua, this);

        //1. 从字符串加载脚本
        //lua.DoString(script);
        //lua.DoString(script, "ToLuaTest.cs"); //第二个参数是干什么的?

        //2. 从TextAsset加载脚本,把lua文件后缀名加上.bytes
        //lua.DoString(luaFile.text, "ToLuaTest.lua");

        //3. 从文件加载lua脚本
        string fullPath = Application.dataPath + "\\LuaFramework/ToLua/Examples/00_ToLuaTest";
        lua.AddSearchPath(fullPath);
        lua.DoFile("BasicTest.lua");
        lua.Require("DatastructTest");      //和dofile有什么区别?

        //TableTest();
        FuncTest();
        //DelegateTest();
        //DataTest();

        //DataStructTest();
    }

    void DataTest()
    {
        // string
        LuaFunction func = lua.GetFunction("TestString");
        func.Call();

        // int
        func = lua.GetFunction("TestInt64");
        func.BeginPCall();
        func.Push(9223372036854775807 - 789);
        func.PCall();
        long n64 = func.CheckLong();
        Debugger.Log("int64 return from lua is: {0}", n64);
        func.EndPCall();

        func.Dispose();
    }

    void Update()
    {
        // out test
        if (Input.GetMouseButtonDown(0))
        {
            Camera camera = Camera.main;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool flag = Physics.Raycast(ray, out hit, 5000, 1 << LayerMask.NameToLayer("Default"));

            if (flag)
            {
                Debugger.Log("pick from c#, point: [{0}, {1}, {2}]", hit.point.x, hit.point.y, hit.point.z);
            }

            LuaFunction  func = lua.GetFunction("TestPick");

            func.BeginPCall();
            func.Push(ray);
            func.PCall();
            func.EndPCall();

            func.Dispose();
        }

        //这两个是干什么的
        lua.Collect();
        lua.CheckTop();
    }

    protected void OnDestroy()
    {
        if(lua!=null)
        {
            lua.Dispose();
            lua = null;
        }
    }

    protected void OnApplicationQuit()
    {
        if (lua != null)
        {
            lua.Dispose();
            lua = null;
        }
    }

    TestEventListener listener = null;
    void DelegateTest()
    {
        TestEventListener.Bind(lua);

        GameObject go = new GameObject("TestGo");
        listener = (TestEventListener)go.AddComponent(typeof(TestEventListener));

        // 1. 在c#端添加lua的回调函数
        LuaFunction func = lua.GetFunction("DoClick1");
        // 添加回调函数
        TestEventListener.OnClick onClick = (TestEventListener.OnClick)DelegateTraits<TestEventListener.OnClick>.Create(func);
        listener.onClick += onClick;

        //执行回调函数
        listener.onClick(gameObject);

        //移除回调函数
        listener.onClick = (TestEventListener.OnClick)DelegateFactory.RemoveDelegate(listener.onClick, func);

        // 2.在lua端添加lua的回调函数 
        func = lua.GetFunction("AddClick");
        func.BeginPCall();
        func.Push(listener);
        func.PCall();
        func.EndPCall();

        //执行回调函数
        listener.onClick(gameObject);
        listener.OnClickEvent(gameObject);

        func.Dispose();
    }

    void TableTest()
    {
        //1.通过LuaState访问
        lua["val"] = 5;
        Debugger.Log("Read table var from lua: {0}", lua["val"]);  //LuaState 拆串式table

        //2.cache成LuaTable进行访问
        LuaTable table = lua.GetTable("varTable");
        table["map.name"] = "new";  //table 字符串只能是key
        Debugger.Log("Read varTable from lua, default: {0} name: {1}", table["default"], table["map.name"]);

        table.AddTable("newmap");
        LuaTable table1 = (LuaTable)table["newmap"];
        table1["name"] = "table1";
        Debugger.Log("varTable.newmap name: {0}", table1["name"]);
        table1.Dispose();

        table1 = table.GetMetaTable();

        if (table1 != null)
        {
            Debugger.Log("varTable metatable name: {0}", table1["name"]);
        }

        object[] list = table.ToArray();

        for (int i = 0; i < list.Length; i++)
        {
            Debugger.Log("varTable[{0}], is {1}", i, list[i]);
        }

        table.Dispose();

        // enum test
        lua["space"] = CameraClearFlags.Color;

        LuaFunction func = lua.GetFunction("TestEnum");
        func.BeginPCall();
        func.Push(Camera.main);

        func.Push(CameraClearFlags.Color);
        func.PCall();
        func.EndPCall();

        func.Dispose();
    }

    void DataStructTest()
    {
        // 1. array test
        //int[] array = { 1, 2, 3, 4, 5 };
        //LuaFunction func = lua.GetFunction("TestArray");

        //func.BeginPCall();
        //func.Push(array);
        //func.PCall();
        //double arg1 = func.CheckNumber();
        //string arg2 = func.CheckString();
        //bool arg3 = func.CheckBoolean();
        //Debugger.Log("return is {0} {1} {2}", arg1, arg2, arg3);
        //func.EndPCall();

        ////调用通用函数需要转换一下类型，避免可变参数拆成多个参数传递
        //object[] objs = func.LazyCall((object)array);

        //if (objs != null)
        //{
        //    Debugger.Log("return is {0} {1} {2}", objs[0], objs[1], objs[2]);
        //}

        // 2. list test
        List<int> list1 = new List<int>();
        list1.Add(1);
        list1.Add(2);
        list1.Add(4);

        LuaFunction func = lua.GetFunction("TestList");
        func.BeginPCall();
        func.Push(new List<int>());
        func.Push(list1);
        func.PCall();
        func.EndPCall();

        // 3. dictionary test
        //BindMap(lua);

        //Dictionary<int, TestAccount> map = new Dictionary<int, TestAccount>();
        //map.Add(1, new TestAccount(1, "水水", 0));
        //map.Add(2, new TestAccount(2, "王伟", 1));
        //map.Add(3, new TestAccount(3, "王芳", 0));

        //LuaFunction func = lua.GetFunction("TestDict");
        //func.BeginPCall();
        //func.Push(map);
        //func.PCall();
        //func.EndPCall();

        func.Dispose();
    }

    //示例方式，方便删除，正常导出无需手写下面代码
    void BindMap(LuaState L)
    {
        L.BeginModule(null);
        TestAccountWrap.Register(L);
        L.BeginModule("System");
        L.BeginModule("Collections");
        L.BeginModule("Generic");
        System_Collections_Generic_Dictionary_int_TestAccountWrap.Register(L);
        System_Collections_Generic_KeyValuePair_int_TestAccountWrap.Register(L);
        L.BeginModule("Dictionary");
        System_Collections_Generic_Dictionary_int_TestAccount_KeyCollectionWrap.Register(L);
        System_Collections_Generic_Dictionary_int_TestAccount_ValueCollectionWrap.Register(L);
        L.EndModule();
        L.EndModule();
        L.EndModule();
        L.EndModule();
        L.EndModule();
    }

    void FuncTest()
    {
        LuaFunction func = lua["TestFunc"] as LuaFunction;

        //0.
        //func.Call();                              //无参数
        //func.Call<int>(123456);       //有参数

        //1.                                      //有返回值,参数可选
        func.BeginPCall();
        func.Push(123456);
        func.PCall();
         int  num = (int)func.CheckNumber();
        func.EndPCall();
        Debugger.Log("expansion call return: {0}", num);

        ////2.
        //num = func.Invoke<int, int>(123456);
        //Debugger.Log("generic call return: {0}", num);

        ////3.
        //Func<int, int> delegateFunc = func.ToDelegate<Func<int, int>>();
        //num = delegateFunc(123456);
        //Debugger.Log("Delegate call return: {0}", num);

        ////4.
        //num = lua.Invoke<int, int>("TestFunc", 123456, true);
        //Debugger.Log("luastate call return: {0}", num);


        func.Dispose();
    }

}
