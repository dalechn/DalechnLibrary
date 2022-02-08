using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ED_Void();
public delegate void ED_Void_I(int i);
public delegate void ED_Void_B(bool b);
public delegate void ED_Void_F(float b);
public delegate void ED_Void_FF(float a, float b);
public delegate void ED_Void_S(string str);
public delegate bool ED_Bool_I(int i);
public delegate void ED_Void_Go(GameObject go);
public delegate void ED_Void_Go_I(GameObject go, int i);

public static class Extensions
{
    public static string GetFullName(this Enum myEnum)
    {
        return string.Format("{0}.{1}", myEnum.GetType().Name, myEnum.ToString());
    }
}

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

namespace CJTools
{
    public static class GameUtils
    {
        public static float GetDegree(Vector2 deltaPos)
        {
            deltaPos.Normalize();

            return Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg; ;
        }

        public static Vector3 GetBetweenPoint(Vector3 start, Vector3 end, float percent = 0.5f)
        {
            Vector3 normal = (end - start).normalized;
            float distance = Vector3.Distance(start, end);
            return normal * (distance * percent) + start;
        }

        public static void DampAnimation(GameObject target, float duration = 1.0f, ED_Void call = null)
        {
            target.SetActive(true);

            target.transform.localScale = Vector3.one;
            DoAction a = bl_UpdateManager.RunAction(null, duration, delegate (float time, float r)
            {
                float val = DoAction.DampMotion(time, 0.309f);
                float val2 = DoAction.DampMotion(time, 0.204f);

                target.transform.localScale = new Vector3(val, val2, 1);

            });
            a.overCall += delegate
            {
                if (call != null)
                {
                    call();
                }
            };
        }

        public static GameObject GetNearestGameObject(List<GameObject> listTemp, GameObject obj)
        {
            if (listTemp != null && listTemp.Count > 0)
            {
                GameObject targetTemp = listTemp.Count > 0 ? listTemp[0] : null;
                float dis = Vector3.Distance(obj.transform.position, listTemp[0].transform.position);
                float disTemp;
                for (int i = 1; i < listTemp.Count; i++)
                {
                    disTemp = Vector3.Distance(obj.transform.position, listTemp[i].transform.position);
                    if (disTemp < dis)
                    {
                        targetTemp = listTemp[i];
                        dis = disTemp;
                    }
                }
                return targetTemp;
            }
            else
            {
                return null;
            }
        }

        public static Vector3 GetNearestGameObject(List<Vector3> listTemp, GameObject obj, out int index)
        {
            if (listTemp != null && listTemp.Count > 0)
            {
                Vector3 targetTemp = listTemp.Count > 0 ? listTemp[0] : Vector3.zero;
                float dis = Vector3.Distance(obj.transform.position, listTemp[0]);
                float disTemp;
                int indexTemp = 0;
                for (int i = 1; i < listTemp.Count; i++)
                {
                    disTemp = Vector3.Distance(obj.transform.position, listTemp[i]);
                    if (disTemp < dis)
                    {
                        targetTemp = listTemp[i];
                        dis = disTemp;

                        indexTemp = i;
                    }
                }
                index = indexTemp;
                return targetTemp;
            }
            else
            {
                index = 0;
                return Vector3.zero;
            }
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

        public static T RandomList<T>(List<T> list)
        {
            if (list.Count != 0)
            {
                int n = UnityEngine.Random.Range(0, list.Count);

                T child = list[n];
                return child;
            }
            return default(T);
        }

        public static T RandomListNotRepeat<T>(List<T> list, HashSet<int> set)
        {
            if (list.Count != 0)
            {
                int n = RandomIntNotRepeat(0, list.Count, set);

                T child = list[n];
                return child;
            }
            return default(T);
        }

        private static bool IsExit(HashSet<int> set, int value)
        {
            if (!set.Contains(value))
            {
                set.Add(value);
                return false;
            }

            return true;
        }

        private static int RandomIntNotRepeat(int min, int max, HashSet<int> set)
        {
            if (max - min == set.Count)
            {
                set.Clear();
            }

            int num = UnityEngine.Random.Range(min, max);
            while (IsExit(set, num))
            {
                num = UnityEngine.Random.Range(min, max);
            }
            return num;
        }

        public static Vector3 Range(Vector3 min, Vector3 max)
        {
            float x = UnityEngine.Random.Range(min.x, max.x);
            float y = UnityEngine.Random.Range(min.y, max.y);
            float z = UnityEngine.Random.Range(min.z, max.z);
            Vector3 temp = new Vector3(x, y, z);
            return temp;
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


