using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and a box
			/// </summary>
			public static float Point3Box3(ref Vector3 point, ref Box3 box)
			{
				return Mathf.Sqrt(SqrPoint3Box3(ref point, ref box));
			}

			/// <summary>
			/// Returns distance between a point and a box
			/// </summary>
			/// <param name="closestPoint">Point projected on a box</param>
			public static float Point3Box3(ref Vector3 point, ref Box3 box, out Vector3 closestPoint)
			{
				return Mathf.Sqrt(SqrPoint3Box3(ref point, ref box, out closestPoint));
			}


			/// <summary>
			/// Returns squared distance between a point and a box
			/// </summary>
			public static float SqrPoint3Box3(ref Vector3 point, ref Box3 box)
			{
				// Work in the box's coordinate system.
				Vector3 diff = point - box.Center;

				// Compute squared distance and closest point on box.
				float distSquared = 0.0f;
				float delta;
				float extent;

				float closest0 = diff.Dot(box.Axis0);
				extent = box.Extents.x;
				if (closest0 < -extent)
				{
					delta = closest0 + extent;
					distSquared += delta * delta;
				}
				else if (closest0 > extent)
				{
					delta = closest0 - extent;
					distSquared += delta * delta;
				}

				float closest1 = diff.Dot(box.Axis1);
				extent = box.Extents.y;
				if (closest1 < -extent)
				{
					delta = closest1 + extent;
					distSquared += delta * delta;
				}
				else if (closest1 > extent)
				{
					delta = closest1 - extent;
					distSquared += delta * delta;
				}

				float closest2 = diff.Dot(box.Axis2);
				extent = box.Extents.z;
				if (closest2 < -extent)
				{
					delta = closest2 + extent;
					distSquared += delta * delta;
				}
				else if (closest2 > extent)
				{
					delta = closest2 - extent;
					distSquared += delta * delta;
				}

				return distSquared;
			}

			/// <summary>
			/// Returns squared distance between a point and a box
			/// </summary>
			/// <param name="closestPoint">Point projected on a box</param>
			public static float SqrPoint3Box3(ref Vector3 point, ref Box3 box, out Vector3 closestPoint)
			{
				// Work in the box's coordinate system.
				Vector3 diff = point - box.Center;

				// Compute squared distance and closest point on box.
				float distSquared = 0.0f;
				float delta;
				float extent;

				float closest0 = diff.Dot(box.Axis0);
				extent = box.Extents.x;
				if (closest0 < -extent)
				{
					delta = closest0 + extent;
					distSquared += delta * delta;
					closest0 = -extent;
				}
				else if (closest0 > extent)
				{
					delta = closest0 - extent;
					distSquared += delta * delta;
					closest0 = extent;
				}

				float closest1 = diff.Dot(box.Axis1);
				extent = box.Extents.y;
				if (closest1 < -extent)
				{
					delta = closest1 + extent;
					distSquared += delta * delta;
					closest1 = -extent;
				}
				else if (closest1 > extent)
				{
					delta = closest1 - extent;
					distSquared += delta * delta;
					closest1 = extent;
				}

				float closest2 = diff.Dot(box.Axis2);
				extent = box.Extents.z;
				if (closest2 < -extent)
				{
					delta = closest2 + extent;
					distSquared += delta * delta;
					closest2 = -extent;
				}
				else if (closest2 > extent)
				{
					delta = closest2 - extent;
					distSquared += delta * delta;
					closest2 = extent;
				}

				closestPoint = box.Center + closest0 * box.Axis0 + closest1 * box.Axis1 + closest2 * box.Axis2;

				return distSquared;
			}
		}
	}
}
