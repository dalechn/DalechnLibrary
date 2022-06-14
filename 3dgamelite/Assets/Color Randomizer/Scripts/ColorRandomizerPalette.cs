namespace ColorRandomizerNamespace {
	using UnityEngine;
	#if UNITY_EDITOR
	using UnityEditor;
	#endif
	using System.Collections;
	using System.Linq;

	[System.Serializable]
	public class ColorRandomizerPalette {

		// constants
		public const int minColorCount = 1;
		public const int maxColorCount = 6;
		const int defaultColorsInUse = 3;
		const float defaultColorPurity = .5f;
		const int paletteResolution = 200;

		const float weightingSliderPower = 1.5f; // how powerful the color weighting sliders are. this is an exponent.


		// palette defining values
		public Color[] colors; // array of all colors, this one gets changed externally
		[SerializeField]
		Color[] colorsApplied; // array of the colors that have been applied to the palette

		public int colorsInUse; // how many of the colors are used
		[SerializeField]
		int colorsInUseApplied; // same as colorsApplied

		public float[] weightings; // the influence weighting of each color
		[SerializeField]
		float[] weightingsApplied; // weightings that have been applied to the texture
		
		public Texture2D texture; // the texture with the color circle
		public static bool usingProSkin; // used for determining background color

		// color algorithm values
		public float colorPurity;
		[SerializeField]
		float colorPurityApplied;

		// make sure to initialize
		public bool hasBeenInitialized = false;

		#if UNITY_EDITOR
		[SerializeField]
		bool updateTextureNextFrame = false;


		// separate initialization method because unity does not like constructors
		public void Initialize () {
//			Debug.Log ("Initializing");
			// init fields
			colorsInUse = defaultColorsInUse;
			colors = new Color[maxColorCount];
			colorsApplied = new Color[maxColorCount];
			weightings = new float[maxColorCount];
			weightingsApplied = new float[maxColorCount];
			colorPurity = defaultColorPurity;

			// assign initial values
			for (int i = 0; i < maxColorCount; i++) {
				colors[i] = CreateRandomColor();
				weightings[i] = .5f;
			}

			// init palette
			texture = new Texture2D(paletteResolution, paletteResolution, TextureFormat.RGBA32, false);
			UpdateTexture();
			
			// find out if the pro skin is used
			usingProSkin = EditorGUIUtility.isProSkin;

			// set initialized to true
			hasBeenInitialized = true;
		}

		// called by the color randomizer monobehaviours every editor update
		public void OnEditorUpdate () {
//			Debug.Log (Event.current);
			// update the texture one editor update frame after it became necessary
			// (to not update in realtime while changing color values, it would get laggy)
			if (updateTextureNextFrame) {
				UpdateTexture();
			}
			updateTextureNextFrame = ShouldUpdateTexture();
//			Debug.Log (updateTextureNextFrame);
		}
		
		#endif

		// returns whether the current colors or color count are no the same as those that have been applied to the texture
		bool ShouldUpdateTexture() {
			if (colorsInUse != colorsInUseApplied) return true;
			if (colorPurity != colorPurityApplied) return true;
//			if (blurriness != blurrinessApplied) return true;
			for (int i = 0; i < maxColorCount; i++) {
				if (!colors[i].Equals(colorsApplied[i])) return true;
				if (weightings[i] != weightingsApplied[i]) return true;
			}
			return false;
		}

		// update the palette texture with everything that might have changed
		public void UpdateTexture () {
			#if UNITY_EDITOR
			// find out if the pro skin is used. purely vsual for the editor, not needed in build.
			usingProSkin = EditorGUIUtility.isProSkin;
			#endif
			// recalculate the texture
			RecalculateTexture();
			// update the values that have been applied
			colorsInUseApplied = colorsInUse;
			colorPurityApplied = colorPurity;
//			blurrinessApplied = blurriness;
			for (int i = 0; i < maxColorCount; i++) {
				colorsApplied[i] = colors[i];
				weightingsApplied[i] = weightings[i];
			}

			#if UNITY_EDITOR 
			// this variable is only needed in the editor
			updateTextureNextFrame = false;
			#endif
		}


		// pick a random color from the palette
		public Color RandomColor() {
			Color result;
			int x = Random.Range(0, paletteResolution);
			int y = Random.Range(0, paletteResolution);
			while (!PointIsInColorCircle(x, y)) {				
				x = Random.Range(0, paletteResolution);
				y = Random.Range(0, paletteResolution);
			}
			result = texture.GetPixel(x, y);
			return result;
		}

		// recalculate the palette texture
		public void RecalculateTexture() {
			float radius = paletteResolution / 2f;
			Vector2 centerPoint = new Vector2(radius, radius);
			Vector2[] colorPoints = MathHelper.CalcCirclePoints(colorsInUse, radius, new Vector2(radius, radius));

			// apply a quadratic function to color urity so that the slider feels more "realistic"
			float adjustedColorPurity = colorPurity;
			adjustedColorPurity = AdjustColorPurity(adjustedColorPurity);

			// this curve determines influence value per distance for colors points
			AnimationCurve colorInfluenceCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
			
			// for all pixels in the texture
			for (int y = 0; y < paletteResolution; y++) {
				int yI = paletteResolution - y; // y is inverted from calc space to texture space! using inverted y for setpixel
				for (int x = 0; x < paletteResolution; x++) {
					// if the pixel is within the color circle
					if (PointIsInCircle(x,y, centerPoint, radius))
						texture.SetPixel(x, yI, CalculateColor(x, y, colorPoints, colorInfluenceCurve, adjustedColorPurity)); // calculate a color
					else
						// if not, make it grey
						texture.SetPixel(x, yI, EditorBGColor());
				}
			}
			texture.Apply();
		}

		// adjusts the color purity to make a more usable slider
		static float AdjustColorPurity (float x) {
			// invert
			x = 1 - x;

			// apply quadratic function
			x -= 1f;
			x = Mathf.Pow(x, 2);
			x *= -1f;
			x += 1f;

			// invert back
			x = 1 - x;

			return x;
		}

		// calculate the color of a pixel by weighting the color points
		Color CalculateColor(int x, int y, Vector2[] colorPoints, AnimationCurve colorInfluenceCurve, float adjustedColorPurity) {
			Vector2 point = new Vector2(x, y);
			// fill an array with weightings based on the distances of the point to each color
			float[] resultWeightings = new float[colorPoints.Length];

			// calc distance-based weightings
			for (int i = 0; i < colorPoints.Length; i++) {

				float currentWeighting = 0f;
				float distance = (point - colorPoints[i]).magnitude; // calc distance from point to color point
				float invertedDistance = paletteResolution - distance;

				// alternative version of algorithm 1, not limiting influence to a circle
				currentWeighting = invertedDistance;
				currentWeighting /= paletteResolution; // normalize
				currentWeighting *= weightings[i];

//				currentWeighting = colorInfluenceCurve.Evaluate(currentWeighting) * currentWeighting * weightings[i]; // apply color influence curve and weighting
				currentWeighting = colorInfluenceCurve.Evaluate(currentWeighting); // apply color influence curve once more because it looks cooler
				currentWeighting = colorInfluenceCurve.Evaluate(currentWeighting); // apply color influence curve once more because it looks cooler
				currentWeighting = colorInfluenceCurve.Evaluate(currentWeighting); // apply color influence curve once more because it looks cooler
//				currentWeighting = colorInfluenceCurve.Evaluate(currentWeighting); // apply color influence curve once more because it looks cooler
//				currentWeighting = colorInfluenceCurve.Evaluate(currentWeighting); // apply color influence curve once more because it looks cooler

				
				float blurryWeighting = WeightingFunction(distance, weightings[i]);

				resultWeightings[i] = Mathf.Lerp(blurryWeighting, currentWeighting, adjustedColorPurity);
			}
			
			// blend the colors
			Color blendedColor = WeightedColor(colors, resultWeightings);
//			Color crisp = StrongestColor(colors, resultWeightings);
			return blendedColor;
		}

		// returns a weighting based on distance
		static float WeightingFunction(float distance, float colorWeighting) {
			return colorWeighting/ distance;
		}

		
		// returns a color by blending a set of colors by weightings
		static Color WeightedColor(Color[] colors, float[] weightings) {
			float r = 0f, g = 0f, b = 0f, a = 0f;
			float weightingSum = weightings.Sum();
			// add each color multiplied with their normalized weighting
			for (int i = 0; i < weightings.Length; i++) {
				r += colors[i].r * (weightings[i] / weightingSum);
				g += colors[i].g * (weightings[i] / weightingSum);
				b += colors[i].b * (weightings[i] / weightingSum);
				a += colors[i].a * (weightings[i] / weightingSum);
			}
			return new Color(r, g, b, a);
		}

		// returns the color with the highest weighting
		static Color StrongestColor(Color[] colors, float[] weightings) {
			int indexWithHighestWeighting = 0;
			float highestWeighting = 0f;
			for (int i = 0; i < weightings.Length; i++) {
				if (weightings[i] > highestWeighting) {
					highestWeighting = weightings[i];
					indexWithHighestWeighting = i;
				}
			}
			return colors[indexWithHighestWeighting];
		}

		// tells if a point is within the color circle
		static bool PointIsInColorCircle(int x, int y) {
			float radius = paletteResolution / 2f;
			return PointIsInCircle(x, y, new Vector2(radius, radius), radius);
		}
		
		// tell if a point is inside a circle
		static bool PointIsInCircle(int x, int y, Vector2 centerPoint, float radius) {
			float squaredRadius = Mathf.Pow(radius, 2f);
			float centerX = centerPoint.x;
			float centerY = centerPoint.y;
			
			return Mathf.Pow((x - centerX), 2f) + Mathf.Pow((y - centerY), 2f) <= squaredRadius;
		}

		// randomly generate a color
		static Color CreateRandomColor() {
			return new Color(
				Random.Range(0f, 1f),
				Random.Range(0f, 1f),
				Random.Range(0f, 1f));
		}

		// returns the current background color of the unity editor
		static Color EditorBGColor () {
			if(usingProSkin) {
				if (!Application.isPlaying) return new Color32(56, 56, 56, 255);
				else return new Color32(45, 45, 45, 255);
			}
			else {
				if (!Application.isPlaying) return new Color32(194, 194, 194, 255);
				else return new Color32(154, 154, 154, 255);
			}
		}
	}
}