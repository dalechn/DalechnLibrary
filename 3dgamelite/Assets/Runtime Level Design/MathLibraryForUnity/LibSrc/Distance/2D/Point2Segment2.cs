using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and a segment
			/// </summary>
			public static float Point2Segment2(ref Vector2 point, ref Segment2 segment)
			{
				return Mathf.Sqrt(SqrPoint2Segment2(ref point, ref segment));
			}

			/// <summary>
			/// Returns distance between a point and a segment
			/// </summary>
			/// <param name="closestPoint">Point projected on a segment and clamped by segment endpoints</param>
			public static float Point2Segment2(ref Vector2 point, ref Segment2 segment, out Vector2 closestPoint)
			{
				return Mathf.Sqrt(SqrPoint2Segment2(ref point, ref segment, out closestPoint));
			}


			/// <summary>
			/// Returns squared distance between a point and a segment
			/// </summary>
			public static float SqrPoint2Segment2(ref Vector2 point, ref Segment2 segment)
			{
				Vector2 diff = point - segment.Center;
				float param = segment.Direction.Dot(diff);
				Vector2 closestPoint;
				if (-segment.Extent < param)
				{
					if (param < segment.Extent)
					{
						closestPoint = segment.Center + param * segment.Direction;
					}
					else
					{
						closestPoint = segment.P1;
					}
				}
				else
				{
					closestPoint = segment.P0;
				}
				diff = closestPoint - point;
				return diff.sqrMagnitude;
			}

			/// <summary>
			/// Returns squared distance between a point and a segment
			/// </summary>
			/// <param name="closestPoint">Point projected on a segment and clamped by segment endpoints</param>
			public static float SqrPoint2Segment2(ref Vector2 point, ref Segment2 segment, out Vector2 closestPoint)
			{
				Vector2 diff = point - segment.Center;
				float param = segment.Direction.Dot(diff);
				if (-segment.Extent < param)
				{
					if (param < segment.Extent)
					{
						closestPoint = segment.Center + param * segment.Direction;
					}
					else
					{
						closestPoint = segment.P1;
					}
				}
				else
				{
					closestPoint = segment.P0;
				}
				diff = closestPoint - point;
				return diff.sqrMagnitude;
			}
		}
	}
}
