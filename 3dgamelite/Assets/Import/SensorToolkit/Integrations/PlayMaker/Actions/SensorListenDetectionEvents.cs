#if PLAYMAKER

using System.Collections;
using HutongGames.PlayMaker;

namespace Micosmo.SensorToolkit.PlayMaker {

    [ActionCategory("SensorToolkit")]
    [Tooltip("Subscribe to a sensors OnDetection, OnLostDetection events.")]
    public class SensorListenDetectionEvents : FsmStateAction {

        public enum EventType { NewDetection, LostDetection }

        [RequiredField]
        [ObjectType(typeof(Sensor))]
        public FsmObject sensor;

        [ActionSection("New Detection")]

        [Tooltip("Event fired when a new object was detected")]
        public FsmEvent newDetectionEvent;

        [UIHint(UIHint.Variable)]
        [Tooltip("Stores the gameobject that was newly detected")]
        public FsmGameObject storeNewDetection;

        [ActionSection("Detection Lost")]

        [Tooltip("Event fired when a detection was lost")]
        public FsmEvent lostDetectionEvent;

        [UIHint(UIHint.Variable)]
        [Tooltip("Stores the gameobject whose detection was lost")]
        public FsmGameObject storeLostDetection;

        Sensor _sensor => sensor.Value as Sensor;

        public override void Reset() {
            OwnerDefaultSensor();
            newDetectionEvent = null;
            storeNewDetection = null;
            lostDetectionEvent = null;
            storeLostDetection = null;
        }

        void OwnerDefaultSensor() {
            if (Owner != null) {
                sensor = new FsmObject() { Value = Owner.GetComponent(typeof(Sensor)) };
            } else {
                sensor = null;
            }
        }

        public override void OnEnter() {
            if (_sensor == null) {
                return;
            }
            _sensor.OnDetected.AddListener(OnDetectionHandler);
            _sensor.OnLostDetection.AddListener(DetectionLostHandler);
        }

        public override void OnExit() {
            if (_sensor == null) {
                return;
            }
            _sensor.OnDetected.RemoveListener(OnDetectionHandler);
            _sensor.OnLostDetection.RemoveListener(DetectionLostHandler);
        }

        void OnDetectionHandler(UnityEngine.GameObject go, Sensor sensor) {
            if (!storeNewDetection.IsNone) {
                storeNewDetection.Value = go;
            }
            Fsm.Event(newDetectionEvent);
        }

        void DetectionLostHandler(UnityEngine.GameObject go, Sensor sensor) {
            if (!storeLostDetection.IsNone) {
                storeLostDetection.Value = go;
            }
            Fsm.Event(lostDetectionEvent);
        }
    }
}

#endif