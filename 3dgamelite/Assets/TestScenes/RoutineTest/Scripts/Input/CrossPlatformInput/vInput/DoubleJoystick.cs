using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//[DefaultExecutionOrder(-100)]
public class DoubleJoystick : MonoBehaviour
{
    public GenericInput horizontalInput;
    public GenericInput verticalInput;

    public UltimateJoystick joystickLeft;
    public UltimateJoystick joystickRight;

    bool left;

    void Start()
    {
        Init();

        joystickLeft.OnPointerDownCallback += () =>
        {
            left = true;
        };

        joystickRight.OnPointerDownCallback += () =>
        {
            left = false;
        };

        joystickRight.OnPointerUpCallback += () =>
        {
            joystickRight.DisableJoystick();
        };

    }

    public void EnableJoystickRight(bool enable)
    {
        if (enable)
        {
            joystickRight.EnableJoystick();
        }
        else
        {
            joystickRight.DisableJoystick();
        }
    }

    private void Init()
    {
        joystickLeft.horizontalInput.useInput = false;
        joystickLeft.verticalInput.useInput = false;
        joystickRight.horizontalInput.useInput = false;
        joystickRight.verticalInput.useInput = false;
    }

    void Update()
    {
        Init();

        if (joystickLeft.GetJoystickState())
        {
            joystickRight.DisableJoystick();
        }
        else
        {
            joystickRight.EnableJoystick();
        }

        if (joystickRight.GetJoystickState())
        //if (joystickRight.gameObject.activeInHierarchy)
        {
            joystickLeft.DisableJoystick();
        }
        else
        {
            joystickLeft.EnableJoystick();
        }

#if MOBILE_INPUT
        CrossPlatformInputManager.SetAxis(horizontalInput.buttonName, left ? joystickLeft.GetHorizontalAxis() : joystickRight.GetHorizontalAxis());
        CrossPlatformInputManager.SetAxis(verticalInput.buttonName, left ? joystickLeft.GetVerticalAxis() : joystickRight.GetVerticalAxis());
#endif

    }
}

