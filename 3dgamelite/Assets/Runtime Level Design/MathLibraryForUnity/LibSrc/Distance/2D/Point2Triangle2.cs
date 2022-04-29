using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and a triangle
			/// </summary>
			public static float Point2Triangle2(ref Vector2 point, ref Triangle2 triangle)
			{
				if (triangle.Contains(point))
				{
					return 0f;
				}

				Segment2 segment0 = new Segment2(ref triangle.V0, ref triangle.V1);
				float dist0 = Distance.Point2Segment2(ref point, ref segment0);

				Segment2 segment1 = new Segment2(ref triangle.V1, ref triangle.V2);
				float dist1 = Distance.Point2Segment2(ref point, ref segment1);

				Segment2 segment2 = new Segment2(ref triangle.V2, ref triangle.V0);
				float dist2 = Distance.Point2Segment2(ref point, ref segment2);

				if (dist0 < dist1)
				{
					if (dist0 < dist2)
					{
						return dist0;
					}
					else
					{
						if (dist1 < dist2)
						{
							return dist1;
						}
						else
						{
							return dist2;
						}
					}
				}
				else
				{
					if (dist1 < dist2)
					{
						return dist1;
					}
					else
					{
						if (dist0 < dist2)
						{
							return dist0;
						}
						else
						{
							return dist2;
						}
					}
				}
			}

			/// <summary>
			/// Returns distance between a point and a triangle
			/// </summary>
			/// <param name="closestPoint">Point projected on a triangle</param>
			public static float Point2Triangle2(ref Vector2 point, ref Triangle2 triangle, out Vector2 closestPoint)
			{
				if (triangle.Contains(point))
				{
					closestPoint = point;
					return 0f;
				}

				Segment2 segment0 = new Segment2(ref triangle.V0, ref triangle.V1);
				Vector2 closest0;
				float dist0 = Distance.Point2Segment2(ref point, ref segment0, out closest0);

				Segment2 segment1 = new Segment2(ref triangle.V1, ref triangle.V2);
				Vector2 closest1;
				float dist1 = Distance.Point2Segment2(ref point, ref segment1, out closest1);

				Segment2 segment2 = new Segment2(ref triangle.V2, ref triangle.V0);
				Vector2 closest2;
				float dist2 = Distance.Point2Segment2(ref point, ref segment2, out closest2);

				if (dist0 < dist1)
				{
					if (dist0 < dist2)
					{
						closestPoint = closest0;
						return dist0;
					}
					else
					{
						if (dist1 < dist2)
						{
							closestPoint = closest1;
							return dist1;
						}
						else
						{
							closestPoint = closest2;
							return dist2;
						}
					}
				}
				else
				{
					if (dist1 < dist2)
					{
						closestPoint = closest1;
						return dist1;
					}
					else
					{
						if (dist0 < dist2)
						{
							closestPoint = closest0;
							return dist0;
						}
						else
						{
							closestPoint = closest2;
							return dist2;
						}
					}
				}
			}


			/// <summary>
			/// Returns squared distance between a point and a triangle
			/// </summary>
			public static float SqrPoint2Triangle2(ref Vector2 point, ref Triangle2 triangle)
			{
				if (triangle.Contains(point))
				{
					return 0f;
				}

				Segment2 segment0 = new Segment2(ref triangle.V0, ref triangle.V1);
				float dist0 = Distance.SqrPoint2Segment2(ref point, ref segment0);

				Segment2 segment1 = new Segment2(ref triangle.V1, ref triangle.V2);
				float dist1 = Distance.SqrPoint2Segment2(ref point, ref segment1);

				Segment2 segment2 = new Segment2(ref triangle.V2, ref triangle.V0);
				float dist2 = Distance.SqrPoint2Segment2(ref point, ref segment2);

				if (dist0 < dist1)
				{
					if (dist0 < dist2)
					{
						return dist0;
					}
					else
					{
						if (dist1 < dist2)
						{
							return dist1;
						}
						else
						{
							return dist2;
						}
					}
				}
				else
				{
					if (dist1 < dist2)
					{
						return dist1;
					}
					else
					{
						if (dist0 < dist2)
						{
							return dist0;
						}
						else
						{
							return dist2;
						}
					}
				}
			}

			/// <summary>
			/// Returns squared distance between a point and a triangle
			/// </summary>
			/// <param name="closestPoint">Point projected on a triangle</param>
			public static float SqrPoint2Triangle2(ref Vector2 point, ref Triangle2 triangle, out Vector2 closestPoint)
			{
				if (triangle.Contains(point))
				{
					closestPoint = point;
					return 0f;
				}

				Segment2 segment0 = new Segment2(ref triangle.V0, ref triangle.V1);
				Vector2 closest0;
				float dist0 = Distance.SqrPoint2Segment2(ref point, ref segment0, out closest0);

				Segment2 segment1 = new Segment2(ref triangle.V1, ref triangle.V2);
				Vector2 closest1;
				float dist1 = Distance.SqrPoint2Segment2(ref point, ref segment1, out closest1);

				Segment2 segment2 = new Segment2(ref triangle.V2, ref triangle.V0);
				Vector2 closest2;
				float dist2 = Distance.SqrPoint2Segment2(ref point, ref segment2, out closest2);

				if (dist0 < dist1)
				{
					if (dist0 < dist2)
					{
						closestPoint = closest0;
						return dist0;
					}
					else
					{
						if (dist1 < dist2)
						{
							closestPoint = closest1;
							return dist1;
						}
						else
						{
							closestPoint = closest2;
							return dist2;
						}
					}
				}
				else
				{
					if (dist1 < dist2)
					{
						closestPoint = closest1;
						return dist1;
					}
					else
					{
						if (dist0 < dist2)
						{
							closestPoint = closest0;
							return dist0;
						}
						else
						{
							closestPoint = closest2;
							return dist2;
						}
					}
				}
			}
		}
	}
}
