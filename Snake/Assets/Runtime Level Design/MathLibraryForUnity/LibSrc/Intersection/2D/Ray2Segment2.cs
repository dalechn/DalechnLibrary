using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Ray2 and Segment2
		/// </summary>
		public struct Ray2Segment2Intr
		{
			/// <summary>
			/// Equals to IntersectionTypes.Point or IntersectionTypes.Segment (ray and segment are collinear and overlap in more than one point)
			/// if intersection occured otherwise IntersectionTypes.Empty
			/// </summary>
			public IntersectionTypes IntersectionType;

			/// <summary>
			/// In case of IntersectionTypes.Point constains single point of intersection.
			/// In case of IntersectionTypes.Segment contains first point of intersection.
			/// Otherwise Vector2.zero.
			/// </summary>
			public Vector2 Point0;

			/// <summary>
			/// In case of IntersectionTypes.Segment contains second point of intersection.
			/// Otherwise Vector2.zero.
			/// </summary>
			public Vector2 Point1;

			/// <summary>
			/// In case of IntersectionTypes.Point contains evaluation parameter of single
			/// intersection point according to ray.
			/// In case of IntersectionTypes.Segment contains evaluation parameter of the
			/// first intersection point according to ray.
			/// Otherwise 0.
			/// </summary>
			public float Parameter0;

			/// <summary>
			/// In case of IntersectionTypes.Segment contains evaluation parameter of the
			/// second intersection point according to ray.
			/// Otherwise 0.
			/// </summary>
			public float Parameter1;
		}

		public static partial class Intersection
		{
			private static IntersectionTypes Classify(ref Ray2 ray, ref Segment2 segment, out float s0, out float s1)
			{
				// The intersection of two lines is a solution to P0+s0*D0 = P1+s1*D1.
				// Rewrite this as s0*D0 - s1*D1 = P1 - P0 = Q.  If D0.Dot(Perp(D1)) = 0,
				// the lines are parallel.  Additionally, if Q.Dot(Perp(D1)) = 0, the
				// lines are the same.  If D0.Dot(Perp(D1)) is not zero, then
				//   s0 = Q.Dot(Perp(D1))/D0.Dot(Perp(D1))
				// produces the point of intersection.  Also,
				//   s1 = Q.Dot(Perp(D0))/D0.Dot(Perp(D1))

				Vector2 originDiff = segment.Center - ray.Center;

				s0 = s1 = 0f;

				float D0DotPerpD1 = ray.Direction.DotPerp(segment.Direction);

				if (Mathf.Abs(D0DotPerpD1) > _dotThreshold)
				{
					// Lines intersect in a single point.
					float invD0DotPerpD1 = 1f / D0DotPerpD1;

					float diffDotPerpD1 = originDiff.DotPerp(segment.Direction);
					s0 = diffDotPerpD1 * invD0DotPerpD1;

					float diffDotPerpD0 = originDiff.DotPerp(ray.Direction);
					s1 = diffDotPerpD0 * invD0DotPerpD1;

					return IntersectionTypes.Point;
				}

				// Lines are parallel.
				originDiff.Normalize();

				float diffNDotPerpD1 = originDiff.DotPerp(segment.Direction);
				if (Mathf.Abs(diffNDotPerpD1) <= _dotThreshold)
				{
					s0 = Vector2.Dot(segment.P0 - ray.Center, ray.Direction);
					s1 = Vector2.Dot(segment.P1 - ray.Center, ray.Direction);

					if (s0 > s1)
					{
						float temp = s0;
						s0 = s1;
						s1 = temp;
					}

					// Lines are colinear.
					return IntersectionTypes.Segment;
				}

				// Lines are parallel, but distinct.
				return IntersectionTypes.Empty;
			}

			/// <summary>
			/// Tests whether ray and segment intersect.
			/// Returns true if intersection occurs (IntersectionTypes.Point, IntersectionTypes.Segment),
			/// or false if ray and segment do not intersect (IntersectionTypes.Empty).
			/// </summary>
			public static bool TestRay2Segment2(ref Ray2 ray, ref Segment2 segment, out IntersectionTypes intersectionType)
			{
				float parameter0, parameter1;
				intersectionType = Classify(ref ray, ref segment, out parameter0, out parameter1);

				if (intersectionType == IntersectionTypes.Point)
				{
					// Test whether the line-line intersection is on the ray and on the segment.
					if (parameter0 >= -_intervalThreshold &&
						Mathf.Abs(parameter1) <= segment.Extent + _intervalThreshold)
					{
						// OK
					}
					else
					{
						intersectionType = IntersectionTypes.Empty;
					}
				}
				else if (intersectionType == IntersectionTypes.Segment)
				{
					float w0, w1;
					int quantity = Intersection.FindSegment1Segment1(0f, float.PositiveInfinity, parameter0, parameter1, out w0, out w1);

					if (quantity == 1)
					{
						intersectionType = IntersectionTypes.Point;
					}
					else if (quantity == 0)
					{
						intersectionType = IntersectionTypes.Empty;
					}
				}

				return intersectionType != IntersectionTypes.Empty;
			}

			/// <summary>
			/// Tests whether ray and segment intersect.
			/// Returns true if intersection occurs (IntersectionTypes.Point, IntersectionTypes.Segment),
			/// or false if ray and segment do not intersect (IntersectionTypes.Empty).
			/// </summary>
			public static bool TestRay2Segment2(ref Ray2 ray, ref Segment2 segment)
			{
				IntersectionTypes intersectionType;
				return TestRay2Segment2(ref ray, ref segment, out intersectionType);
			}

			/// <summary>
			/// Tests whether ray and segment intersect and finds actual intersection parameters.
			/// Returns true if intersection occurs (IntersectionTypes.Point, IntersectionTypes.Segment),
			/// or false if ray and segment do not intersect (IntersectionTypes.Empty).
			/// </summary>
			public static bool FindRay2Segment2(ref Ray2 ray, ref Segment2 segment, out Ray2Segment2Intr info)
			{
				float parameter0, parameter1;
				info.IntersectionType = Classify(ref ray, ref segment, out parameter0, out parameter1);
				info.Point0 = info.Point1 = Vector2.zero;
				info.Parameter0 = info.Parameter1 = 0f;

				if (info.IntersectionType == IntersectionTypes.Point)
				{
					// Test whether the line-line intersection is on the ray and on the segment.
					if (parameter0 >= -_intervalThreshold &&
						Mathf.Abs(parameter1) <= segment.Extent)
					{
						info.Point0 = ray.Center + parameter0 * ray.Direction;
						info.Parameter0 = parameter0;
					}
					else
					{
						info.IntersectionType = IntersectionTypes.Empty;
					}
				}
				else if (info.IntersectionType == IntersectionTypes.Segment)
				{
					float w0, w1;
					int quantity = Intersection.FindSegment1Segment1(0f, float.PositiveInfinity, parameter0, parameter1, out w0, out w1);

					if (quantity == 2)
					{
						info.Point0 = ray.Center + w0 * ray.Direction;
						info.Point1 = ray.Center + w1 * ray.Direction;
						info.Parameter0 = w0;
						info.Parameter1 = w1;
					}
					else if (quantity == 1)
					{
						info.IntersectionType = IntersectionTypes.Point;
						info.Point0 = ray.Center + w0 * ray.Direction;
						info.Parameter0 = w0;
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
