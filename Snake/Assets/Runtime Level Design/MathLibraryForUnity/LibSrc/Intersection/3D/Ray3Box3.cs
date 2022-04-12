using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Ray3 and Box3
		/// </summary>
		public struct Ray3Box3Intr
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
			/// Tests if a ray intersects a box. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestRay3Box3(ref Ray3 ray, ref Box3 box)
			{
				float RHS;
				Vector3 diff = ray.Center - box.Center;

				float WdU0 = ray.Direction.Dot(box.Axis0);
				float AWdU0 = Mathf.Abs(WdU0);
				float DdU0 = diff.Dot(box.Axis0);
				float ADdU0 = Mathf.Abs(DdU0);
				if (ADdU0 > box.Extents.x && DdU0 * WdU0 >= 0.0f)
				{
					return false;
				}

				float WdU1 = ray.Direction.Dot(box.Axis1);
				float AWdU1 = Mathf.Abs(WdU1);
				float DdU1 = diff.Dot(box.Axis1);
				float ADdU1 = Mathf.Abs(DdU1);
				if (ADdU1 > box.Extents.y && DdU1 * WdU1 >= 0.0f)
				{
					return false;
				}

				float WdU2 = ray.Direction.Dot(box.Axis2);
				float AWdU2 = Mathf.Abs(WdU2);
				float DdU2 = diff.Dot(box.Axis2);
				float ADdU2 = Mathf.Abs(DdU2);
				if (ADdU2 > box.Extents.z && DdU2 * WdU2 >= 0.0f)
				{
					return false;
				}

				Vector3 WxD = ray.Direction.Cross(diff);

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
			/// Tests if a ray intersects a box and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindRay3Box3(ref Ray3 ray, ref Box3 box, out Ray3Box3Intr info)
			{
				return DoClipping(
					0.0f, float.PositiveInfinity,
					ref ray.Center, ref ray.Direction, ref box, true,
					out info.Quantity, out info.Point0, out info.Point1, out info.IntersectionType);
			}
		}
	}
}
