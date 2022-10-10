using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridSelector))]
public class GridSelectorEditor : Editor
{
    public SerializedProperty
        smoothMove_Prop,
        moveSpeed_Prop,
        hoverDistance_Prop,
        dragBuild_Prop,
        invalidPlacementFeedback_Prop,
        showInvalidPreviewObj_Prop,
        invalidPlacementMat_Prop,
        objectPlacer_Prop,
        objPreviewMat_Prop;

    static bool generalSettings = true;
    public void OnEnable()
    {
        smoothMove_Prop = serializedObject.FindProperty("smoothMove");
        moveSpeed_Prop = serializedObject.FindProperty("moveSpeed");
        hoverDistance_Prop = serializedObject.FindProperty("hoverDistance");
        dragBuild_Prop = serializedObject.FindProperty("dragBuild");
        invalidPlacementFeedback_Prop = serializedObject.FindProperty("invalidPlacementFeedback");
        showInvalidPreviewObj_Prop = serializedObject.FindProperty("showInvalidPreviewObj");
        invalidPlacementMat_Prop = serializedObject.FindProperty("invalidPlacementMat");
        objectPlacer_Prop = serializedObject.FindProperty("objectPlacer");
        objPreviewMat_Prop = serializedObject.FindProperty("objPreviewMat");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        generalSettings = EditorGUILayout.BeginFoldoutHeaderGroup(generalSettings, "General Settings");
        if (generalSettings)
        {
            EditorGUILayout.HelpBox("General settings for the behavior of the cell selector", UnityEditor.MessageType.None);
            EditorGUILayout.PropertyField(smoothMove_Prop);
            if(smoothMove_Prop.boolValue)
            {
                GUI.enabled = true;
            }
            else
            {
                GUI.enabled = false;
            }
            EditorGUILayout.PropertyField(moveSpeed_Prop);
            GUI.enabled = true;
            EditorGUILayout.PropertyField(hoverDistance_Prop);
            EditorGUILayout.PropertyField(invalidPlacementFeedback_Prop);
            if(invalidPlacementFeedback_Prop.boolValue)
            {
                GUI.enabled = true;
            }
            else
            {
                GUI.enabled = false;
            }
            EditorGUILayout.PropertyField(invalidPlacementMat_Prop);
            GUI.enabled = true;

            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(objectPlacer_Prop);
            
            if (objectPlacer_Prop.objectReferenceValue != null)
            {
                GUI.enabled = true;
            }
            else
            {
                GUI.enabled = false;
            }
            EditorGUILayout.PropertyField(objPreviewMat_Prop);
            EditorGUILayout.PropertyField(showInvalidPreviewObj_Prop);
            EditorGUILayout.PropertyField(dragBuild_Prop);

            GUI.enabled = true;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        serializedObject.ApplyModifiedProperties();
    }
}


