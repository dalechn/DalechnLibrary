using UnityEditor;
using UnityEngine;


namespace CJTools
{


    [CustomPropertyDrawer(typeof(DoubleVector))]
    public class DoubleVectorDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 16 + 18;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.indentLevel = 0;

            label = EditorGUI.BeginProperty(position, label, property);
            Rect contentPosition = EditorGUI.PrefixLabel(position, label);

            //EditorGUI.indentLevel += 1;
            //contentPosition = EditorGUI.IndentedRect(position);

            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Minimum"), GUIContent.none);

            contentPosition.y += contentPosition.height / 2;
            contentPosition.height /= 2;
            EditorGUIUtility.labelWidth = 14f;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Maximum"), GUIContent.none);
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(DoubleGameObject))]
    public class RangeOfGameObjectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            position = EditorGUI.PrefixLabel(position, label);
            position.x -= 40;

            DrawGameObject(position, property);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private void DrawGameObject(Rect position, SerializedProperty prop)
        {
            EditorGUIUtility.labelWidth = 40.0f;
            float width = position.width / 1.5f;
            position.width = width;

            EditorGUI.PropertyField(position, prop.FindPropertyRelative("Origin"), new GUIContent("Origin"));
            position.x += width;
            EditorGUI.PropertyField(position, prop.FindPropertyRelative("Dest"), new GUIContent("Dest"));
        }
    }
}