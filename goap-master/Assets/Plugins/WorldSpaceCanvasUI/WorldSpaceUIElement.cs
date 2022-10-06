/*
 * This is the script component that has to be attached in a child game objects inside canvas, that we want it to follow a certain world space object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Calcatz.WorldSpaceCanvasUI {
	[RequireComponent(typeof(RectTransform)), ExecuteInEditMode]
	public class WorldSpaceUIElement : MonoBehaviour {

		[SerializeField] private WorldSpaceCanvasUI m_canvas;
		public WorldSpaceCanvasUI canvas {
			get {
				if (m_canvas == null) {
					m_canvas = GetComponentInParent<WorldSpaceCanvasUI>();
					if (m_canvas == null) {
						Debug.LogError("There is no WorldToScreenCanvas component attached in the root canvas.", this);
					}
				}
				return m_canvas;
			}
		}

		[SerializeField] private Transform m_WorldSpaceTargetObject;
		public Transform worldSpaceTargetObject {
			get { return m_WorldSpaceTargetObject; }
			set { m_WorldSpaceTargetObject = value; }
		}

		[Tooltip("If enabled, the UI position will be automatically updated every frame, without manually calling UpdateUIPosition() method.")]
		[SerializeField] private bool m_AlwaysUpdateUIPosition = true;
		public bool alwaysUpdateUIPosition {
			get { return m_AlwaysUpdateUIPosition; }
			set { m_AlwaysUpdateUIPosition = value; }
		}

		[Tooltip("Prevent rendering element on screen when the world object target is behind the camera.")]
		[SerializeField] private bool m_DisableIfBehindCamera = true;
		public bool disableIfBehindCamera {
			get { return m_DisableIfBehindCamera; }
			set { m_DisableIfBehindCamera = value; }
		}

#if UNITY_EDITOR
		[Tooltip("If enabled, the UI position will be updated when the game is not playing.")]
		[SerializeField] public bool m_AutoUpdateInEditMode = true;
#endif 

		[System.Serializable]
		public class ProximityBasedScaler {

			public float nearestDistance = 10f;
			public float farthestDistance = 30f;
			public float minScale = 0.4f;
			public float maxScale = 1.0f;

			public Vector2 initialScale = Vector2.one;
			public float worldSpaceScaleFactor = 0.0001f;

			public Vector2 CalculateScaleMultiplier(float _distance) {
				if (_distance <= nearestDistance) {
					return maxScale * initialScale;
                }
				else if (_distance >= farthestDistance) {
					return minScale * initialScale;
                }
				float multiplier = (farthestDistance - _distance) / (farthestDistance - nearestDistance);
				float deltaScale = multiplier * (maxScale - minScale);

				//float multiplier = (_distance - nearestDistance) / (farthestDistance - nearestDistance);
				return (minScale + deltaScale) * initialScale;
            }
        }

		[HideInInspector][SerializeField] private bool m_EnableProximityBasedScaling = false;
		public bool enableProximityBasedScaling {
			get { return m_EnableProximityBasedScaling; }
			set { m_EnableProximityBasedScaling = value; }
		}

		[HideInInspector][SerializeField] ProximityBasedScaler m_scaler;
		public ProximityBasedScaler scaler { get { return m_scaler; } }
		public void SetScaler(float _farthestDistance, float _nearestDistance, float _minScale, float _maxScale) {
			m_scaler.farthestDistance = _farthestDistance;
			m_scaler.nearestDistance = _nearestDistance;
			m_scaler.minScale = _minScale;
			m_scaler.maxScale = _maxScale;
        }

		private Canvas m_CanvasComponent;
		public Canvas canvasComponent {
            get {
				if (m_CanvasComponent == null) {
					if (canvas != null) {
						m_CanvasComponent = canvas.GetComponent<Canvas>();
                    }
					else {
						m_CanvasComponent = GetComponentInParent<Canvas>();
					}
				}
				return m_CanvasComponent;
			}
        }

        private void Awake() {
			if (m_WorldSpaceTargetObject == null) {
				Debug.LogWarning("World Space Target for object " + this.name + " is null.");
			}
			InitReferences();
		}

		private void OnValidate() {
			InitReferences();
		}

		private void InitReferences() {

		}

		private void Update() {
#if UNITY_EDITOR
			if (!Application.isPlaying) {
				if (!m_AutoUpdateInEditMode) return;
			}
#endif
			if (m_AlwaysUpdateUIPosition) {
				UpdateUIPosition();
			}
		}

        public void UpdateUIPosition() {
			if (m_WorldSpaceTargetObject != null) {
				if (canvas != null) {
					if (canvasComponent.renderMode == RenderMode.WorldSpace) {
                        UpdateWorldSpaceRenderMode();
                    }
                    else {
                        UpdateScreenScaleRenderMode();
                    }
                }
			}
		}

        private void UpdateWorldSpaceRenderMode() {
            transform.position = worldSpaceTargetObject.position;
            transform.rotation = canvas.cam.transform.rotation;

			var distance = (canvas.cam.transform.position - transform.position).magnitude;
			var size = distance * scaler.worldSpaceScaleFactor * canvas.cam.fieldOfView;
			transform.localScale = scaler.CalculateScaleMultiplier(distance) * size;
		}

		private void UpdateScreenScaleRenderMode() {
			canvas.MoveToWorldPoint(this, m_WorldSpaceTargetObject.position, m_DisableIfBehindCamera);
		}

	}
}
