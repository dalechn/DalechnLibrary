using RootMotion;
using UnityEngine;


public class CharacterCtrl : MonoBehaviour
{
    public RootMotion.Demos.UserControlAI userCtrl;
    public CameraController cameCtrl;
    public CrosshairCtrl crosshairCtrl;

    public float playerSpeed = 5f;
    public float rotateSpeed = 10f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    public LayerMask testRayMask;

    private CharacterController controller;
    private Rigidbody body;
    private CapsuleCollider capsule;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private float fixedDeltaTime;
    private Vector3 fixedDeltaPosition;
    private Quaternion fixedDeltaRotation = Quaternion.identity;

    private bool fixedFrame;
    private PhysicMaterial zeroFrictionMaterial;
    private PhysicMaterial highFrictionMaterial;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        body = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        cameCtrl.enabled = false;
        crosshairCtrl.crosshairEvent += userCtrl.SetLookObject; // 设置lookObject 暂时不测试了

        // Physics materials
        zeroFrictionMaterial = new PhysicMaterial();
        zeroFrictionMaterial.dynamicFriction = 0f;
        zeroFrictionMaterial.staticFriction = 0f;
        zeroFrictionMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        zeroFrictionMaterial.bounciness = 0f;
        zeroFrictionMaterial.bounceCombine = PhysicMaterialCombine.Minimum;

