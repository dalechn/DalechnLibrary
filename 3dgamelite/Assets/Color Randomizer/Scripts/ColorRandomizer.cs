using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using ColorRandomizerNamespace;

/// <summary>
/// Randomizes colors from a mix of colors specified in the inspector
/// </summary>

[ExecuteInEditMode]
public class ColorRandomizer : MonoBehaviour {

	// the palette that contains all the random colors
	public ColorRandomizerPalette palette;

	/// <summary>
	/// Returns a random color.
	/// </summary>
	public Color RandomColor() {
		return palette.RandomColor();
	}

	// in unity editor, pass on the editor update event to the palette
	#if UNITY_EDITOR	
	void OnEnable()
	{
		EditorApplication.update += OnEditorUpdate;
		EditorApplication.playmodeStateChanged += OnPlayModeChanged;
	}
	void OnDisable()
	{
		EditorApplication.update -= OnEditorUpdate;
		EditorApplication.playmodeStateChanged -= OnPlayModeChanged;
	}		
	void OnEditorUpdate()
	{
		if (palette != null) {
			palette.OnEditorUpdate();
		}
	}
	void OnPlayModeChanged () {
		if (palette != null) {
			palette.UpdateTexture();
		}
	}
	#endif
}
