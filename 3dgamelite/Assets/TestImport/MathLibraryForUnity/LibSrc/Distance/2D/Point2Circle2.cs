using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and a circle
			/// </summary>
			public static float Point2Circle2(ref Vector2 point, ref Circle2 circle)
			{
				float diff = (point - circle.Center).magnitude - circle.Radius;
				return diff > 0f ? diff : 0f;
			}

			/// <summary>
			/// Returns distance between a point and a circle
			/// </summary>
			/// <param name="closestPoint">Point projected on a circle</param>
			public static float Point2Circle2(ref Vector2 point, ref Circle2 circle, out Vector2 closestPoint)
			{
				Vector2 diff = point - circle.Center;
				float diffSqrLen = diff.sqrMagnitude;
				if (diffSqrLen > circle.Radius * circle.Radius)
				{
					float diffLen = Mathf.Sqrt(diffSqrLen);
					closestPoint = circle.Center + diff * (circle.Radius / diffLen);
					return diffLen - circle.Radius;
				}
				closestPoint = point;
				return 0f;
			}

			
			/// <summary>
			/// Returns squared distance between a point and a circle
			/// </summary>
			public static float SqrPoint2Circle2(ref Vector2 point, ref Circle2 circle)
			{
				float diff = (point - circle.Center).magnitude - circle.Radius;
				return diff > 0f ? diff * diff : 0f;
			}

			/// <summary>
			/// Returns squared distance between a point and a circle
			/// </summary>
			/// <param name="closestPoint">Point projected on a circle</param>
			public static float SqrPoint2Circle2(ref Vector2 point, ref Circle2 circle, out Vector2 closestPoint)
			{
				Vector2 diff = point - circle.Center;
				float diffSqrLen = diff.sqrMagnitude;
				if (diffSqrLen > circle.Radius * circle.Radius)
				{
					float diffLen = Mathf.Sqrt(diffSqrLen);
					closestPoint = circle.Center + diff * (circle.Radius / diffLen);
					float result = diffLen - circle.Radius;
					return result * result;
				}
				closestPoint = point;
				return 0f;
			}
		}
	}
}