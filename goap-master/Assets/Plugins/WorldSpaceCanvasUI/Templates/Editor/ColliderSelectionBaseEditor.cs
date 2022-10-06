using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Calcatz.WorldSpaceCanvasUI.Templates {

    [CustomEditor(typeof(ColliderSelectionBase), editorForChildClasses: true), CanEditMultipleObjects]
    public class ColliderSelectionBaseEditor : Editor {

        public override void OnInspectorGUI() {
            //ColliderSelectionBase colliderSelection = (ColliderSelectionBase)target;
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_camera"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_selectionLayerMask"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_selectingState"));
            SerializedProperty multiSelection = serializedObject.FindProperty("m_multiSelection");
            EditorGUILayout.PropertyField(multiSelection);
            if (multiSelection.boolValue) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_selectionSprite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_selectionBoxColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_inclusiveSelectionKey"));
            }
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_selectedGameObjects"));
            GUI.enabled = true;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onSelectedGameObjectsChanged"));

            serializedObject.ApplyModifiedProperties();
        }

    }
}
