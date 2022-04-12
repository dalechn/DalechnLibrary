using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Calcatz.WorldSpaceCanvasUI.Templates {

    [CustomEditor(typeof(ProgressBar), editorForChildClasses: true), CanEditMultipleObjects]
    public class ProgressBarEditor : Editor {

        public override void OnInspectorGUI() {
            ProgressBar bar = (ProgressBar)target;
            serializedObject.Update();

            SerializedProperty barImageProperty = serializedObject.FindProperty("m_barImage");
            EditorGUILayout.PropertyField(barImageProperty);

            if (((Image)barImageProperty.objectReferenceValue).type != Image.Type.Filled) {
                SerializedProperty initialWidthProperty = serializedObject.FindProperty("initialWidth");
                EditorGUILayout.PropertyField(initialWidthProperty);
            }

            SerializedProperty minValueProperty = serializedObject.FindProperty("m_minValue");
            EditorGUILayout.PropertyField(minValueProperty);

            SerializedProperty maxValueProperty = serializedObject.FindProperty("m_maxValue");
            EditorGUILayout.PropertyField(maxValueProperty);

            SerializedProperty wholeNumbersProperty = serializedObject.FindProperty("m_wholeNumbers");
            EditorGUILayout.PropertyField(wholeNumbersProperty);

            SerializedProperty valueProperty = serializedObject.FindProperty("m_value");

            EditorGUILayout.LabelField("Value");
            if (wholeNumbersProperty.boolValue) {
                valueProperty.floatValue = EditorGUILayout.IntSlider((int)valueProperty.floatValue, (int)minValueProperty.floatValue, (int)maxValueProperty.floatValue);
            }
            else {
                valueProperty.floatValue = EditorGUILayout.Slider(valueProperty.floatValue, minValueProperty.floatValue, maxValueProperty.floatValue);
            }

            EditorGUILayout.LabelField("Normalized Value");
            bar.normalizedValue = EditorGUILayout.Slider(bar.normalizedValue, 0, 1);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Default Inspector", EditorStyles.boldLabel);
            base.OnInspectorGUI();
        }

    }
}
