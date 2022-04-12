using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Segment3 and AxisAlignedBox3
		/// </summary>
		public struct Segment3AAB3Intr
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
			public Vector3 Point0;

			/// <summary>
			/// Second intersection point
			/// </summary>
			public Vector3 Point1;
		}

		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a segment intersects an axis aligned box. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment3AAB3(ref Segment3 segment, ref AAB3 box)
			{
				Vector3 boxCenter;
				Vector3 boxExtents;
				box.CalcCenterExtents(out boxCenter, out boxExtents);

				float RHS;
				Vector3 diff = segment.Center - boxCenter;

				float AWdU0 = Mathf.Abs(segment.Direction.x);
				float ADdU0 = Mathf.Abs(diff.x);
				RHS = boxExtents.x + segment.Extent * AWdU0;
				if (ADdU0 > RHS)
				{
					return false;
				}

				float AWdU1 = Mathf.Abs(segment.Direction.y);
				float ADdU1 = Mathf.Abs(diff.y);
				RHS = boxExtents.y + segment.Extent * AWdU1;
				if (ADdU1 > RHS)
				{
					return false;
				}

				float AWdU2 = Mathf.Abs(segment.Direction.z);
				float ADdU2 = Mathf.Abs(diff.z);
				RHS = boxExtents.z + segment.Extent * AWdU2;
				if (ADdU2 > RHS)
				{
					return false;
				}

				Vector3 WxD = segment.Direction.Cross(diff);

				float AWxDdU0 = Mathf.Abs(WxD.x);
				RHS = boxExtents.y * AWdU2 + boxExtents.z * AWdU1;
				if (AWxDdU0 > RHS)
				{
					return false;
				}

				float AWxDdU1 = Mathf.Abs(WxD.y);
				RHS = boxExtents.x * AWdU2 + boxExtents.z * AWdU0;
				if (AWxDdU1 > RHS)
				{
					return false;
				}

				float AWxDdU2 = Mathf.Abs(WxD.z);
				RHS = boxExtents.x * AWdU1 + boxExtents.y * AWdU0;
				if (AWxDdU2 > RHS)
				{
					return false;
				}

				return true;
			}

			/// <summary>
			/// Tests if a segment intersects an axis aligned box and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindSegment3AAB3(ref Segment3 segment, ref AAB3 box, out Segment3AAB3Intr info)
			{
				return DoClipping(
					-segment.Extent, segment.Extent,
					ref segment.Center, ref segment.Direction, ref box, true,
					out info.Quantity, out info.Point0, out info.Point1, out info.IntersectionType);
			}
		}
	}
}
