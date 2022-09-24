using UnityEngine;
using UnityEditor;

namespace Chindianese.OnScreenConsole
{
    [CustomEditor(typeof(OnScreenConsoleHandler))]
    public class OnScreenConsoleEditor : Editor
    {
        OnScreenConsoleHandler script;

        void OnEnable()
        {
            script = (OnScreenConsoleHandler)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Clear Console"))
            {
                script.ClearConsole();                
            }
        }
    }
}