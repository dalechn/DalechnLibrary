using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//[DefaultExecutionOrder(-100)]
public class AxisVirtualButton : MonoBehaviour
{
    public class AxisValue
    {
        public string axisName;
        public float axisValue;
    }

    public UltimateButton buttonLeft;
    public UltimateButton buttonRight;

    public GenericInput buttonInput;
    public float sensitivity = 3;
    public bool snap = true; // 按下反方向时立即变为从0开始

    private List<AxisValue> axisStack = new List<AxisValue>();
    private AxisValue leftValue = new AxisValue();
    private AxisValue rightValue = new AxisValue();

    private float realValue;
    CrossPlatformInputManager.VirtualAxis m_Axis;

    private void OnEnable()
    {
        if (!CrossPlatformInputManager.AxisExists(buttonInput.buttonName))
        {
            m_Axis = new CrossPlatformInputManager.VirtualAxis(buttonInput.buttonName);
            CrossPlatformInputManager.RegisterVirtualAxis(m_Axis);
        }
        else
        {
            m_Axis = CrossPlatformInputManager.VirtualAxisReference(buttonInput.buttonName);
        }
    }

    void Start()
    {
        InitButton();

        if (buttonLeft.buttonName=="")
        {
            buttonLeft.buttonName = "buttonLeft";
        }
        if (buttonRight.buttonName == "")
        {
            buttonRight.buttonName = "buttonRight";
        }

        // 没啥用.
        leftValue.axisName = buttonLeft.buttonName;
        rightValue.axisName = buttonRight.buttonName;

        buttonLeft.OnGetButtonDown += () =>
        {
            if (axisStack.Count < 2 && !axisStack.Contains(leftValue))
            {
                leftValue.axisValue = -1;
                axisStack.Add(leftValue);
            }
        };
        buttonLeft.OnGetButton += () => { leftValue.axisValue = -1; };
        buttonLeft.OnGetButtonUp += () =>
        {
            leftValue.axisValue = 0;
            axisStack.Remove(leftValue);
            if (snap)
            {
                m_Axis.Update(0);
            }
        };

        buttonRight.OnGetButtonDown += () =>
        {
            if (axisStack.Count < 2 && !axisStack.Contains(rightValue))
            {
                rightValue.axisValue = 1;
                axisStack.Add(rightValue);
            }
        };
        buttonRight.OnGetButton += () => { rightValue.axisValue = 1; };
        buttonRight.OnGetButtonUp += () =>
        {
            rightValue.axisValue = 0;
            axisStack.Remove(rightValue);
            if (snap)
            {
                m_Axis.Update(0);
            }
        };

    }

    private void InitButton()
    {
        buttonLeft.regist = false;
        buttonRight.regist = false;
    }

    void Update()
    {
        InitButton();

        realValue = axisStack.Count > 0 ? axisStack[axisStack.Count - 1].axisValue : 0;

#if MOBILE_INPUT
        //两个键同时按下时不更新Axis只更新AxisRaw
        m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, realValue, sensitivity * Time.deltaTime), axisStack.Count <2);
#endif
        //Debug.Log(axisStack.Count + " " + realValue + " " + buttonInput.GetAxis());
        //Debug.Log("Horizontal Axis,AxisRaw: " + Input.GetAxis("Horizontal") + " " + Input.GetAxisRaw("Horizontal") + " "
        //    + buttonInput.GetAxis() + " " + buttonInput.GetAxisRaw()); //范围(-1,1), 范围-1,0,1(只有3个值)
    }
}
