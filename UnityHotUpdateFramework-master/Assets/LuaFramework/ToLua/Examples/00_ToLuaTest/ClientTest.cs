using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.Reflection;

public class ClientTest : LuaClient 
{

    LuaThread thread = null;

    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    protected override void OpenLibs()
    {
        base.OpenLibs();
        OpenCJson();
    }

    protected override void OnLoadFinished()
    {
        base.OnLoadFinished();

        //如果代码里有require需要在dofile之前注册
        Bind(luaState);

        string fullPath = Application.dataPath + "\\LuaFramework/ToLua/Examples/00_ToLuaTest";
        luaState.AddSearchPath(fullPath);
        luaState.DoFile("CoroutineTest.lua");
        luaState.DoFile("OverloadTest.lua");

        //CoroutineTest();
        //OverloadTest();
        ReflectionTest();
    }

    void Bind(LuaState state)
    {
        state.BeginModule(null);
        TestExportWrap.Register(state);
        state.BeginModule("TestExport");
        TestExport_SpaceWrap.Register(state);
        state.EndModule();
        state.EndModule();
    }

    private void ReflectionTest()
    {
        //Type t = typeof(TestExport);
        //MethodInfo md = t.GetMethod("TestReflection");
        //md.Invoke(null, null);

        //Vector3[] array = new Vector3[] { Vector3.zero, Vector3.one };
        //object obj = Activator.CreateInstance(t, array);
        //md = t.GetMethod("Test", new Type[] { typeof(int).MakeByRefType() });
        //object o = 123;
        //object[] args = new object[] { o };
        //object ret = md.Invoke(obj, args);
        //Debugger.Log(ret + " : " + args[0]);

        //PropertyInfo p = t.GetProperty("Number");
        //int num = (int)p.GetValue(obj, null);
        //Debugger.Log("object Number: {0}", num);
        //p.SetValue(obj, 456, null);
        //num = (int)p.GetValue(obj, null);
        //Debugger.Log("object Number: {0}", num);

        //FieldInfo f = t.GetField("field");
        //num = (int)f.GetValue(obj);
        //Debugger.Log("object field: {0}", num);
        //f.SetValue(obj, 2048);
        //num = (int)f.GetValue(obj);
        //Debugger.Log("object field: {0}", num);


        LuaFunction func = luaState.GetFunction("ReflectionTest");
        func.Call();

        func.Dispose();
    }

    private void OverloadTest()
    {
        TestExport to = new TestExport();
        LuaFunction func = luaState.GetFunction("OverloadTest");
        func.Call(to);

        func.Dispose();
    }

    private void CoroutineTest()
    {
        TextAsset text = (TextAsset)Resources.Load("jsonexample", typeof(TextAsset));
        string str = text.ToString();

        LuaFunction func = luaState.GetFunction("CoroutineTest");
        func.BeginPCall();
        func.Push(str);
        func.PCall();
        thread = func.CheckLuaThread();
        thread.name = "LuaThread";
        func.EndPCall();
        func.Dispose();

        thread.Resume(10);
    }

    //屏蔽，例子不需要运行
    protected override void CallMain() { }

    protected void Start(){}

    public override void Destroy()
    {
        base.Destroy();

        if (thread != null)
        {
            thread.Dispose();
            thread = null;
        }

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 50, 120, 40), "Resume Thead"))
        {
            int ret = -1;

            if (thread != null && thread.Resume(true, out ret) == (int)LuaThreadStatus.LUA_YIELD)
            {
                Debugger.Log("lua yield: " + ret);
            }
        }
        else if (GUI.Button(new Rect(10, 150, 120, 40), "Close Thread"))
        {
            if (thread != null)
            {
                thread.Dispose();
                thread = null;
            }
        }
        else if (GUI.Button(new Rect(10, 460, 120, 40), "Force GC"))
        {
            //自动gc log: collect lua reference name , id xxx in thread 
            luaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT, 0);
            GC.Collect();
        }
    }

}
