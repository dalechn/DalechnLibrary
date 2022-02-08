using RootMotion.FinalIK;
using UnityEngine;

public class IKReference : MonoBehaviour
{
    private static IKReference _instance;
    public static IKReference Instance
    {
        get
        {
            if (_instance == null) { _instance = FindObjectOfType<IKReference>(); }
            return _instance;
        }
    }

    //public LookAtIK lookAtIK;
    public AimIK lookAtIK;
    public AimIK aimIK;
    public GrounderFBBIK grounder;
    public FullBodyBipedIK ik;
    public InteractionSystem interactionSystem;
    public Recoil recoil;

    public float aimAtIKSmooth = 30;
    private float lookAtIKSmooth;
    [Tooltip("The direction in which the weapon is aimed in animation (in character space). Tweak this value to adjust the aiming.")]
    public Vector3 animatedAimDirection = Vector3.forward;

    [Range(0f, 1f)] public float headLookWeight = 1f;
    public Vector3 gunHoldOffset;
    public Vector3 leftHandOffset;

    private Vector3 headLookAxis;
    private Vector3 leftHandPosRelToRightHand;
    private Quaternion leftHandRotRelToRightHand;
    //private Vector3 aimTarget;
    private Quaternion rightHandRotation;

    //private float lookAtIKDisableTime;

    //private void FixedUpdate()
    //{
    //    lookAtIKDisableTime = Mathf.Max(0, lookAtIKDisableTime - Time.fixedDeltaTime);
    //}

    protected void Awake()
    {
        if (ik != null)
        {
            ik.enabled = false;

            ik.solver.OnPreRead += OnPreRead;
        }
        if (lookAtIK != null) lookAtIK.enabled = false;
        if (aimIK != null) aimIK.enabled = false;
        if (grounder != null) grounder.enabled = false;
        if (interactionSystem != null) interactionSystem.enabled = false;

        headLookAxis = ik.references.head.InverseTransformVector(ik.references.root.forward);
    }

    public void SetUPLookAtIK(Vector3 point, float headWeight, float spineWeight, float smooth)
    {
        lookAtIKSmooth = smooth;
        if (lookAtIK)
        {
            lookAtIK.solver.IKPosition = point;

            for(int i= 0;i <lookAtIK.solver.bones.Length-1;i++)
            {
                lookAtIK.solver.bones[i].weight = spineWeight;
            }
            lookAtIK.solver.bones[lookAtIK.solver.bones.Length-1].weight = headWeight;

            //lookAtIK.solver.headWeight = headWeight;
            //lookAtIK.solver.bodyWeight = spineWeight;

        }

    }

    public void SetUpAimIK(Transform aimTransform, Transform poleTransform, Vector3 target, float aimWeight/*, bool reloading*/)
    {
        //// 暂时禁用lookatIK 0.3秒
        //if (aimIK.solver.IKPositionWeight > 0.1f)
        //{
        //    lookAtIKDisableTime = 0.3f;
        //}

        //lookAtIK.solver.IKPositionWeight = aimWeight > 0 || lookAtIKDisableTime > 0 || reloading ? 0 : Mathf.Lerp(lookAtIK.solver.IKPositionWeight, 1, lookAtIKSmooth);

        aimIK.solver.IKPositionWeight = aimWeight < 1 ? 0 : Mathf.Lerp(aimIK.solver.IKPositionWeight, 1, Time.deltaTime * aimAtIKSmooth);
        aimIK.solver.transform = aimTransform;
        aimIK.solver.poleTarget = poleTransform;

        if (animatedAimDirection != Vector3.zero)
        {
            aimIK.solver.axis = aimIK.solver.transform.InverseTransformVector(aimIK.transform.rotation * animatedAimDirection);
        }

        Read();
        if (aimWeight > 0)
        {
            lookAtIK.solver.IKPosition = target;
        }
        lookAtIK.solver.Update();

        aimIK.solver.IKPosition = target;
        aimIK.solver.Update();

        FBBIK();
        aimIK.solver.IKPosition = target;
        aimIK.solver.Update();

        //HeadLookAt(target);

    }

    public void FireRecoil(float magnitude)
    {
        recoil?.Fire(magnitude);
    }

    private void OnPreRead()
    {
        Quaternion r = recoil != null ? recoil.rotationOffset * rightHandRotation : rightHandRotation;
        Vector3 leftHandTarget = ik.references.rightHand.position + ik.solver.rightHandEffector.positionOffset + r * leftHandPosRelToRightHand;
        ik.solver.leftHandEffector.positionOffset += leftHandTarget - ik.references.leftHand.position - ik.solver.leftHandEffector.positionOffset + r * leftHandOffset;
    }


    private void Read()
    {
        leftHandPosRelToRightHand = ik.references.rightHand.InverseTransformPoint(ik.references.leftHand.position);
        leftHandRotRelToRightHand = Quaternion.Inverse(ik.references.rightHand.rotation) * ik.references.leftHand.rotation;
    }

    private void FBBIK()
    {
        rightHandRotation = ik.references.rightHand.rotation;

        Vector3 rightHandOffset = ik.references.rightHand.rotation * gunHoldOffset;
        ik.solver.rightHandEffector.positionOffset += rightHandOffset;

        if (recoil != null) recoil.SetHandRotations(rightHandRotation * leftHandRotRelToRightHand, rightHandRotation);

        ik.solver.Update();

        if (recoil != null)
        {
            ik.references.rightHand.rotation = recoil.rotationOffset * rightHandRotation;
            ik.references.leftHand.rotation = recoil.rotationOffset * rightHandRotation * leftHandRotRelToRightHand;
        }
        else
        {
            ik.references.rightHand.rotation = rightHandRotation;
            ik.references.leftHand.rotation = rightHandRotation * leftHandRotRelToRightHand;
        }
    }

    private void HeadLookAt(Vector3 lookAtTarget)
    {
        Quaternion headRotationTarget = Quaternion.FromToRotation(ik.references.head.rotation * headLookAxis, lookAtTarget - ik.references.head.position);
        ik.references.head.rotation = Quaternion.Lerp(Quaternion.identity, headRotationTarget, headLookWeight) * ik.references.head.rotation;
    }

    private void OnDestroy()
    {
        if (ik != null) ik.solver.OnPreRead -= OnPreRead;
    }
}
