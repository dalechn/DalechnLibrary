namespace ColorRandomizerNamespace {

	using UnityEngine;
	using UnityEditor;
	using System.Collections;

	// Script created by Custom Inspector Generator
	[CustomEditor(typeof(ColorRandomizer))]
	public class ColorRandomizerEditor : Editor 
	{
		// The target script in serialized and non-serialized form
		ColorRandomizer targetScript;
		SerializedObject serializedTargetScript;
		
		// The serialized properties of the target script
		SerializedProperty palette;

		void OnEnable() {
			// Get a reference to the target script and serialize it
			targetScript = (ColorRandomizer)target;
			serializedTargetScript = new SerializedObject(targetScript);

			// If the palette has not been initialized (= script has just beed added), call init method
			if (!targetScript.palette.hasBeenInitialized) {
				targetScript.palette.Initialize();
			}
			// update texture in case of skin change
			targetScript.palette.UpdateTexture();

			// update if pro skin is used
			ColorRandomizerPalette.usingProSkin = EditorGUIUtility.isProSkin;
			
			// Find serialized properties
			palette = serializedTargetScript.FindProperty("palette");

		}
		
	    public override void OnInspectorGUI() {
//			DrawDefaultInspector();

	    	// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
			serializedTargetScript.Update();
			
			// Draw serialized properties
			EditorGUILayout.PropertyField(palette, new GUIContent("palette"));
		
			// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
			serializedTargetScript.ApplyModifiedProperties();
		}

		

	}
	
}

