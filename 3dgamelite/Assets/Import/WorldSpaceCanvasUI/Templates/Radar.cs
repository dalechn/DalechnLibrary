using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Calcatz.WorldSpaceCanvasUI.Templates {

    [ExecuteInEditMode]
    public class Radar : MonoBehaviour {

        public enum DirectionMethod {
            GetFromSelfTarget,
            CustomValue
        }

        [SerializeField] private Transform m_selfTarget;
        public Transform selfTarget {
            get { return m_selfTarget; }
            set { m_selfTarget = value; }
        }

        [SerializeField] private Transform m_detectionTarget;
        public Transform detectionTarget {
            get { return m_detectionTarget; }
            set { m_detectionTarget = value; }
        }

        [SerializeField] private RectTransform m_radarPivot;
        public RectTransform radarPivot {
            get { return m_radarPivot; }
            set { m_radarPivot = value; }
        } 

        [SerializeField] private RectTransform m_radarPoint;
        public RectTransform radarPoint {
            get { return m_radarPoint; }
            set { m_radarPoint = value; }
        }

        [SerializeField] private float m_maxWorldSpaceRadius = 5;
        public float maxWorldSpaceRadius {
            get { return m_maxWorldSpaceRadius; }
            set { m_maxWorldSpaceRadius = value; }
        }

        [SerializeField] private bool m_fixedUIRadius;
        public bool fixedUIRadius {
            get { return m_fixedUIRadius; }
            set { m_fixedUIRadius = value; }
        }

        [SerializeField] private float m_maxUIPointRadius = 55;
        public float maxUIPointRadius {
            get { return m_maxUIPointRadius; }
            set { m_maxUIPointRadius = value; }
        }

        [SerializeField] private DirectionMethod m_upDirectionMethod;
        public DirectionMethod upDirectionMethod {
            get { return m_upDirectionMethod; }
            set { m_upDirectionMethod = value; }
        }


        [SerializeField] private Vector3 m_upDirection = Vector3.up;
        public Vector3 upDirection {
            get { return m_upDirection; }
            set {   m_upDirection = value; }
        }


        [SerializeField] private DirectionMethod m_forwardDirectionMethod;
        public DirectionMethod forwardDirectionMethod {
            get { return m_forwardDirectionMethod; }
            set { m_forwardDirectionMethod = value; }
        }


        [SerializeField] private Vector3 m_forwardDirection = Vector3.forward;
        public Vector3 forwardDirection {
            get { return m_forwardDirection; }
            set { m_forwardDirection = value; }
        }

        [Tooltip("If enabled, the UI position will be automatically updated every frame, without manually calling UpdateUIPosition() method.")]
        [SerializeField] private bool m_AlwaysUpdateRadarPoint = true;
        public bool alwaysUpdateRadarPoint {
            get { return m_AlwaysUpdateRadarPoint; }
            set { m_AlwaysUpdateRadarPoint = value; }
        }

#if UNITY_EDITOR
        [Tooltip("If enabled, the UI position will be updated when the game is not playing.")]
        [SerializeField] public bool m_AutoUpdateInEditMode = true;
#endif

        private void Awake() {
            UpdateRadarPoint();
        }

        private void Update() {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                if (!m_AutoUpdateInEditMode) return;
            }
#endif
            if (m_AlwaysUpdateRadarPoint) {
                UpdateRadarPoint();
            }
        }

        public void UpdateRadarPoint() {
            if (m_detectionTarget == null) return;
            float distance = Vector3.Distance(m_selfTarget.position, m_detectionTarget.position);
            float scaledRadius = distance / m_maxWorldSpaceRadius * m_maxUIPointRadius;
            if (scaledRadius > m_maxUIPointRadius || m_fixedUIRadius) scaledRadius = m_maxUIPointRadius;

            Vector3 dir = m_detectionTarget.position - m_selfTarget.position;
            Vector3 upDirection = m_upDirectionMethod == DirectionMethod.GetFromSelfTarget ? m_selfTarget.up : m_upDirection;
            Vector3 forwardDirection = m_forwardDirectionMethod == DirectionMethod.GetFromSelfTarget? m_selfTarget.forward : m_forwardDirection;
            float angle = -Vector3.SignedAngle(forwardDirection, dir, upDirection);
            m_radarPivot.rotation = Quaternion.Euler(0, 0, angle);
            m_radarPoint.anchoredPosition = new Vector2(0, scaledRadius);
        }
    }

}