        highFrictionMaterial = new PhysicMaterial();
    }

    //不触发情况:Static Trigger Collider / Static Trigger Collider
    private void OnTriggerEnter(Collider other)
    {

    }

    //不触发情况:Static Collider,Kinematic Rigidbody Collider之间的相互碰撞
    private void OnCollisionEnter(Collision collision)
    {

    }

    protected static RaycastHit[] s_RaycastHitCache = new RaycastHit[32];
    protected static Collider[] s_ColliderCache = new Collider[32];

    private void PhysicsTest()
    {
        // raycast和linecast的开销非常小
        // CheckXXX <OverlapXXX<XXXCast
        // Box < Sphere < Capsule

        //detect (发现)
        //obstruct (阻碍)
        RaycastHit result;
        int resultCounts;
        bool hit;

        //const float kDetectLength = 1;
        //Vector3 direction = transform.forward;
        //Ray ray = new Ray(transform.position - direction * kDetectLength, direction);
        // hit = Physics.Raycast(ray, out result, kDetectLength*2, testRayMask, QueryTriggerInteraction.Ignore);
        //resultCounts= Physics.RaycastNonAlloc(ray, s_RaycastHitCache, kDetectLength * 2, testRayMask, QueryTriggerInteraction.Ignore);
        //s_RaycastHitCache = Physics.RaycastAll(ray, kDetectLength * 2, testRayMask, QueryTriggerInteraction.Ignore);
        //if (hit)
        //{
        //    Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        //}

        ////box
        //const float kDetectLength = 1;
        //Vector3 halfExtents = Vector3.one;
        //Vector3 direction = transform.forward;
        //hit = Physics.BoxCast(transform.position - direction * kDetectLength, halfExtents, direction,
        //    out result, transform.rotation, kDetectLength * 2f, testRayMask, QueryTriggerInteraction.Ignore);
        //resultCounts = Physics.BoxCastNonAlloc(transform.position + direction * kDetectLength, halfExtents, direction,
        //    s_RaycastHitCache, transform.rotation, kDetectLength * 2f, testRayMask, QueryTriggerInteraction.Ignore);

        //bool check = Physics.CheckBox(transform.position, halfExtents, transform.rotation, testRayMask, QueryTriggerInteraction.Ignore);
        //s_ColliderCache = Physics.OverlapBox(transform.position, halfExtents, transform.rotation, testRayMask, QueryTriggerInteraction.Ignore);
        //resultCounts = Physics.OverlapBoxNonAlloc(transform.position, halfExtents,  s_ColliderCache, transform.rotation, testRayMask, QueryTriggerInteraction.Ignore);

        ////capsule
        //const float kDetectLength = 1;
        //const float kSpherecastRadius = 0.5f;
        //const float kCastHeight = 1;
        //Vector3 direction = transform.forward;
        //var center = transform.position;
        //var up = transform.up;
        //var pt1 = center + up * kCastHeight / 2f;
        //var pt2 = center - up * kCastHeight / 2f;
        //hit = Physics.CapsuleCast(pt1, pt2, kSpherecastRadius, direction, out result, kDetectLength, testRayMask, QueryTriggerInteraction.Ignore);
        //resultCounts = Physics.CapsuleCastNonAlloc(pt1, pt2, kSpherecastRadius, direction, s_RaycastHitCache, kDetectLength, testRayMask, QueryTriggerInteraction.Ignore);

        //bool check  = Physics.CheckCapsule(pt1, pt2, kSpherecastRadius, testRayMask, QueryTriggerInteraction.Ignore);
        //s_ColliderCache = Physics.OverlapCapsule(pt1, pt2, kSpherecastRadius, testRayMask, QueryTriggerInteraction.Ignore);
        //resultCounts = Physics.OverlapCapsuleNonAlloc(pt1, pt2, kSpherecastRadius, s_ColliderCache, testRayMask, QueryTriggerInteraction.Ignore);

        ////sphere
        //const float kDetectLength = 1;
        //const float kSpherecastRadius = 0.5f;
        //Vector3 direction = -transform.up;
        //Ray ray = new Ray(transform.position - direction * kDetectLength, direction);
        //hit = Physics.SphereCast(ray, kSpherecastRadius, out result, kDetectLength * 2f, testRayMask, QueryTriggerInteraction.Ignore);
        //resultCounts = Physics.SphereCastNonAlloc(ray, kSpherecastRadius, s_RaycastHitCache, kDetectLength * 2f, testRayMask, QueryTriggerInteraction.Ignore);

        //bool check = Physics.CheckSphere(transform.position, kSpherecastRadius, testRayMask, QueryTriggerInteraction.Ignore);
        //s_ColliderCache = Physics.OverlapSphere(transform.position, kSpherecastRadius, testRayMask, QueryTriggerInteraction.Ignore);
        //resultCounts = Physics.OverlapSphereNonAlloc(transform.position, kSpherecastRadius, s_ColliderCache, testRayMask, QueryTriggerInteraction.Ignore);
    }

    private void Update()
    {
        if (userCtrl.state.move != Vector3.zero)
        {
            Vector3 rotateDir = userCtrl.state.strafe ? userCtrl.state.lookPos - transform.position : userCtrl.state.move;
            Vector3 _rotNormal = transform.up;
            Vector3.OrthoNormalize(ref _rotNormal, ref rotateDir);

            fixedDeltaRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateDir), rotateSpeed * Time.deltaTime);
            //transform.rotation = fixedDeltaRotation;
        }

        Vector3 moveDir = userCtrl.state.strafe ? userCtrl.state.move : userCtrl.state.move.magnitude * transform.forward;
        Vector3 v = moveDir * Time.deltaTime * playerSpeed;

        //Debug.DrawRay(transform.position,transform.InverseTransformDirection(userCtrl.state.move));
        //if(controller)
        //{
        //    controller.Move(v);
        //    controller.SimpleMove(moveDir * playerSpeed);
        //}

        //transform.position += v;
        //RaycastHit hit;
        //if (!body.SweepTest(moveDir, out hit, Time.deltaTime * playerSpeed))
        //{
        //    body.MovePosition(body.position + v);
        //}
        //body.MoveRotation(fixedDeltaRotation);

        fixedDeltaTime += Time.deltaTime;
        fixedDeltaPosition += v;
        fixedDeltaRotation *= Quaternion.identity;

    }

    private void FixedUpdate()
    {
        Vector3 velocity = fixedDeltaTime > 0f ? fixedDeltaPosition / fixedDeltaTime : Vector3.zero;

        body.velocity = velocity;
        body.MoveRotation(fixedDeltaRotation);

        fixedDeltaTime = 0f;
        fixedDeltaPosition = Vector3.zero;

        if (userCtrl.state.move == Vector3.zero) capsule.material = highFrictionMaterial;
        else capsule.material = zeroFrictionMaterial;

        fixedFrame = true;

        PhysicsTest();
    }

    // 未知: 相机跟随Interpolation会卡顿?
    private void LateUpdate()
    {
        if (cameCtrl == null) return;

        cameCtrl.UpdateInput();

        if (!fixedFrame && body.interpolation == RigidbodyInterpolation.None)
        {
            //Debug.Log("return");
            return;
        }

        cameCtrl.UpdateTransform(body.interpolation == RigidbodyInterpolation.None ? Time.fixedDeltaTime : Time.deltaTime);
        //crosshairCtrl.UpdateTouch();

        fixedFrame = false;
    }

}