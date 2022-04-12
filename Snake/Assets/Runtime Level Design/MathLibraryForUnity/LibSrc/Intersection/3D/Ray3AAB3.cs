using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Ray3 and AxisAlignedBox3
		/// </summary>
		public struct Ray3AAB3Intr
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
			/// Tests if a ray intersects an axis aligned box. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestRay3AAB3(ref Ray3 ray, ref AAB3 box)
			{
				Vector3 boxCenter;
				Vector3 boxExtents;
				box.CalcCenterExtents(out boxCenter, out boxExtents);

				float RHS;
				Vector3 diff = ray.Center - boxCenter;

				float WdU0 = ray.Direction.x;
				float AWdU0 = Mathf.Abs(WdU0);
				float DdU0 = diff.x;
				float ADdU0 = Mathf.Abs(DdU0);
				if (ADdU0 > boxExtents.x && DdU0 * WdU0 >= 0.0f)
				{
					return false;
				}

				float WdU1 = ray.Direction.y;
				float AWdU1 = Mathf.Abs(WdU1);
				float DdU1 = diff.y;
				float ADdU1 = Mathf.Abs(DdU1);
				if (ADdU1 > boxExtents.y && DdU1 * WdU1 >= 0.0f)
				{
					return false;
				}

				float WdU2 = ray.Direction.z;
				float AWdU2 = Mathf.Abs(WdU2);
				float DdU2 = diff.z;
				float ADdU2 = Mathf.Abs(DdU2);
				if (ADdU2 > boxExtents.z && DdU2 * WdU2 >= 0.0f)
				{
					return false;
				}

				Vector3 WxD = ray.Direction.Cross(diff);

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
			/// Tests if a ray intersects an axis aligned box and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindRay3AAB3(ref Ray3 ray, ref AAB3 box, out Ray3AAB3Intr info)
			{
				return DoClipping(
					0.0f, float.PositiveInfinity,
					ref ray.Center, ref ray.Direction, ref box, true,
					out info.Quantity, out info.Point0, out info.Point1, out info.IntersectionType);
			}
		}
	}
}
