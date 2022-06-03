using CW.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using Lean.Touch;
using UnityEngine.Events;

public class CrosshairCtrl : MonoBehaviour
{
    //public DoubleJoystick doubleJoystick;

    public GameObject clickAni;
    public float damping = -1.0f;
    public float rotDamping = -1.0f;
    private Quaternion rot;

    private LeanTouchEvent touch;
    public UnityAction<GameObject> crosshairEvent;

    void Start()
    {
        bl_UCrosshair.Instance.SetActive(false);

        touch = GetComponent<LeanTouchEvent>();
        //touch.enabled = false;
    }

    public void OnTouchBegin(Lean.Touch.TouchData data)
    {

        //doubleJoystick.EnableJoystickRight(true);
        if(clickAni)
        {
            clickAni.SetActive(true);
        }
        else
        {
            bl_UCrosshair.Instance.SetActive(true);
        }
        crosshairEvent?.Invoke(gameObject);
    }

    public void OnTouchMoved(Lean.Touch.TouchData data)
    {
        //bl_UCrosshair.Instance.SetActive(true);

        bl_UCrosshair.Instance.SetColor(data.worldToState);
        bl_UCrosshair.Instance.OnFire();
        bl_UCrosshair.Instance.FollowMouseControll(data.screenTo);

        var factor = CwHelper.DampenFactor(damping, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, data.worldTo, factor);

        var rotFactor = CwHelper.DampenFactor(rotDamping, Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, data.worldNormal) * transform.rotation, rotFactor);

    }

    public void OnTouchEnded(Lean.Touch.TouchData date)
    {
        if (clickAni)
        {
            clickAni.SetActive(false);
        }
        else
        {
            bl_UCrosshair.Instance.SetActive(false);
            crosshairEvent?.Invoke(null);
        }

        //doubleJoystick.EnableJoystickRight(false);
    }

    public void UpdateTouch()
    {
        touch.UpdateTransform();
    }
}
