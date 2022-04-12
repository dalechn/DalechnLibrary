using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Segment2 and Circle2
		/// </summary>
		public struct Segment2Circle2Intr
		{
			/// <summary>
			/// Equals to IntersectionTypes.Point or IntersectionTypes.Segment
			/// if intersection occured otherwise IntersectionTypes.Empty
			/// </summary>
			public IntersectionTypes IntersectionType;

			/// <summary>
			/// First point of intersection (in case of IntersectionTypes.Point or IntersectionTypes.Segment)
			/// </summary>
			public Vector2 Point0;

			/// <summary>
			/// Second point of intersection (in case of IntersectionTypes.Segment)
			/// </summary>
			public Vector2 Point1;
		}

		public static partial class Intersection
		{
			/// <summary>
			/// Tests whether segment and circle intersect.
			/// Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment2Circle2(ref Segment2 segment, ref Circle2 circle)
			{
				Vector2 delta = segment.Center - circle.Center;
				float a0 = delta.sqrMagnitude - circle.Radius * circle.Radius;

				if (a0 <= Mathfex.ZeroTolerance)
				{
					// P is inside or on the sphere.
					return true;
				}
				// Else: P is outside the sphere.

				float a1 = Vector2.Dot(segment.Direction, delta);
				float discr = a1 * a1 - a0;
				if (discr < -Mathfex.ZeroTolerance)
				{
					// two complex-valued roots, no intersections
					return false;
				}

				float absA1 = Mathf.Abs(a1);
				float qval = segment.Extent * (segment.Extent - 2 * absA1) + a0;
				return qval <= Mathfex.ZeroTolerance || absA1 <= segment.Extent;
			}

			/// <summary>
			/// Tests whether segment and circle intersect and finds actual intersection parameters.
			/// Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindSegment2Circle2(ref Segment2 segment, ref Circle2 circle, out Segment2Circle2Intr info)
			{
				float t0, t1;
				int quantity;
				bool intersects = Find(ref segment.Center, ref segment.Direction, ref circle.Center, circle.Radius, out quantity, out t0, out t1);

				info.Point0 = info.Point1 = Vector2.zero;

				if (intersects)
				{
					// Reduce root count if line-circle intersections are not on segment.
					if (quantity == 1)
					{
						if (Mathf.Abs(t0) > (segment.Extent + Mathfex.ZeroTolerance))
						{
							info.IntersectionType = IntersectionTypes.Empty;
						}
						else
						{
							info.IntersectionType = IntersectionTypes.Point;
							info.Point0 = segment.Center + t0 * segment.Direction;
						}
					}
					else
					{
						float tolerance = segment.Extent + Mathfex.ZeroTolerance;
						if (t1 < -tolerance || t0 > tolerance)
						{
							info.IntersectionType = IntersectionTypes.Empty;
						}
						else
						{
							if (t1 <= tolerance)
							{
								if (t0 < -tolerance)
								{
									quantity = 1;
									t0 = t1;
								}
							}
							else
							{
								quantity = t0 >= -tolerance ? 1 : 0;
							}

							switch (quantity)
							{
								default:
								case 0:
									info.IntersectionType = IntersectionTypes.Empty;
									break;

								case 1:
									info.IntersectionType = IntersectionTypes.Point;
									info.Point0 = segment.Center + t0 * segment.Direction;
									break;

								case 2:
									info.IntersectionType = IntersectionTypes.Segment;
									info.Point0  = segment.Center + t0 * segment.Direction;
									info.Point1 = segment.Center + t1 * segment.Direction;
									break;
							}
						}
					}
				}
				else
				{
					info.IntersectionType = IntersectionTypes.Empty;
				}

				return info.IntersectionType != IntersectionTypes.Empty;
			}
		}
	}
}
