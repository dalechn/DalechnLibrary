namespace ColorRandomizerNamespace {

	using UnityEditor;
	using UnityEngine;
	
	// IngredientDrawer
	[CustomPropertyDrawer (typeof (ColorRandomizerPalette))]
	public class ColorRandomizerPaletteDrawer : PropertyDrawer {

		// constants
		const float paletteResolution = 200f;
		const float yOffset = 50f; // offset of colors circle from top of window

		// for color fields fine tuning
		const float colorFieldRadialOffset = 32; // how far center points of color fields are from the circle
		const float colorFieldWidth = 40;
		const float colorFieldHeight = 20;
		const float colorFieldXOffset = 7; // used so that the color rectangle of the color fields will look centered
		const float circleDistortionFactor = .14f; // adjusts the y coordinate of color fields based on how far they are from the circle center, for visual purposes

		// for weighting sliders
		const float sliderWidthShown = 65; // small hack to only show the slider, not the number next to it
		const float sliderWidthReal = 115; // slider is drawn in a GUI Area that is smaller than the slider
		const float sliderHeight = 20;
		const float sliderXOffset = -6; // x offset relative to color fields
		const float sliderOffsetUp = 16; // how far sliders are under or above color pickers
		const float sliderOffsetDown = 18; // how far sliders are under or above color pickers
		
		// Draw the property inside the given rect
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			// Using BeginProperty / EndProperty on the parent property means that
			// prefab override logic works on the entire property.
			EditorGUI.BeginProperty (position, label, property);

			// calc rects
			float halfTotalWidth = position.width / 2f;
			Rect textureRect = new Rect(
				halfTotalWidth - (paletteResolution / 2),
				position.y + yOffset,
				paletteResolution,
				paletteResolution);
			Vector2 circleMidpoint = textureRect.center;
			float circleRadius = paletteResolution / 2f;

			// draw the texture
			Texture2D texture = property.FindPropertyRelative("texture").objectReferenceValue as Texture2D;
			EditorGUI.DrawTextureTransparent(textureRect, texture);
//			EditorGUI.DrawTextureAlpha(textureRect, texture);

			// get the color info
			int colorsInUse = property.FindPropertyRelative("colorsInUse").intValue;
			Color[] colors = new Color[ColorRandomizerPalette.maxColorCount];
			for (int i = 0; i < ColorRandomizerPalette.maxColorCount; i++) {
				colors[i] = property.FindPropertyRelative("colors").GetArrayElementAtIndex(i).colorValue;
			}

			// draw color fields and weighting sliders
			// calculate the circle points
			Vector2[] circlePoints = MathHelper.CalcCirclePoints(colorsInUse, (circleRadius + colorFieldRadialOffset), circleMidpoint);
			float midpointY = position.y + yOffset + circleRadius; // the y coordinate of the circle midpoint
			for (int i = 0; i < colorsInUse; i++) {
				// calculate the color field rect
				Rect colorFieldRect = new Rect(
					circlePoints[i].x - (colorFieldWidth / 2f) + colorFieldXOffset,
					circlePoints[i].y - (colorFieldHeight / 2f),
					colorFieldWidth,
					colorFieldHeight);

				// adjust the color field rect based on height, using distortion factor
				float yDist = colorFieldRect.center.y - midpointY;
				yDist *= circleDistortionFactor;
				colorFieldRect.y -= yDist;

				// draw the color field and assign newly selected colors
				property.FindPropertyRelative("colors").GetArrayElementAtIndex(i).colorValue = EditorGUI.ColorField(colorFieldRect, colors[i]);

//				Debug.Log (position.width);
				// calculate the slider rect
				Rect sliderRectArea = new Rect(colorFieldRect.center.x - (sliderWidthShown / 2f) + sliderXOffset,
				                               colorFieldRect.y,
				                               sliderWidthShown,
				                               textureRect.height + (yOffset * 2));

//				Debug.Log (colorFieldRect.y + ", " + circleRadius);
				if (colorFieldRect.center.y < (midpointY)) sliderRectArea.y -= sliderOffsetUp;
				else sliderRectArea.y += sliderOffsetDown;
				Rect sliderRectInner = new Rect(0, 0, sliderWidthReal, sliderHeight);

				// draw weighting slider
				GUI.BeginGroup(sliderRectArea);
				property.FindPropertyRelative("weightings").GetArrayElementAtIndex(i).floatValue = EditorGUI.Slider(sliderRectInner, property.FindPropertyRelative("weightings").GetArrayElementAtIndex(i).floatValue, 0.001f, 1f);
				GUI.EndGroup();
			}

			// color count adjusting
			GUILayout.BeginHorizontal();

			// label
			EditorGUILayout.LabelField("Color Count:");

			// dec button
			GUI.enabled = colorsInUse > ColorRandomizerPalette.minColorCount;
			if (GUILayout.Button("-", GUILayout.MaxWidth(30))) DecColorCount(property, colorsInUse);
			GUI.enabled = true;

			// number
			EditorGUILayout.LabelField(colorsInUse.ToString(), GUILayout.MaxWidth(12));

			// inc button
			GUI.enabled = colorsInUse < ColorRandomizerPalette.maxColorCount;
			if (GUILayout.Button("+", GUILayout.MaxWidth(30))) IncColorCount(property, colorsInUse);
			GUI.enabled = true;
			GUILayout.EndHorizontal();

			// color purity slider
			EditorGUILayout.Space();
			property.FindPropertyRelative("colorPurity").floatValue = EditorGUILayout.Slider(new GUIContent("Color Purity"),property.FindPropertyRelative("colorPurity").floatValue, 0f, 1f);


			EditorGUILayout.Space(); // looks nicer
			
			EditorGUI.EndProperty ();

		}
		// increment the color count
		void IncColorCount(SerializedProperty property, int colorsInUse) {
			if (colorsInUse < ColorRandomizerPalette.maxColorCount) property.FindPropertyRelative("colorsInUse").intValue = colorsInUse + 1;
		}
		// decrement the color count
		void DecColorCount(SerializedProperty property, int colorsInUse) {
			if (colorsInUse > ColorRandomizerPalette.minColorCount) property.FindPropertyRelative("colorsInUse").intValue = colorsInUse - 1;
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
//			return base.GetPropertyHeight (property, label) * rows;  // assuming original is one row
			return paletteResolution + (yOffset * 2);
		}
	}
}
