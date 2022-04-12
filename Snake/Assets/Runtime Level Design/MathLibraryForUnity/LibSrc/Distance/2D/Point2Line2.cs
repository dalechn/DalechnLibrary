using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and a line
			/// </summary>
			public static float Point2Line2(ref Vector2 point, ref Line2 line)
			{
				return Mathf.Sqrt(SqrPoint2Line2(ref point, ref line));
			}

			/// <summary>
			/// Returns distance between a point and a line
			/// </summary>
			/// <param name="closestPoint">Point projected on a line</param>
			public static float Point2Line2(ref Vector2 point, ref Line2 line, out Vector2 closestPoint)
			{
				return Mathf.Sqrt(SqrPoint2Line2(ref point, ref line, out closestPoint));
			}


			/// <summary>
			/// Returns squared distance between a point and a line
			/// </summary>
			public static float SqrPoint2Line2(ref Vector2 point, ref Line2 line)
			{
				Vector2 diff = point - line.Center;
				float param = line.Direction.Dot(diff);
				Vector2 closestPoint = line.Center + param * line.Direction;
				diff = closestPoint - point;
				return diff.sqrMagnitude;
			}

			/// <summary>
			/// Returns squared distance between a point and a line
			/// </summary>
			/// <param name="closestPoint">Point projected on a line</param>
			public static float SqrPoint2Line2(ref Vector2 point, ref Line2 line, out Vector2 closestPoint)
			{
				Vector2 diff = point - line.Center;
				float param = line.Direction.Dot(diff);
				closestPoint = line.Center + param * line.Direction;
				diff = closestPoint - point;
				return diff.sqrMagnitude;
			}
		}
	}
}
