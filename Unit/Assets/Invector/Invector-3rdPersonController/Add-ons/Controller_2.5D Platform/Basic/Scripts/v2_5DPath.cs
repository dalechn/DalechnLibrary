using System.Collections;
using UnityEngine;

namespace Invector.vCharacterController.v2_5D
{
    [SelectionBase]
    [vClassHeader("2.5D PathNode", false)]
    public class v2_5DPath : vMonoBehaviour
    {
        public bool autoUpdateCameraAngle = true;
        public bool loopPath = true;

        protected Transform pathHelper;
        internal Transform reference;
        public Transform[] points;
        public v2_5DPathPoint currentPoint;

        [SerializeField]
        protected bool _invertPath;
        protected Transform _mainCamera;
        protected vCamera.vThirdPersonCamera _tpCamera;

        public bool invertPath
        {
            get
            {
                return _invertPath;
            }
            set
            {
                _invertPath = value;
            }
        }
        public Transform cameraTransform
        {
            get
            {
                return _tpCamera ? _tpCamera.transform : _mainCamera;
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!Application.isPlaying && (points == null || points.Length != transform.childCount))
            {
                points = new Transform[transform.childCount];
                for (int i = 1; i < points.Length; i++)
                {
                    points[i] = transform.GetChild(i);
                }
            }

            if (points != null && points.Length > 1)
            {
                var point = transform.GetChild(0);
                if (!Application.isPlaying)
                {
                    points[0] = transform.GetChild(0);
                }

                bool isSelected = (UnityEditor.Selection.activeGameObject == gameObject || (UnityEditor.Selection.activeGameObject != null && UnityEditor.Selection.activeGameObject.transform.parent && UnityEditor.Selection.activeGameObject.transform.parent.gameObject == gameObject));
                try
                {
                    for (int i = 1; i < points.Length; i++)
                    {
                        Gizmos.color = isSelected ? Color.green : Color.white * 0.8f;
                        Transform pointB = points[i];
                        if (!Application.isPlaying)
                        {                            
                            Gizmos.DrawSphere(point.position, 0.2f);
                        }

                        if (i > 0)
                        {
                            Gizmos.color = isSelected ? Color.white : Color.white * 0.8f;
                            Gizmos.DrawLine(point.position, pointB.position);
                            if (isSelected)
                            {
                                Vector3 pathForward = (point.position - pointB.position);
                                Vector3 pathRight = Quaternion.AngleAxis(90, Vector3.up) * pathForward.normalized;
                                Gizmos.color = invertPath ? Color.blue : Color.green;
                                for (int a = 0; a < (int)(pathForward.magnitude); a++)
                                {
                                    DrawArrow(point.position - pathForward.normalized * a, pathForward.normalized, pathRight);
                                }

                            }
                        }
                        point = pointB;
                    }
                }
                catch
                { }
                Gizmos.color = Color.white;
                if (loopPath)
                {                    
                    Gizmos.DrawLine(points[0].position, points[points.Length - 1].position);

                    if (isSelected)
                    {
                        Vector3 pathForward = (points[points.Length - 1].position - points[0].position);
                        Vector3 pathRight = Quaternion.AngleAxis(90, Vector3.up) * pathForward.normalized;
                        Gizmos.color = invertPath ? Color.blue : Color.green;
                        for (int a = 0; a < (int)(pathForward.magnitude); a++)
                        {
                            DrawArrow(points[points.Length - 1].position - pathForward.normalized * a, pathForward.normalized, pathRight);
                        }
                    }
                }
                else
                {
                    if(points[points.Length - 1])
                    {
                        Gizmos.color = isSelected ? Color.green : Color.white * 0.8f;
                        Gizmos.DrawSphere(points[points.Length - 1].position, 0.2f);
                    }                    
                }
            }

            if (currentPoint != null)
            {
                Gizmos.color = Color.red;
                if (currentPoint.center)
                {
                    Gizmos.DrawSphere(currentPoint.center.position, 0.2f);
                }

                Gizmos.color = Color.green;
                if (currentPoint.forward)
                {
                    Gizmos.DrawSphere(currentPoint.forward.position, 0.2f);
                }

                Gizmos.color = Color.blue;
                if (currentPoint.backward)
                {
                    Gizmos.DrawSphere(currentPoint.backward.position, 0.2f);
                }
            }
        }        

        void DrawArrow(Vector3 position, Vector3 pathForward, Vector3 pathRight)
        {
            Vector3 arrowLineA = position + (invertPath ? -pathForward : pathForward) * 0.5f + pathRight * 0.25f;
            Vector3 arrowLineB = position + (invertPath ? -pathForward : pathForward) * 0.5f - pathRight * 0.25f;
            Gizmos.DrawLine(position, arrowLineA);
            Gizmos.DrawLine(position, arrowLineB);
        }
#endif
        public virtual void Init()
        {
            _mainCamera = Camera.main?.transform;
            currentPoint = null;
            _tpCamera = FindObjectOfType<vCamera.vThirdPersonCamera>();

            if (cameraTransform == null)
            {
                Debug.LogWarning("No camera founded.<b> Please tag the scene camera as MainCamera or/and use ThirdPersonCamera</b>");
            }
        }

