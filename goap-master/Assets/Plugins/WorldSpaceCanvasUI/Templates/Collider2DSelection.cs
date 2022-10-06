using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Calcatz.WorldSpaceCanvasUI.Templates {
    public class Collider2DSelection : ColliderSelectionBase {

        protected override void HandleSingleObjectSelection(Camera currentCamera, bool _mouseUp) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, selectionLayerMask);

            if (hit.collider != null) {
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
            currentPos = Input.mousePosition;

            if (!Input.GetKey(KeyCode.LeftShift) && selectingState == SelectingState.PointerUp) {
                ClearSelectedGameObjects();
            }

            Collider2D[] overlaps = Physics2D.OverlapAreaAll(Camera.main.ScreenToWorldPoint(startPos), Camera.main.ScreenToWorldPoint(currentPos), selectionLayerMask);
            ClearSelectedGameObjects();
            for(int i=0; i<overlaps.Length; i++) {
                AddSelectedGameObject(overlaps[i].gameObject);
            }

            onSelectedGameObjectsChanged.Invoke(selectedGameObjects);
        }

    }
}