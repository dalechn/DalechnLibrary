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

        public LeanFingerFilter Use = new LeanFingerFilter(true);
        public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);
        public CoordinateType coordinate;

        public float multiplier = 1.0f;
        public Vector2 offset;

        [System.Serializable] public class TouchEvent : UnityEvent<TouchData> { }
        public TouchEvent onTouchBegan;
        public TouchEvent onTouchMoved;
        public TouchEvent onTouchEnded;

        Vector3 worldFrom;
        Vector3 worldTo;
        LeanScreenDepth.ConversionType originType;
        TouchData touch = new TouchData();

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            Use.UpdateRequiredSelectable(gameObject);
        }
#endif

        protected virtual void Awake()
        {
            Use.UpdateRequiredSelectable(gameObject);

            originType = ScreenDepth.Conversion;

        }

        protected virtual void Update()
        {
            var fingers = Use.UpdateAndGetFingers(false);

            if (fingers.Count > 0)
            {
                var screenFrom = LeanGesture.GetLastScreenCenter(fingers) + offset;
                var screenTo = LeanGesture.GetScreenCenter(fingers) + offset;
                var finalDelta = screenTo - screenFrom;

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

                onTouchMoved?.Invoke(touch);

                // 最后一根手指抬起
                if (fingers.Count == 1)
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
                //Debug.Log(fingers.Count);
            }
        }

    }
}
