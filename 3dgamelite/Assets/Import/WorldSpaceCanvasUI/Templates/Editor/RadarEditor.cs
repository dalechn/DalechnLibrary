using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Calcatz.WorldSpaceCanvasUI.Templates {

    [CustomEditor(typeof(Radar), editorForChildClasses: true), CanEditMultipleObjects]
    public class RadarEditor : Editor {

        public override void OnInspectorGUI() {
            Radar radar = (Radar)target;
            serializedObject.Update();

            SerializedProperty selfTargetProperty = serializedObject.FindProperty("m_selfTarget");
            if (selfTargetProperty.objectReferenceValue == null) {
                WorldSpaceUIElement uiElement = radar.GetComponent<WorldSpaceUIElement>();
                if (uiElement != null) {
                    selfTargetProperty.objectReferenceValue = uiElement.worldSpaceTargetObject;
                }
            }
            EditorGUILayout.PropertyField(selfTargetProperty);

            SerializedProperty detectionTargetProperty = serializedObject.FindProperty("m_detectionTarget");
            EditorGUILayout.PropertyField(detectionTargetProperty);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_radarPivot"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_radarPoint"));

            SerializedProperty maxWorldSpaceRadiusProperty = serializedObject.FindProperty("m_maxWorldSpaceRadius");
            EditorGUILayout.PropertyField(maxWorldSpaceRadiusProperty);

            SerializedProperty maxUIPointRadiusProperty = serializedObject.FindProperty("m_maxUIPointRadius");
            EditorGUILayout.PropertyField(maxUIPointRadiusProperty);

            SerializedProperty fixedRadiusProperty = serializedObject.FindProperty("m_fixedUIRadius");
            EditorGUILayout.PropertyField(fixedRadiusProperty);

            SerializedProperty upDirectionMethodProperty = serializedObject.FindProperty("m_upDirectionMethod");
            EditorGUILayout.PropertyField(upDirectionMethodProperty);

            if (radar.upDirectionMethod == Radar.DirectionMethod.CustomValue) {
                SerializedProperty upDirectionProperty = serializedObject.FindProperty("m_upDirection");
                EditorGUILayout.PropertyField(upDirectionProperty);
                if (GUILayout.Button("Normalize", new GUIStyle(EditorStyles.miniButtonRight))) {
                    upDirectionProperty.vector3Value = upDirectionProperty.vector3Value.normalized;
                }
            }

            SerializedProperty forwardDirectionMethodProperty = serializedObject.FindProperty("m_forwardDirectionMethod");
            EditorGUILayout.PropertyField(forwardDirectionMethodProperty);

            if (radar.forwardDirectionMethod == Radar.DirectionMethod.CustomValue) {
                SerializedProperty forwardDirectionProperty = serializedObject.FindProperty("m_forwardDirection");
                EditorGUILayout.PropertyField(forwardDirectionProperty);
                if (GUILayout.Button("Normalize", new GUIStyle(EditorStyles.miniButtonRight))) {
                    forwardDirectionProperty.vector3Value = forwardDirectionProperty.vector3Value.normalized;
                }
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_AlwaysUpdateRadarPoint"));
#if UNITY_EDITOR
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_AutoUpdateInEditMode"));
#endif

            serializedObject.ApplyModifiedProperties();
        }

    }
}
