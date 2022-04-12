/*
 * This is an editor script to add a button to update UI's position if it's not set to Auto Update Position
 */

using UnityEditor;
using UnityEngine;

namespace Calcatz.WorldSpaceCanvasUI {
    [CustomEditor(typeof(WorldSpaceUIElement)), CanEditMultipleObjects]
    class WorldSpaceUIElementEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            serializedObject.Update();
            WorldSpaceUIElement element = (WorldSpaceUIElement)target;
            SerializedProperty scaler = serializedObject.FindProperty("m_scaler");
            SerializedProperty initialScale = scaler.FindPropertyRelative("initialScale");
            SerializedProperty nearestDistance = scaler.FindPropertyRelative("nearestDistance");
            SerializedProperty farthestDistance = scaler.FindPropertyRelative("farthestDistance");
            SerializedProperty minScale = scaler.FindPropertyRelative("minScale");
            SerializedProperty maxScale = scaler.FindPropertyRelative("maxScale");
            SerializedProperty enableProximityBasedScaling = serializedObject.FindProperty("m_EnableProximityBasedScaling");
            SerializedProperty worldSpaceScaleFactor = scaler.FindPropertyRelative("worldSpaceScaleFactor");

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(enableProximityBasedScaling);

            if (enableProximityBasedScaling.boolValue) {
                GUILayout.Label(new GUIContent("Scale Multiplier"), EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Initial Scale", GUILayout.Width(85));
                    EditorGUILayout.PropertyField(initialScale, GUIContent.none, GUILayout.Width(161));
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Distance:", GUILayout.Width(55));
                    float labelWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 30f;
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(farthestDistance, new GUIContent("Far"));
                    if (EditorGUI.EndChangeCheck()) {
                        if (farthestDistance.floatValue < 0) {
                            farthestDistance.floatValue = 0;
                        }
                        if (farthestDistance.floatValue < nearestDistance.floatValue) {
                            nearestDistance.floatValue = farthestDistance.floatValue;
                        }
                    }
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(nearestDistance, new GUIContent("Near"));
                    if (EditorGUI.EndChangeCheck()) {
                        if (nearestDistance.floatValue < 0) {
                            nearestDistance.floatValue = 0;
                        }
                        if (nearestDistance.floatValue > farthestDistance.floatValue) {
                            farthestDistance.floatValue = nearestDistance.floatValue;
                        }
                    }
                    EditorGUIUtility.labelWidth = labelWidth;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Scale:", GUILayout.Width(55));
                    float labelWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 30f;
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(minScale, new GUIContent("Min"));
                    if (EditorGUI.EndChangeCheck()) {
                        if (minScale.floatValue < 0) {
                            minScale.floatValue = 0;
                        }
                        if (minScale.floatValue > maxScale.floatValue) {
                            maxScale.floatValue = minScale.floatValue;
                        }
                    }
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(maxScale, new GUIContent("Max"));
                    if (EditorGUI.EndChangeCheck()) {
                        if (maxScale.floatValue < 0) {
                            maxScale.floatValue = 0;
                        }
                        if (maxScale.floatValue < minScale.floatValue) {
                            minScale.floatValue = maxScale.floatValue;
                        }
                    }
                    EditorGUIUtility.labelWidth = labelWidth;
                }
                GUILayout.EndHorizontal();
                if (element.canvasComponent.renderMode == RenderMode.WorldSpace) {
                    EditorGUILayout.PropertyField(worldSpaceScaleFactor, new GUIContent("World Space Scale Factor", "Scale factor used for world space render mode only."));
                }
            }

            serializedObject.ApplyModifiedProperties();
            if (!element.m_AutoUpdateInEditMode) {
                if (GUILayout.Button("Update UI Position")) {
                    element.UpdateUIPosition();
                }
            }

            EditorGUILayout.Space();
        }
    }
}