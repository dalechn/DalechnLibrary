using System.Collections;
using UnityEngine;

namespace Invector.vCharacterController.v2_5D
{
    [vClassHeader("2.5D CONTROLLER")]
    public class v2_5DController : vThirdPersonController
    {
        [vEditorToolbar("Cursor Options")]
        public bool rotateToCursorInStrafe = true;
        [vHideInInspector("rotateToCursorInStrafe")]
        
        public v2_5DPath path;

        internal Vector3 targetForward;

        public override void Init()
        {
            base.Init();
            if (path == null) path = FindObjectOfType<v2_5DPath>();
            if (path) InitPath();
        }
       
        public virtual void InitPath()
        {           
            transform.position = path.ConstraintPosition(transform.position);
            RotateToDirection(path.reference.right);
            path.Init();
        }

        /// <summary>
        /// Main Camera
        /// </summary>
        public virtual Camera cameraMain
        {
            get
            {
                return vMousePositionHandler.Instance.mainCamera;
            }
        }

        /// <summary>
        /// Get cursor position on the screen
        /// </summary>
        public virtual Vector2 cursorPosition
        {
            get
            {
                return vMousePositionHandler.Instance.mousePosition;
            }
        }

        /// <summary>
        /// Get World position of the cursor relative to Controller transform
        /// </summary>
        public virtual Vector3 worldCursorPosition
        {
            get
            {
                return cursorRelativeToPath;
            }
        }

        /// <summary>
        /// Get Local position of the cursor relative to Controller transform
        /// </summary>
        public virtual Vector3 localCursorPosition
        {
            get
            {
                var localPos = transform.InverseTransformPoint(cursorRelativeToPath);

                return localPos;
            }
        }      

        protected virtual Vector3 cursorDirection
        {
           get
            {
                Vector3 selfLocal = cameraMain.transform.InverseTransformPoint(transform.position);
                Vector3 cursorLocal = cameraMain.transform.InverseTransformPoint(worldCursorPosition);
                if (cursorLocal.x > selfLocal.x + 0.1f) return targetForward= path.reference.right;
                else if (cursorLocal.x < selfLocal.x - 0.1f) return targetForward= - path.reference.right;
                else return targetForward;

            }
        }

        protected virtual Vector3 cursorRelativeToPath
        {
            get
            {
                var mouseDirection = cameraMain.ScreenPointToRay(vMousePositionHandler.Instance.mousePosition).direction;
                var position = cameraMain.transform.position + mouseDirection * 100f;
                return GetIntersectWithLineAndPlane(position, mouseDirection.normalized, path.reference.forward, _capsuleCollider.bounds.center);
            }
        }

        protected virtual Vector3 GetIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal, Vector3 planePoint)
        {
            float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);
            return d * direct.normalized + point;
        }

        public override void ControlLocomotionType()
        {
            base.ControlLocomotionType();
            if (!isDead && !ragdolled&&!customAction)
            {
                transform.position = Vector3.Lerp(transform.position, path.ConstraintPosition(transform.position), 80 * Time.deltaTime);
            }
          
        }        

        public override void ControlRotationType()
        {
            if (lockAnimRotation || customAction || isRolling) return;

            bool validInput = input != Vector3.zero || (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

            if (validInput)
            {
                bool useStrafe = (isStrafing && rotateToCursorInStrafe && (!isSprinting || sprintOnlyFree == false) || (freeSpeed.rotateWithCamera && input == Vector3.zero));
                Vector3 dir = useStrafe ? cursorDirection:path? path.reference.right*input.x:moveDirection;
                RotateToDirection(dir);
            }
        }

        public override void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if (isRolling && !rollControl) return;
           
                var _forward =path?path.reference.right: referenceTransform?referenceTransform.right:Vector3.right;            
               moveDirection = (inputSmooth.x * _forward);
        }
    }
}

