/*
 * This is the script component that has to be attached in a canvas with a Canvas Scaler component with it.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Calcatz.WorldSpaceCanvasUI {
    [RequireComponent(typeof(CanvasScaler))]
    public class WorldSpaceCanvasUI : MonoBehaviour {

        [Tooltip("If canvas scaler is set to none, then it will automatically search the Canvas Scaler available in the scene.")]
        [SerializeField] private CanvasScaler m_CanvasScaler;
        [Tooltip("If camera is set to none, then it will take the main camera by default.")]
        [SerializeField] private Camera m_Camera;
        public Camera cam {
            get { return m_Camera; }
            set {
                m_Camera = value;
            }
        }

        private RectTransform rectTransform;

        //Initialize all variable references when we start the game or when there is a change in inspector 
        private void Awake() {
            InitReferences();
        }

        private void OnValidate() {
            InitReferences();
        }

        private void InitReferences() {
            if (m_CanvasScaler == null) {
                m_CanvasScaler = GetComponent<CanvasScaler>();
                if (m_CanvasScaler == null) {
                    m_CanvasScaler = FindObjectOfType<CanvasScaler>();
                }
            }
            if (m_Camera == null) {
                m_Camera = Camera.main;
            }
            if (m_CanvasScaler != null) {
                rectTransform = GetComponent<RectTransform>();
            }
        }

        //This will calculate anchored position of a Rect Transform from target's world position
        public void MoveToWorldPoint(WorldSpaceUIElement _objectToMove, Vector3 _worldObject, bool _disableIfBehindCamera) {
            RectTransform objectTransform = (RectTransform)_objectToMove.transform;
            m_Camera.ResetWorldToCameraMatrix();
            Vector3 screenPosition = m_Camera.WorldToScreenPoint(_worldObject);
            if (_disableIfBehindCamera) {
                if (screenPosition.z < 0) {
                    Vector2 anchoredPos = objectTransform.anchoredPosition;
                    if (anchoredPos.x < rectTransform.sizeDelta.x * 5) {
                        anchoredPos.x = rectTransform.sizeDelta.x * 6;
                        objectTransform.anchoredPosition = anchoredPos;
                    }
                    return;
                }
            }
            if (_objectToMove.enableProximityBasedScaling) {
                float distance = Vector3.Distance(_worldObject, cam.transform.position);
                Vector3 newScale = _objectToMove.scaler.CalculateScaleMultiplier(distance);
                newScale.z = 1;
                objectTransform.localScale = newScale;
            }
            int width = Screen.width;
            int height = Screen.height;
#if UNITY_EDITOR
            //Screen.width and Screen.height in edit mode is somehow retrieved from Inspector window size, but we want to get the Main Game View size
            if (!Application.isPlaying) {
                Vector2 screenSize = GetMainGameViewSize();
                width = (int)screenSize.x;
                height = (int)screenSize.y;
            }
#endif

            //Calculate anchored position based on Canvas Scaler's UI scale mode

            if (m_CanvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize) {
                screenPosition.x = screenPosition.x - width * objectTransform.anchorMin.x;
                screenPosition.y = screenPosition.y - height * objectTransform.anchorMin.y;
                objectTransform.anchoredPosition = screenPosition;
            }
            else if (m_CanvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) {
                Vector2 refRes = rectTransform.sizeDelta;
                screenPosition.x = (screenPosition.x / width * refRes.x) - refRes.x * objectTransform.anchorMin.x;
                screenPosition.y = (screenPosition.y / height * refRes.y) - refRes.y * objectTransform.anchorMin.y;
                objectTransform.anchoredPosition = screenPosition;
            }
            else {
                screenPosition.x = screenPosition.x - width * objectTransform.anchorMin.x;
                screenPosition.y = screenPosition.y - height * objectTransform.anchorMin.y;
                objectTransform.anchoredPosition = screenPosition/transform.localScale.x;
            }
        }

#if UNITY_EDITOR
        public static Vector2 GetMainGameViewSize() {
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
        }
#endif

        public Vector2 GetMousePosition() {
            Vector2 screenPoint = EventSystem.current.currentInputModule.input.mousePosition;
            if (m_CanvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) {
                Vector2 scaledPoint = screenPoint / new Vector2(Screen.width, Screen.height) * m_CanvasScaler.referenceResolution;
                return scaledPoint;
            }
            return screenPoint;
        }

        public Vector2 GetNormalizedMousePosition() {
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 screenPoint = EventSystem.current.currentInputModule.input.mousePosition - screenSize * 0.5f;
            Vector2 normalizedPoint = screenPoint / screenSize*2;
            return normalizedPoint;
        }
    }
}
