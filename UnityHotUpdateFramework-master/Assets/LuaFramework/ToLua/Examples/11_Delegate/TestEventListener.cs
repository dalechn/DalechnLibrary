using UnityEngine;
using System;
using System.Collections;
using LuaInterface;

public sealed class TestEventListener : MonoBehaviour
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void OnClick(GameObject go);
    public OnClick onClick = delegate { };

    public event OnClick onClickEvent = delegate { };

    public Func<bool> TestFunc = null;

    public void SetOnFinished(OnClick click)
    {
        Debugger.Log("SetOnFinished OnClick");
    }

    public void SetOnFinished(VoidDelegate click)
    {
        Debugger.Log("SetOnFinished VoidDelegate");
    }

    [NoToLuaAttribute]
    public void OnClickEvent(GameObject go)
    {
        onClickEvent(go);
    }


    public static void Bind(LuaState L)
    {
        L.BeginModule(null);
        TestEventListenerWrap.Register(L);
        L.EndModule();

        DelegateFactory.dict.Add(typeof(TestEventListener.OnClick), TestEventListener.TestEventListener_OnClick);
        DelegateFactory.dict.Add(typeof(TestEventListener.VoidDelegate), TestEventListener.TestEventListener_VoidDelegate);

        DelegateTraits<TestEventListener.OnClick>.Init(TestEventListener.TestEventListener_OnClick);
        DelegateTraits<TestEventListener.VoidDelegate>.Init(TestEventListener.TestEventListener_VoidDelegate);
    }


    public static TestEventListener.OnClick TestEventListener_OnClick(LuaFunction func, LuaTable self, bool flag)
    {
        if (func == null)
        {
            TestEventListener.OnClick fn = delegate { };
            return fn;
        }

        TestEventListener_OnClick_Event target = new TestEventListener_OnClick_Event(func);
        TestEventListener.OnClick d = target.Call;
        target.method = d.Method;
        return d;
    }

    public static TestEventListener.VoidDelegate TestEventListener_VoidDelegate(LuaFunction func, LuaTable self, bool flag)
    {
        if (func == null)
        {
            TestEventListener.VoidDelegate fn = delegate { };
            return fn;
        }

        TestEventListener_VoidDelegate_Event target = new TestEventListener_VoidDelegate_Event(func);
        TestEventListener.VoidDelegate d = target.Call;
        target.method = d.Method;
        return d;
    }

}


//自动生成代码后拷贝过来
class TestEventListener_OnClick_Event : LuaDelegate
{
    public TestEventListener_OnClick_Event(LuaFunction func) : base(func) { }

    public void Call(UnityEngine.GameObject param0)
    {
        func.BeginPCall();
        func.Push(param0);
        func.PCall();
        func.EndPCall();
    }
}

class TestEventListener_VoidDelegate_Event : LuaDelegate
{
    public TestEventListener_VoidDelegate_Event(LuaFunction func) : base(func) { }

    public void Call(UnityEngine.GameObject param0)
    {
        func.BeginPCall();
        func.Push(param0);
        func.PCall();
        func.EndPCall();
    }
}
