using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and an abb
			/// </summary>
			public static float Point3AAB3(ref Vector3 point, ref AAB3 box)
			{
				// Compute squared distance and closest point on box.
				float distSquared = 0.0f;
				float pointCoord;
				float delta;

				pointCoord = point.x;
				if (pointCoord < box.Min.x)
				{
					delta = box.Min.x - pointCoord;
					distSquared += delta * delta;
				}
				else if (pointCoord > box.Max.x)
				{
					delta = pointCoord - box.Max.x;
					distSquared += delta * delta;
				}

				pointCoord = point.y;
				if (pointCoord < box.Min.y)
				{
					delta = box.Min.y - pointCoord;
					distSquared += delta * delta;
				}
				else if (pointCoord > box.Max.y)
				{
					delta = pointCoord - box.Max.y;
					distSquared += delta * delta;
				}

				pointCoord = point.z;
				if (pointCoord < box.Min.z)
				{
					delta = box.Min.z - pointCoord;
					distSquared += delta * delta;
				}
				else if (pointCoord > box.Max.z)
				{
					delta = pointCoord - box.Max.z;
					distSquared += delta * delta;
				}

				return Mathf.Sqrt(distSquared);
			}

			/// <summary>
			/// Returns distance between a point and an abb
			/// </summary>
			/// <param name="closestPoint">Point projected on an aab</param>
			public static float Point3AAB3(ref Vector3 point, ref AAB3 box, out Vector3 closestPoint)
			{
				// Compute squared distance and closest point on box.
				float distSquared = 0.0f;
				float pointCoord;
				float delta;

				closestPoint = point;

				pointCoord = point.x;
				if (pointCoord < box.Min.x)
				{
					delta = box.Min.x - pointCoord;
					distSquared += delta * delta;
					closestPoint.x += delta;
				}
				else if (pointCoord > box.Max.x)
				{
					delta = pointCoord - box.Max.x;
					distSquared += delta * delta;
					closestPoint.x -= delta;
				}

				pointCoord = point.y;
				if (pointCoord < box.Min.y)
				{
					delta = box.Min.y - pointCoord;
					distSquared += delta * delta;
					closestPoint.y += delta;
				}
				else if (pointCoord > box.Max.y)
				{
					delta = pointCoord - box.Max.y;
					distSquared += delta * delta;
					closestPoint.y -= delta;
				}

				pointCoord = point.z;
				if (pointCoord < box.Min.z)
				{
					delta = box.Min.z - pointCoord;
					distSquared += delta * delta;
					closestPoint.z += delta;
				}
				else if (pointCoord > box.Max.z)
				{
					delta = pointCoord - box.Max.z;
					distSquared += delta * delta;
					closestPoint.z -= delta;
				}

				return Mathf.Sqrt(distSquared);
			}


			/// <summary>
			/// Returns squared distance between a point and an abb
			/// </summary>
			public static float SqrPoint3AAB3(ref Vector3 point, ref AAB3 box)
			{
				// Compute squared distance and closest point on box.
				float distSquared = 0.0f;
				float pointCoord;
				float delta;

				pointCoord = point.x;
				if (pointCoord < box.Min.x)
				{
					delta = box.Min.x - pointCoord;
					distSquared += delta * delta;
				}
				else if (pointCoord > box.Max.x)
				{
					delta = pointCoord - box.Max.x;
					distSquared += delta * delta;
				}

				pointCoord = point.y;
				if (pointCoord < box.Min.y)
				{
					delta = box.Min.y - pointCoord;
					distSquared += delta * delta;
				}
				else if (pointCoord > box.Max.y)
				{
					delta = pointCoord - box.Max.y;
					distSquared += delta * delta;
				}

				pointCoord = point.z;
				if (pointCoord < box.Min.z)
				{
					delta = box.Min.z - pointCoord;
					distSquared += delta * delta;
				}
				else if (pointCoord > box.Max.z)
				{
					delta = pointCoord - box.Max.z;
					distSquared += delta * delta;
				}

				return distSquared;
			}

			/// <summary>
			/// Returns squared distance between a point and an abb
			/// </summary>
			/// <param name="closestPoint">Point projected on an aab</param>
			public static float SqrPoint3AAB3(ref Vector3 point, ref AAB3 box, out Vector3 closestPoint)
			{
				// Compute squared distance and closest point on box.
				float distSquared = 0.0f;
				float pointCoord;
				float delta;

				closestPoint = point;

				pointCoord = point.x;
				if (pointCoord < box.Min.x)
				{
					delta = box.Min.x - pointCoord;
					distSquared += delta * delta;
					closestPoint.x += delta;
				}
				else if (pointCoord > box.Max.x)
				{
					delta = pointCoord - box.Max.x;
					distSquared += delta * delta;
					closestPoint.x -= delta;
				}

				pointCoord = point.y;
				if (pointCoord < box.Min.y)
				{
					delta = box.Min.y - pointCoord;
					distSquared += delta * delta;
					closestPoint.y += delta;
				}
				else if (pointCoord > box.Max.y)
				{
					delta = pointCoord - box.Max.y;
					distSquared += delta * delta;
					closestPoint.y -= delta;
				}

				pointCoord = point.z;
				if (pointCoord < box.Min.z)
				{
					delta = box.Min.z - pointCoord;
					distSquared += delta * delta;
					closestPoint.z += delta;
				}
				else if (pointCoord > box.Max.z)
				{
					delta = pointCoord - box.Max.z;
					distSquared += delta * delta;
					closestPoint.z -= delta;
				}

				return distSquared;
			}
		}
	}
}
