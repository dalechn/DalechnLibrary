using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Internal;
using UnityEngine.Events;

public static class Extensions
{
    public static T ToEnum<T>(this string value, bool ignoreCase = true)
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    public static bool Contains<T>(this Enum value, Enum lookingForFlag) where T : struct
    {
        int intValue = (int)(object)value;
        int intLookingForFlag = (int)(object)lookingForFlag;
        return ((intValue & intLookingForFlag) == intLookingForFlag);
    }

    public static T[] Append<T>(this T[] arrayInitial, T[] arrayToAppend)
    {
        if (arrayToAppend == null)
        {
            throw new ArgumentNullException("The appended object cannot be null");
        }
        if ((arrayInitial is string) || (arrayToAppend is string))
        {
            throw new ArgumentException("The argument must be an enumerable");
        }
        T[] ret = new T[arrayInitial.Length + arrayToAppend.Length];
        arrayInitial.CopyTo(ret, 0);
        arrayToAppend.CopyTo(ret, arrayInitial.Length);

        return ret;
    }

    public static List<T> vCopy<T>(this List<T> list)
    {
        List<T> _list = new List<T>();
        if (list == null || list.Count == 0)
        {
            return list;
        }

        for (int i = 0; i < list.Count; i++)
        {
            _list.Add(list[i]);
        }
        return _list;
    }

    public static List<T> vToList<T>(this T[] array)
    {
        List<T> list = new List<T>();
        if (array == null || array.Length == 0)
        {
            return list;
        }

        for (int i = 0; i < array.Length; i++)
        {
            list.Add(array[i]);
        }
        return list;
    }

    public static T[] vToArray<T>(this List<T> list)
    {
        T[] array = new T[list.Count];
        if (list == null || list.Count == 0)
        {
            return array;
        }

        for (int i = 0; i < list.Count; i++)
        {
            array[i] = list[i];
        }
        return array;
    }


    //单位面向角度
    public static float NormalizeAngle360(this float eulerAngle)
    {
        if (eulerAngle < -360) eulerAngle += 360;
        if (eulerAngle > 360) eulerAngle -= 360;
        return eulerAngle;
    }
    //两点间角度
    public static float NormalizeAngle180(this float eulerAngle)
    {
        if (eulerAngle < -180) eulerAngle += 360;
        if (eulerAngle > 180) eulerAngle -= 360;
        return eulerAngle;
    }

    public static Vector3 NormalizeAngle(this Vector3 eulerAngle)
    {
        return new Vector3(eulerAngle.x.NormalizeAngle180(), eulerAngle.y.NormalizeAngle180(), eulerAngle.z.NormalizeAngle180());
    }

}

namespace Dalechn
{
    public static partial class GameUtils
    {
        public static float DampMotion(float x, float offset, float dampingFactor = 5, float velocityFactor = 20)
        {
            return 1.0f - Mathf.Exp(-1 * dampingFactor * (x + offset)) * Mathf.Cos(velocityFactor * (x + offset));
        }

        public static void DampAnimation(GameObject target, float duration = 1.0f, UnityAction call = null)
        {
            target.SetActive(true);

            //target.transform.localScale = Vector3.one;
            Vector3 originScal = target.transform.localScale;
            DoAction a = bl_UpdateManager.RunAction(null, duration, delegate (float time, float r)
            {
                float val = DampMotion(time, 0.309f);
                float val2 = DampMotion(time, 0.204f);

                target.transform.localScale =  new Vector3(originScal.x*val, originScal.y * val2, originScal.z * 1);

            });
            a.overCall += delegate
            {
                if (call != null)
                {
                    call();
                }
            };
        }

        public static void FlashUI(Image obj, bool overShow = true, float duration = 0.8f)
        {
            bl_UpdateManager.RunAction(null, duration, (float time, float r) =>
            {
                obj.enabled = (r % 0.4f) > 0.4f / 2;
            }, () =>
            {
                obj.enabled = overShow;
            });
        }

        public static void Flash(GameObject obj, List<MeshRenderer> originVisableMeshs, bool overShow = true, float duration = 0.8f)
        {
            foreach (var m in originVisableMeshs)
            {
                m.enabled = false;
            }

            bl_UpdateManager.RunAction(null, duration, (float time, float r) =>
        {
            for (int i = 0; i < originVisableMeshs.Count; i++)
            {
                originVisableMeshs[i].enabled = (r % 0.4f) > 0.4f / 2;
            }
        }, () =>
        {
            for (int i = 0; i < originVisableMeshs.Count; i++)
            {
                originVisableMeshs[i].enabled = overShow;
            }
        });
        }

        #region CanClick

        public static bool CanClick(float t = 0.5f)
        {
            return CanClick("_default_", t);
        }

        private static Dictionary<string, float> s_canClickLastTimeMap = new Dictionary<string, float>();
        public static bool CanClick(string key, float t = 0.5f)
        {
            float now = Time.realtimeSinceStartup;

            float ptime;
            if (!s_canClickLastTimeMap.TryGetValue(key, out ptime))
            {
                s_canClickLastTimeMap.Add(key, now);
                return true;
            }

            if (now - ptime > t)
            {
                s_canClickLastTimeMap[key] = now;
                return true;
            }

            return false;
        }

        #endregion
    }
}


