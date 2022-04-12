using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Calcatz.WorldSpaceCanvasUI {
    [RequireComponent(typeof(RectTransform)), ExecuteInEditMode]
    public class WorldSpaceRenderModeUIElement : MonoBehaviour {

        public Transform target;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            transform.position = target.position;
            transform.LookAt(Camera.main.transform);
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
