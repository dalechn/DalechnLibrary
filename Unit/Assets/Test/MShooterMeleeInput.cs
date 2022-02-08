using Invector;
using Invector.vCharacterController;
using UnityEngine;

public class mShooterMeleeInput : vShooterMeleeInput
{
    public virtual Vector3 AimIKPosition { get; protected set; }

    protected override void UpdateAimBehaviour()
    {
        if (cc.isDead)
        {
            return;
        }

        UpdateAimPosition();
        UpdateHeadTrack();

        if (shooterManager && CurrentActiveWeapon)
        {
            armAlignmentWeight = IsAiming && aimConditions && CanRotateAimArm() ? Mathf.Lerp(armAlignmentWeight, Mathf.Clamp(cc.upperBodyInfo.normalizedTime, 0, 1f), shooterManager.smoothArmWeight * (.001f + Time.deltaTime)) : 0;
            UpdateCheckAimHelpers(shooterManager.IsLeftWeapon);

            AimIKPosition = cameraMain.transform.position + cameraMain.transform.forward * 100;
            if (aimConditions)
            {
                float aimWeight =IsAiming &&!isReloading &&  aimConditions ? 1 : 0;
                IKReference.Instance?.SetUpAimIK(CurrentActiveWeapon.aimReference, null, AimIKPosition, aimWeight/*, isReloading*/);
            }
        }

        CheckAimConditions();
        UpdateAimHud();
        DoShots();

    }

    //protected override void InputHandle()
    //{
    //    base.InputHandle();

    //    if (!animator.GetBool(vAnimatorParameters.Shoot))
    //    {
    //        bl_EventHandler.DoCameraRecoilBack();
    //    }
    //}

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(AimPosition, 0.1f);

    }

}