﻿using UnityEngine;
using System.Collections;

namespace RootMotion.Demos
{

    /// <summary>
    /// Third person character controller. This class is based on the ThirdPersonCharacter.cs of the Unity Exmaple Assets.
    /// </summary>
    public class CharacterThirdPerson : CharacterBase
    {

        //删除
        // Is the character always rotating to face the move direction or is he strafing?
        //[System.Serializable]
        //public enum MoveMode
        //{
        //    Directional,
        //    Strafe
        //}

        // Animation state
        public struct AnimState
        {
            public Vector3 moveDirection; // the forward speed
            public bool jump; // should the character be jumping?
            public bool crouch; // should the character be crouching?
            public bool onGround; // is the character grounded
            public bool isStrafing; // should the character always rotate to face the move direction or strafe?
            public float yVelocity; // y velocity of the character
            public bool doubleJump;
        }

        [vSeparator("References")]
        public CharacterAnimationBase characterAnimation; // the animation controller
        public UserControlThirdPerson userControl; // user input
        public CameraController cam; // Camera controller (optional). If assigned will update the camera in LateUpdate only if character moves
        public FinalIK.GrounderIK ik;  //新增

        [vSeparator("Movement")]
        //public MoveMode moveMode; // Is the character always rotating to face the move direction or is he strafing?
        public bool smoothPhysics = true; // If true, will use interpolation to smooth out the fixed time step.
        public float smoothAccelerationTime = 0.2f; // The smooth acceleration of the speed of the character (using Vector3.SmoothDamp)
        public float linearAccelerationSpeed = 3f; // The linear acceleration of the speed of the character (using Vector3.MoveTowards)
        public float platformFriction = 7f;                 // the acceleration of adapting the velocities of moving platforms
        public float groundStickyEffect = 4f;               // power of 'stick to ground' effect - prevents bumping down slopes.
        public float maxVerticalVelocityOnGround = 3f;      // the maximum y velocity while the character is grounded
        public float velocityToGroundTangentWeight = 0f;    // the weight of rotating character velocity vector to the ground tangent

        [vSeparator("Rotation")]
        public bool lookInCameraDirection; // should the character be looking in the same direction that the camera is facing
        public float turnSpeed = 5f;                    // additional turn speed added when the player is moving (added to animation root rotation)
        public float stationaryTurnSpeedMlp = 1f;           // additional turn speed added when the player is stationary (added to animation root rotation)

        [vSeparator("Jumping and Falling")]
        public bool smoothJump = true; // If true, adds jump force over a few fixed time steps, not in a single step
        public float airSpeed = 6f; // determines the max speed of the character while airborne
        public float airControl = 2f; // determines the response speed of controlling the character while airborne
        public float jumpPower = 12f; // determines the jump force applied when jumping (and therefore the jump height)
        public float jumpRepeatDelayTime = 0f;          // amount of time that must elapse between landing and being able to jump again
        public bool doubleJumpEnabled;
        public float doubleJumpPowerMlp = 1f;

        [vSeparator("Wall Running")]

        public LayerMask wallRunLayers; // walkable vertical surfaces
        public float wallRunMaxLength = 1f;                 // max duration of a wallrun
        public float wallRunMinMoveMag = 0.6f;              // the minumum magnitude of the user control input move vector
        public float wallRunMinVelocityY = -1f;             // the minimum vertical velocity of doing a wall run
        public float wallRunRotationSpeed = 1.5f;           // the speed of rotating the character to the wall normal
        public float wallRunMaxRotationAngle = 70f;         // max angle of character rotation
        public float wallRunWeightSpeed = 5f;               // the speed of blending in/out the wall running effect

        [vSeparator("Crouching")]
        public float crouchCapsuleScaleMlp = 0.6f;			// the capsule collider scale multiplier while crouching

        /// <summary>
        /// Enable this while playing an animation that should be driven 100% by root motion, such as climbing walls
        /// </summary>
        public bool fullRootMotion { get; set; }

        public bool onGround { get; private set; }
        public AnimState animState = new AnimState();

