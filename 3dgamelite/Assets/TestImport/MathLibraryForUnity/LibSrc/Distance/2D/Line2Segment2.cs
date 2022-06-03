using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a line and a segment
			/// </summary>
			public static float Line2Segment2(ref Line2 line, ref Segment2 segment)
			{
				return Mathf.Sqrt(SqrLine2Segment2(ref line, ref segment));
			}

			/// <summary>
			/// Returns distance between a line and a segment
			/// </summary>
			/// <param name="closestPoint0">Point on line closest to segment</param>
			/// <param name="closestPoint1">Point on segment closest to line</param>
			public static float Line2Segment2(ref Line2 line, ref Segment2 segment, out Vector2 closestPoint0, out Vector2 closestPoint1)
			{
				return Mathf.Sqrt(SqrLine2Segment2(ref line, ref segment, out closestPoint0, out closestPoint1));
			}


			/// <summary>
			/// Returns squared distance between a line and a segment
			/// </summary>
			public static float SqrLine2Segment2(ref Line2 line, ref Segment2 segment)
			{
				Vector2 diff = line.Center - segment.Center;
				float a01 = -line.Direction.Dot(segment.Direction);
				float b0 = diff.Dot(line.Direction);
				float c = diff.sqrMagnitude;
				float det = Mathf.Abs(1f - a01 * a01);
				float b1, s0, s1, sqrDist, extDet;

				if (det >= Mathfex.ZeroTolerance)
				{
					// The line and segment are not parallel.
					b1 = -diff.Dot(segment.Direction);
					s1 = a01 * b0 - b1;
					extDet = segment.Extent * det;

					if (s1 >= -extDet)
					{
						if (s1 <= extDet)
						{
							// Two interior points are closest, one on the line and one
							// on the segment.
							sqrDist = 0f;
						}
						else
						{
							// The endpoint e1 of the segment and an interior point of
							// the line are closest.
							s1 = segment.Extent;
							s0 = -(a01 * s1 + b0);
							sqrDist = -s0 * s0 + s1 * (s1 + 2f * b1) + c;
						}
					}
					else
					{
						// The endpoint e0 of the segment and an interior point of the
						// line are closest.
						s1 = -segment.Extent;
						s0 = -(a01 * s1 + b0);
						sqrDist = -s0 * s0 + s1 * (s1 + 2f * b1) + c;
					}
				}
				else
				{
					// The line and segment are parallel.  Choose the closest pair so that
					// one point is at segment origin.
					s1 = 0f;
					s0 = -b0;
					sqrDist = b0 * s0 + c;
				}

				// Account for numerical round-off errors.
				if (sqrDist < 0f)
				{
					sqrDist = 0f;
				}
				return sqrDist;
			}

			/// <summary>
			/// Returns squared distance between a line and a segment
			/// </summary>
			/// <param name="closestPoint0">Point on line closest to segment</param>
			/// <param name="closestPoint1">Point on segment closest to line</param>
			public static float SqrLine2Segment2(ref Line2 line, ref Segment2 segment, out Vector2 closestPoint0, out Vector2 closestPoint1)
			{
				Vector2 diff = line.Center - segment.Center;
				float a01 = -line.Direction.Dot(segment.Direction);
				float b0 = diff.Dot(line.Direction);
				float c = diff.sqrMagnitude;
				float det = Mathf.Abs(1f - a01 * a01);
				float b1, s0, s1, sqrDist, extDet;

				if (det >= Mathfex.ZeroTolerance)
				{
					// The line and segment are not parallel.
					b1 = -diff.Dot(segment.Direction);
					s1 = a01 * b0 - b1;
					extDet = segment.Extent * det;

					if (s1 >= -extDet)
					{
						if (s1 <= extDet)
						{
							// Two interior points are closest, one on the line and one
							// on the segment.
							float invDet = 1f / det;
							s0 = (a01 * b1 - b0) * invDet;
							s1 *= invDet;
							sqrDist = 0f;
						}
						else
						{
							// The endpoint e1 of the segment and an interior point of
							// the line are closest.
							s1 = segment.Extent;
							s0 = -(a01 * s1 + b0);
							sqrDist = -s0 * s0 + s1 * (s1 + 2f * b1) + c;
						}
					}
					else
					{
						// The endpoint e0 of the segment and an interior point of the
						// line are closest.
						s1 = -segment.Extent;
						s0 = -(a01 * s1 + b0);
						sqrDist = -s0 * s0 + s1 * (s1 + 2f * b1) + c;
					}
				}
				else
				{
					// The line and segment are parallel.  Choose the closest pair so that
					// one point is at segment origin.
					s1 = 0f;
					s0 = -b0;
					sqrDist = b0 * s0 + c;
				}

				closestPoint0 = line.Center + s0 * line.Direction;
				closestPoint1 = segment.Center + s1 * segment.Direction;

				// Account for numerical round-off errors.
				if (sqrDist < 0f)
				{
					sqrDist = 0f;
				}
				return sqrDist;
			}
		}
	}
}