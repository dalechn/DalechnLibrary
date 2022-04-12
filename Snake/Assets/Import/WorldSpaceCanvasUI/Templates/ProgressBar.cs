using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Calcatz.WorldSpaceCanvasUI.Templates {

    [RequireComponent(typeof(Image)), ExecuteInEditMode]
    public class ProgressBar : MonoBehaviour {

        [SerializeField] private float m_minValue = 0;
        [SerializeField] private float m_maxValue = 1;
        [SerializeField] private bool m_wholeNumbers = false;
        [SerializeField] private float m_value = 1;

        [SerializeField] private Image m_barImage;
        public Image barImage {
            get { return m_barImage; }
            set { m_barImage = value; }
        }

        private RectTransform rectTransform {
            get {
                return (RectTransform)m_barImage.transform;
            }
        }
        private System.Action updateMethod = null;
        private System.Action UpdateMethod {
            get {
                if (updateMethod == null) {
                    if (m_barImage == null) return null;
                    if ( m_barImage.type == Image.Type.Filled) {
                        updateMethod = HandleFilledImage;
                    }
                    else {
                        updateMethod = HandleSlicedImage;
                    }
                }
                return updateMethod;
            }
        }
        [SerializeField] private float initialWidth = -1;

        public float minValue {
            get { return m_minValue; }
            set { 
                m_minValue = value;
                UpdateValueDisplay();
            }
        }

        public float maxValue {
            get { return m_maxValue; }
            set { 
                m_maxValue = value;
                UpdateValueDisplay();
            }
        }

        public bool wholeNumbers {
            get { return m_wholeNumbers; }
            set { 
                m_wholeNumbers = value;
                UpdateValueDisplay();
            }
        }

        public float value {
            get { return m_value; }
            set { 
                m_value = value;
                UpdateValueDisplay();
            }
        }

        public float normalizedValue {
            get { return (m_value - m_minValue) / (m_maxValue - m_minValue); }
            set {
                float range = m_maxValue - m_minValue;
                float delta = value * range;
                m_value = m_minValue + delta;
                if (m_wholeNumbers) {
                    m_value = (float)(int)m_value;
                }
                UpdateValueDisplay();
            }
        }

        private void OnValidate() {
            if (initialWidth == -1) {
                if (rectTransform.anchorMin.x == rectTransform.anchorMax.x) {
                    initialWidth = rectTransform.sizeDelta.x;
                }
                else {
                    initialWidth = rectTransform.rect.width;
                }
            }
        }

        private void Awake() {
            if (m_barImage == null) {
                m_barImage = GetComponent<Image>();
            }
            UpdateValueDisplay();
        }

        private void UpdateValueDisplay() {
            if (UpdateMethod != null) {
                UpdateMethod.Invoke();
            }
        }

        private void HandleFilledImage() {
            m_barImage.fillAmount = normalizedValue;
        }

        private void HandleSlicedImage() {
            if (rectTransform.anchorMin.x == rectTransform.anchorMax.x) {
                rectTransform.sizeDelta = new Vector2(initialWidth * normalizedValue, rectTransform.sizeDelta.y);
            }
            else {
                if (rectTransform.pivot.x <= 0.5f) {
                    rectTransform.offsetMax = new Vector2(-initialWidth * (1-normalizedValue) - rectTransform.offsetMin.x, rectTransform.offsetMax.y);
                } 
                else {
                    rectTransform.offsetMin = new Vector2(initialWidth * (1-normalizedValue) - rectTransform.offsetMax.x, rectTransform.offsetMin.y);
                }
            }
        }

    }
}
