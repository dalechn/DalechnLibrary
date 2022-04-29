#if PLAYMAKER

using System.Collections;
using HutongGames.PlayMaker;

namespace Micosmo.SensorToolkit.PlayMaker {

    [ActionCategory("SensorToolkit")]
    [Tooltip("Manually pulses the sensor.")]
    public class SensorPulse : FsmStateAction {

        [RequiredField]
        [ObjectType(typeof(BasePulsableSensor))]
        public FsmObject sensor;

        [Tooltip("Pulse sensor each frame.")]
        public bool everyFrame;

        BasePulsableSensor _sensor => sensor.Value as BasePulsableSensor;

        public override void Reset() {
            OwnerDefaultSensor();
            everyFrame = false;
        }

        void OwnerDefaultSensor() {
            if (Owner != null) {
                sensor = new FsmObject() { Value = Owner.GetComponent(typeof(IPulseRoutine)) };
            } else {
                sensor = null;
            }
        }

        public override string ErrorCheck() {
            if (sensor.Value != null && _sensor == null) {
                return "'Sensor' does not have Pulse Routine. Must implement IPulseRoutine";
            }
            return base.ErrorCheck();
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
            _sensor.Pulse();
        }
    }

}

#endif