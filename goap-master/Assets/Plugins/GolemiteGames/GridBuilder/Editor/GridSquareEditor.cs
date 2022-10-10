using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(GridSquare))]
public class GridSquareEditor : Editor
{
    public SerializedProperty
        gridWidth_Prop,
        gridHeight_Prop,
        cellSize_Prop,
        gridType_Prop,
        drawSimple_Prop,
        checkMatRuntime_Prop,
        tileX_Prop,
        tileY_Prop,
        autoCellBlocking_Prop,
        blocktype_Prop,
        showAboveBoxColliders_Prop,
        showBelowRays_Prop,
        checkGroundHits_Prop,
        aboveCheckBoxSize_Prop,
        aboveCheckBoxHeight_Prop,
        groundLayer_Prop,
        groundDistance_Prop,
        gridCellPrefab_Prop,
        secondGridCellPrefab_Prop,
        blockedAboveCellPrefab_Prop,
        blockedBelowCellPrefab_Prop,
        drawGridPositions_Prop,
        drawCellPositions_Prop;

    static bool generalSettings = true;
    static bool autoCellBlock = false;
    static bool prefabs = true;
    static bool debug = false;
    public void OnEnable()
    {
        gridWidth_Prop = serializedObject.FindProperty("gridWidth");
        gridHeight_Prop = serializedObject.FindProperty("gridHeight");
        cellSize_Prop = serializedObject.FindProperty("cellSize");
        gridType_Prop = serializedObject.FindProperty("gridType");
        drawSimple_Prop = serializedObject.FindProperty("drawSimple");
        checkMatRuntime_Prop = serializedObject.FindProperty("checkMatRuntime");
        tileX_Prop = serializedObject.FindProperty("tileX");
        tileY_Prop = serializedObject.FindProperty("tileY");
        autoCellBlocking_Prop = serializedObject.FindProperty("autoCellBlocking");
        blocktype_Prop = serializedObject.FindProperty("blocktype");
        groundLayer_Prop = serializedObject.FindProperty("groundLayer");
        groundDistance_Prop = serializedObject.FindProperty("groundDistance");
        aboveCheckBoxSize_Prop = serializedObject.FindProperty("aboveCheckBoxSize");
        aboveCheckBoxHeight_Prop = serializedObject.FindProperty("aboveCheckBoxHeight");
        showAboveBoxColliders_Prop = serializedObject.FindProperty("showAboveBoxColliders");
        showBelowRays_Prop = serializedObject.FindProperty("showBelowRays");
        checkGroundHits_Prop = serializedObject.FindProperty("checkGroundHits");
        gridCellPrefab_Prop = serializedObject.FindProperty("gridCellPrefab");
        secondGridCellPrefab_Prop = serializedObject.FindProperty("secondGridCellPrefab");
        blockedAboveCellPrefab_Prop = serializedObject.FindProperty("blockedAboveCellPrefab");
        blockedBelowCellPrefab_Prop = serializedObject.FindProperty("blockedBelowCellPrefab");
        drawGridPositions_Prop = serializedObject.FindProperty("drawGridPositions");
        drawCellPositions_Prop = serializedObject.FindProperty("drawCellPositions");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        generalSettings = EditorGUILayout.BeginFoldoutHeaderGroup(generalSettings, "General Settings");
        if(generalSettings)
        {
            EditorGUILayout.HelpBox("General settings for setting the size of the grid. \n" +
                "Set in Metres", MessageType.None);
            EditorGUILayout.PropertyField(gridWidth_Prop);
            EditorGUILayout.PropertyField(gridHeight_Prop);
            EditorGUILayout.PropertyField(cellSize_Prop);
            EditorGUILayout.PropertyField(gridType_Prop);
            EditorGUILayout.PropertyField(drawSimple_Prop);
            if(gridType_Prop.enumValueIndex == 2)
            {
                EditorGUILayout.HelpBox("Use this to tune the tiling of the material on the Simple grid type in play mode. \n" +
                    "Uncheck for final build", UnityEditor.MessageType.None);
                EditorGUILayout.PropertyField(checkMatRuntime_Prop);
                EditorGUILayout.PropertyField(tileX_Prop);
                EditorGUILayout.PropertyField(tileY_Prop);
            }
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        if(gridType_Prop.enumValueIndex != 2)
        {
            autoCellBlock = EditorGUILayout.BeginFoldoutHeaderGroup(autoCellBlock, "AutoCellBlock");
            if (autoCellBlock)
            {
                EditorGUILayout.HelpBox("If enabled this will automatically block cells depending on settings", UnityEditor.MessageType.None);
                EditorGUILayout.PropertyField(autoCellBlocking_Prop);
                if(autoCellBlocking_Prop.boolValue)
                {
                    GUI.enabled = true;
                }
                else
                {
                    GUI.enabled = false;
                }
                EditorGUILayout.PropertyField(blocktype_Prop);
                EditorGUILayout.PropertyField(groundLayer_Prop);
                EditorGUILayout.PropertyField(groundDistance_Prop);
                EditorGUILayout.PropertyField(aboveCheckBoxSize_Prop);
                EditorGUILayout.PropertyField(aboveCheckBoxHeight_Prop);
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Use these tools to get a visual look at what is being done, " +
                    "and to adjust your settings accordingly", UnityEditor.MessageType.None);
                EditorGUILayout.PropertyField(showAboveBoxColliders_Prop);
                EditorGUILayout.PropertyField(showBelowRays_Prop);
                EditorGUILayout.PropertyField(checkGroundHits_Prop);
                GUI.enabled = true;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        EditorGUILayout.Space();
        if (gridType_Prop.enumValueIndex != 2)
        {
            prefabs = EditorGUILayout.BeginFoldoutHeaderGroup(prefabs, "Prefabs");
            if (prefabs)
            {
                EditorGUILayout.HelpBox("Prefabs for creating the Single and Checkered grid types", UnityEditor.MessageType.None);
                EditorGUILayout.PropertyField(gridCellPrefab_Prop);
                if (gridType_Prop.enumValueIndex == 1)
                {
                    GUI.enabled = true;
                }
                else
                {
                    GUI.enabled = false;
                }
                EditorGUILayout.PropertyField(secondGridCellPrefab_Prop);
                GUI.enabled = true;
                EditorGUILayout.PropertyField(blockedAboveCellPrefab_Prop);
                EditorGUILayout.PropertyField(blockedBelowCellPrefab_Prop);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        EditorGUILayout.Space();

        debug = EditorGUILayout.BeginFoldoutHeaderGroup(debug, "Debug");
        if (debug)
        {
            EditorGUILayout.HelpBox("Shows the cell positions and point positions as they are stored, " +
                "turning this on for large grids will significantly impact performance", UnityEditor.MessageType.None);
            EditorGUILayout.PropertyField(drawCellPositions_Prop);
            EditorGUILayout.PropertyField(drawGridPositions_Prop);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
