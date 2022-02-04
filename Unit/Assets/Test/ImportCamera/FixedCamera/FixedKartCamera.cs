//using UnityEngine;

//namespace PowerslideKartPhysics
//{
//    public class FixedKartCamera : MonoBehaviour
//    {
//        public Kalt targetKart;
//        public float height;
//        public float distance;
//        public float smoothRate = 5;
//        public Vector3 lookAtAngle = new Vector3(15, 0, 0);
//        public float maxVelDist = 0.5f;
//        public float fovVal = 17;
//        public float disVal = 0.5f;

//        [Tooltip("Mask for which objects will be checked in between the camera and target vehicle")]
//        public LayerMask castMask;

//        [System.NonSerialized]
//        public Transform targetTransform;
//        //[System.NonSerialized] public ED_Void resetEvent;
//        private Camera cam;

//        private float xInput;
//        private float yInput;
//        private Vector3 lookDir;
//        private float smoothYRot;
//        private Transform lookObj;
//        private Vector3 forwardLook;
//        private Vector3 upLook;
//        private Vector3 targetForward;
//        private Vector3 targetUp;

//        private float originFov;

//        private void Start()
//        {
//            cam = GetComponentInChildren<Camera>();

//            if (!lookObj)
//            {
//                GameObject lookTemp = new GameObject("Camera Looker");
//                lookObj = lookTemp.transform;
//                lookObj.parent = transform;

//                GameObject targetTemp = new GameObject("Camera Target");
//                targetTransform = targetTemp.transform;
//                targetTransform.parent = transform;
//            }

//            GetComponentInChildren<AudioListener>().velocityUpdateMode = AudioVelocityUpdateMode.Fixed;

//            originFov = cam.fieldOfView;

//            forwardLook = targetKart.forwardDir;
//            upLook = targetKart.upDir;
//        }

//        private float boostFov;
//        private float boostDis;
//        private void FixedUpdate()
//        {
//            if (targetKart && targetKart.gameObject.activeSelf)
//            {
//                Transform target = targetKart.rotator;

//                float val = (Mathf.Clamp01(targetKart.boostReserve + targetKart.padBoostTimer) + Mathf.Clamp01(targetKart.realsBoostTimer) + (targetKart.startBoostTimer) + Mathf.Clamp01(targetKart.airTime));
//                boostFov = Mathf.Lerp(boostFov, fovVal * val, Time.fixedDeltaTime * 1);
//                boostDis = Mathf.Lerp(boostDis, disVal * val, Time.fixedDeltaTime * 1);

//                cam.fieldOfView = originFov + boostFov;

//                targetForward = targetKart.LookDir();
//                targetUp = targetKart.upDir;
//                lookDir = Vector3.Slerp(lookDir, (xInput == 0 && yInput == 0 ? Vector3.forward : new Vector3(xInput, 0, yInput).normalized), Time.fixedDeltaTime * 10);
//                smoothYRot = Mathf.Lerp(smoothYRot, targetKart.rb.angularVelocity.y, Time.fixedDeltaTime * 2);

//                //Determine the upwards direction of the camera
//                RaycastHit hit;
//                if (Physics.Raycast(target.position, -targetUp, out hit, 1, castMask))
//                {
//                    upLook = Vector3.Lerp(upLook, (Vector3.Dot(hit.normal, targetUp) > 0.5 ? hit.normal : targetUp), Time.fixedDeltaTime * smoothRate);
//                }
//                else
//                {
//                    upLook = Vector3.Lerp(upLook, targetUp, Time.fixedDeltaTime * smoothRate);
//                }

//                //Calculate rotation and position variables
//                forwardLook = Vector3.Lerp(forwardLook, targetForward, Time.fixedDeltaTime * smoothRate);
//                lookObj.rotation = Quaternion.LookRotation(forwardLook, upLook);
//                lookObj.position = target.position;
//                Vector3 lookDirActual = (lookDir - new Vector3(Mathf.Sin(smoothYRot), 0, Mathf.Cos(smoothYRot)) * Mathf.Abs(smoothYRot) * 0.2f).normalized;
//                Vector3 forwardDir = lookObj.TransformDirection(lookDirActual);
//                Vector3 localOffset = lookObj.TransformPoint(-lookDirActual * (distance + boostDis) - lookDirActual * Mathf.Min(targetKart.rb.velocity.magnitude * 0.05f, maxVelDist) + new Vector3(0, height, 0));

//                //Check if there is an object between the camera and target vehicle and move the camera in front of it
//                if (Physics.Linecast(target.position, localOffset, out hit, castMask))
//                {
//                    targetTransform.position = hit.point + (target.position - localOffset).normalized * (cam.nearClipPlane + 0.1f);
//                }
//                else
//                {
//                    targetTransform.position = localOffset;
//                }

//                targetTransform.rotation = Quaternion.LookRotation(forwardDir, lookObj.up)
//                    * Quaternion.AngleAxis(lookAtAngle.x, Vector3.right) * Quaternion.AngleAxis(lookAtAngle.y, Vector3.up) * Quaternion.AngleAxis(lookAtAngle.z, Vector3.forward);
//            }
//        }

//        public void SetInput(float x, float y)
//        {
//            xInput = x;
//            yInput = y;
//        }

//        public void ResetRot(Vector3 look)
//        {
//            forwardLook = look;
//        }

//        private void OnDestroy()
//        {
//            if (lookObj)
//            {
//                Destroy(lookObj.gameObject);
//            }
//        }
//    }
//}