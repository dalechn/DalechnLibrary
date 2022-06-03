using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a plane intersects a box. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestPlane3Box3(ref Plane3 plane, ref Box3 box)
			{
				float tmp0 = box.Extents.x * (plane.Normal.Dot(box.Axis0));
				float tmp1 = box.Extents.y * (plane.Normal.Dot(box.Axis1));
				float tmp2 = box.Extents.z * (plane.Normal.Dot(box.Axis2));

				float radius = Mathf.Abs(tmp0) + Mathf.Abs(tmp1) + Mathf.Abs(tmp2);

				float signedDistance = plane.SignedDistanceTo(ref box.Center);
				return Mathf.Abs(signedDistance) <= radius;
			}
		}
	}
}
