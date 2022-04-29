#if PLAYMAKER

using System.Collections;
using HutongGames.PlayMaker;

namespace Micosmo.SensorToolkit.PlayMaker {

    [ActionCategory("SensorToolkit")]
    [Tooltip("Configure how often a sensor should pulse.")]
    public class SensorConfigurePulseRoutine : FsmStateAction {

        [RequiredField]
        [ObjectType(typeof(BasePulsableSensor))]
        public FsmObject sensor;

        [ObjectType(typeof(PulseRoutine.Modes))]
        public FsmEnum pulseMode;

        [HideIf("HidePulseInterval")]
        public FsmFloat pulseInterval;

        [Tooltip("Configure the pulse routine each frame.")]
        public bool everyFrame;

        IPulseRoutine _sensor => sensor.Value as IPulseRoutine;
        PulseRoutine.Modes _pulseMode => (PulseRoutine.Modes)pulseMode.Value;

        public bool HidePulseInterval() => _pulseMode != PulseRoutine.Modes.FixedInterval;

        public override void Reset() {
            OwnerDefaultSensor();
            pulseMode = null;
            pulseInterval = 1;
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
            _sensor.PulseMode = _pulseMode;
            _sensor.PulseInterval = pulseInterval.Value;
        }
    }

}

#endif