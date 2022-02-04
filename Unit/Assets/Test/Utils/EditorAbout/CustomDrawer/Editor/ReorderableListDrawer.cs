using UnityEditor;
using UnityEngine;

namespace CJTools
{
    [CustomPropertyDrawer(typeof(ReorderableListBase), true)]
    public class ReorderableListDrawer : PropertyDrawer
    {
        private UnityEditorInternal.ReorderableList list;
        private string name = "";

        private UnityEditorInternal.ReorderableList getList(SerializedProperty property)
        {
            if (list == null)
            {
                list = new UnityEditorInternal.ReorderableList(property.serializedObject, property, true, true, true, true);
                list.drawElementCallback = (UnityEngine.Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.width -= 40;
                    rect.x += 20;
                    EditorGUI.PropertyField(rect, property.GetArrayElementAtIndex(index), true);
                };
                list.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, name);
                };
                list.onRemoveCallback = delegate
                {
                    if (EditorUtility.DisplayDialog("Warning", "Do you want to delete it?", "yes", "no"))
                    {
                        UnityEditorInternal.ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    }
                };
            }
            return list;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return getList(property.FindPropertyRelative("List")).GetHeight();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            name = label.text;

            var listProperty = property.FindPropertyRelative("List");
            var list = getList(listProperty);
            var height = 0f;
            for (var i = 0; i < listProperty.arraySize; i++)
            {
                height = Mathf.Max(height, EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(i)));
            }
            list.elementHeight = height;
            list.DoList(new Rect(position.x, position.y, position.width, position.height));
        }
    }
}
