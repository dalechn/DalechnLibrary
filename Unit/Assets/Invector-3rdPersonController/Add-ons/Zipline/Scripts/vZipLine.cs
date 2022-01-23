using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.vActions
{
    /// <summary>
    /// vZipline Add-on
    /// On this Add-on we're locking the tpInput and disabling it, so we can manipulate some variables of the controller and create the zipline behaviour.
    /// We can still access those scripts and methods, and call just what we need to use for example the CameraInput method
    /// This way the add-on become modular and plug&play easy to modify without changing the core of the controller.
    /// </summary>

    [vClassHeader("Zipline Action")]
    public class vZipLine : vActionListener
    {
        #region Zipline Variables

        [Tooltip("Max speed to use the Zipline")]
        public float maxSpeed = 10f;
        [Tooltip("Height offset to match the character Y position to the zipline")]
        public float heightOffSet = .2f;
        [Tooltip("Name of the animation clip that will play when you use the Zipline")]
        public string animationClip = "Zipline";
        [Tooltip("Name of the tag assign into the Zipline object")]
        public string ziplineTag = "Zipline";
        [Tooltip("Make sure to enable when you're using and disable when you exit using Events")]
        public GameObject ziplineHandler;

        [Tooltip("Debug Mode will show the current behaviour at the console window")]
        public bool debugMode;

        public GenericInput enterZipline = new GenericInput("E", false, "A", false, "A", false);
        public GenericInput exitZipline = new GenericInput("Space", false, "X", false, "X", false);

        private float currentSpeed;
        private RigidbodyConstraints originalConstrains;
        [vReadOnly, SerializeField]
        protected bool isUsingZipline;
        private Transform nearestPoint;
        private vThirdPersonInput tpInput;
        private bool inExitZipline;
        /// <summary>
        /// get nearest anchor point to enter the zipline
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>    
        public Transform getNearPoint(Transform _directionRef, Vector3 position)
        {
            Transform point = null;
            float distance = Mathf.Infinity;

            if (Vector3.Distance(position, _directionRef.position) < distance)
            {
                distance = Vector3.Distance(position, _directionRef.position);
                point = _directionRef;
            }

            return point;
        }

        public UnityEvent onZiplineEnter;
        public UnityEvent onZiplineUsing;
        public UnityEvent onZiplineExit;
        #endregion

        protected override void SetUpListener()
        {
            tpInput = GetComponent<vThirdPersonInput>();
            actionEnter = true;
            actionStay = true;
            actionExit = true;
        }

        protected override void Start()
        {
            base.Start();

            originalConstrains = GetComponent<Rigidbody>().constraints;
        }

        public override void OnActionEnter(Collider other)
        {
            if (other.gameObject.CompareTag(ziplineTag) && !isUsingZipline)
            {
                var ap = other.gameObject.GetComponent<vZiplineAnchorPoints>();
                nearestPoint = getNearPoint(ap.ziplineDirectionRef, tpInput.transform.position + Vector3.up * (tpInput.cc._capsuleCollider.height + heightOffSet));
                if (debugMode)
                {
                    Debug.Log("NearestPoint", nearestPoint.gameObject);
                }

                // if you want to automatically enter the zipline, disable the input enterZipline in the inspector
                if (!enterZipline.useInput)
                {
                    InitiateZipline(other);
                }
            }
        }

        public override void OnActionStay(Collider other)
        {
            if (other.gameObject.CompareTag(ziplineTag))
            {
                // enter the zipline only if you press the enterZipline input
                if (enterZipline.GetButton() && !isUsingZipline && !inExitZipline)
                {
                    InitiateZipline(other);
                }
                // exit the zipline by pressing the exitZipline input
                if (exitZipline.GetButtonDown() && isUsingZipline && !inExitZipline)
                {
                    inExitZipline = true;
                    ExitZipline();
                }
                UsingZipline(other);
            }
        }

        public override void OnActionExit(Collider other)
        {
            if (other.gameObject.CompareTag(ziplineTag) && isUsingZipline)
            {
                inExitZipline = false;
                if (debugMode)
                {
                    Debug.Log("Player exit the Zipline");
                }

                if (isUsingZipline)
                {
                    ExitZipline();
                }
            }
        }

        /// <summary>
        /// Prepare the Controller to use the Zipline
        /// </summary>
        /// <param name="other"></param>
        protected virtual void InitiateZipline(Collider other)
        {
            if (tpInput && nearestPoint)
            {
                if (debugMode)
                {
                    Debug.Log("Player Initiate the Zipline");
                }

                isUsingZipline = true;
                tpInput.SetLockAllInput(true);
                tpInput.cc.animator.CrossFadeInFixedTime(animationClip, 0.2f);
                if (tpInput.cc._rigidbody.useGravity)
                {
                    tpInput.cc._rigidbody.useGravity = false;
                }
                //if(!tpInput.cc._rigidbody.isKinematic)
                //    tpInput.cc._rigidbody.isKinematic = true;
                originalConstrains = tpInput.cc._rigidbody.constraints;
                tpInput.cc._rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                tpInput.cc.animator.SetBool(vAnimatorParameters.IsGrounded, true);

                var localPos = nearestPoint.InverseTransformPoint(transform.position);
                localPos.x = 0f;
                localPos.y = 0f;
                tpInput.transform.position = nearestPoint.TransformPoint(localPos) + ((tpInput.cc._capsuleCollider.height + heightOffSet) * -Vector3.up);
                tpInput.enabled = false;
                onZiplineEnter.Invoke();
            }
        }

        /// <summary>
        /// Behaviour while using the Zipline
        /// </summary>
        /// <param name="other"></param>
        protected virtual void UsingZipline(Collider other)
        {
            if (!isUsingZipline)
            {
                return;
            }

            if (tpInput)
            {
                if (debugMode)
                {
                    Debug.Log("Player is using the Zipline");
                }

                tpInput.cc.transform.rotation = other.transform.rotation;
                tpInput.cc.heightReached = transform.position.y;
                currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, .5f * Time.deltaTime);
                transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.Self);
                var euler = transform.eulerAngles;
                euler.x = 0f;
                tpInput.transform.eulerAngles = euler;
                tpInput.CameraInput();
                onZiplineUsing.Invoke();

                if (ziplineHandler != null)
                {
                    ziplineHandler.transform.position = tpInput.cc.transform.position + Vector3.up * (tpInput.cc._capsuleCollider.height + heightOffSet);
                    ziplineHandler.transform.rotation = transform.rotation;
                }
            }
        }

        /// <summary>
        /// Behaviour to Exit the Zipline
        /// </summary>
        /// <param name="other"></param>
        public virtual void ExitZipline()
        {
            if (!isUsingZipline)
            {
                return;
            }

            if (tpInput)
            {
                tpInput.cc.isGrounded = false;
                tpInput.cc.animator.SetBool(vAnimatorParameters.IsGrounded, false);
                currentSpeed = 0f;
                tpInput.cc._rigidbody.useGravity = true;
                //tpInput.cc._rigidbody.isKinematic = false;
                tpInput.cc._rigidbody.constraints = originalConstrains;
                tpInput.cc.animator.CrossFadeInFixedTime("Falling", .2f);
                tpInput.enabled = true;
                tpInput.SetLockAllInput(false);
                isUsingZipline = false;
                onZiplineExit.Invoke();
            }
        }
    }
}