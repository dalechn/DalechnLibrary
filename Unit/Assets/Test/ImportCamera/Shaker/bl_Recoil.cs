using Invector.vCamera;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class bl_Recoil : MonoBehaviour
{
    public bool AutomaticallyComeBack = true;
    public vThirdPersonCamera cam;

    private Transform m_Transform;
    private bool wasFiring = false;

    public bool applyRecoilToCamera = true;
    public bool applySwayToCamera = true;
    public float cameraMaxSwayAmount = 2f;
    public float cameraSwaySpeed = .5f;

    private float tempH;
    private float tempV;

    private void OnEnable()
    {
        bl_EventHandler.onCameraRecoil += RecoilControl;
        bl_EventHandler.onCameraRecoilBack += RecoilBack;
        bl_EventHandler.onCameraSway += CameraSway;
        bl_EventHandler.onIKRecoil += IKRecoil;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        bl_EventHandler.onCameraRecoil -= RecoilControl;
        bl_EventHandler.onCameraRecoilBack -= RecoilBack;
        bl_EventHandler.onCameraSway -= CameraSway;
        bl_EventHandler.onIKRecoil -= IKRecoil;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        GameObject g = new GameObject("Recoil");
        m_Transform = g.transform;
        m_Transform.parent = transform.parent;
        m_Transform.localPosition = Vector3.zero;
        m_Transform.localEulerAngles = Vector3.zero;

        transform.parent = m_Transform;
    }

    /// <summary>
    /// 
    /// </summary>
    private void RecoilControl(float horizontal, float up)
    {
        if (applyRecoilToCamera)
        {
            wasFiring = true;
            cam.RotateCamera(horizontal, up);

            tempH = horizontal;
            tempV = up;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RecoilBack()
    {
        if (applyRecoilToCamera&&wasFiring)
        {
            wasFiring = false;
            StartCoroutine(Recoil());
        }
    }

    protected virtual IEnumerator Recoil()
    {
        cam.RotateCamera(-tempH , -tempV );

        yield return new WaitForEndOfFrame();
    }


    private void IKRecoil(Vector3 pos)
    {
    }

    private void CameraSway(float cameraStability)
    {
        if (applySwayToCamera)
        {
            float bx = (Mathf.PerlinNoise(0, Time.time * cameraSwaySpeed) - 0.5f);
            float by = (Mathf.PerlinNoise(0, (Time.time * cameraSwaySpeed) + 100)) - 0.5f;

            var swayAmount = cameraMaxSwayAmount * (1f - cameraStability);
            if (swayAmount == 0)
            {
                return;
            }

            bx *= swayAmount;
            by *= swayAmount;

            float tx = (Mathf.PerlinNoise(0, Time.time * cameraSwaySpeed) - 0.5f);
            float ty = ((Mathf.PerlinNoise(0, (Time.time * cameraSwaySpeed) + 100)) - 0.5f);

            tx *= -(swayAmount * 0.25f);
            ty *= (swayAmount * 0.25f);

            if (cam != null)
            {
                cam.offsetMouse.x = bx + tx;
                cam.offsetMouse.y = by + ty;
            }
        }
    }
}