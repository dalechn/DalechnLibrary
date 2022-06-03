using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and a rectangle
			/// </summary>
			public static float Point3Rectangle3(ref Vector3 point, ref Rectangle3 rectangle)
			{
				return Mathf.Sqrt(SqrPoint3Rectangle3(ref point, ref rectangle));
			}

			/// <summary>
			/// Returns distance between a point and a rectangle
			/// </summary>
			/// <param name="closestPoint">Point projected on a rectangle</param>
			public static float Point3Rectangle3(ref Vector3 point, ref Rectangle3 rectangle, out Vector3 closestPoint)
			{
				return Mathf.Sqrt(SqrPoint3Rectangle3(ref point, ref rectangle, out closestPoint));
			}


			/// <summary>
			/// Returns squared distance between a point and a rectangle
			/// </summary>
			public static float SqrPoint3Rectangle3(ref Vector3 point, ref Rectangle3 rectangle)
			{
				Vector3 diff = rectangle.Center - point;
				float b0 = diff.Dot(rectangle.Axis0);
				float b1 = diff.Dot(rectangle.Axis1);
				float s0 = -b0, s1 = -b1;
				float sqrDistance = diff.sqrMagnitude;
				float extent;

				extent = rectangle.Extents.x;
				if (s0 < -extent)
				{
					s0 = -extent;
				}
				else if (s0 > extent)
				{
					s0 = extent;
				}
				sqrDistance += s0 * (s0 + 2f * b0);

				extent = rectangle.Extents.y;
				if (s1 < -extent)
				{
					s1 = -extent;
				}
				else if (s1 > extent)
				{
					s1 = extent;
				}
				sqrDistance += s1 * (s1 + 2f * b1);

				// Account for numerical round-off error.
				if (sqrDistance < 0f)
				{
					sqrDistance = 0f;
				}

				return sqrDistance;
			}

			/// <summary>
			/// Returns squared distance between a point and a rectangle
			/// </summary>
			/// <param name="closestPoint">Point projected on a rectangle</param>
			public static float SqrPoint3Rectangle3(ref Vector3 point, ref Rectangle3 rectangle, out Vector3 closestPoint)
			{
				Vector3 diff = rectangle.Center - point;
				float b0 = diff.Dot(rectangle.Axis0);
				float b1 = diff.Dot(rectangle.Axis1);
				float s0 = -b0, s1 = -b1;
				float sqrDistance = diff.sqrMagnitude;
				float extent;

				extent = rectangle.Extents.x;
				if (s0 < -extent)
				{
					s0 = -extent;
				}
				else if (s0 > extent)
				{
					s0 = extent;
				}
				sqrDistance += s0 * (s0 + 2f * b0);

				extent = rectangle.Extents.y;
				if (s1 < -extent)
				{
					s1 = -extent;
				}
				else if (s1 > extent)
				{
					s1 = extent;
				}
				sqrDistance += s1 * (s1 + 2f * b1);

				// Account for numerical round-off error.
				if (sqrDistance < 0f)
				{
					sqrDistance = 0f;
				}

				closestPoint = rectangle.Center + s0 * rectangle.Axis0 + s1 * rectangle.Axis1;

				return sqrDistance;
			}
		}
	}
}
