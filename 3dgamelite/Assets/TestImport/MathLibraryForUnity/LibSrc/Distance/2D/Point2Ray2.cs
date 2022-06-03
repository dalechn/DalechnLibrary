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
			public static float Point2Ray2(ref Vector2 point, ref Ray2 ray)
			{
				return Mathf.Sqrt(SqrPoint2Ray2(ref point, ref ray));
			}

			/// <summary>
			/// Returns distance between a point and a ray
			/// </summary>
			/// <param name="closestPoint">Point projected on a ray and clamped by ray origin</param>
			public static float Point2Ray2(ref Vector2 point, ref Ray2 ray, out Vector2 closestPoint)
			{
				return Mathf.Sqrt(SqrPoint2Ray2(ref point, ref ray, out closestPoint));
			}


			/// <summary>
			/// Returns squared distance between a point and a ray
			/// </summary>
			public static float SqrPoint2Ray2(ref Vector2 point, ref Ray2 ray)
			{
				Vector2 diff = point - ray.Center;
				float param = ray.Direction.Dot(diff);
				Vector2 closestPoint;
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
			public static float SqrPoint2Ray2(ref Vector2 point, ref Ray2 ray, out Vector2 closestPoint)
			{
				Vector2 diff = point - ray.Center;
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
