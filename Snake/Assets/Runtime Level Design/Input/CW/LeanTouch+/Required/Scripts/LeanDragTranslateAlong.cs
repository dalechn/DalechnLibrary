using UnityEngine;
using UnityEngine.Events;
using CW.Common;

namespace Lean.Touch
{
    /// <summary>This component allows you to translate the current GameObject along the specified surface.</summary>
    //[ExecuteInEditMode]
    [HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanDragTranslateAlong")]
    [AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drag Translate Along")]
    public class LeanDragTranslateAlong : MonoBehaviour
    {
        /// <summary>If you want this component to work on a different <b>Transform</b>, then specify it here. This can be used to improve organization if your GameObject already has many components.
        /// None/null = Current Transform.</summary>
        public Transform Target { set { target = value; } get { return target; } }
        [SerializeField] private Transform target;

        /// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
        public LeanFingerFilter Use = new LeanFingerFilter(true);

        /// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
        public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

        /// <summary>If your ScreenDepth settings cause the position values to clamp, there will be a difference between where the finger is and where the object is. Should this difference be tracked?</summary>
        public bool TrackScreenPosition { set { trackScreenPosition = value; } get { return trackScreenPosition; } }
        [SerializeField] private bool trackScreenPosition = true;

        /// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
        /// -1 = Instantly change.
        /// 1 = Slowly change.
        /// 10 = Quickly change.</summary>
        public float Damping { set { damping = value; } get { return damping; } }
        [SerializeField] private float damping = -1.0f;

        public float Sensitivity { set { sensitivity = value; } get { return sensitivity; } }
        [SerializeField] private float sensitivity = 1.0f;

        //[System.NonSerialized]
        private Vector2 deltaDifference;

        //[SerializeField]
        private Vector3 remainingDelta;

        private Vector2 screenDelta;

        /// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
        public void AddFinger(LeanFinger finger)
        {
            Use.AddFinger(finger);
        }

        /// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
        public void RemoveFinger(LeanFinger finger)
        {
            Use.RemoveFinger(finger);
        }

        /// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
        public void RemoveAllFingers()
        {
            Use.RemoveAllFingers();
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            Use.UpdateRequiredSelectable(gameObject);
        }
#endif

        protected virtual void Awake()
        {
            Use.UpdateRequiredSelectable(gameObject);
        }

        protected virtual void Update()
        {
            var finalTransform = target != null ? target : transform;

            // Store smoothed position
            var smoothPosition = finalTransform.localPosition;

            // Snap to target
            finalTransform.localPosition += remainingDelta;

            // Store old position
            var oldPosition = finalTransform.localPosition;


            // Get the fingers we want to use
            var fingers = Use.UpdateAndGetFingers();

            // Calculate the screenDelta value based on these fingers and make sure there is movement
            screenDelta = LeanGesture.GetScreenDelta(fingers);

            // Make sure the camera exists
            var camera = CwHelper.GetCamera(ScreenDepth.Camera, gameObject);


            if (screenDelta != Vector2.zero)
            {
                if (camera != null)
                {
                    var worldPosition = finalTransform.position;
                    var oldScreenPoint = camera.WorldToScreenPoint(worldPosition);

                    if (trackScreenPosition == true)
                    {
                        if (ScreenDepth.TryConvert(ref worldPosition, oldScreenPoint + (Vector3)(screenDelta + deltaDifference) * sensitivity, gameObject) == true)
                        {
                            finalTransform.position = worldPosition;
                        }

                        var newScreenPoint = camera.WorldToScreenPoint(worldPosition);
                        var oldNewDelta = (Vector2)(newScreenPoint - oldScreenPoint);

                        deltaDifference += screenDelta - oldNewDelta;
                    }
                    else
                    {
                        if (ScreenDepth.TryConvert(ref worldPosition, oldScreenPoint + (Vector3)screenDelta * sensitivity, gameObject) == true)
                        {
                            finalTransform.position = worldPosition;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
                }
            }


            // Shift delta by old new delta
            remainingDelta += finalTransform.localPosition - oldPosition;

            // Get t value
            var factor = CwHelper.DampenFactor(damping, Time.deltaTime);

            // Dampen remainingDelta
            var newDelta = Vector3.Lerp(remainingDelta, Vector3.zero, factor);

            // Shift this position by the change in delta
            finalTransform.localPosition = smoothPosition + remainingDelta - newDelta;

            // Update remainingDelta with the dampened value
            remainingDelta = newDelta;
        }
    }
}

#if UNITY_EDITOR
namespace Lean.Touch.Editor
{
    using UnityEditor;
    using TARGET = LeanDragTranslateAlong;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET))]
    public class LeanDragTranslateAlong_Editor : CwEditor
    {
        protected override void OnInspector()
        {
            TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

            Draw("target", "This allows you to control how quickly the target value is reached.");
            Draw("Use");
            Draw("ScreenDepth");
            Draw("trackScreenPosition", "If your ScreenDepth settings cause the position values to clamp, there will be a difference between where the finger is and where the object is. Should this difference be tracked?");
            Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
            Draw("sensitivity");
        }
    }
}
#endif