        protected virtual v2_5DPathPoint GetStartPoint(Vector3 position)
        {
            var distance = Mathf.Infinity;
            v2_5DPathPoint point = new v2_5DPathPoint();
            for (int i = 0; i < points.Length; i++)
            {
                var _distance = Vector3.Distance(points[i].position, position);
                if (_distance < distance)
                {
                    distance = _distance;
                    point.center = points[i];
                    if (i + 1 < points.Length)
                    {
                        point.forward = points[i + 1];
                    }
                    else if (i == points.Length - 1 && loopPath)
                    {
                        point.forward = points[0];
                    }

                    if (i - 1 > -1)
                    {
                        point.backward = points[i - 1];
                    }
                    else if (i == 0 && loopPath)
                    {
                        point.backward = points[points.Length - 1];
                    }
                }
            }
            return point;
        }

        public virtual bool isNearForward(Vector3 position)
        {
            if (currentPoint == null || !currentPoint.forward)
            {
                return false;
            }

            var pA = currentPoint.forward.position;
            var pB = position;
            pA.y = pB.y;
            return Vector3.Distance(pA, pB) < 0.25f;
        }

        public virtual bool isNearBackward(Vector3 position)
        {
            if (currentPoint == null || !currentPoint.backward)
            {
                return false;
            }

            var pA = currentPoint.backward.position;
            var pB = position;
            pA.y = pB.y;
            return Vector3.Distance(pA, pB) < 0.25f;
        }

        v2_5DPathPoint GetNextPoint(Transform center)
        {
            v2_5DPathPoint point = new v2_5DPathPoint();
            point.center = center;
            var pointIndex = System.Array.IndexOf(points, center);
            if (pointIndex + 1 < points.Length)
            {
                point.forward = points[pointIndex + 1];
            }
            else if (pointIndex == points.Length - 1 && loopPath)
            {
                point.forward = points[0];
            }

            if (pointIndex - 1 > -1)
            {
                point.backward = points[pointIndex - 1];
            }
            else if (pointIndex == 0 && loopPath)
            {
                point.backward = points[points.Length - 1];
            }

            return point;
        }       

        public virtual Vector3 ConstraintPosition(Vector3 pos, bool checkChangePoint = true)
        {
            var position = pos;
            if (currentPoint == null)
            {
                currentPoint = GetStartPoint(pos);
            }

            if (currentPoint.center)
            {
                if (!reference)
                {
                    var obj = new GameObject("PathReference");
                    reference = obj.transform;

                    obj = new GameObject("PathHelper");
                    pathHelper = obj.transform;

                    reference.parent = pathHelper;
                    reference.localEulerAngles = Vector3.zero;
                    reference.localPosition = Vector3.zero;
                }

                position.y = currentPoint.center.position.y;

                if (checkChangePoint)
                {
                    if (isNearBackward(position))
                    {
                        currentPoint = GetNextPoint(currentPoint.backward);
                    }

                    if (isNearForward(position))
                    {
                        currentPoint = GetNextPoint(currentPoint.forward);
                    }
                }

                if (currentPoint.forward != null)
                {
                    UpdateReferenceTransform(position);
                }

                if (autoUpdateCameraAngle && _tpCamera != null)
                {
                    var rot = Quaternion.LookRotation(reference.forward, Vector3.up);
                    var angle = rot.eulerAngles.NormalizeAngle().y;
                    _tpCamera.lerpState.fixedAngle.x = angle;
                }
                var localPosition = reference.InverseTransformPoint(pos);
                localPosition.z = 0;

                return reference.TransformPoint(localPosition);
            }

            return position;
        }

        protected virtual void UpdateReferenceTransform(Vector3 position)
        {
            CalculateDirectionFromCamera(position);
        }

        protected virtual Vector3 NormalizedPoint(Vector3 point)
        {
            point.y = 0;
            return point;
        }

        protected virtual void CalculateDirectionFromPath(Vector3 position)
        {
            var dirA = (currentPoint.backward) ? NormalizedPoint(currentPoint.backward.position) - NormalizedPoint(currentPoint.center.position) : -pathHelper.right;
            var pA = currentPoint.center.position + dirA;
            var dirB = (currentPoint.forward) ? NormalizedPoint(currentPoint.forward.position) - NormalizedPoint(currentPoint.center.position) : pathHelper.right;
            var pB = currentPoint.center.position + dirB;

            pathHelper.position = currentPoint.center.position;
            
            var distanceA = currentPoint.backward ? Vector3.Distance(NormalizedPoint(currentPoint.center.position), NormalizedPoint(currentPoint.backward.position)) : Mathf.Infinity;
            var distanceB = currentPoint.forward ? Vector3.Distance(NormalizedPoint(currentPoint.center.position), NormalizedPoint(currentPoint.forward.position)) : Mathf.Infinity;

            if (Vector3.Distance(pA, position) > distanceA + 0.1f)
            {
                pathHelper.right = dirB;
            }
            else if (Vector3.Distance(pB, position) > distanceB + 0.1f)
            {
                pathHelper.right = -dirA;
            }
        }

        protected virtual void CalculateDirectionFromCamera(Vector3 position)
        {
            CalculateDirectionFromPath(position);

            //if(invertPath)
            {
                reference.forward = invertPath ? -pathHelper.forward : pathHelper.forward;
            }

            //if (cameraTransform)
            //{
            //    var cameraDirection = cameraTransform.forward;
            //    cameraDirection.y = 0;
            //    cameraDirection.Normalize();
            //    var angleDireference = Vector3.Angle(cameraDirection, reference.forward);
            //    isInverted = angleDireference > 90;
            //    if (isInverted)
            //    {
            //        reference.forward = -reference.forward;
            //    }
            //}
        }

        public class v2_5DPathPoint
        {
            public Transform center;
            public Transform forward;
            public Transform backward;
        }
    }
}