        protected Vector3 moveDirection; // The current move direction of the character in Strafe move mode
        private Animator animator;
        private Vector3 /*normal, */platformVelocity, platformAngularVelocity;
        private RaycastHit hit;
        private float jumpLeg, jumpEndTime, forwardMlp, groundDistance, lastAirTime, stickyForce;
        private Vector3 wallNormal = Vector3.up;
        private Vector3 moveDirectionVelocity;
        private float wallRunWeight;
        private float lastWallRunWeight;
        private float fixedDeltaTime;
        private Vector3 fixedDeltaPosition;
        private Quaternion fixedDeltaRotation = Quaternion.identity;
        private bool fixedFrame;
        private float wallRunEndTime;
        private Vector3 gravity;
        private Vector3 verticalVelocity;
        private float velocityY;
        private bool doubleJumped;
        private bool jumpReleased;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            animator = GetComponent<Animator>();
            if (animator == null) animator = characterAnimation.GetComponent<Animator>();

            wallNormal = -gravity.normalized;
            onGround = true;
            animState.onGround = true;

            if (cam != null) cam.enabled = false;
        }

        void OnAnimatorMove()
        {
            Move(animator.deltaPosition, animator.deltaRotation);
        }

        // When the Animator moves
        public override void Move(Vector3 deltaPosition, Quaternion deltaRotation)
        {
            // Accumulate delta position, update in FixedUpdate to maintain consitency
            fixedDeltaTime += Time.deltaTime;
            fixedDeltaPosition += deltaPosition;
            fixedDeltaRotation *= deltaRotation;
        }

        void FixedUpdate()
        {
            GroundCheck(); // detect and stick to ground

            // 删除
            // gravity = fullRootMotion ? Vector3.zero : GetGravity();

            verticalVelocity = V3Tools.ExtractVertical(r.velocity, gravity, 1f);
            velocityY = verticalVelocity.magnitude;
            if (Vector3.Dot(verticalVelocity, gravity) > 0f) velocityY = -velocityY;

            // Smoothing out the fixed time step
            r.interpolation = smoothPhysics ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
            characterAnimation.smoothFollow = smoothPhysics;

            // Move
            MoveFixed(fixedDeltaPosition);

            fixedDeltaTime = 0f;
            fixedDeltaPosition = Vector3.zero;

            r.MoveRotation(transform.rotation * fixedDeltaRotation);
            fixedDeltaRotation = Quaternion.identity;

            Rotate();

            // Friction
            if (userControl.state.move == Vector3.zero && groundDistance < airborneThreshold * 0.5f) HighFriction();
            else ZeroFriction();

            bool stopSlide = !fullRootMotion && onGround && userControl.state.move == Vector3.zero && r.velocity.magnitude < 0.5f && groundDistance < airborneThreshold * 0.5f;

            // Individual gravity
            if (gravityTarget != null)
            {
                r.useGravity = false;

                if (!stopSlide) r.AddForce(gravity);
            }
            else if (ik)
            {
                r.AddForce(gravity);
            }
            if (stopSlide)
            {
                r.useGravity = false;
                r.velocity = Vector3.zero;
            }
            else if (gravityTarget == null) r.useGravity = true;

            if (onGround)
            {
                // Jumping
                animState.jump = Jump();
                jumpReleased = false;
                doubleJumped = false;
            }
            else
            {
                if (!userControl.state.jump) jumpReleased = true;

                r.AddForce(gravity * gravityMultiplier);
                if (jumpReleased && userControl.state.jump && !doubleJumped && doubleJumpEnabled)
                {
                    jumpEndTime = Time.time + 0.1f;
                    animState.doubleJump = true;

                    Vector3 jumpVelocity = userControl.state.move * airSpeed;
                    r.velocity = jumpVelocity;
                    r.velocity += transform.up * jumpPower * doubleJumpPowerMlp;
                    doubleJumped = true;
                }
            }

            // Scale the capsule colllider while crouching
            ScaleCapsule(userControl.state.crouch ? crouchCapsuleScaleMlp : 1f);

            fixedFrame = true;
        }

