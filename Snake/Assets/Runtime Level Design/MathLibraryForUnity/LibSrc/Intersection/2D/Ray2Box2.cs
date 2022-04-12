using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Ray2 and Box2
		/// </summary>
		public struct Ray2Box2Intr
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
			/// Tests whether ray and box intersect.
			/// Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestRay2Box2(ref Ray2 ray, ref Box2 box)
			{
				Vector2 diff = ray.Center - box.Center;

				float WdU0  = ray.Direction.Dot(box.Axis0);
				float DdU0  = diff.Dot(box.Axis0);
				float ADdU0 = Mathf.Abs(DdU0);
				if (ADdU0 > box.Extents.x && DdU0 * WdU0 >= 0f)
				{
					return false;
				}

				float WdU1  = ray.Direction.Dot(box.Axis1);
				float DdU1  = diff.Dot(box.Axis1);
				float ADdU1 = Mathf.Abs(DdU1);
				if (ADdU1 > box.Extents.y && DdU1 * WdU1 >= 0f)
				{
					return false;
				}

				Vector2 perp = ray.Direction.Perp();

				float LHS   = Mathf.Abs(perp.Dot(diff));
				float part0 = Mathf.Abs(perp.Dot(box.Axis0));
				float part1 = Mathf.Abs(perp.Dot(box.Axis1));
				float RHS   = box.Extents.x * part0 + box.Extents.y * part1;

				return LHS <= RHS;
			}

			/// <summary>
			/// Tests whether ray and box intersect and finds actual intersection parameters.
			/// Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindRay2Box2(ref Ray2 ray, ref Box2 box, out Ray2Box2Intr info)
			{
				return DoClipping(
					0.0f, float.PositiveInfinity,
					ref ray.Center, ref ray.Direction, ref box, true,
					out info.Quantity, out info.Point0, out info.Point1, out info.IntersectionType);
			}
		}
	}
}
