using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between two rays
			/// </summary>
			public static float Ray3Ray3(ref Ray3 ray0, ref Ray3 ray1)
			{
				Vector3 closestPoint0, closestPoint1;
				return Mathf.Sqrt(SqrRay3Ray3(ref ray0, ref ray1, out closestPoint0, out closestPoint1));
			}

			/// <summary>
			/// Returns distance between two rays
			/// </summary>
			/// <param name="closestPoint0">Point on ray0 closest to ray1</param>
			/// <param name="closestPoint1">Point on ray1 closest to ray0</param>
			public static float Ray3Ray3(ref Ray3 ray0, ref Ray3 ray1, out Vector3 closestPoint0, out Vector3 closestPoint1)
			{
				return Mathf.Sqrt(SqrRay3Ray3(ref ray0, ref ray1, out closestPoint0, out closestPoint1));
			}


			/// <summary>
			/// Returns squared distance between two rays
			/// </summary>
			public static float SqrRay3Ray3(ref Ray3 ray0, ref Ray3 ray1)
			{
				Vector3 closestPoint0, closestPoint1;
				return SqrRay3Ray3(ref ray0, ref ray1, out closestPoint0, out closestPoint1);
			}

			/// <summary>
			/// Returns squared distance between two rays
			/// </summary>
			/// <param name="closestPoint0">Point on ray0 closest to ray1</param>
			/// <param name="closestPoint1">Point on ray1 closest to ray0</param>
			public static float SqrRay3Ray3(ref Ray3 ray0, ref Ray3 ray1, out Vector3 closestPoint0, out Vector3 closestPoint1)
			{
				Vector3 diff = ray0.Center - ray1.Center;
				float a01 = -ray0.Direction.Dot(ray1.Direction);
				float b0 = diff.Dot(ray0.Direction);
				float c = diff.sqrMagnitude;
				float det = Mathf.Abs((float)1 - a01 * a01);
				float b1, s0, s1, sqrDist;

				if (det >= Mathfex.ZeroTolerance)
				{
					// Rays are not parallel.
					b1 = -diff.Dot(ray1.Direction);
					s0 = a01 * b1 - b0;
					s1 = a01 * b0 - b1;

					if (s0 >= (float)0)
					{
						if (s1 >= (float)0)  // region 0 (interior)
						{
							// Minimum at two interior points of rays.
							float invDet = ((float)1) / det;
							s0 *= invDet;
							s1 *= invDet;
							sqrDist = s0 * (s0 + a01 * s1 + ((float)2) * b0) +
								s1 * (a01 * s0 + s1 + ((float)2) * b1) + c;
						}
						else  // region 3 (side)
						{
							s1 = (float)0;
							if (b0 >= (float)0)
							{
								s0 = (float)0;
								sqrDist = c;
							}
							else
							{
								s0 = -b0;
								sqrDist = b0 * s0 + c;
							}
						}
					}
					else
					{
						if (s1 >= (float)0)  // region 1 (side)
						{
							s0 = (float)0;
							if (b1 >= (float)0)
							{
								s1 = (float)0;
								sqrDist = c;
							}
							else
							{
								s1 = -b1;
								sqrDist = b1 * s1 + c;
							}
						}
						else  // region 2 (corner)
						{
							if (b0 < (float)0)
							{
								s0 = -b0;
								s1 = (float)0;
								sqrDist = b0 * s0 + c;
							}
							else
							{
								s0 = (float)0;
								if (b1 >= (float)0)
								{
									s1 = (float)0;
									sqrDist = c;
								}
								else
								{
									s1 = -b1;
									sqrDist = b1 * s1 + c;
								}
							}
						}
					}
				}
				else
				{
					// Rays are parallel.
					if (a01 > (float)0)
					{
						// Opposite direction vectors.
						s1 = (float)0;
						if (b0 >= (float)0)
						{
							s0 = (float)0;
							sqrDist = c;
						}
						else
						{
							s0 = -b0;
							sqrDist = b0 * s0 + c;
						}
					}
					else
					{
						// Same direction vectors.
						if (b0 >= (float)0)
						{
							b1 = -diff.Dot(ray1.Direction);
							s0 = (float)0;
							s1 = -b1;
							sqrDist = b1 * s1 + c;
						}
						else
						{
							s0 = -b0;
							s1 = (float)0;
							sqrDist = b0 * s0 + c;
						}
					}
				}

				closestPoint0 = ray0.Center + s0 * ray0.Direction;
				closestPoint1 = ray1.Center + s1 * ray1.Direction;

				// Account for numerical round-off errors.
				if (sqrDist < (float)0)
				{
					sqrDist = (float)0;
				}
				return sqrDist;
			}
		}
	}
}