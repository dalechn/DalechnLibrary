using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Line2 and AxisAlignedBox2
		/// </summary>
		public struct Ray2AAB2Intr
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
			/// Tests if a ray intersects an axis aligned box. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestRay2AAB2(ref Ray2 ray, ref AAB2 box)
			{
				Vector2 boxCenter, boxExtents;
				box.CalcCenterExtents(out boxCenter, out boxExtents);
				Vector2 diff = ray.Center - boxCenter;

				float WdU0 = ray.Direction.x;
				float DdU0 = diff.x;
				float ADdU0 = Mathf.Abs(DdU0);
				if (ADdU0 > boxExtents.x && DdU0 * WdU0 >= 0f)
				{
					return false;
				}

				float WdU1 = ray.Direction.y;
				float DdU1 = diff.y;
				float ADdU1 = Mathf.Abs(DdU1);
				if (ADdU1 > boxExtents.y && DdU1 * WdU1 >= 0f)
				{
					return false;
				}

				Vector2 perp = ray.Direction.Perp();

				float LHS   = Mathf.Abs(perp.Dot(diff));
				float part0 = Mathf.Abs(perp.x);
				float part1 = Mathf.Abs(perp.y);
				float RHS   = boxExtents.x * part0 + boxExtents.y * part1;

				return LHS <= RHS;
			}

			/// <summary>
			/// Tests if a ray intersects an axis aligned box and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindRay2AAB2(ref Ray2 ray, ref AAB2 box, out Ray2AAB2Intr info)
			{
				return DoClipping(
					0.0f, float.PositiveInfinity,
					ref ray.Center, ref ray.Direction, ref box, true,
					out info.Quantity, out info.Point0, out info.Point1, out info.IntersectionType);
			}
		}
	}
}
