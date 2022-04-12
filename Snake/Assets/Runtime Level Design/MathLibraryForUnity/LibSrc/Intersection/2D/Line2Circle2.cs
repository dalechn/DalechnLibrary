using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Line2 and Circle2
		/// </summary>
		public struct Line2Circle2Intr
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
			private static bool Find(ref Vector2 origin, ref Vector2 direction, ref Vector2 center, float radius, out int rootCount, out float t0, out float t1)
			{
				// Intersection of a the line P+t*D and the circle |X-C| = R.  The line
				// direction is unit length. The t value is a root to the quadratic
				// equation:
				//   0 = |t*D+P-C|^2 - R^2
				//     = t^2 + 2*Dot(D,P-C)*t + |P-C|^2-R^2
				//     = t^2 + 2*a1*t + a0
				// If two roots are returned, the order is T[0] < T[1].

				Vector2 diff = origin - center;
				float a0 = diff.sqrMagnitude - radius * radius;
				float a1 = direction.Dot(diff);
				float discr = a1*a1 - a0;

				if (discr > Mathfex.ZeroTolerance)
				{
					rootCount = 2;
					discr = Mathf.Sqrt(discr);
					t0 = -a1 - discr;
					t1 = -a1 + discr;
				}
				else if (discr < -Mathfex.ZeroTolerance)
				{
					rootCount = 0;
					t0 = t1 = 0f;
				}
				else  // discr == 0
				{
					rootCount = 1;
					t0 = -a1;
					t1 = 0f;
				}

				return rootCount != 0;
			}

			/// <summary>
			/// Tests whether line and circle intersect.
			/// Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestLine2Circle2(ref Line2 line, ref Circle2 circle)
			{
				Vector2 delta = line.Center - circle.Center;
				float a0 = delta.sqrMagnitude - circle.Radius * circle.Radius;

				if (a0 <= Mathfex.ZeroTolerance)
				{
					// line.P is inside or on the sphere.
					return true;
				}
				// Else: line.P is outside the sphere.

				float a1 = Vector2.Dot(line.Direction, delta);
				float discr = a1 * a1 - a0;
				
				return discr >= -Mathfex.ZeroTolerance;
			}

			/// <summary>
			/// Tests whether line and circle intersect and finds actual intersection parameters.
			/// Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindLine2Circle2(ref Line2 line, ref Circle2 circle, out Line2Circle2Intr info)
			{
				float t0, t1;
				int quantity;
				bool intersects = Find(ref line.Center, ref line.Direction, ref circle.Center, circle.Radius, out quantity, out t0, out t1);

				if (intersects)
				{
					if (quantity == 1)
					{
						info.IntersectionType = IntersectionTypes.Point;
						info.Point0 = line.Center + t0 * line.Direction;
						info.Point1 = Vector2.zero;
					}
					else
					{
						info.IntersectionType = IntersectionTypes.Segment;
						info.Point0  = line.Center + t0 * line.Direction;
						info.Point1 = line.Center + t1 * line.Direction;
					}
				}
				else
				{
					info.IntersectionType = IntersectionTypes.Empty;
					info.Point0 = info.Point1 = Vector2.zero;
				}

				return info.IntersectionType != IntersectionTypes.Empty;
			}
		}
	}
}
