using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using CW.Common;

namespace Lean.Touch
{
    public struct TouchData
    {
        public Vector2 screenFrom;
        public Vector2 screenTo;
        public Vector2 screenDelta;

        public bool worldFromState;
        public bool worldToState;

        public Vector3 worldFrom;
        public Vector3 worldTo;
        public Vector3 worldDelta;
        public Vector3 worldNormal;

        public List<LeanFinger> fingerList;
    }

    public class LeanTouchEvent : MonoBehaviour
    {
        public enum CoordinateType
        {
            ScaledPixels,
            ScreenPixels,
            ScreenPercentage
        }

        [System.Serializable]
        public enum UpdateMode
        {
            Update,
            FixedUpdate,
            LateUpdate,
            FixedLateUpdate
        }

        public UpdateMode updateMode = UpdateMode.Update;

        public LeanFingerFilter Use = new LeanFingerFilter(true);
        public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);
        public CoordinateType coordinate;

        public float multiplier = 1.0f;
        public Vector2 offset;

        [System.Serializable] public class TouchEvent : UnityEvent<TouchData> { }
        //[System.Serializable] public class TouchDeltaEvent : UnityEvent<Vector2> { }

        public TouchEvent onTouchBegan;
        public TouchEvent onTouchMoved;
        public TouchEvent onTouchEnded;
        //public TouchDeltaEvent onUpdate;

        private Vector3 worldFrom;
        private Vector3 worldTo;
        private LeanScreenDepth.ConversionType originType;
        private TouchData touch = new TouchData();

        private bool fixedFrame;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            Use.UpdateRequiredSelectable(gameObject);
        }
#endif

        public virtual void Awake()
        {
            Use.UpdateRequiredSelectable(gameObject);

            originType = ScreenDepth.Conversion;

        }

        public virtual void Update()
        {
            if (updateMode == UpdateMode.Update) UpdateTransform();
        }

        public virtual void FixedUpdate()
        {
            fixedFrame = true;
            if (updateMode == UpdateMode.FixedUpdate) UpdateTransform();
        }

        public virtual void LateUpdate()
        {
            if (updateMode == UpdateMode.LateUpdate) UpdateTransform();

            if (updateMode == UpdateMode.FixedLateUpdate && fixedFrame)
            {
                UpdateTransform();
                fixedFrame = false;
            }
        }


        public virtual void UpdateTransform()
        {
            var fingers = Use.UpdateAndGetFingers(false);

            var screenFrom = LeanGesture.GetLastScreenCenter(fingers) + offset;
            var screenTo = LeanGesture.GetScreenCenter(fingers) + offset;
            var finalDelta = screenTo - screenFrom;

            switch (coordinate)
            {
                case CoordinateType.ScaledPixels: finalDelta *= LeanTouch.ScalingFactor; break;
                case CoordinateType.ScreenPercentage: finalDelta *= LeanTouch.ScreenFactor; break;
            }

            finalDelta *= multiplier;

            bool worldFromState = ScreenDepth.TryConvert(ref worldFrom, screenFrom, gameObject);
            bool worldToState = ScreenDepth.TryConvert(ref worldTo, screenTo, gameObject);

            touch.screenFrom = screenFrom;
            touch.screenTo = screenTo;
            touch.screenDelta = finalDelta;

            touch.worldFromState = worldFromState;
            touch.worldToState = worldToState;

            if (!worldFromState)
            {
                ScreenDepth.Conversion = LeanScreenDepth.ConversionType.AutoDistance;
                ScreenDepth.TryConvert(ref worldFrom, screenFrom, gameObject);
            }

            if (!worldToState)
            {
                ScreenDepth.Conversion = LeanScreenDepth.ConversionType.AutoDistance;
                ScreenDepth.TryConvert(ref worldTo, screenTo, gameObject);
            }
            ScreenDepth.Conversion = originType;

            touch.worldFrom = worldFrom;
            touch.worldTo = worldTo;
            touch.worldDelta = worldFrom - worldTo;

            touch.worldNormal = LeanScreenDepth.LastWorldNormal;
            touch.fingerList = fingers;

         
            if (fingers.Count > 0)
            {
                // 第一根手指按下
                foreach (var finger in fingers)
                {
                    if (finger.Down == true)
                    {
                        onTouchBegan?.Invoke(touch);
                        //Debug.Log("onTouchBegan");
                        break;
                    }
                }

                onTouchMoved?.Invoke(touch);

                // 最后一根手指抬起
                //if (fingers.Count == 1)
                {
                    foreach (var finger in fingers)
                    {
                        if (finger.Up == true)
                        {
                            onTouchEnded?.Invoke(touch);
                            //Debug.Log("onTouchEnded");
                            break;
                        }
                    }
                }

            }
            //onUpdate?.Invoke(finalDelta);

        }

    }
}
