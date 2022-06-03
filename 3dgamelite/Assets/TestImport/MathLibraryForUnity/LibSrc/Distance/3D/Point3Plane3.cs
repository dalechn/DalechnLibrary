using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and a plane
			/// </summary>
			public static float Point3Plane3(ref Vector3 point, ref Plane3 plane)
			{
				float signedDistance = plane.Normal.Dot(point) - plane.Constant;
				return Mathf.Abs(signedDistance);
			}

			/// <summary>
			/// Returns distance between a point and a plane
			/// </summary>
			/// <param name="closestPoint">Point projected on a plane</param>
			public static float Point3Plane3(ref Vector3 point, ref Plane3 plane, out Vector3 closestPoint)
			{
				float signedDistance = plane.Normal.Dot(point) - plane.Constant;
				closestPoint = point - signedDistance * plane.Normal;
				return Mathf.Abs(signedDistance);
			}


			/// <summary>
			/// Returns squared distance between a point and a plane
			/// </summary>
			public static float SqrPoint3Plane3(ref Vector3 point, ref Plane3 plane)
			{
				float signedDistance = plane.Normal.Dot(point) - plane.Constant;
				return signedDistance * signedDistance;
			}

			/// <summary>
			/// Returns squared distance between a point and a plane
			/// </summary>
			/// <param name="closestPoint">Point projected on a plane</param>
			public static float SqrPoint3Plane3(ref Vector3 point, ref Plane3 plane, out Vector3 closestPoint)
			{
				float signedDistance = plane.Normal.Dot(point) - plane.Constant;
				closestPoint = point - signedDistance * plane.Normal;
				return signedDistance * signedDistance;
			}
		}
	}
}
