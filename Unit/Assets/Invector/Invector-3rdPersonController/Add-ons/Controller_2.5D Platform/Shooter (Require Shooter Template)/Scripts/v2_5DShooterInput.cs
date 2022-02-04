using System.Collections;
using UnityEngine;
namespace Invector.vCharacterController.v2_5D
{
    using vShooter;
    [vClassHeader("Shooter 2.5D Input")]
    public class v2_5DShooterInput : vShooterMeleeInput
    {
        [vEditorToolbar("2D Aiming")]
        public bool lookToCursorOnAiming = true;

        private v2_5DController _controller;
        RaycastHit hitTarget;
        public v2_5DController controller
        {
            get
            {
                if (cc && cc is v2_5DController && _controller == null)
                {
                    _controller = cc as v2_5DController;
                }

                return _controller;
            }
        }

        protected override void Start()
        {
            base.Start();
            vMousePositionHandler.Instance.SetMousePosition(cameraMain.WorldToScreenPoint(transform.position + Vector3.up + transform.forward * 100f));
        }

        protected override bool IsAimAlignWithForward()
        {
            return true;
        }

        protected override void UpdateAimPosition()
        {
            if (!isAimingByInput || !controller)
            {
                return;
            }

            if (lookToCursorOnAiming)
            {
                UpdateAimPositionFromCursor();
            }
            else
            {
                UpdateAimPositionFromForward();
            }
        }

        protected virtual void UpdateAimPositionFromCursor()
        {
            Vector3 localPos = controller.localCursorPosition;
            localPos.x = 0;

            if (localPos.z < .1f)
            {
                localPos.z = .1f;
            }

            Vector3 wordPos = transform.TransformPoint(localPos);
            Vector3 lookDirection = (wordPos - aimAngleReference.transform.position);
            lookDirection = lookDirection.normalized * (lookDirection.magnitude < 2f ? 2f : lookDirection.magnitude);

            if (localPos.z < 1f)
            {
                localPos.z = 1f;
            }

            wordPos = transform.TransformPoint(localPos);
            AimPosition = aimAngleReference.transform.position + lookDirection;
            headTrack.SetTemporaryLookPoint(wordPos, 0.1f);
        }

        //protected virtual void UpdateAimPositionFromCursor()
        //{
        //    var localAimPos = transform.InverseTransformPoint(controller.worldCursorPosition);
        //    if (localAimPos.z >= 0 && localAimPos.z < 1)
        //    {
        //        localAimPos.z = 1f;
        //    }
        //    else if (localAimPos.z < 0 && localAimPos.z > -1)
        //    {
        //        localAimPos.z = -1f;
        //    }
        //    AimPosition = transform.TransformPoint(localAimPos);
        //    headTrack.SetTemporaryLookPoint(AimPosition, 0.1f);
        //}

        protected override void UpdateHeadTrackLookPoint()
        {
            if (IsAiming && !isUsingScopeView)
            {
                headTrack.SetTemporaryLookPoint(AimPosition, 0.1f);
            }
        }
        protected virtual void UpdateAimPositionFromForward()
        {
            AimPosition = aimAngleReference.gameObject.transform.position + transform.forward * 100f;
        }

        protected override void UpdateAimHud()
        {
            if (!shooterManager || !controlAimCanvas)
            {
                return;
            }

            if (CurrentActiveWeapon == null)
            {
                return;
            }

            controlAimCanvas.SetAimCanvasID(CurrentActiveWeapon.scopeID);
            if (isAimingByInput)
            {
                controlAimCanvas.SetWordPosition(controller.worldCursorPosition, aimConditions);
            }
            else
            {
                controlAimCanvas.SetAimToCenter(true);
            }
        }

        protected override Vector3 targetArmAligmentDirection
        {
            get
            {
                return transform.forward;
            }
        }

        public override void ScopeViewInput()
        {
            ///Ignore ScopeView
        }
    }
}