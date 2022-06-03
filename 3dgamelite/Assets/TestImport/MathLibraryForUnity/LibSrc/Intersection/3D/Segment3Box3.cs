using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Segment3 and Box3
		/// </summary>
		public struct Segment3Box3Intr
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
			/// Tests if a segment intersects a box. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment3Box3(ref Segment3 segment, ref Box3 box)
			{
				float RHS;
				Vector3 diff = segment.Center - box.Center;

				float AWdU0 = Mathf.Abs(segment.Direction.Dot(box.Axis0));
				float ADdU0 = Mathf.Abs(diff.Dot(box.Axis0));
				RHS = box.Extents.x + segment.Extent * AWdU0;
				if (ADdU0 > RHS)
				{
					return false;
				}

				float AWdU1 = Mathf.Abs(segment.Direction.Dot(box.Axis1));
				float ADdU1 = Mathf.Abs(diff.Dot(box.Axis1));
				RHS = box.Extents.y + segment.Extent * AWdU1;
				if (ADdU1 > RHS)
				{
					return false;
				}

				float AWdU2 = Mathf.Abs(segment.Direction.Dot(box.Axis2));
				float ADdU2 = Mathf.Abs(diff.Dot(box.Axis2));
				RHS = box.Extents.z + segment.Extent * AWdU2;
				if (ADdU2 > RHS)
				{
					return false;
				}

				Vector3 WxD = segment.Direction.Cross(diff);

				float AWxDdU0 = Mathf.Abs(WxD.Dot(box.Axis0));
				RHS = box.Extents.y * AWdU2 + box.Extents.z * AWdU1;
				if (AWxDdU0 > RHS)
				{
					return false;
				}

				float AWxDdU1 = Mathf.Abs(WxD.Dot(box.Axis1));
				RHS = box.Extents.x * AWdU2 + box.Extents.z * AWdU0;
				if (AWxDdU1 > RHS)
				{
					return false;
				}

				float AWxDdU2 = Mathf.Abs(WxD.Dot(box.Axis2));
				RHS = box.Extents.x * AWdU1 + box.Extents.y * AWdU0;
				if (AWxDdU2 > RHS)
				{
					return false;
				}

				return true;
			}

			/// <summary>
			/// Tests if a segment intersects a box and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindSegment3Box3(ref Segment3 segment, ref Box3 box, out Segment3Box3Intr info)
			{
				return DoClipping(
					-segment.Extent, segment.Extent,
					ref segment.Center, ref segment.Direction, ref box, true,
					out info.Quantity, out info.Point0, out info.Point1, out info.IntersectionType);
			}
		}
	}
}
