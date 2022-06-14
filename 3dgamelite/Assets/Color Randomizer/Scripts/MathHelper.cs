namespace ColorRandomizerNamespace {

	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// Math helper for the color randomizer
	/// </summary>

	public class MathHelper : MonoBehaviour {

		
		// calculates points that divide a circle into equal parts
		public static Vector2[] CalcCirclePoints(int pointCount, float radius, Vector2 midPoint) {

			float angle = 0f;
			float angleOffset = 270f -  (360f / pointCount); // apply an angle offset so that one point is always fixed at the top of the circle
			Vector2[] results = new Vector2[pointCount];
			
			// calculate the points for midpoint at (0,0)
			for(int i = 0 ; i < pointCount  ;i++)
			{
				angle = i * (360/pointCount);
				angle += angleOffset;
				
				float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
				float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
				results[i] = new Vector2(x, y);				
			}
			
			// apply the midpoint offset
			for(int i = 0 ; i < pointCount  ;i++)
			{
				results[i] += midPoint;
				
			}
			
			return results;
		}
	}

}
