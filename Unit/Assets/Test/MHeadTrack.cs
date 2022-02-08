using Invector;
using Invector.vCharacterController;
using UnityEngine;

public class mHeadTrack : vHeadTrack
{
    [vEditorToolbar("Settings")]

    public float crouchHeadWeight = 1f;
    public float crouchBodyWeight = 1f;

    public override void SetLookAtPosition(Vector3 point, float headWeight, float spineWeight)
    {
        IKReference.Instance?.SetUPLookAtIK(point,headWeight,spineWeight,Smooth);
    }

    protected override Vector3 GetLookPoint()
    {
        if (animator == null)
        {
            return Vector3.zero;
        }

        var distanceToLook = 100;
        if (lookConditions && !IgnoreHeadTrack())
        {
            var dir = transform.forward;
            if (temporaryLookTime <= 0)
            {
                var lookPosition = headPoint + (transform.forward * distanceToLook);
                if (followCamera)
                {
                    lookPosition = (cameraMain.transform.position + (cameraMain.transform.forward * distanceToLook));
                }

                dir = lookPosition - headPoint;
                if ((followCamera && !alwaysFollowCamera) || !followCamera)
                {
                    if (simpleTarget != null)
                    {
                        dir = simpleTarget.position - headPoint;
                        if (currentLookTarget && currentLookTarget == lastLookTarget)
                        {
                            currentLookTarget.ExitLook(this);
                            lastLookTarget = null;
                        }
                    }
                    else if (currentLookTarget != null && (currentLookTarget.ignoreHeadTrackAngle || TargetIsOnRange(currentLookTarget.lookPoint - headPoint)) && currentLookTarget.IsVisible(headPoint, obstacleLayer))
                    {
                        dir = currentLookTarget.lookPoint - headPoint;
                        if (currentLookTarget != lastLookTarget)
                        {
                            currentLookTarget.EnterLook(this);
                            lastLookTarget = currentLookTarget;
                        }
                    }
                    else if (currentLookTarget && currentLookTarget == lastLookTarget)
                    {
                        currentLookTarget.ExitLook(this);
                        lastLookTarget = null;
                    }
                }
            }
            else
            {
                dir = temporaryLookPoint - headPoint;
                temporaryLookTime -= Time.deltaTime;
                if (currentLookTarget && currentLookTarget == lastLookTarget)
                {
                    currentLookTarget.ExitLook(this);
                    lastLookTarget = null;
                }
            }

            var angle = GetTargetAngle(dir);
            if (cancelTrackOutOfAngle && (lastLookTarget == null || !lastLookTarget.ignoreHeadTrackAngle))
            {
                if (TargetIsOnRange(dir))
                {
                    if (animator.GetBool(vAnimatorParameters.IsStrafing) && !IsAnimatorTag("Upperbody Pose"))
                    {
                        SmoothValues(strafeHeadWeight, strafeBodyWeight, angle.x, angle.y);
                    }
                    else if (animator.GetBool(vAnimatorParameters.IsStrafing) && IsAnimatorTag("Upperbody Pose"))
                    {
                        SmoothValues(aimingHeadWeight, aimingBodyWeight, angle.x, angle.y);
                    }
                    else
                    {
                        if (animator.GetBool(vAnimatorParameters.IsCrouching))
                        {
                            SmoothValues(crouchHeadWeight, crouchBodyWeight, angle.x, angle.y);
                        }
                        else
                        {
                            SmoothValues(freeHeadWeight, freeBodyWeight, angle.x, angle.y);
                        }
                    }
                }
                else
                {
                    SmoothValues();
                }
            }
            else
            {
                if (animator.GetBool(vAnimatorParameters.IsStrafing) && !IsAnimatorTag("Upperbody Pose"))
                {
                    SmoothValues(strafeHeadWeight, strafeBodyWeight, angle.x, angle.y);
                }
                else if (animator.GetBool(vAnimatorParameters.IsStrafing) && IsAnimatorTag("Upperbody Pose"))
                {
                    SmoothValues(aimingHeadWeight, aimingBodyWeight, angle.x, angle.y);
                }
                else
                {
                    if(animator.GetBool(vAnimatorParameters.IsCrouching))
                    {
                        SmoothValues(crouchHeadWeight, crouchBodyWeight, angle.x, angle.y);
                    }
                    else
                    {
                        SmoothValues(freeHeadWeight, freeBodyWeight, angle.x, angle.y);
                    }
                }
            }
            if (targetsInArea.Count > 1)
            {
                SortTargets();
            }
        }
        else
        {
            SmoothValues();
            if (targetsInArea.Count > 1)
            {
                SortTargets();
            }
        }

        var rotA = Quaternion.AngleAxis(yRotation, transform.up);
        var rotB = Quaternion.AngleAxis(xRotation, transform.right);
        var finalRotation = (rotA * rotB);
        var lookDirection = finalRotation * transform.forward;
        return headPoint + (lookDirection * distanceToLook);
    }

    private void OnDrawGizmosSelected()
    {
        if (head)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(head.position, currentLookPosition);
        }
    }
}
