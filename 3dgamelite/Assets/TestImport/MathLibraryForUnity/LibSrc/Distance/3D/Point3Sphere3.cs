using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and a sphere
			/// </summary>
			public static float Point3Sphere3(ref Vector3 point, ref Sphere3 sphere)
			{
				float diff = (point - sphere.Center).magnitude - sphere.Radius;
				return diff > 0f ? diff : 0f;
			}

			/// <summary>
			/// Returns distance between a point and a sphere
			/// </summary>
			/// <param name="closestPoint">Point projected on a sphere</param>
			public static float Point3Sphere3(ref Vector3 point, ref Sphere3 sphere, out Vector3 closestPoint)
			{
				Vector3 diff = point - sphere.Center;
				float diffSqrLen = diff.sqrMagnitude;
				if (diffSqrLen > sphere.Radius * sphere.Radius)
				{
					float diffLen = Mathf.Sqrt(diffSqrLen);
					closestPoint = sphere.Center + diff * (sphere.Radius / diffLen);
					return diffLen - sphere.Radius;
				}
				closestPoint = point;
				return 0f;
			}


			/// <summary>
			/// Returns squared distance between a point and a sphere
			/// </summary>
			public static float SqrPoint3Sphere3(ref Vector3 point, ref Sphere3 sphere)
			{
				float diff = (point - sphere.Center).magnitude - sphere.Radius;
				return diff > 0f ? diff * diff : 0f;
			}

			/// <summary>
			/// Returns squared distance between a point and a sphere
			/// </summary>
			/// <param name="closestPoint">Point projected on a sphere</param>
			public static float SqrPoint3Sphere3(ref Vector3 point, ref Sphere3 sphere, out Vector3 closestPoint)
			{
				Vector3 diff = point - sphere.Center;
				float diffSqrLen = diff.sqrMagnitude;
				if (diffSqrLen > sphere.Radius * sphere.Radius)
				{
					float diffLen = Mathf.Sqrt(diffSqrLen);
					closestPoint = sphere.Center + diff * (sphere.Radius / diffLen);
					float result = diffLen - sphere.Radius;
					return result * result;
				}
				closestPoint = point;
				return 0f;
			}
		}
	}
}