        protected virtual void Update()
        {
            // 提前旋转input方向
            //if (ik) userControl.UpdateInput(Quaternion.FromToRotation(transform.up, ik.normal));

            // Fill in animState
            animState.onGround = onGround;
            animState.moveDirection = GetMoveDirection();
            animState.yVelocity = Mathf.Lerp(animState.yVelocity, velocityY, Time.deltaTime * 10f);
            animState.crouch = userControl.state.crouch;
            // 修改
            animState.isStrafing = userControl.state.strafe;
        }

        protected virtual void LateUpdate()
        {
            if (cam == null) return;

            cam.UpdateInput();

            if (!fixedFrame && r.interpolation == RigidbodyInterpolation.None) return;

            // Update camera only if character moves
            cam.UpdateTransform(r.interpolation == RigidbodyInterpolation.None ? Time.fixedDeltaTime : Time.deltaTime);

            fixedFrame = false;
        }

        private void MoveFixed(Vector3 deltaPosition)
        {
            // Process horizontal wall-running
            //WallRun();

            Vector3 velocity = fixedDeltaTime > 0f ? deltaPosition / fixedDeltaTime : Vector3.zero;

            // Add velocity of the rigidbody the character is standing on
            if (!fullRootMotion)
            {
                velocity += V3Tools.ExtractHorizontal(platformVelocity, gravity, 1f);

                if (onGround)
                {
                    // Rotate velocity to ground tangent
                    if (velocityToGroundTangentWeight > 0f)
                    {
                        Quaternion rotation = Quaternion.FromToRotation(transform.up, hit.normal);
                        velocity = Quaternion.Lerp(Quaternion.identity, rotation, velocityToGroundTangentWeight) * velocity;
                    }
                }
                else
                {
                    // Air move
                    //Vector3 airMove = new Vector3 (userControl.state.move.x * airSpeed, 0f, userControl.state.move.z * airSpeed);
                    Vector3 airMove = V3Tools.ExtractHorizontal(userControl.state.move * airSpeed, gravity, 1f);
                    velocity = Vector3.Lerp(r.velocity, airMove, Time.deltaTime * airControl);
                }

                if (onGround && Time.time > jumpEndTime)
                {
                    r.velocity = r.velocity - transform.up * stickyForce * Time.deltaTime;
                }

                // Vertical velocity
                Vector3 verticalVelocity = V3Tools.ExtractVertical(r.velocity, gravity, 1f);
                Vector3 horizontalVelocity = V3Tools.ExtractHorizontal(velocity, gravity, 1f);

                if (onGround)
                {
                    if (Vector3.Dot(verticalVelocity, gravity) < 0f)
                    {
                        verticalVelocity = Vector3.ClampMagnitude(verticalVelocity, maxVerticalVelocityOnGround);
                    }
                }

                r.velocity = horizontalVelocity + verticalVelocity;
            }
            else
            {
                r.velocity = velocity;
            }

            // Dampering forward speed on the slopes (Not working since Unity 2017.2)
            //float slopeDamper = !onGround ? 1f : GetSlopeDamper(-deltaPosition / Time.deltaTime, hit.normal);
            //forwardMlp = Mathf.Lerp(forwardMlp, slopeDamper, Time.deltaTime * 5f);

            forwardMlp = 1;
        }

