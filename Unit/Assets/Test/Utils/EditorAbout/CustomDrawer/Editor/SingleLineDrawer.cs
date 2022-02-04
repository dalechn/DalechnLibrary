using UnityEditor;
using UnityEngine;

namespace CJTools
{
    [CustomPropertyDrawer(typeof(SingleLineAttribute))]
    [CustomPropertyDrawer(typeof(SingleLineClampAttribute))]
    public class SingleLineDrawer : PropertyDrawer
    {
        public enum DrawType
        {
            FLOAT,
            INT,
            BOOL
        }

        private void DrawIntTextField(Rect position, string text, SerializedProperty prop)
        {
            EditorGUI.BeginChangeCheck();
            int value = EditorGUI.IntField(position, new GUIContent(text), prop.intValue);
            SingleLineClampAttribute clamp = attribute as SingleLineClampAttribute;
            if (clamp != null)
            {
                value = Mathf.Clamp(value, (int)clamp.MinValue, (int)clamp.MaxValue);
            }
            if (EditorGUI.EndChangeCheck())
            {
                prop.intValue = value;
            }
        }

        private void DrawFloatTextField(Rect position, string text, SerializedProperty prop)
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorGUI.FloatField(position, new GUIContent(text), prop.floatValue);
            SingleLineClampAttribute clamp = attribute as SingleLineClampAttribute;
            if (clamp != null)
            {
                value = Mathf.Clamp(value, (float)clamp.MinValue, (float)clamp.MaxValue);
            }
            if (EditorGUI.EndChangeCheck())
            {
                prop.floatValue = value;
            }

        }

        private void DrawRangeField(Rect position, SerializedProperty prop, DrawType type, bool hasValue)
        {
            EditorGUIUtility.labelWidth = 40.0f;
            EditorGUIUtility.fieldWidth = 40.0f;
            float width;
            if (hasValue)
            {
                width = position.width / 3;
            }
            else
            {
                width = position.width / 2;
            }
            position.width = width;

            if (type == DrawType.FLOAT)
            {
                DrawFloatTextField(position, "Min", prop.FindPropertyRelative("Minimum"));
            }
            else if (type == DrawType.INT)
            {
                DrawIntTextField(position, "Min", prop.FindPropertyRelative("Minimum"));
            }
            else
            {
                SerializedProperty pro = prop.FindPropertyRelative("Minimum");
                bool value = EditorGUI.Toggle(position, new GUIContent("Min"), pro.boolValue);
                pro.boolValue = value;
            }

            if (hasValue)
            {
                position.x += width * 2;
            }
            else
            {
                position.x += width;
            }
            if (type == DrawType.FLOAT)
            {
                DrawFloatTextField(position, "Max", prop.FindPropertyRelative("Maximum"));
            }
            else if (type == DrawType.INT)
            {
                DrawIntTextField(position, "Max", prop.FindPropertyRelative("Maximum"));
            }
            else
            {
                SerializedProperty pro = prop.FindPropertyRelative("Maximum");
                bool value = EditorGUI.Toggle(position, new GUIContent("Max"), pro.boolValue);
                pro.boolValue = value;
            }

            position.x -= width;
            if (hasValue)
            {
                EditorGUI.BeginChangeCheck();
                SerializedProperty v = prop.FindPropertyRelative("Value");
                if (type == DrawType.FLOAT)
                {
                    float value = EditorGUI.FloatField(position, new GUIContent("Value"), v.floatValue);
                    float min = prop.FindPropertyRelative("Minimum").floatValue;
                    float max = prop.FindPropertyRelative("Maximum").floatValue;
                    value = Mathf.Clamp(value, min, max);

                    if (EditorGUI.EndChangeCheck())
                    {
                        v.floatValue = value;
                    }
                }
                else if (type == DrawType.INT)
                {
                    int value = EditorGUI.IntField(position, new GUIContent("Value"), v.intValue);
                    int min = prop.FindPropertyRelative("Minimum").intValue;
                    int max = prop.FindPropertyRelative("Maximum").intValue;
                    value = Mathf.Clamp(value, min, max);

                    if (EditorGUI.EndChangeCheck())
                    {
                        v.intValue = value;
                    }
                }
            }
        }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, prop);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(label.text, (attribute as SingleLineAttribute).Tooltip));

            switch (prop.type)
            {
                case "DoubleInt":
                    DrawRangeField(position, prop, DrawType.INT, false);
                    break;

                case "DoubleFloat":
                    DrawRangeField(position, prop, DrawType.FLOAT, false);
                    break;

                case "DoubleBool":
                    DrawRangeField(position, prop, DrawType.BOOL, false);
                    break;

                case "ThreeInt":
                    DrawRangeField(position, prop, DrawType.INT, true);
                    break;

                case "ThreeFloat":
                    DrawRangeField(position, prop, DrawType.FLOAT, true);
                    break;

                default:
                    EditorGUI.HelpBox(position, "[SingleLineDrawer] doesn't work with type '" + prop.type + "'", MessageType.Error);
                    break;
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}