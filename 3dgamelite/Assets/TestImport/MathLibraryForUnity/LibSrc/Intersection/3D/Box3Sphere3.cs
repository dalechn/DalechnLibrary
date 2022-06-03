using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a box intersects a sphere. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestBox3Sphere3(ref Box3 box, ref Sphere3 sphere)
			{
				float distSquared = 0f;
				float delta;
				float proj;
				float extent;

				Vector3 diff = sphere.Center - box.Center;

				proj = diff.Dot(box.Axis0);
				extent = box.Extents.x;
				if (proj < -extent)
				{
					delta = proj + extent;
					distSquared += delta * delta;
				}
				else if (proj > extent)
				{
					delta = proj - extent;
					distSquared += delta * delta;
				}

				proj = diff.Dot(box.Axis1);
				extent = box.Extents.y;
				if (proj < -extent)
				{
					delta = proj + extent;
					distSquared += delta * delta;
				}
				else if (proj > extent)
				{
					delta = proj - extent;
					distSquared += delta * delta;
				}

				proj = diff.Dot(box.Axis2);
				extent = box.Extents.z;
				if (proj < -extent)
				{
					delta = proj + extent;
					distSquared += delta * delta;
				}
				else if (proj > extent)
				{
					delta = proj - extent;
					distSquared += delta * delta;
				}

				return distSquared <= sphere.Radius * sphere.Radius;
			}
		}
	}
}
