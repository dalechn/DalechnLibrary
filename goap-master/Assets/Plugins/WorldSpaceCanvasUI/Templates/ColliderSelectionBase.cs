using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Calcatz.WorldSpaceCanvasUI.Templates {
    public abstract 
        class ColliderSelectionBase : MonoBehaviour {

        [System.Serializable]
        public class GameObjectsEvent : UnityEvent<GameObject[]> { }

        public enum SelectingState {
            PointerUp,
            PointerHold
        }

        [Tooltip("If camera is not set, then it will set to main camera by default.")]
        [SerializeField] private Camera m_camera;
        public Camera cam {
            get { return m_camera; }
            set { m_camera = value; }
        }

        [SerializeField] protected LayerMask m_selectionLayerMask = 1 << 0;
        public LayerMask selectionLayerMask {
            get { return m_selectionLayerMask; }
            set { m_selectionLayerMask = value; }
        }

        [SerializeField] private bool m_multiSelection = true;
        public bool multiSelection {
            get { return m_multiSelection; }
            set { m_multiSelection = value; }
        }

        [SerializeField] private SelectingState m_selectingState;
        public SelectingState selectingState {
            get { return m_selectingState; }
            set { m_selectingState = value; }
        }

        [SerializeField] private Sprite m_selectionSprite;
        public Sprite selectionSprite {
            get { return m_selectionSprite; }
            set { m_selectionSprite = value; }
        }

        [SerializeField] private Color m_selectionBoxColor = new Color(0.8f, 0.8f, 0.95f, 0.25f);
        public Color selectionBoxColor {
            get { return m_selectionBoxColor; }
            set { m_selectionBoxColor = value; }
        }

        [SerializeField] private KeyCode m_inclusiveSelectionKey = KeyCode.LeftShift;
        public KeyCode inclusiveSelectionKey {
            get { return m_inclusiveSelectionKey; }
            set { m_inclusiveSelectionKey = value; }
        }

        [SerializeField] private List<GameObject> m_selectedGameObjects = new List<GameObject>();
        public GameObject[] selectedGameObjects {
            get { return m_selectedGameObjects.ToArray(); }
        }
        public GameObject selectedGameObject {
            get { return m_selectedGameObjects[0]; }
        }

        protected void AddSelectedGameObject(GameObject _gameObject) {
            if (!m_selectedGameObjects.Contains(_gameObject)) {
                m_selectedGameObjects.Add(_gameObject);
            }
        }
        protected void RemoveSelectedGameObject(GameObject _gameObject) {
            if (m_selectedGameObjects.Contains(_gameObject)) {
                Debug.Log("Removed: " + _gameObject.name);
                m_selectedGameObjects.Remove(_gameObject);
            }
        }
        protected void ClearSelectedGameObjects() {
            m_selectedGameObjects.Clear();
        }

        public GameObjectsEvent onSelectedGameObjectsChanged = new GameObjectsEvent();

        protected bool isDragging;

        protected Vector3 startPos;
        protected Vector3 currentPos;

        private static Texture2D m_whiteTexture;
        public static Texture2D whiteTexture {
            get {
                if (m_whiteTexture == null) {
                    m_whiteTexture = new Texture2D(1, 1);
                    m_whiteTexture.SetPixel(0, 0, Color.white);
                    m_whiteTexture.Apply();
                }
                return m_whiteTexture;
            }
        }


        protected virtual void Start() {
            isDragging = false;
        }

        protected virtual void Update() {
            Camera currentCamera = cam != null ? cam : Camera.main;
            if (Input.GetMouseButtonDown(0)) {
                startPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0)) {
                if ((startPos - Input.mousePosition).magnitude > 40) {
                    isDragging = true;
                }
                if (selectingState == SelectingState.PointerHold) {
                    if (!isDragging) {
                        MouseDragSingleSelection(currentCamera);
                    }
                    else if (multiSelection) {
                        MouseDragMultiSelection(currentCamera);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) {

                if (!isDragging) {
                    HandleSingleObjectSelection(currentCamera, true);
                }
                else if (multiSelection) {
                    HandleMultiObjectSelection(currentCamera, true);
                }

                isDragging = false;

            }

        }

        private void MouseDragSingleSelection(Camera currentCamera) {
            GameObject[] previousSelection = selectedGameObjects;
            HandleSingleObjectSelection(currentCamera, false);
            bool isChanged = previousSelection.Length != m_selectedGameObjects.Count;
            if (!isChanged) {
                for (int i = 0; i < previousSelection.Length; i++) {
                    if (previousSelection[i] != m_selectedGameObjects[i]) {
                        isChanged = true;
                        break;
                    }
                }
            }
            if (isChanged) {
                onSelectedGameObjectsChanged.Invoke(selectedGameObjects);
            }
        }

        private void MouseDragMultiSelection(Camera currentCamera) {
            GameObject[] previousSelection = selectedGameObjects;
            HandleMultiObjectSelection(currentCamera, false);
            InvokeNextEndOfFrame(delegate {
                bool isChanged = previousSelection.Length != m_selectedGameObjects.Count;
                if (!isChanged) {
                    for (int i = 0; i < previousSelection.Length; i++) {
                        if (previousSelection[i] != m_selectedGameObjects[i]) {
                            isChanged = true;
                            break;
                        }
                    }
                }
                if (isChanged) {
                    onSelectedGameObjectsChanged.Invoke(selectedGameObjects);
                }
            });
        }

        protected virtual void HandleSingleObjectSelection(Camera currentCamera, bool _mouseUp) {
            
        }

        protected virtual void HandleMultiObjectSelection(Camera currentCamera, bool _mouseUp) {
            
        }

        private void OnGUI() {
            if (isDragging && multiSelection) {
                var rect = GetScreenRect(startPos, Input.mousePosition);

                if (selectionSprite != null) {
                    GUI.color = selectionBoxColor;
                    GUIStyle style = new GUIStyle("label");
                    style.normal.background = selectionSprite.texture;
                    style.border = new RectOffset((int)selectionSprite.border.x, (int)selectionSprite.border.y, (int)selectionSprite.border.z, (int)selectionSprite.border.w);
                    GUI.Label(rect, GUIContent.none, style);
                    GUI.color = Color.white;
                }
                else {
                    Color rectColor = selectionBoxColor;
                    rectColor.a = selectionBoxColor.a == 1f ? 0.25f : selectionBoxColor.a;
                    DrawRect(rect, rectColor);
                    DrawBorder(rect, 2, new Color(selectionBoxColor.r, selectionBoxColor.g, selectionBoxColor.b));
                }

            }
        }

        protected Vector2[] GetBoundingBox(Vector2 _point1, Vector2 _point2) {
            Vector2 newPoint1;
            Vector2 newPoint2;
            Vector2 newPoint3;
            Vector2 newPoint4;

            if (_point1.x < _point2.x) {
                if (_point1.y > _point2.y) {
                    newPoint1 = _point1;
                    newPoint2 = new Vector2(_point2.x, _point1.y);
                    newPoint3 = new Vector2(_point1.x, _point2.y);
                    newPoint4 = _point2;
                }
                else {
                    newPoint1 = new Vector2(_point1.x, _point2.y);
                    newPoint2 = _point2;
                    newPoint3 = _point1;
                    newPoint4 = new Vector2(_point2.x, _point1.y);
                }
            }
            else {
                if (_point1.y > _point2.y) {
                    newPoint1 = new Vector2(_point2.x, _point1.y);
                    newPoint2 = _point1;
                    newPoint3 = _point2;
                    newPoint4 = new Vector2(_point1.x, _point2.y);
                }
                else {
                    newPoint1 = _point2;
                    newPoint2 = new Vector2(_point1.x, _point2.y);
                    newPoint3 = new Vector2(_point2.x, _point1.y);
                    newPoint4 = _point1;
                }

            }

            Vector2[] corners = { newPoint1, newPoint2, newPoint3, newPoint4 };
            return corners;

        }

        protected static Rect GetScreenRect(Vector3 _screenPosition1, Vector3 _screenPosition2) {
            _screenPosition1.y = Screen.height - _screenPosition1.y;
            _screenPosition2.y = Screen.height - _screenPosition2.y;

            var topLeft = Vector3.Min(_screenPosition1, _screenPosition2);
            var bottomRight = Vector3.Max(_screenPosition1, _screenPosition2);

            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }

        public static void DrawRect(Rect rect, Color color) {
            GUI.color = color;
            GUI.DrawTexture(rect, whiteTexture);
            GUI.color = Color.white;
        }

        public static void DrawBorder(Rect _rect, float _size, Color _color) {
            DrawRect(new Rect(_rect.xMin, _rect.yMin, _rect.width, _size), _color);
            DrawRect(new Rect(_rect.xMin, _rect.yMin, _size, _rect.height), _color);
            DrawRect(new Rect(_rect.xMax - _size, _rect.yMin, _size, _rect.height), _color);
            DrawRect(new Rect(_rect.xMin, _rect.yMax - _size, _rect.width, _size), _color);
        }

        public void InvokeNextEndOfFrame(UnityAction _callback) {
            try {
                StartCoroutine(_InvokeNextEndOfFrame(_callback));
            }
            catch {
                Debug.Log("Trying to invoke " + _callback.ToString() + " but it doesnt seem to exist");
            }
        }

        private IEnumerator _InvokeNextEndOfFrame(UnityAction _callback) {
            yield return null;
            yield return new WaitForEndOfFrame();
            _callback.Invoke();
        }

        public static bool IsInLayerMask(int _layer, LayerMask _layermask) {
            return _layermask == (_layermask | (1 << _layer));
        }

        public void TestChangedGameObjectsEvent(GameObject[] _gameObjects) {
            string result = "On GameObjects Changed Event: ";
            foreach(GameObject go in _gameObjects) {
                result += go.name + ", ";
            }
            if (_gameObjects.Length > 0) {
                result = result.Substring(0, result.Length - 2);
            }
            Debug.Log(result);
        }
    }
}