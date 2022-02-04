using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vShooter;
using Invector;
using RootMotion.FinalIK;

public class MShooterWeapon : vShooterWeapon
{
    [vEditorToolbar("Weapon Settings")]
    public ShakerPresent shakerPresent;
    public float  influence = 0.5f;
    public float recoilMagnitude = 1;

    protected override void HandleShot(Vector3 aimPosition)
    {
        base.HandleShot(aimPosition);

        if (shakerPresent)
        {
            bl_EventHandler.DoPlayerCameraShake(shakerPresent, ShakeType.Gun.ToString(), isAiming ? influence / 2 : influence);
        }
        IKReference.Instance?.FireRecoil(recoilMagnitude);

    }

}