        // Processing horizontal wall running
        private void WallRun()
        {
            bool canWallRun = CanWallRun();

            // Remove flickering in and out of wall-running
            if (wallRunWeight > 0f && !canWallRun) wallRunEndTime = Time.time;
            if (Time.time < wallRunEndTime + 0.5f) canWallRun = false;

            wallRunWeight = Mathf.MoveTowards(wallRunWeight, (canWallRun ? 1f : 0f), Time.deltaTime * wallRunWeightSpeed);

            if (wallRunWeight <= 0f)
            {
                // Reset
                if (lastWallRunWeight > 0f)
                {
                    Vector3 frw = V3Tools.ExtractHorizontal(transform.forward, gravity, 1f);
                    transform.rotation = Quaternion.LookRotation(frw, -gravity);
                    wallNormal = -gravity.normalized;
                }
            }

            lastWallRunWeight = wallRunWeight;

            if (wallRunWeight <= 0f) return;

            // Make sure the character won't fall down
            if (onGround && velocityY < 0f) r.velocity = V3Tools.ExtractHorizontal(r.velocity, gravity, 1f);

            // transform.forward flattened
            Vector3 f = V3Tools.ExtractHorizontal(transform.forward, gravity, 1f);

            // Raycasting to find a walkable wall
            RaycastHit velocityHit = new RaycastHit();
            velocityHit.normal = -gravity.normalized;
            Physics.Raycast(onGround ? transform.position : capsule.bounds.center, f, out velocityHit, 3f, wallRunLayers);

            // Finding the normal to rotate to
            wallNormal = Vector3.Lerp(wallNormal, velocityHit.normal, Time.deltaTime * wallRunRotationSpeed);

            // Clamping wall normal to max rotation angle
            wallNormal = Vector3.RotateTowards(-gravity.normalized, wallNormal, wallRunMaxRotationAngle * Mathf.Deg2Rad, 0f);

            // Get transform.forward ortho-normalized to the wall normal
            Vector3 fW = transform.forward;
            Vector3 nW = wallNormal;
            Vector3.OrthoNormalize(ref nW, ref fW);

            // Rotate from upright to wall normal
            transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(f, -gravity), Quaternion.LookRotation(fW, wallNormal), wallRunWeight);
        }

        // Should the character be enabled to do a wall run?
        private bool CanWallRun()
        {
            if (fullRootMotion) return false;
            if (Time.time < jumpEndTime - 0.1f) return false;
            if (Time.time > jumpEndTime - 0.1f + wallRunMaxLength) return false;
            if (velocityY < wallRunMinVelocityY) return false;
            if (userControl.state.move.magnitude < wallRunMinMoveMag) return false;
            return true;
        }

        // 修改
        // Get the move direction of the character relative to the character rotation
        private Vector3 GetMoveDirection()
        {
            if (userControl.state.strafe)
            {
                moveDirection = Vector3.SmoothDamp(moveDirection, userControl.state.move, ref moveDirectionVelocity, smoothAccelerationTime);
                moveDirection = Vector3.MoveTowards(moveDirection, userControl.state.move, Time.deltaTime * linearAccelerationSpeed);
                return transform.InverseTransformDirection(moveDirection);
            }
            else
            {
                moveDirection = Vector3.SmoothDamp(moveDirection, new Vector3(0f, 0f, userControl.state.move.magnitude), ref moveDirectionVelocity, smoothAccelerationTime);
                moveDirection = Vector3.MoveTowards(moveDirection, new Vector3(0f, 0f, userControl.state.move.magnitude), Time.deltaTime * linearAccelerationSpeed);
                return moveDirection * forwardMlp;
            }
        }

        // Rotate the character
        protected virtual void Rotate()
        {
            if (gravityTarget != null) r.MoveRotation(Quaternion.FromToRotation(transform.up, transform.position - gravityTarget.position) * transform.rotation);
            else if (ik && !h)
            {
                r.MoveRotation(Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation); //新增
            }
            if (platformAngularVelocity != Vector3.zero) r.MoveRotation(Quaternion.Euler(platformAngularVelocity) * transform.rotation);

            float angle = GetAngleFromForward(GetForwardDirection());

            if (userControl.state.move == Vector3.zero) angle *= (1.01f - (Mathf.Abs(angle) / 180f)) * stationaryTurnSpeedMlp;

            // Rotating the character
            //RigidbodyRotateAround(characterAnimation.GetPivotPoint(), transform.up, angle * Time.deltaTime * turnSpeed);
            r.MoveRotation(Quaternion.AngleAxis(angle * Time.deltaTime * turnSpeed, transform.up) * r.rotation);
        }

        // 修改
        // Which way to look at?
        private Vector3 GetForwardDirection()
        {
            if (userControl.state.move != Vector3.zero)
            {
                return userControl.state.strafe ? userControl.state.lookPos - r.position : userControl.state.move;
            }
            else
            {
                return lookInCameraDirection ? userControl.state.lookPos - r.position : transform.forward;
            }
        }

        protected virtual bool Jump()
        {
            // check whether conditions are right to allow a jump:
            if (!userControl.state.jump) return false;
            if (userControl.state.crouch) return false;
            if (!characterAnimation.animationGrounded) return false;
            if (Time.time < lastAirTime + jumpRepeatDelayTime) return false;

            // Jump
            onGround = false;
            jumpEndTime = Time.time + 0.1f;

            Vector3 jumpVelocity = userControl.state.move * airSpeed;
            jumpVelocity += transform.up * jumpPower;

            if (smoothJump)
            {
                StopAllCoroutines();
                StartCoroutine(JumpSmooth(jumpVelocity - r.velocity));
            }
            else
            {
                r.velocity = jumpVelocity;
            }

            return true;
        }

        // Add jump velocity smoothly to avoid puppets launching to space when unpinned during jump acceleration
        private IEnumerator JumpSmooth(Vector3 jumpVelocity)
        {
            int steps = 0;
            int stepsToTake = 3;
            while (steps < stepsToTake)
            {
                r.AddForce((jumpVelocity) / stepsToTake, ForceMode.VelocityChange);
                steps++;
                yield return new WaitForFixedUpdate();
            }
        }

        private bool h;
        private void Raycast()
        {
            Ray ray = new Ray(r.position + transform.up * airborneThreshold, -transform.up);
            hit = new RaycastHit();
            h = Physics.SphereCast(ray, spherecastRadius, out hit, airborneThreshold * 2f, groundLayers);

            // 相当于hit.normal没用射线的值
            hit.normal = transform.up;
            if (!h)
            {
                hit.point = transform.position - transform.transform.up * airborneThreshold;
                hit.normal = Vector3.up;
            }

            if (fullRootMotion)
            {
                gravity = Vector3.zero;
            }
            else
            {
                if (gravityTarget != null)
                {
                    gravity = (gravityTarget.position - transform.position).normalized * Physics.gravity.magnitude;
                }
                else if (ik)
                {
                    gravity = -hit.normal;
                }
                else
                {
                    gravity = Physics.gravity;
                }
            }

        }

        // Is the character grounded?
        private void GroundCheck()
        {
            Vector3 platformVelocityTarget = Vector3.zero;
            platformAngularVelocity = Vector3.zero;
            float stickyForceTarget = 0f;

            Raycast();

            // 删除
            // Spherecasting
            // //hit = GetSpherecastHit();
            //  //normal = transform.up;

            //groundDistance = r.position.y - hit.point.y;
            groundDistance = Vector3.Project(r.position - hit.point, transform.up).magnitude;

            // if not jumping...
            bool findGround = Time.time > jumpEndTime && velocityY < jumpPower * 0.5f;

            if (findGround)
            {
                bool g = onGround;
                onGround = false;

                // The distance of considering the character grounded
                float groundHeight = !g ? airborneThreshold * 0.5f : airborneThreshold;

                //Vector3 horizontalVelocity = r.velocity;
                Vector3 horizontalVelocity = V3Tools.ExtractHorizontal(r.velocity, gravity, 1f);

                float velocityF = horizontalVelocity.magnitude;

                if (groundDistance < groundHeight)
                {
                    // Force the character on the ground
                    stickyForceTarget = groundStickyEffect * velocityF * groundHeight;

                    // On moving platforms
                    if (hit.rigidbody != null)
                    {
                        platformVelocityTarget = hit.rigidbody.GetPointVelocity(hit.point); //修改,之前是normal属性
                        platformAngularVelocity = Vector3.Project(hit.rigidbody.angularVelocity, transform.up);
                    }

                    // Flag the character grounded
                    onGround = true;
                }
            }

            // Interpolate the additive velocity of the platform the character might be standing on
            platformVelocity = Vector3.Lerp(platformVelocity, platformVelocityTarget, Time.deltaTime * platformFriction);
            if (fullRootMotion) stickyForce = 0f;

            stickyForce = stickyForceTarget;//Mathf.Lerp(stickyForce, stickyForceTarget, Time.deltaTime * 5f);

            // remember when we were last in air, for jump delay
            if (!onGround) lastAirTime = Time.time;
        }
    }
}
