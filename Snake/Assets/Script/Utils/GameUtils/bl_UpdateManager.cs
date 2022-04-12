using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CJTools
{
    public class bl_UpdateManager : MonoBehaviour
    {
        public float SlowUpdateTime = 0.5f;
        private int regularUpdateArrayCount = 0;
        private int fixedUpdateArrayCount = 0;
        private int lateUpdateArrayCount = 0;
        private int slowUpdateArrayCount = 0;
        private bl_MonoBehaviour[] regularArray = new bl_MonoBehaviour[0];
        private bl_MonoBehaviour[] fixedArray = new bl_MonoBehaviour[0];
        private bl_MonoBehaviour[] lateArray = new bl_MonoBehaviour[0];
        private bl_MonoBehaviour[] slowArray = new bl_MonoBehaviour[0];
        private bool Initializate = false;
        private float lastSlowCall = 0;

        [SerializeField]
        private List<DoAction> actionList = new List<DoAction>();

        /// <summary>
        /// 
        /// </summary>
        public static void AddItem(bl_MonoBehaviour behaviour)
        {
            if (instance == null) return;
            instance.AddItemToArray(behaviour);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            Initializate = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behaviour"></param>
        public static void RemoveSpecificItem(bl_MonoBehaviour behaviour)
        {
            if (instance != null)
            {
                instance.RemoveSpecificItemFromArray(behaviour);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behaviour"></param>
        public static void RemoveSpecificItemAndDestroyIt(bl_MonoBehaviour behaviour)
        {
            instance.RemoveSpecificItemFromArray(behaviour);

            Destroy(behaviour.gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDestroy()
        {
            regularArray = new bl_MonoBehaviour[0];
            fixedArray = new bl_MonoBehaviour[0];
            lateArray = new bl_MonoBehaviour[0];
            slowArray = new bl_MonoBehaviour[0];

            actionList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behaviour"></param>
        private void AddItemToArray(bl_MonoBehaviour behaviour)
        {
            if (behaviour.GetType().GetMethod("OnUpdate").DeclaringType != typeof(bl_MonoBehaviour))
            {
                regularArray = ExtendAndAddItemToArray(regularArray, behaviour);
                regularUpdateArrayCount++;
            }

            if (behaviour.GetType().GetMethod("OnFixedUpdate").DeclaringType != typeof(bl_MonoBehaviour))
            {
                fixedArray = ExtendAndAddItemToArray(fixedArray, behaviour);
                fixedUpdateArrayCount++;
            }

            if (behaviour.GetType().GetMethod("OnSlowUpdate").DeclaringType != typeof(bl_MonoBehaviour))
            {
                slowArray = ExtendAndAddItemToArray(slowArray, behaviour);
                slowUpdateArrayCount++;
            }

            if (behaviour.GetType().GetMethod("OnLateUpdate").DeclaringType == typeof(bl_MonoBehaviour))
                return;

            lateArray = ExtendAndAddItemToArray(lateArray, behaviour);
            lateUpdateArrayCount++;
        }

        private int size = 0;
        public bl_MonoBehaviour[] ExtendAndAddItemToArray(bl_MonoBehaviour[] original, bl_MonoBehaviour itemToAdd)
        {
            size = original.Length;
            bl_MonoBehaviour[] finalArray = new bl_MonoBehaviour[size + 1];
            for (int i = 0; i < size; i++)
            {
                finalArray[i] = original[i];
            }
            finalArray[finalArray.Length - 1] = itemToAdd;
            return finalArray;
        }

        private void RemoveSpecificItemFromArray(bl_MonoBehaviour behaviour)
        {
            if (CheckIfArrayContainsItem(regularArray, behaviour))
            {
                regularArray = ShrinkAndRemoveItemToArray(regularArray, behaviour);
                regularUpdateArrayCount--;
            }

            if (CheckIfArrayContainsItem(fixedArray, behaviour))
            {
                fixedArray = ShrinkAndRemoveItemToArray(fixedArray, behaviour);
                fixedUpdateArrayCount--;
            }

            if (CheckIfArrayContainsItem(slowArray, behaviour))
            {
                slowArray = ShrinkAndRemoveItemToArray(slowArray, behaviour);
                slowUpdateArrayCount--;
            }

            if (!CheckIfArrayContainsItem(lateArray, behaviour)) return;

            lateArray = ShrinkAndRemoveItemToArray(lateArray, behaviour);
            lateUpdateArrayCount--;
        }

        public bool CheckIfArrayContainsItem(bl_MonoBehaviour[] arrayToCheck, bl_MonoBehaviour objectToCheckFor)
        {
            int size = arrayToCheck.Length;

            for (int i = 0; i < size; i++)
            {
                if (objectToCheckFor == arrayToCheck[i]) return true;
            }

            return false;
        }

        public bl_MonoBehaviour[] ShrinkAndRemoveItemToArray(bl_MonoBehaviour[] original, bl_MonoBehaviour itemToRemove)
        {
            int size = original.Length;
            int fix = 0;
            bl_MonoBehaviour[] finalArray = new bl_MonoBehaviour[size - 1];
            for (int i = 0; i < size; i++)
            {
                if (original[i] != itemToRemove)
                {
                    finalArray[i - fix] = original[i];
                }
                else
                {
                    fix++;
                }
            }
            return finalArray;
        }

        private void Update()
        {
            if (!Initializate)
                return;

            if (actionList.Count > 0)
            {
                for (int i = 0; i < actionList.Count; i++)
                {
                    if (!actionList[i].removed)
                    {
                        actionList[i].OnUpdate();
                    }
                }
            }

            // 执行完毕清理action
            for (int i = actionList.Count - 1; i >= 0; i--)
            {
                if (actionList[i].removed)
                    actionList.Remove(actionList[i]);
            }

            SlowUpdate();
            if (regularUpdateArrayCount == 0) return;

            for (int i = 0; i < regularUpdateArrayCount; i++)
            {
                if (regularArray[i] != null && regularArray[i].enabled)
                {
                    regularArray[i].OnUpdate();
                }
            }
        }

        private void SlowUpdate()
        {
            if (slowUpdateArrayCount == 0) return;
            if ((Time.time - lastSlowCall) < SlowUpdateTime) return;

            lastSlowCall = Time.time;
            for (int i = 0; i < slowUpdateArrayCount; i++)
            {
                if (slowArray[i] != null && slowArray[i].enabled)
                {
                    slowArray[i].OnSlowUpdate();
                }
            }
        }

        private void FixedUpdate()
        {
            if (!Initializate)
                return;

            if (actionList.Count > 0)
            {
                for (int i = 0; i < actionList.Count; i++)
                {
                    if (!actionList[i].removed)
                    {
                        actionList[i].OnFixedUpdate();
                    }
                }
            }

            if (fixedUpdateArrayCount == 0) return;

            for (int i = 0; i < fixedUpdateArrayCount; i++)
            {
                if (fixedArray[i] == null || !fixedArray[i].enabled) continue;

                fixedArray[i].OnFixedUpdate();
            }
        }

        private void LateUpdate()
        {
            if (!Initializate)
                return;

            if (actionList.Count > 0)
            {
                for (int i = 0; i < actionList.Count; i++)
                {
                    if (!actionList[i].removed)
                    {
                        actionList[i].OnLateUpdate();
                    }
                }
            }

            if (lateUpdateArrayCount == 0) return;

            for (int i = 0; i < lateUpdateArrayCount; i++)
            {
                if (lateArray[i] == null || !lateArray[i].enabled) continue;

                lateArray[i].OnLateUpdate();
            }
        }

        private static bl_UpdateManager _instance;
        public static bl_UpdateManager instance
        {
            get
            {
                if (_instance == null) { _instance = FindObjectOfType<bl_UpdateManager>(); }
                return _instance;
            }
        }

        public void RemoveAction(string actName)
        {
            for (int i = 0; i < actionList.Count; i++)
            {
                if (actionList[i].actionName == actName)
                {
                    actionList[i].removed = true;
                }
            }
        }

        public void RemoveAction(DoAction act)
        {
            for (int i = 0; i < actionList.Count; i++)
            {
                if (actionList[i] == act)
                {
                    actionList[i].removed = true;
                }
            }
        }

        #region StaticFunc

        public static DoAction RunAction(string actName, float duration, UnityAction<float, float> stepCall, UnityAction overCall = null, EaseType type = EaseType.Lerp, ActionMode mode = ActionMode.MUpdate)
        {
            DoAction s = new DoAction();
            s.mode = mode;
            s.easeType = type;
            s.duration = duration;
            s.stepCall += (stepCall);
            s.overCall += (overCall);
            s.actionName = actName;

            instance?.actionList.Add(s);

            return s;
        }

        public static DoAction RunAction(string actName, float duration, float delay, UnityAction<float,float> stepCall, UnityAction onceCall = null, UnityAction overCall = null, EaseType type = EaseType.Lerp, ActionMode mode = ActionMode.MUpdate)
        {
            DoAction s = new DoAction();
            s.mode = mode;
            s.easeType = type;
            s.duration = duration;
            s.delayTime = delay;
            s.stepCall += (stepCall);
            s.overCall += (overCall);
            s.onceCall += (onceCall);
            s.actionName = actName;

            instance?.actionList.Add(s);

            return s;
        }

        public static DoAction RunActionOnce(string actName, float delay, UnityAction onceCall, EaseType type = EaseType.Lerp, ActionMode mode = ActionMode.MUpdate)
        {
            DoAction s = new DoAction();
            s.mode = mode;
            s.easeType = type;
            s.delayTime = delay;
            s.onceCall += (onceCall);
            s.actionName = actName;
            instance?.actionList.Add(s);

            return s;
        }

        public static void StopAction(string actName, ActionMode mode = ActionMode.MUpdate)
        {
            instance?.RemoveAction(actName);
        }

        #endregion

    }

    public enum ActionMode
    {
        MFixedUpdate,
        MUpdate,
        MLateUpdate
    }

    [System.Serializable]
    public class DoAction
    {
        public string actionName;
        public float duration = 0;
        public float delayTime = 0;
        public ActionMode mode = ActionMode.MFixedUpdate;
        public EaseType easeType = EaseType.Lerp;
        public float realTime = 0;
        public bool removed;

        public UnityAction<float,float> stepCall;
        public UnityAction overCall;
        public UnityAction onceCall;

        private float calcDelayTime = 0;
        private float time = 0;
        private bool delayRelease = true;

        public void OnFixedUpdate()
        {
            if (mode == ActionMode.MFixedUpdate)
            {
                UpdateMode(Time.fixedDeltaTime);
            }
        }

        public void OnLateUpdate()
        {
            if (mode == ActionMode.MLateUpdate)
            {
                UpdateMode(Time.deltaTime);
            }
        }

        public void OnUpdate()
        {
            if (mode == ActionMode.MUpdate)
            {
                UpdateMode(Time.deltaTime);
            }
        }

        private void UpdateMode(float mode)
        {
            if (calcDelayTime > delayTime)
            {
                if (delayRelease)
                {
                    delayRelease = false;
                    onceCall?.Invoke();
                }
                if (duration <= 0)
                {
                    removed = true;
                }
                stepCall?.Invoke(Easing.Ease(easeType, 0, 1, time), realTime);

                if (time > 1)
                {
                    removed = true;
                    overCall?.Invoke();
                }

                time += mode / duration;
                realTime += mode;
            }
            else
            {
                calcDelayTime += mode;
            }
        }

        public static float DampMotion(float x, float offset, float dampingFactor = 5, float velocityFactor = 20)
        {
            return 1.0f - Mathf.Exp(-1 * dampingFactor * (x + offset)) * Mathf.Cos(velocityFactor * (x + offset));
        }

    }
}