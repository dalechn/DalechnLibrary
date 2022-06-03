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
			public static float Point3Segment3(ref Vector3 point, ref Segment3 segment)
			{
				return Mathf.Sqrt(SqrPoint3Segment3(ref point, ref segment));
			}

			/// <summary>
			/// Returns distance between a point and a segment
			/// </summary>
			/// <param name="closestPoint">Point projected on a segment and clamped by segment endpoints</param>
			public static float Point3Segment3(ref Vector3 point, ref Segment3 segment, out Vector3 closestPoint)
			{
				return Mathf.Sqrt(SqrPoint3Segment3(ref point, ref segment, out closestPoint));
			}


			/// <summary>
			/// Returns squared distance between a point and a segment
			/// </summary>
			public static float SqrPoint3Segment3(ref Vector3 point, ref Segment3 segment)
			{
				Vector3 diff = point - segment.Center;
				float param = segment.Direction.Dot(diff);
				Vector3 closestPoint;
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
			public static float SqrPoint3Segment3(ref Vector3 point, ref Segment3 segment, out Vector3 closestPoint)
			{
				Vector3 diff = point - segment.Center;
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
