using UnityEngine;

namespace PowerslideKartPhysics
{
    public class FixedCamera : MonoBehaviour
    {
        public Transform targetKart;
        public float height;
        public float distance;
        public float smoothRate = 5;
        public Vector3 lookAtAngle = new Vector3(15, 0, 0);
        public float maxVelDist = 0.5f;
        public float fovVal = 17;
        public float disVal = 0.5f;
        public bool transformFollow = true;

        [Tooltip("Mask for which objects will be checked in between the camera and target vehicle")]
        public LayerMask castMask;

        [System.NonSerialized]
        public Transform targetTransform;
        //[System.NonSerialized] public ED_Void resetEvent;
        protected Camera cam;
        protected Rigidbody rb;

        protected float xInput;
        protected float yInput;
        protected Vector3 lookDir;
        protected float smoothYRot;
        protected Transform lookObj;
        protected Vector3 forwardLook;
        protected Vector3 upLook;
        protected Vector3 targetForward;
        protected Vector3 targetUp;

        protected float boostFov;
        protected float boostDis;
        protected float originFov;

        private void Start()
        {
            cam = GetComponentInChildren<Camera>();
            rb = targetKart.GetComponentInChildren<Rigidbody>();
            GetComponentInChildren<AudioListener>().velocityUpdateMode = AudioVelocityUpdateMode.Fixed;

            if (!lookObj)
            {
                GameObject lookTemp = new GameObject("Camera Looker");
                lookObj = lookTemp.transform;
                lookObj.parent = transform;

                GameObject targetTemp = new GameObject("Camera Target");
                targetTransform = targetTemp.transform;
                targetTransform.parent = transform;
            }

            originFov = cam.fieldOfView;

            forwardLook = targetKart.forward;
            upLook = targetKart.up;
        }

        private void FixedUpdate()
        {
            if (targetKart && targetKart.gameObject.activeSelf)
            {
                Transform target = targetKart;

                float val = Fov();
                boostFov = Mathf.Lerp(boostFov, fovVal * val, Time.fixedDeltaTime * 1);
                boostDis = Mathf.Lerp(boostDis, disVal * val, Time.fixedDeltaTime * 1);

                cam.fieldOfView = originFov + boostFov;

                LookDir();
                lookDir = Vector3.Slerp(lookDir, (xInput == 0 && yInput == 0 ? Vector3.forward : new Vector3(xInput, 0, yInput).normalized), Time.fixedDeltaTime * 10);
                smoothYRot = Mathf.Lerp(smoothYRot, rb.angularVelocity.y, Time.fixedDeltaTime * 2);

                //Determine the upwards direction of the camera
                RaycastHit hit;
                if (Physics.Raycast(target.position, -targetUp, out hit, 1, castMask))
                {
                    upLook = Vector3.Lerp(upLook, (Vector3.Dot(hit.normal, targetUp) > 0.5 ? hit.normal : targetUp), Time.fixedDeltaTime * smoothRate);
                }
                else
                {
                    upLook = Vector3.Lerp(upLook, targetUp, Time.fixedDeltaTime * smoothRate);
                }

                //Calculate rotation and position variables
                forwardLook = Vector3.Lerp(forwardLook, targetForward, Time.fixedDeltaTime * smoothRate);
                lookObj.rotation = Quaternion.LookRotation(forwardLook, upLook);
                lookObj.position = target.position;
                Vector3 lookDirActual = (lookDir - new Vector3(Mathf.Sin(smoothYRot), 0, Mathf.Cos(smoothYRot)) * Mathf.Abs(smoothYRot) * 0.2f).normalized;
                Vector3 forwardDir = lookObj.TransformDirection(lookDirActual);
                Vector3 localOffset = lookObj.TransformPoint(-lookDirActual * (distance + boostDis) - lookDirActual * Mathf.Min(rb.velocity.magnitude * 0.05f, maxVelDist) + new Vector3(0, height, 0));

                //Check if there is an object between the camera and target vehicle and move the camera in front of it
                if (Physics.Linecast(target.position, localOffset, out hit, castMask))
                {
                    targetTransform.position = hit.point + (target.position - localOffset).normalized * (cam.nearClipPlane + 0.1f);
                }
                else
                {
                    targetTransform.position = localOffset;
                }

                targetTransform.rotation = Quaternion.LookRotation(forwardDir, lookObj.up)
                    * Quaternion.AngleAxis(lookAtAngle.x, Vector3.right) * Quaternion.AngleAxis(lookAtAngle.y, Vector3.up) * Quaternion.AngleAxis(lookAtAngle.z, Vector3.forward);

                if (transformFollow)
                {
                    transform.position = targetTransform.position;
                    transform.rotation = targetTransform.rotation;
                }
            }
        }

        public void SetInput(float x, float y)
        {
            xInput = x;
            yInput = y;
        }

        public void ResetRot(Vector3 look)
        {
            forwardLook = look;
        }

        protected void OnDestroy()
        {
            if (lookObj)
            {
                Destroy(lookObj.gameObject);
            }
        }

        protected float Fov()
        {
            return 1;
        }

        protected void LookDir()
        {
            targetForward = targetKart.forward;
            targetUp = targetKart.up;
        }
    }
}