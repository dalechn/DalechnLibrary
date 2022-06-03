using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Line2 and Segment2
		/// </summary>
		public struct Line2Segment2Intr
		{
			/// <summary>
			/// Equals to IntersectionTypes.Point or IntersectionTypes.Segment (line and segment are collinear)
			/// if intersection occured otherwise IntersectionTypes.Empty
			/// </summary>
			public IntersectionTypes IntersectionType;

			/// <summary>
			/// In case of IntersectionTypes.Point constains single point of intersection.
			/// Otherwise Vector2.zero.
			/// </summary>
			public Vector2 Point;

			/// <summary>
			/// In case of IntersectionTypes.Point contains evaluation parameter of single
			/// intersection point according to line.
			/// Otherwise 0.
			/// </summary>
			public float Parameter;
		}

		public static partial class Intersection
		{
			private static IntersectionTypes Classify(ref Segment2 segment, ref Line2 line, out float s0, out float s1)
			{
				// The intersection of two lines is a solution to P0+s0*D0 = P1+s1*D1.
				// Rewrite this as s0*D0 - s1*D1 = P1 - P0 = Q.  If D0.Dot(Perp(D1)) = 0,
				// the lines are parallel.  Additionally, if Q.Dot(Perp(D1)) = 0, the
				// lines are the same.  If D0.Dot(Perp(D1)) is not zero, then
				//   s0 = Q.Dot(Perp(D1))/D0.Dot(Perp(D1))
				// produces the point of intersection.  Also,
				//   s1 = Q.Dot(Perp(D0))/D0.Dot(Perp(D1))

				Vector2 originDiff = segment.Center - line.Center;

				s0 = s1 = 0f;

				float D0DotPerpD1 = line.Direction.DotPerp(segment.Direction);

				if (Mathf.Abs(D0DotPerpD1) > _dotThreshold)
				{
					// Lines intersect in a single point.
					float invD0DotPerpD1 = 1f / D0DotPerpD1;

					float diffDotPerpD1 = originDiff.DotPerp(segment.Direction);
					s0 = diffDotPerpD1 * invD0DotPerpD1;

					float diffDotPerpD0 = originDiff.DotPerp(line.Direction);
					s1 = diffDotPerpD0 * invD0DotPerpD1;

					return IntersectionTypes.Point;
				}

				// Lines are parallel.
				originDiff.Normalize();

				float diffNDotPerpD1 = originDiff.DotPerp(segment.Direction);
				if (Mathf.Abs(diffNDotPerpD1) <= _dotThreshold)
				{
					// Lines are colinear.
					return IntersectionTypes.Segment;
				}

				// Lines are parallel, but distinct.
				return IntersectionTypes.Empty;
			}

			/// <summary>
			/// Tests whether line and segment intersect.
			/// Returns true if intersection occurs (IntersectionTypes.Point, IntersectionTypes.Segment),
			/// or false if line and segment do not intersect (IntersectionTypes.Empty).
			/// </summary>
			public static bool TestLine2Segment2(ref Line2 line, ref Segment2 segment, out IntersectionTypes intersectionType)
			{
				float parameter0, parameter1;
				intersectionType = Classify(ref segment, ref line, out parameter0, out parameter1);

				if (intersectionType == IntersectionTypes.Point)
				{
					// Test whether the line-line intersection is on the segment.
					if (Mathf.Abs(parameter1) <= segment.Extent)
					{
						// OK
					}
					else
					{
						intersectionType = IntersectionTypes.Empty;
					}
				}

				return intersectionType != IntersectionTypes.Empty;
			}

			/// <summary>
			/// Tests whether line and segment intersect.
			/// Returns true if intersection occurs (IntersectionTypes.Point, IntersectionTypes.Segment),
			/// or false if line and segment do not intersect (IntersectionTypes.Empty).
			/// </summary>
			public static bool TestLine2Segment2(ref Line2 line, ref Segment2 segment)
			{
				IntersectionTypes intersectionType;
				return TestLine2Segment2(ref line, ref segment, out intersectionType);
			}

			/// <summary>
			/// Tests whether line and segment intersect and finds actual intersection parameters.
			/// Returns true if intersection occurs (IntersectionTypes.Point, IntersectionTypes.Segment),
			/// or false if line and segment do not intersect (IntersectionTypes.Empty).
			/// </summary>
			public static bool FindLine2Segment2(ref Line2 line, ref Segment2 segment, out Line2Segment2Intr info)
			{
				float parameter0, parameter1;
				info.IntersectionType = Classify(ref segment, ref line, out parameter0, out parameter1);
				info.Point = Vector2.zero;
				info.Parameter = 0f;

				if (info.IntersectionType == IntersectionTypes.Point)
				{
					// Test whether the line-line intersection is on the segment.
					if (Mathf.Abs(parameter1) <= segment.Extent + _intervalThreshold)
					{
						info.Point = line.Center + parameter0 * line.Direction;
						info.Parameter = parameter0;
					}
					else
					{
						info.IntersectionType = IntersectionTypes.Empty;
					}
				}

				return info.IntersectionType != IntersectionTypes.Empty;
			}
		}
	}
}
