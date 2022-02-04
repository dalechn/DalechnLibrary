using UnityEngine;
using System;
using UnityEngine.Events;

public static class bl_EventHandler
{
    [Serializable]public class UEvent : UnityEvent { }

    public delegate void LocalPlayerShakeEvent(ShakerPresent present, string key, float influence = 1);
    public static LocalPlayerShakeEvent onLocalPlayerShake;

    public delegate void CameraRecoilEvent(float horizontal, float up);
    public static CameraRecoilEvent onCameraRecoil;

    public delegate void CameraRecoilBackEvent();
    public static CameraRecoilBackEvent onCameraRecoilBack;

    public delegate void CameraSwayEvent(float cameraStability);
    public static CameraSwayEvent onCameraSway;

    public delegate void IKRecoil(Vector3 pos);
    public static IKRecoil onIKRecoil;

    public static void DoPlayerCameraShake(ShakerPresent present, string key, float influence = 1) => onLocalPlayerShake?.Invoke(present, key, influence);

    public static void DoCameraRecoil(float horizontal, float up) => onCameraRecoil?.Invoke( horizontal, up);
    public static void DoCameraRecoilBack() => onCameraRecoilBack?.Invoke();
    public static void DoCameraSway(float cameraStability) => onCameraSway?.Invoke(cameraStability);

    public static void DoIKRecoil(Vector3 pos) => onIKRecoil?.Invoke(pos);
}