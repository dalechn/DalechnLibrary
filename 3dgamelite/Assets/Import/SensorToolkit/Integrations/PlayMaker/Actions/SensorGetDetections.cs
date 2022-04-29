#if PLAYMAKER

using System.Linq;
using System.Collections;
using HutongGames.PlayMaker;

namespace Micosmo.SensorToolkit.PlayMaker {

    [ActionCategory("SensorToolkit")]
    [Tooltip("Queries the sensor for the GameObjects it detects. There's a handful of query types including: 'All' to get all detected objects and 'Nearest' to get only the nearest detected object by distance. The query can be fine-tuned to return objects with specific tags, or objects that have specific components.")]
    public class SensorGetDetections : FsmStateAction {

        public enum QueryType { All, ByDistance, ByDistanceToPoint, Nearest, NearestToPoint }
        
        [RequiredField]
        [ObjectType(typeof(Sensor))]
        public FsmObject sensor;

        [ActionSection("Inputs")]

        [ObjectType(typeof(QueryType))]
        public FsmEnum queryType;

        [Tooltip("Find only detected objects with this tag.")]
        public FsmString tag;

        [HideIf("hideTestPoint")]
        [Tooltip("Order detections by distance to this point.")]
        public FsmVector3 testPoint;

        [Tooltip("Set steering configurations each frame.")]
        public bool everyFrame;

        [ActionSection("Outputs")]

        [HideIf("isSingleResult")]
        [UIHint(UIHint.Variable)]
        [ArrayEditor(VariableType.GameObject)]
        [Tooltip("Stores GameObjects detected by the sensor, if there is one.")]
        public FsmArray storeAllDetected;

        [HideIf("isSingleResult")]
        [UIHint(UIHint.Variable)]
        [ArrayEditor(VariableType.Object)]
        [Tooltip("Detections must have matching component. Store all the components here.")]
        public FsmArray storeAllComponents;

        [ActionSection("Outputs")]

        [HideIf("isArrayResult")]
        [UIHint(UIHint.Variable)]
        [Tooltip("Stores all GameObjects detected by the sensor.")]
        public FsmGameObject storeDetected;

        [HideIf("isArrayResult")]
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(UnityEngine.Component))]
        [Tooltip("Detections must have matching component. Store the component here.")]
        public FsmObject storeComponent;

        [ActionSection("Events")]
        [Tooltip("Fires this event if there is at least one detected GameObject that matches the search filters.")]
        public FsmEvent detectedEvent;
        [Tooltip("Fires this event if no GameObject is detected that matches the search filters.")]
        public FsmEvent noneDetectedEvent;

        QueryType _queryType => (QueryType)queryType.Value;
        Sensor _sensor => sensor.Value as Sensor;

        string _tag => tag.Value;
        bool useTag => !string.IsNullOrEmpty(_tag);

        System.Type componentType => isSingleResult() ? storeComponent.ObjectType : storeAllComponents.ObjectType;
        bool useComponent => isSingleResult() ? !storeComponent.IsNone : !storeAllComponents.IsNone;

        public bool isArrayResult() => _queryType != QueryType.Nearest && _queryType != QueryType.NearestToPoint;
        public bool isSingleResult() => !isArrayResult();
        public bool hideTestPoint() => !(_queryType == QueryType.ByDistanceToPoint || _queryType == QueryType.NearestToPoint);

        public override void Reset() {
            OwnerDefaultSensor();
            queryType = QueryType.All;
            storeAllComponents = null;
            storeAllDetected = null;
            storeComponent = null;
            storeDetected = null;
            testPoint = null;
            tag = null;
            detectedEvent = null;
            noneDetectedEvent = null;
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
            if (_sensor == null) {
                return;
            }
            var isSomethingDetected = switchAction();
            if (isSomethingDetected) {
                Fsm.Event(detectedEvent);
            } else {
                Fsm.Event(noneDetectedEvent);
            }
        }

        bool switchAction() {
            switch(_queryType) {
                case QueryType.All:
                    return DoAll();
                case QueryType.ByDistance:
                    return DoByDistance();
                case QueryType.ByDistanceToPoint:
                    return DoByDistanceToPoint();
                case QueryType.Nearest:
                    return DoNearest();
                case QueryType.NearestToPoint:
                    return DoNearestToPoint();
                default:
                    return false;
            }
        }

        bool DoAll() {
            if (useComponent) {
                var components = useTag
                    ? _sensor.GetDetectedComponents(componentType, _tag).ToArray()
                    : _sensor.GetDetectedComponents(componentType).ToArray();
                storeAllComponents.Values = components;
                storeAllDetected.Values = components.Select(c => c.gameObject).ToArray();
                return storeAllComponents.Values.Length > 0;
            } else {
                storeAllDetected.Values = useTag
                    ? _sensor.GetDetections(_tag).ToArray()
                    : _sensor.GetDetections().ToArray();
                return storeAllDetected.Values.Length > 0;
            }
        }

        bool DoByDistance() {
            if (useComponent) {
                var components = useTag
                    ? _sensor.GetDetectedComponentsByDistance(componentType, _tag).ToArray()
                    : _sensor.GetDetectedComponentsByDistance(componentType).ToArray();
                storeAllComponents.Values = components;
                storeAllDetected.Values = components.Select(c => c.gameObject).ToArray();
                return storeAllComponents.Values.Length > 0;
            } else {
                storeAllDetected.Values = useTag
                    ? _sensor.GetDetectionsByDistance(_tag).ToArray()
                    : _sensor.GetDetectionsByDistance().ToArray();
                return storeAllDetected.Values.Length > 0;
            }
        }

        bool DoByDistanceToPoint() {
            if (useComponent) {
                var components = useTag
                    ? _sensor.GetDetectedComponentsByDistanceToPoint(testPoint.Value, componentType, _tag).ToArray()
                    : _sensor.GetDetectedComponentsByDistanceToPoint(testPoint.Value, componentType).ToArray();
                storeAllComponents.Values = components;
                storeAllDetected.Values = components.Select(c => c.gameObject).ToArray();
                return storeAllComponents.Values.Length > 0;
            } else {
                storeAllDetected.Values = useTag
                    ? _sensor.GetDetectionsByDistanceToPoint(testPoint.Value, _tag).ToArray()
                    : _sensor.GetDetectionsByDistanceToPoint(testPoint.Value).ToArray();
                return storeAllDetected.Values.Length > 0;
            }
        }

        bool DoNearest() {
            if (useComponent) {
                var component = useTag 
                    ? _sensor.GetNearestComponent(componentType, _tag) 
                    : _sensor.GetNearestComponent(componentType);
                storeComponent.Value = component;
                if (component != null) {
                    storeDetected.Value = component.gameObject;
                }
                return storeComponent.Value != null;
            } else {
                storeDetected.Value = useTag
                    ? _sensor.GetNearestDetection(_tag)
                    : _sensor.GetNearestDetection();
                return storeDetected.Value != null;
            }
        }

        bool DoNearestToPoint() {
            if (useComponent) {
                var component = useTag
                    ? _sensor.GetNearestComponentToPoint(testPoint.Value, componentType, _tag)
                    : _sensor.GetNearestComponentToPoint(testPoint.Value, componentType);
                storeComponent.Value = component;
                if (component != null) {
                    storeDetected.Value = component.gameObject;
                }
                return storeComponent.Value != null;
            } else {
                storeDetected = useTag
                    ? _sensor.GetNearestDetectionToPoint(testPoint.Value, _tag)
                    : _sensor.GetNearestDetectionToPoint(testPoint.Value);
                return storeDetected.Value != null;
            }
        }
    }

}

#endif