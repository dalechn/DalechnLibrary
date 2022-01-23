using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vCharacterController.PointClick
{
    [vClassHeader("POINT CLICK INPUT")]
    public class vPointAndClickInput : vThirdPersonInput
    {
        #region Variables 

        [System.Serializable]
        public class vCursorByTag
        {
            public string tag;
            public Texture2D cursorTexture;
            public CursorMode cursorMode = CursorMode.Auto;
        }

        [vEditorToolbar("Cursor")]
        public List<vCursorByTag> cursorByTag;

        [vEditorToolbar("Layer")]
        [Header("Click To Move Properties")]
        public LayerMask clickMoveLayer = 1 << 0;

        [Tooltip("Press and hold the Mouse Middle Button and rotate it to rotate the Camera")]
        public bool rotateCamera = true;

        [System.Serializable]
        public class vOnEnableCursor : UnityEngine.Events.UnityEvent<Vector3> { }
        [vEditorToolbar("Events")]
        public vOnEnableCursor onEnableCursor = new vOnEnableCursor();
        public UnityEngine.Events.UnityEvent onDisableCursor;

        [HideInInspector]
        public Vector3 cursorPoint;
        public Collider target { get; set; }
        public Dictionary<string, vCursorByTag> customCursor;

        #endregion

        protected override void Start()
        {
            base.Start();
            customCursor = new Dictionary<string, vCursorByTag>();

            for (int i = 0; i < cursorByTag.Count; i++)
            {
                if (!customCursor.ContainsKey(cursorByTag[i].tag))
                {
                    customCursor.Add(cursorByTag[i].tag, cursorByTag[i]);
                }
            }
        }

        protected override IEnumerator CharacterInit()
        {
            yield return StartCoroutine(base.CharacterInit());
            cursorPoint = transform.position;
        }

        protected override void Update()
        {
            CameraRotation(cc.rotateTarget);
            base.Update();

        }
        public override void MoveInput()
        {
            cc.rotateByWorld = true;
            PointAndClickMovement();
        }

        protected virtual void PointAndClickMovement()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, clickMoveLayer))
            {
                var tag = hit.collider.gameObject.tag;
                ChangeCursorByTag(tag);
                CheckClickPoint(hit);
            }
            MoveToPoint();
        }

        protected virtual void CheckClickPoint(RaycastHit hit)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    target = hit.collider;
                }

                if (onEnableCursor != null)
                {
                    onEnableCursor.Invoke(hit.point);
                }
                cursorPoint = hit.point;
            }
        }

        protected virtual void ChangeCursorByTag(string tag)
        {
            if (customCursor.Count <= 0) return;

            if (customCursor.ContainsKey(tag))
            {
                Cursor.SetCursor(customCursor[tag].cursorTexture, Vector2.zero, customCursor[tag].cursorMode);
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        public virtual void MoveToPoint()
        {
            if (!NearPoint(cursorPoint, transform.position) && target)
            {
                Vector3 dir = cursorPoint - transform.position;
                dir = dir.normalized * Mathf.Clamp(dir.magnitude, 0f, 1f);
                var targetDir = cc.isStrafing ? transform.InverseTransformDirection(dir).normalized : dir.normalized;
                cc.input = targetDir;
            }
            else
            {
                if (onDisableCursor != null)
                    onDisableCursor.Invoke();

                cc.input = Vector2.Lerp(cc.input, Vector3.zero, 20 * Time.deltaTime);
            }
        }

        public void SetTargetPosition(Vector3 value)
        {
            cursorPoint = value;
            var dir = (value - transform.position).normalized;
            cc.input = new Vector2(dir.x, dir.z);
        }

        public void ClearTarget()
        {
            cc.input = Vector2.zero;
            target = null;
        }

        protected virtual bool NearPoint(Vector3 a, Vector3 b)
        {
            var _a = new Vector3(a.x, transform.position.y, a.z);
            var _b = new Vector3(b.x, transform.position.y, b.z);
            return Vector3.Distance(_a, _b) <= 0.5f;
        }

        protected void CameraRotation(Transform cameraTransform)
        {
            if (!rotateCamera) return;
            // press middle mouse button and rotate the mouse to rotate the camera
            if (Input.GetMouseButton(2) && rotateCameraXInput.GetAxis() > 0.1f)
            {
                tpCamera.lerpState.fixedAngle.x += 2f;
            }
            else if (Input.GetMouseButton(2) && rotateCameraXInput.GetAxis() < -0.1f)
            {
                tpCamera.lerpState.fixedAngle.x += -2f;
            }
        }
    }
}