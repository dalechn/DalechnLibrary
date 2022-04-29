using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using CW.Common;
using UnityEngine.EventSystems;

namespace Lean.Touch
{
    public class LeanButton : MonoBehaviour
    {
        public class ButtonState
        {
            public UltimateButton button;
            public LeanFinger fingger;

            public ButtonState(UltimateButton button, LeanFinger fingger)
            {
                this.button = button;
                this.fingger = fingger;
            }
        }

        public LeanFingerFilter Use = new LeanFingerFilter(true);
        public List<ButtonState> buttonList = new List<ButtonState>();
        public bool forAllButtons;

        PointerEventData data = new PointerEventData(EventSystem.current);

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            Use.UpdateRequiredSelectable(gameObject);
        }
#endif

        public virtual void Awake()
        {
            Use.UpdateRequiredSelectable(gameObject);

            if (forAllButtons)
            {
                foreach (var val in FindObjectsOfType<UltimateButton>())
                {
                    buttonList.Add(new ButtonState(val, null));
                }
            }
            else
            {
                buttonList.Add(new ButtonState(GetComponent<UltimateButton>(), null));
            }
        }

        public virtual void Update()
        {
            var fingers = Use.UpdateAndGetFingers(false);

            foreach (var val in buttonList)
            {
                foreach (var finger in fingers)
                {
                    if (val.button.IsInRange(finger.ScreenPosition))
                    {
                        val.fingger = finger;
                        data.position = finger.ScreenPosition;
                        val.button.OnPointerDown(data);
                    }

                    if (finger.Up == true && finger == val.fingger)
                    {
                        val.fingger = null;

                    }
                }

                if (val.fingger == null || !val.button.IsInRange(val.fingger.ScreenPosition))
                {
                    val.button.OnPointerUp(null);
                }
            }
        }

    }
}
