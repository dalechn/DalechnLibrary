using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[Invector.vClassHeader("VirtualHandler")]
public class VirtualHandler : vMonoBehaviour
{
    [Invector.vEditorToolbar("button||mouseScroll")]
    public GenericInput buttonInput;

    [Invector.vEditorToolbar("joystick||mouse")]
    public GenericInput horizontalInput;
    public GenericInput verticalInput;

    void Start()
    {

    }

    public void SetDownState()
    {
#if MOBILE_INPUT
        CrossPlatformInputManager.SetButtonDown(buttonInput.buttonName);
#endif
    }

    public void SetUpState()
    {
#if MOBILE_INPUT
        CrossPlatformInputManager.SetButtonUp(buttonInput.buttonName);
#endif
    }

    public void SetAxisPositiveState()
    {
#if MOBILE_INPUT
        CrossPlatformInputManager.SetAxisPositive(buttonInput.buttonName);
#endif
    }

    public void SetAxisNeutralState()
    {
#if MOBILE_INPUT
        CrossPlatformInputManager.SetAxisZero(buttonInput.buttonName);
#endif
    }

    public void SetAxisNegativeState()
    {
#if MOBILE_INPUT
        CrossPlatformInputManager.SetAxisNegative(buttonInput.buttonName);
#endif
    }

    public void SetAxis(float value)
    {
#if MOBILE_INPUT
        CrossPlatformInputManager.SetAxis(buttonInput.buttonName, value);
#endif
    }

    public void SetMouseAxis(Vector2 delta)
    {

#if MOBILE_INPUT
        CrossPlatformInputManager.SetAxis(horizontalInput.buttonName, delta.x);
        CrossPlatformInputManager.SetAxis(verticalInput.buttonName, delta.y);
#endif

    }
}
