using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Line2 and AxisAlignedBox2
		/// </summary>
		public struct Segment2AAB2Intr
		{
			/// <summary>
			/// Equals to IntersectionTypes.Point or IntersectionTypes.Segment if intersection occured otherwise IntersectionTypes.Empty
			/// </summary>
			public IntersectionTypes IntersectionType;

			/// <summary>
			/// Number of intersection points.
			/// IntersectionTypes.Empty: 0;
			/// IntersectionTypes.Point: 1;
			/// IntersectionTypes.Segment: 2.
			/// </summary>
			public int Quantity;

			/// <summary>
			/// First intersection point
			/// </summary>
			public Vector2 Point0;

			/// <summary>
			/// Second intersection point
			/// </summary>
			public Vector2 Point1;
		}

		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a segment intersects an axis aligned box. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment2AAB2(ref Segment2 segment, ref AAB2 box)
			{
				Vector2 boxCenter, boxExtents;
				box.CalcCenterExtents(out boxCenter, out boxExtents);
				Vector2 diff = segment.Center - boxCenter;
				float RHS;

				float AWdU0 = Mathf.Abs(segment.Direction.x);
				float ADdU0 = Mathf.Abs(diff.x);
				RHS         = boxExtents.x + segment.Extent * AWdU0;
				if (ADdU0 > RHS)
				{
					return false;
				}

				float AWdU1 = Mathf.Abs(segment.Direction.y);
				float ADdU1 = Mathf.Abs(diff.y);
				RHS         = boxExtents.y + segment.Extent * AWdU1;
				if (ADdU1 > RHS)
				{
					return false;
				}

				Vector2 perp = segment.Direction.Perp();

				float LHS   = Mathf.Abs(perp.Dot(diff));
				float part0 = Mathf.Abs(perp.x);
				float part1 = Mathf.Abs(perp.y);
				RHS         = boxExtents.x * part0 + boxExtents.y * part1;

				return LHS <= RHS;
			}

			/// <summary>
			/// Tests if a segment intersects an axis aligned box and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindSegment2AAB2(ref Segment2 segment, ref AAB2 box, out Segment2AAB2Intr info)
			{
				return DoClipping(
					-segment.Extent, segment.Extent,
					ref segment.Center, ref segment.Direction, ref box, true,
					out info.Quantity, out info.Point0, out info.Point1, out info.IntersectionType);
			}
		}
	}
}
