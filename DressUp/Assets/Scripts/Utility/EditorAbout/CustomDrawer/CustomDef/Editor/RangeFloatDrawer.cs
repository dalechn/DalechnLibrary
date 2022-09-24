#pragma warning disable 0649

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RangeFloat))]
public class RangeFloatDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 只能在 position 指定的范围内绘制
        EditorGUI.BeginProperty(position, label, property);
        {
            float x, w;
            int elementCount = 3;
            for (int i = 0; i < elementCount; i++)
            {
                w = position.width / elementCount;
                x = position.x + w * i;
                switch (i)
                {
                    case 0:
                        EditorGUIUtility.labelWidth = 25;
                        EditorGUI.LabelField(new Rect(x, position.y, w, position.height), property.displayName);
                        break;
                    case 1:
                        EditorGUIUtility.labelWidth = 25;
                        EditorGUI.PropertyField(new Rect(x, position.y, w, position.height), property.FindPropertyRelative("min"));
                        break;
                    case 2:
                        EditorGUIUtility.labelWidth = 25;
                        EditorGUI.PropertyField(new Rect(x, position.y, w, position.height), property.FindPropertyRelative("max"));
                        break;
                }
            }
        }
        EditorGUI.EndProperty();
        EditorGUIUtility.labelWidth = 0; // 将其设置为 0 会重置为默认值
    }

}


[CustomPropertyDrawer(typeof(RangeFloat3))]
public class RangeFloat3Drawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 只能在 position 指定的范围内绘制
        EditorGUI.BeginProperty(position, label, property);
        {
            float x, w;
            int elementCount = 4;
            for (int i = 0; i < elementCount; i++)
            {
                w = position.width / elementCount;
                x = position.x + w * i;
                switch (i)
                {
                    case 0:
                        EditorGUIUtility.labelWidth = 25;
                        EditorGUI.LabelField(new Rect(x, position.y, w, position.height), property.displayName);
                        break;
                    case 1:
                        EditorGUIUtility.labelWidth = 25;
                        EditorGUI.PropertyField(new Rect(x, position.y, w, position.height), property.FindPropertyRelative("min"));
                        break;
                    case 2:
                        EditorGUIUtility.labelWidth = 25;
                        EditorGUI.PropertyField(new Rect(x, position.y, w, position.height), property.FindPropertyRelative("max"));
                        break;
                    case 3:
                        EditorGUIUtility.labelWidth = 35;
                        EditorGUI.PropertyField(new Rect(x, position.y, w, position.height), property.FindPropertyRelative("value"));
                        break;
                }
            }
        }
        EditorGUI.EndProperty();
        EditorGUIUtility.labelWidth = 0; // 将其设置为 0 会重置为默认值
    }

}