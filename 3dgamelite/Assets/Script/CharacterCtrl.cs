using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;

public class CharacterCtrl : MonoBehaviour
{
    public RootMotion.Demos.UserControlAI userCtrl;
    public CameraController cameCtrl;
    public CrosshairCtrl crosshairCtrl;

    public float playerSpeed = 5f;
    public float rotateSpeed = 10f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

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
        crosshairCtrl.crosshairEvent += userCtrl.SetLookObject;

        // Physics materials
        zeroFrictionMaterial = new PhysicMaterial();
        zeroFrictionMaterial.dynamicFriction = 0f;
        zeroFrictionMaterial.staticFriction = 0f;
        zeroFrictionMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        zeroFrictionMaterial.bounciness = 0f;
        zeroFrictionMaterial.bounceCombine = PhysicMaterialCombine.Minimum;

        highFrictionMaterial = new PhysicMaterial();
    }

    void Update()
    {
        //groundedPlayer = controller.isGrounded;
        //if (groundedPlayer && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = 0f;
        //}

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

        //if (userCtrl.state.jump && groundedPlayer)
        //{
        //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //}

        //playerVelocity.y += gravityValue * Time.deltaTime;
        //controller.Move(playerVelocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = fixedDeltaTime > 0f ? fixedDeltaPosition / fixedDeltaTime : Vector3.zero;

        body.velocity = velocity;
        body.MoveRotation(fixedDeltaRotation);

        fixedDeltaTime = 0f;
        fixedDeltaPosition = Vector3.zero;

        if (userCtrl.state.move == Vector3.zero ) capsule.material = highFrictionMaterial;
        else capsule.material = zeroFrictionMaterial;

        fixedFrame = true;
    }

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