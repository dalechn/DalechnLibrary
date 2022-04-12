using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Calcatz.WorldSpaceCanvasUI.Templates {
    public class ColliderSelection : ColliderSelectionBase {

        private MeshCollider selectionBox;
        private Rigidbody selectionRigidbody;


        protected override void HandleSingleObjectSelection(Camera currentCamera, bool _mouseUp) {
            RaycastHit hit;
            Ray ray = currentCamera.ScreenPointToRay(startPos);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectionLayerMask)) {
                if (Input.GetKey(inclusiveSelectionKey)) {
                    if (!multiSelection || _mouseUp) { ClearSelectedGameObjects(); }
                   AddSelectedGameObject(hit.transform.gameObject);
                }
                else {
                    if (_mouseUp) {
                        ClearSelectedGameObjects();
                    }
                    AddSelectedGameObject(hit.transform.gameObject);
                }
            }
            else if (_mouseUp) {
                if (!Input.GetKey(inclusiveSelectionKey)) {
                    ClearSelectedGameObjects();
                }
            }

            onSelectedGameObjectsChanged.Invoke(selectedGameObjects);
        }

        protected override void HandleMultiObjectSelection(Camera currentCamera, bool _mouseUp) {
            Vector3[] verts = new Vector3[4];
            Vector3[] vecs = new Vector3[4];
            int i = 0;
            currentPos = Input.mousePosition;
            Vector2[] corners = GetBoundingBox(startPos, currentPos);

            foreach (Vector2 corner in corners) {
                RaycastHit hit;
                Ray ray = currentCamera.ScreenPointToRay(corner);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectionLayerMask)) {
                    verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    vecs[i] = ray.origin - hit.point;
                }
                i++;
            }

            if (selectionBox == null) {
                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.convex = true;
                selectionBox.isTrigger = true;
                Mesh selectionMesh = GenerateSelectionMesh(verts, vecs);
                selectionBox.sharedMesh = selectionMesh;
            }
            else {
                selectionBox.sharedMesh = GenerateSelectionMesh(selectionBox.sharedMesh, verts, vecs);
            }


            if (selectionRigidbody == null) {
                selectionRigidbody = gameObject.AddComponent<Rigidbody>();
                selectionRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }

            if (!Input.GetKey(KeyCode.LeftShift) && selectingState == SelectingState.PointerUp) {
                ClearSelectedGameObjects();
            }

            if (_mouseUp) {
                Destroy(selectionBox, 0.02f);
                Destroy(selectionRigidbody, 0.02f);
                selectionBox = null;
                selectionRigidbody = null;
            }

            InvokeNextEndOfFrame(delegate {
                onSelectedGameObjectsChanged.Invoke(selectedGameObjects);
            });
        }

        private Mesh GenerateSelectionMesh(Vector3[] _corners, Vector3[] _vecs) {
            Vector3[] verts = new Vector3[8];
            int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

            for (int i = 0; i < 4; i++) {
                verts[i] = _corners[i];
            }

            for (int j = 4; j < 8; j++) {
                verts[j] = _corners[j - 4] + _vecs[j - 4];
            }

            Mesh selectionMesh = new Mesh();
            selectionMesh.vertices = verts;
            selectionMesh.triangles = tris;

            return selectionMesh;
        }

        private Mesh GenerateSelectionMesh(Mesh _selectionMesh, Vector3[] _corners, Vector3[] _vecs) {
            Vector3[] verts = new Vector3[8];
            int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

            for (int i = 0; i < 4; i++) {
                verts[i] = _corners[i];
            }

            for (int j = 4; j < 8; j++) {
                verts[j] = _corners[j - 4] + _vecs[j - 4];
            }

            _selectionMesh.vertices = verts;
            _selectionMesh.triangles = tris;
            return _selectionMesh;
        }

        private void FixedUpdate() {
            if(selectionRigidbody != null && selectingState == SelectingState.PointerHold) {
                ClearSelectedGameObjects();
            }
        }

        private void OnTriggerStay(Collider other) {
            if (IsInLayerMask(other.gameObject.layer, selectionLayerMask)) {
                AddSelectedGameObject(other.gameObject);
            }
        }

    }
}