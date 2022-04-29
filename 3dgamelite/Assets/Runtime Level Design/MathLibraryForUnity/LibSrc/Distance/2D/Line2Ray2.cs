using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a line and a ray
			/// </summary>
			public static float Line2Ray2(ref Line2 line, ref Ray2 ray)
			{
				return Mathf.Sqrt(SqrLine2Ray2(ref line, ref ray));
			}

			/// <summary>
			/// Returns distance between a line and a ray
			/// </summary>
			/// <param name="closestPoint0">Point on line closest to ray</param>
			/// <param name="closestPoint1">Point on ray closest to line</param>
			public static float Line2Ray2(ref Line2 line, ref Ray2 ray, out Vector2 closestPoint0, out Vector2 closestPoint1)
			{
				return Mathf.Sqrt(SqrLine2Ray2(ref line, ref ray, out closestPoint0, out closestPoint1));
			}


			/// <summary>
			/// Returns squared distance between a line and a ray
			/// </summary>
			public static float SqrLine2Ray2(ref Line2 line, ref Ray2 ray)
			{
				Vector2 diff = line.Center - ray.Center;
				float a01 = -line.Direction.Dot(ray.Direction);
				float b0 = diff.Dot(line.Direction);
				float c = diff.sqrMagnitude;
				float det = Mathf.Abs(1f - a01 * a01);
				float b1, s0, s1, sqrDist;

				if (det >= Mathfex.ZeroTolerance)
				{
					b1 = -diff.Dot(ray.Direction);
					s1 = a01 * b0 - b1;

					if (s1 >= 0f)
					{
						// Two interior points are closest, one on line and one on ray.
						sqrDist = 0f;
					}
					else
					{
						// Origin of ray and interior point of line are closest.
						s0 = -b0;
						s1 = 0f;
						sqrDist = b0 * s0 + c;

						// Account for numerical round-off errors.
						if (sqrDist < 0f)
						{
							sqrDist = 0f;
						}
					}
				}
				else
				{
					// Lines are parallel, closest pair with one point at ray origin.
					s0 = -b0;
					s1 = (float)0;
					sqrDist = b0 * s0 + c;

					// Account for numerical round-off errors.
					if (sqrDist < 0f)
					{
						sqrDist = 0f;
					}
				}

				return sqrDist;
			}

			/// <summary>
			/// Returns squared distance between a line and a ray
			/// </summary>
			/// <param name="closestPoint0">Point on line closest to ray</param>
			/// <param name="closestPoint1">Point on ray closest to line</param>
			public static float SqrLine2Ray2(ref Line2 line, ref Ray2 ray, out Vector2 closestPoint0, out Vector2 closestPoint1)
			{
				Vector2 diff = line.Center - ray.Center;
				float a01 = -line.Direction.Dot(ray.Direction);
				float b0 = diff.Dot(line.Direction);
				float c = diff.sqrMagnitude;
				float det = Mathf.Abs(1f - a01 * a01);
				float b1, s0, s1, sqrDist;

				if (det >= Mathfex.ZeroTolerance)
				{
					b1 = -diff.Dot(ray.Direction);
					s1 = a01 * b0 - b1;

					if (s1 >= 0f)
					{
						// Two interior points are closest, one on line and one on ray.
						float invDet = 1f / det;
						s0 = (a01 * b1 - b0) * invDet;
						s1 *= invDet;
						sqrDist = 0f;
					}
					else
					{
						// Origin of ray and interior point of line are closest.
						s0 = -b0;
						s1 = 0f;
						sqrDist = b0 * s0 + c;

						// Account for numerical round-off errors.
						if (sqrDist < 0f)
						{
							sqrDist = 0f;
						}
					}
				}
				else
				{
					// Lines are parallel, closest pair with one point at ray origin.
					s0 = -b0;
					s1 = (float)0;
					sqrDist = b0 * s0 + c;

					// Account for numerical round-off errors.
					if (sqrDist < 0f)
					{
						sqrDist = 0f;
					}
				}

				closestPoint0 = line.Center + s0 * line.Direction;
				closestPoint1 = ray.Center + s1 * ray.Direction;
				return sqrDist;
			}
		}
	}
}