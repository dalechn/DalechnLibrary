using CW.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCtrl : MonoBehaviour
{
    public float damping = -1.0f;
    public float rotDamping = -1.0f;
    protected Quaternion rot;

    void Start()
    {
        bl_UCrosshair.Instance.SetActive(false);
    }

    public  void SetPosition(Lean.Touch.TouchData data)
    {
        bl_UCrosshair.Instance.SetActive(true);

        bl_UCrosshair.Instance.SetColor(data.worldToState);
        bl_UCrosshair.Instance.OnFire();
        bl_UCrosshair.Instance.FollowMouseControll(data.screenTo);

        var factor = CwHelper.DampenFactor(damping, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, data.worldTo, factor);

        var rotFactor = CwHelper.DampenFactor(rotDamping, Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, data.worldNormal) * transform.rotation, rotFactor);

    }

    public void CrosshairDeactive(Lean.Touch.TouchData date)
    {
        bl_UCrosshair.Instance.SetActive(false);
    }
}
