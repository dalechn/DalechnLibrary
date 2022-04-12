using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and a ray
			/// </summary>
			public static float Point3Ray3(ref Vector3 point, ref Ray3 ray)
			{
				return Mathf.Sqrt(SqrPoint3Ray3(ref point, ref ray));
			}

			/// <summary>
			/// Returns distance between a point and a ray
			/// </summary>
			/// <param name="closestPoint">Point projected on a ray and clamped by ray origin</param>
			public static float Point3Ray3(ref Vector3 point, ref Ray3 ray, out Vector3 closestPoint)
			{
				return Mathf.Sqrt(SqrPoint3Ray3(ref point, ref ray, out closestPoint));
			}


			/// <summary>
			/// Returns squared distance between a point and a ray
			/// </summary>
			public static float SqrPoint3Ray3(ref Vector3 point, ref Ray3 ray)
			{
				Vector3 diff = point - ray.Center;
				float param = ray.Direction.Dot(diff);
				Vector3 closestPoint;
				if (param > 0.0f)
				{
					closestPoint = ray.Center + param * ray.Direction;
				}
				else
				{
					closestPoint = ray.Center;
				}
				diff = closestPoint - point;
				return diff.sqrMagnitude;
			}

			/// <summary>
			/// Returns squared distance between a point and a ray
			/// </summary>
			/// <param name="closestPoint">Point projected on a ray and clamped by ray origin</param>
			public static float SqrPoint3Ray3(ref Vector3 point, ref Ray3 ray, out Vector3 closestPoint)
			{
				Vector3 diff = point - ray.Center;
				float param = ray.Direction.Dot(diff);
				if (param > 0.0f)
				{
					closestPoint = ray.Center + param * ray.Direction;
				}
				else
				{
					closestPoint = ray.Center;
				}
				diff = closestPoint - point;
				return diff.sqrMagnitude;
			}
		}
	}
}