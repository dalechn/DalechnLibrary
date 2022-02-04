using UnityEngine;
using System.Collections.Generic;
using CJTools;

namespace CJTools
{
    public enum DoActionMode
    {
        FixedUpdate,
        Update,
        LateUpdate
    }

    public class DoAction : MonoBehaviour
    {
        public float duration = 0;
        public float delayTime = 0;
        public DoActionMode mode = DoActionMode.FixedUpdate;
        public EaseType easeType = EaseType.Lerp;
        public float realTime = 0;

        public ED_Void_FF stepCall;
        public ED_Void overCall;
        public ED_Void onceCall;

        private float calcDelayTime = 0;
        private float time = 0;
        private bool delayRelease = true;

        private void Start()
        {
        }

        private void FixedUpdate()
        {
            if (mode == DoActionMode.FixedUpdate)
            {
                UpdateMode(Time.fixedDeltaTime);
            }
        }

        private void LateUpdate()
        {
            if (mode == DoActionMode.LateUpdate)
            {
                UpdateMode(Time.deltaTime);
            }
        }

        private void Update()
        {
            if (mode == DoActionMode.Update)
            {
                UpdateMode(Time.deltaTime);
            }
        }

        private void UpdateMode(float mode)
        {
            if (calcDelayTime > delayTime)
            {
                if (delayRelease && onceCall != null)
                {
                    delayRelease = false;
                    onceCall.Invoke();
                }
                if (duration <= 0)
                {
                    Destroy(this);
                }

                if (stepCall != null)
                {
                    stepCall.Invoke(Easing.Ease(easeType, 0, 1, time), realTime);
                }

                if (time > 1)
                {
                    if (overCall != null)
                    {
                        overCall.Invoke();
                    }
                    Destroy(this);
                }

                time += mode / duration;
                realTime += mode;
            }
            else
            {
                calcDelayTime += mode;
            }
        }

        public void RunActionMember(params ActionMemberBase[] actionList)
        {
            foreach (var val in actionList)
            {
                val.RunAction(gameObject, time);
            }
        }

        #region StaticFunc
        public static DoAction RunAction(GameObject gameObject, float duration, ED_Void_FF stepCall, ED_Void overCall = null, EaseType type = EaseType.Lerp, DoActionMode mode = DoActionMode.Update)
        {
            DoAction s = gameObject.AddComponent<DoAction>();
            s.mode = mode;
            s.easeType = type;
            s.duration = duration;
            s.stepCall += (stepCall);
            s.overCall += (overCall);
            return s;
        }

        public static DoAction RunAction(GameObject gameObject, float duration, float delay, ED_Void_FF stepCall, ED_Void onceCall = null, ED_Void overCall = null, EaseType type = EaseType.Lerp, DoActionMode mode = DoActionMode.Update)
        {
            DoAction s = gameObject.AddComponent<DoAction>();
            s.mode = mode;
            s.easeType = type;
            s.duration = duration;
            s.delayTime = delay;
            s.stepCall += (stepCall);
            s.overCall += (overCall);
            s.onceCall += (onceCall);
            return s;
        }

        public static DoAction RunActionOnce(GameObject gameObject, float delay, ED_Void onceCall, EaseType type = EaseType.Lerp, DoActionMode mode = DoActionMode.Update)
        {
            DoAction s = gameObject.AddComponent<DoAction>();
            s.mode = mode;
            s.easeType = type;
            s.delayTime = delay;
            s.onceCall += (onceCall);
            return s;
        }

        public static void StopAllAction(GameObject target)
        {
            DoAction[] s = target.GetComponents<DoAction>();
            foreach (var schedule in s)
            {
                Destroy(schedule);
            }
        }

        public static float DampMotion(float x, float offset, float dampingFactor = 5, float velocityFactor = 20)
        {
            return 1.0f - Mathf.Exp(-1 * dampingFactor * (x + offset)) * Mathf.Cos(velocityFactor * (x + offset));
        }

        #endregion
    }
}



