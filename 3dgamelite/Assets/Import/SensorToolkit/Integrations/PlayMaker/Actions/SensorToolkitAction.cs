#if PLAYMAKER

using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

namespace Micosmo.SensorToolkit.PlayMaker {

    public abstract class SensorToolkitAction<T1, T2> : FsmStateAction where T1 : BasePulsableSensor where T2 : BasePulsableSensor {

        [RequiredField, DisplayOrder(0)]
        [ObjectType(typeof(BasePulsableSensor))]
        public FsmObject sensor;

        protected T1 sensor3D => sensor.Value as T1;
        protected T2 sensor2D => sensor.Value as T2;

        public override void Reset() {
            OwnerDefaultSensor();
        }

        void OwnerDefaultSensor() {
            if (Owner != null) {
                var st1 = Owner.GetComponent<T1>();
                var st2 = Owner.GetComponent<T2>();
                sensor = new FsmObject() {
                    Value = st1 != null ? (BasePulsableSensor)st1 : (BasePulsableSensor)st2
                };
            } else {
                sensor = null;
            }
        }

        public override void OnEnter() {
            if (sensor3D != null) {
                OnEnter3D(sensor3D);
            } else if (sensor2D != null) {
                OnEnter2D(sensor2D);
            }
        }

        public override void OnExit() {
            if (sensor3D != null) {
                OnExit3D(sensor3D);
            } else if (sensor2D != null) {
                OnExit2D(sensor2D);
            }
        }

        public override void OnUpdate() {
            if (sensor3D != null) {
                OnUpdate3D(sensor3D);
            } else if (sensor2D != null) {
                OnUpdate2D(sensor2D);
            }
        }

        public abstract void OnEnter3D(T1 sensor);
        public abstract void OnEnter2D(T2 sensor);

        public abstract void OnExit3D(T1 sensor);
        public abstract void OnExit2D(T2 sensor);

        public abstract void OnUpdate3D(T1 sensor);
        public abstract void OnUpdate2D(T2 sensor);

        public override string ErrorCheck() {
            if (sensor.Value != null && sensor3D == null && sensor2D == null) {
                return $"Sensor must be either a {typeof(T1)} or {typeof(T2)}";
            }
            return base.ErrorCheck();
        }

    }

}

#endif