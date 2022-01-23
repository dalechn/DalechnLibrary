using Invector;
using Invector.vCharacterController.v2_5D;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(v2_5DPath))]
public class v2_5DPathEditor : vEditorBase
{
    private void OnSceneGUI()
    {
        var path = (v2_5DPath)target;

        if (path.points == null || Application.isPlaying)
        {
            return;
        }

        for (int i = 0; i < path.points.Length; i++)
        {
            if (path.points[i] != null)
            {
                var position = path.points[i].position;
                EditorGUI.BeginChangeCheck();
                position = Handles.PositionHandle(position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(path.points[i], "Position");
                    path.points[i].position = position;
                }
            }
        }
    }
}
