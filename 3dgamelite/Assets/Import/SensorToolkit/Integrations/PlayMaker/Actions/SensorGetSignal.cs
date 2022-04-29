#if PLAYMAKER

using System.Collections;
using HutongGames.PlayMaker;

namespace Micosmo.SensorToolkit.PlayMaker {

    [ActionCategory("SensorToolkit")]
    [Tooltip("Retrieve the Signal data for a GameObject. This will give you the objects visibility (Signal Strength) and it's center point. Use this also if you want to check if a GameObject is currently detected or not.")]
    public class SensorGetSignal : FsmStateAction {

        [RequiredField]
        [ObjectType(typeof(Sensor))]
        public FsmObject sensor;

        [ActionSection("Inputs")]
        [RequiredField]
        [Tooltip("Retrieves the Signal for this GameObject")]
        public FsmGameObject targetObject;

        [Tooltip("Run each frame?")]
        public bool everyFrame;

        [ActionSection("Outputs")]

        [UIHint(UIHint.Variable)]
        [Tooltip("Stores size of the signal's bounding box. Taken from Signal.Bounds.size")]
        public FsmVector3 storeSignalBoundsSize;

        [UIHint(UIHint.Variable)]
        [Tooltip("Stores the center-point of the signal's bounding box (world space). Taken from Signal.Bounds.center")]
        public FsmVector3 storeSignalBoundsCenter;

        [UIHint(UIHint.Variable)]
        [Tooltip("Stores the signals 'strength'. Can be interpreted as visibility score between 0-1.")]
        public FsmFloat storeSignalStrength;

        [ActionSection("Events")]

        [Tooltip("Invoked if a signal exists for the target object")]
        public FsmEvent isDetectedEvent;

        [Tooltip("Invoked when there is no signal for the target object")]
        public FsmEvent notDetectedEvent;

        Sensor _sensor => sensor.Value as Sensor;
        UnityEngine.GameObject _targetObject => targetObject.Value;

        public override void Reset() {
            OwnerDefaultSensor();
            targetObject = null;
            storeSignalBoundsSize = null;
            storeSignalBoundsCenter = null;
            storeSignalStrength = null;
            isDetectedEvent = null;
            notDetectedEvent = null;
            everyFrame = false;
        }

        void OwnerDefaultSensor() {
            if (Owner != null) {
                sensor = new FsmObject() { Value = Owner.GetComponent(typeof(Sensor)) };
            } else {
                sensor = null;
            }
        }

        public override void OnEnter() {
            DoAction();
            if (!everyFrame) {
                Finish();
            }
        }

        public override void OnUpdate() {
            DoAction();
        }

        void DoAction() {
            if (_sensor == null || _targetObject == null) {
                return;
            }
            Signal signal;
            if (_sensor.TryGetSignal(_targetObject, out signal)) {
                storeSignalBoundsSize.Value = signal.Bounds.size;
                storeSignalBoundsCenter.Value = signal.Bounds.center;
                storeSignalStrength.Value = signal.Strength;
                Fsm.Event(isDetectedEvent);
            } else {
                storeSignalBoundsSize.Value = UnityEngine.Vector3.zero;
                storeSignalBoundsCenter.Value = _targetObject.transform.position;
                storeSignalStrength.Value = 0;
                Fsm.Event(notDetectedEvent);
            }
        }
    }
}

#endif