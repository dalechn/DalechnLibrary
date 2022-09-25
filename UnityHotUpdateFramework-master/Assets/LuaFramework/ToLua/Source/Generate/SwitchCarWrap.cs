﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class SwitchCarWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(SwitchCar), typeof(frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>));
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("itemPrefab", get_itemPrefab, set_itemPrefab);
		L.RegVar("closeBtn", get_closeBtn, set_closeBtn);
		L.EndClass();
	}


	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_itemPrefab(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			SwitchCar obj = (SwitchCar)o;
			UnityEngine.RectTransform ret = obj.itemPrefab;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index itemPrefab on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_closeBtn(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			SwitchCar obj = (SwitchCar)o;
			UnityEngine.UI.Button ret = obj.closeBtn;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index closeBtn on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_itemPrefab(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			SwitchCar obj = (SwitchCar)o;
			UnityEngine.RectTransform arg0 = (UnityEngine.RectTransform)ToLua.CheckObject(L, 2, typeof(UnityEngine.RectTransform));
			obj.itemPrefab = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index itemPrefab on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_closeBtn(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			SwitchCar obj = (SwitchCar)o;
			UnityEngine.UI.Button arg0 = (UnityEngine.UI.Button)ToLua.CheckObject<UnityEngine.UI.Button>(L, 2);
			obj.closeBtn = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index closeBtn on a nil value");
		}
	}
}
