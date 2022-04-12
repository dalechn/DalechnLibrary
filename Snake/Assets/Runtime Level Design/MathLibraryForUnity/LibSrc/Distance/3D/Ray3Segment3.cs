using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a ray and a segment
			/// </summary>
			public static float Ray3Segment3(ref Ray3 ray, ref Segment3 segment)
			{
				Vector3 closestPoint0, closestPoint1;
				return Mathf.Sqrt(SqrRay3Segment3(ref ray, ref segment, out closestPoint0, out closestPoint1));
			}

			/// <summary>
			/// Returns distance between a ray and a segment
			/// </summary>
			/// <param name="closestPoint0">Point on ray closest to segment</param>
			/// <param name="closestPoint1">Point on segment closest to ray</param>
			public static float Ray3Segment3(ref Ray3 ray, ref Segment3 segment, out Vector3 closestPoint0, out Vector3 closestPoint1)
			{
				return Mathf.Sqrt(SqrRay3Segment3(ref ray, ref segment, out closestPoint0, out closestPoint1));
			}


			/// <summary>
			/// Returns squared distance between a ray and a segment
			/// </summary>
			public static float SqrRay3Segment3(ref Ray3 ray, ref Segment3 segment)
			{
				Vector3 closestPoint0, closestPoint1;
				return SqrRay3Segment3(ref ray, ref segment, out closestPoint0, out closestPoint1);
			}

			/// <summary>
			/// Returns squared distance between a ray and a segment
			/// </summary>
			/// <param name="closestPoint0">Point on ray closest to segment</param>
			/// <param name="closestPoint1">Point on segment closest to ray</param>
			public static float SqrRay3Segment3(ref Ray3 ray, ref Segment3 segment, out Vector3 closestPoint0, out Vector3 closestPoint1)
			{
				Vector3 diff = ray.Center - segment.Center;
				float a01 = -ray.Direction.Dot(segment.Direction);
				float b0 = diff.Dot(ray.Direction);
				float b1 = -diff.Dot(segment.Direction);
				float c = diff.sqrMagnitude;
				float det = Mathf.Abs((float)1 - a01 * a01);
				float s0, s1, sqrDist, extDet;

				if (det >= Mathfex.ZeroTolerance)
				{
					// The ray and segment are not parallel.
					s0 = a01 * b1 - b0;
					s1 = a01 * b0 - b1;
					extDet = segment.Extent * det;

					if (s0 >= (float)0)
					{
						if (s1 >= -extDet)
						{
							if (s1 <= extDet)  // region 0
							{
								// Minimum at interior points of ray and segment.
								float invDet = ((float)1) / det;
								s0 *= invDet;
								s1 *= invDet;
								sqrDist = s0 * (s0 + a01 * s1 + ((float)2) * b0) +
									s1 * (a01 * s0 + s1 + ((float)2) * b1) + c;
							}
							else  // region 1
							{
								s1 = segment.Extent;
								s0 = -(a01 * s1 + b0);
								if (s0 > (float)0)
								{
									sqrDist = -s0 * s0 + s1 * (s1 + ((float)2) * b1) + c;
								}
								else
								{
									s0 = (float)0;
									sqrDist = s1 * (s1 + ((float)2) * b1) + c;
								}
							}
						}
						else  // region 5
						{
							s1 = -segment.Extent;
							s0 = -(a01 * s1 + b0);
							if (s0 > (float)0)
							{
								sqrDist = -s0 * s0 + s1 * (s1 + ((float)2) * b1) + c;
							}
							else
							{
								s0 = (float)0;
								sqrDist = s1 * (s1 + ((float)2) * b1) + c;
							}
						}
					}
					else
					{
						if (s1 <= -extDet)  // region 4
						{
							s0 = -(-a01 * segment.Extent + b0);
							if (s0 > (float)0)
							{
								s1 = -segment.Extent;
								sqrDist = -s0 * s0 + s1 * (s1 + ((float)2) * b1) + c;
							}
							else
							{
								s0 = (float)0;
								s1 = -b1;
								if (s1 < -segment.Extent)
								{
									s1 = -segment.Extent;
								}
								else if (s1 > segment.Extent)
								{
									s1 = segment.Extent;
								}
								sqrDist = s1 * (s1 + ((float)2) * b1) + c;
							}
						}
						else if (s1 <= extDet)  // region 3
						{
							s0 = (float)0;
							s1 = -b1;
							if (s1 < -segment.Extent)
							{
								s1 = -segment.Extent;
							}
							else if (s1 > segment.Extent)
							{
								s1 = segment.Extent;
							}
							sqrDist = s1 * (s1 + ((float)2) * b1) + c;
						}
						else  // region 2
						{
							s0 = -(a01 * segment.Extent + b0);
							if (s0 > (float)0)
							{
								s1 = segment.Extent;
								sqrDist = -s0 * s0 + s1 * (s1 + ((float)2) * b1) + c;
							}
							else
							{
								s0 = (float)0;
								s1 = -b1;
								if (s1 < -segment.Extent)
								{
									s1 = -segment.Extent;
								}
								else if (s1 > segment.Extent)
								{
									s1 = segment.Extent;
								}
								sqrDist = s1 * (s1 + ((float)2) * b1) + c;
							}
						}
					}
				}
				else
				{
					// Ray and segment are parallel.
					if (a01 > (float)0)
					{
						// Opposite direction vectors.
						s1 = -segment.Extent;
					}
					else
					{
						// Same direction vectors.
						s1 = segment.Extent;
					}

					s0 = -(a01 * s1 + b0);
					if (s0 > (float)0)
					{
						sqrDist = -s0 * s0 + s1 * (s1 + ((float)2) * b1) + c;
					}
					else
					{
						s0 = (float)0;
						sqrDist = s1 * (s1 + ((float)2) * b1) + c;
					}
				}

				closestPoint0 = ray.Center + s0 * ray.Direction;
				closestPoint1 = segment.Center + s1 * segment.Direction;

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