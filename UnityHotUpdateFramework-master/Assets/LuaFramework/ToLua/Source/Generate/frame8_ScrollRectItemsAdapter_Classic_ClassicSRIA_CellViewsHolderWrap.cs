﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class frame8_ScrollRectItemsAdapter_Classic_ClassicSRIA_CellViewsHolderWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>), typeof(UnityEngine.MonoBehaviour), "ClassicSRIA_CellViewsHolder");
		L.RegFunction("Refresh", Refresh);
		L.RegFunction("ResetItems", ResetItems);
		L.RegFunction("InsertItems", InsertItems);
		L.RegFunction("RemoveItems", RemoveItems);
		L.RegFunction("SmoothScrollTo", SmoothScrollTo);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("viewport", get_viewport, set_viewport);
		L.RegVar("viewsHolders", get_viewsHolders, set_viewsHolders);
		L.RegVar("ScrollRectComponent", get_ScrollRectComponent, null);
		L.RegVar("ContentLayoutGroup", get_ContentLayoutGroup, null);
		L.RegVar("ScrollRectRT", get_ScrollRectRT, null);
		L.RegVar("IsHorizontal", get_IsHorizontal, null);
		L.RegVar("AbstractNormalizedPosition", get_AbstractNormalizedPosition, set_AbstractNormalizedPosition);
		L.RegVar("ContentSize", get_ContentSize, null);
		L.RegVar("ViewportSize", get_ViewportSize, null);
		L.RegVar("Padding", get_Padding, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Refresh(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
			obj.Refresh();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetItems(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				obj.ResetItems(arg0);
				return 0;
			}
			else if (count == 3)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				obj.ResetItems(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>.ResetItems");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InsertItems(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 3)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				obj.InsertItems(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 4);
				obj.InsertItems(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>.InsertItems");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveItems(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 3)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				obj.RemoveItems(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 4);
				obj.RemoveItems(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>.RemoveItems");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SmoothScrollTo(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				bool o = obj.SmoothScrollTo(arg0);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 3)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				bool o = obj.SmoothScrollTo(arg0, arg1);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 4)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
				bool o = obj.SmoothScrollTo(arg0, arg1, arg2);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 5)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
				float arg3 = (float)LuaDLL.luaL_checknumber(L, 5);
				bool o = obj.SmoothScrollTo(arg0, arg1, arg2, arg3);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 6)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
				float arg3 = (float)LuaDLL.luaL_checknumber(L, 5);
				System.Action arg4 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 6);
				bool o = obj.SmoothScrollTo(arg0, arg1, arg2, arg3, arg4);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 7)
			{
				frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)ToLua.CheckObject<frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
				float arg3 = (float)LuaDLL.luaL_checknumber(L, 5);
				System.Action arg4 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 6);
				bool arg5 = LuaDLL.luaL_checkboolean(L, 7);
				bool o = obj.SmoothScrollTo(arg0, arg1, arg2, arg3, arg4, arg5);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>.SmoothScrollTo");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
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
	static int get_viewport(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			UnityEngine.RectTransform ret = obj.viewport;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index viewport on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_viewsHolders(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			System.Collections.Generic.List<CellViewsHolder> ret = obj.viewsHolders;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index viewsHolders on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ScrollRectComponent(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			UnityEngine.UI.ScrollRect ret = obj.ScrollRectComponent;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index ScrollRectComponent on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ContentLayoutGroup(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			UnityEngine.UI.LayoutGroup ret = obj.ContentLayoutGroup;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index ContentLayoutGroup on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ScrollRectRT(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			UnityEngine.RectTransform ret = obj.ScrollRectRT;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index ScrollRectRT on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsHorizontal(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			bool ret = obj.IsHorizontal;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index IsHorizontal on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AbstractNormalizedPosition(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			float ret = obj.AbstractNormalizedPosition;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index AbstractNormalizedPosition on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ContentSize(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			float ret = obj.ContentSize;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index ContentSize on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ViewportSize(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			float ret = obj.ViewportSize;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index ViewportSize on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Padding(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			UnityEngine.RectOffset ret = obj.Padding;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Padding on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_viewport(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			UnityEngine.RectTransform arg0 = (UnityEngine.RectTransform)ToLua.CheckObject(L, 2, typeof(UnityEngine.RectTransform));
			obj.viewport = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index viewport on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_viewsHolders(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			System.Collections.Generic.List<CellViewsHolder> arg0 = (System.Collections.Generic.List<CellViewsHolder>)ToLua.CheckObject(L, 2, typeof(System.Collections.Generic.List<CellViewsHolder>));
			obj.viewsHolders = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index viewsHolders on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_AbstractNormalizedPosition(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder> obj = (frame8.ScrollRectItemsAdapter.Classic.ClassicSRIA<CellViewsHolder>)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.AbstractNormalizedPosition = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index AbstractNormalizedPosition on a nil value");
		}
	}
}
