using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Ray and Triangle3
		/// </summary>
		public struct Ray3Triangle3Intr
		{
			/// <summary>
			/// Equals to IntersectionTypes.Point if intersection occured otherwise IntersectionTypes.Empty (even when a ray lies in a triangle plane)
			/// </summary>
			public IntersectionTypes IntersectionType;

			/// <summary>
			/// Intersection point (in case of IntersectionTypes.Point)
			/// </summary>
			public Vector3 Point;

			/// <summary>
			/// Ray evaluation parameter of the intersection point (in case of IntersectionTypes.Point)
			/// </summary>
			public float RayParameter;

			/// <summary>
			/// First barycentric coordinate of the intersection point
			/// </summary>
			public float TriBary0;

			/// <summary>
			/// Second barycentric coordinate of the intersection point
			/// </summary>
			public float TriBary1;

			/// <summary>
			/// Third barycentric coordinate of the intersection point
			/// </summary>
			public float TriBary2;
		}

		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a ray intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestRay3Triangle3(ref Ray3 ray, ref Triangle3 triangle, out IntersectionTypes intersectionType)
			{
				// Compute the offset origin, edges, and normal.
				Vector3 diff   = ray.Center  - triangle.V0;
				Vector3 edge1  = triangle.V1 - triangle.V0;
				Vector3 edge2  = triangle.V2 - triangle.V0;
				Vector3 normal = edge1.Cross(edge2);

				// Solve Q + t*D = b1*E1 + b2*E2 (Q = kDiff, D = ray direction,
				// E1 = kEdge1, E2 = kEdge2, N = Cross(E1,E2)) by
				//   |Dot(D,N)|*b1 = sign(Dot(D,N))*Dot(D,Cross(Q,E2))
				//   |Dot(D,N)|*b2 = sign(Dot(D,N))*Dot(D,Cross(E1,Q))
				//   |Dot(D,N)|*t = -sign(Dot(D,N))*Dot(Q,N)
				float DdN = ray.Direction.Dot(normal);
				float sign;
				if (DdN > _dotThreshold)
				{
					sign = 1f;
				}
				else if (DdN < -_dotThreshold)
				{
					sign = -1f;
					DdN = -DdN;
				}
				else
				{
					// Ray and triangle are parallel, call it a "no intersection"
					// even if the ray does intersect.
					intersectionType = IntersectionTypes.Empty;
					return false;
				}

				float DdQxE2 = sign * ray.Direction.Dot(diff.Cross(edge2));
				if (DdQxE2 >= -Mathfex.ZeroTolerance)
				{
					float DdE1xQ = sign * ray.Direction.Dot(edge1.Cross(diff));
					if (DdE1xQ >= -Mathfex.ZeroTolerance)
					{
						if (DdQxE2 + DdE1xQ <= DdN + Mathfex.ZeroTolerance)
						{
							// Line intersects triangle, check if ray does.
							float QdN = -sign * diff.Dot(normal);
							if (QdN >= -_intervalThreshold)
							{
								// Ray intersects triangle.
								intersectionType = IntersectionTypes.Point;
								return true;
							}
							// else: t < 0, no intersection
						}
						// else: b1+b2 > 1, no intersection
					}
					// else: b2 < 0, no intersection
				}
				// else: b1 < 0, no intersection

				intersectionType = IntersectionTypes.Empty;
				return false;
			}

			/// <summary>
			/// Tests if a ray intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestRay3Triangle3(ref Ray3 ray, ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, out IntersectionTypes intersectionType)
			{
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return TestRay3Triangle3(ref ray, ref triangle, out intersectionType);
			}

			/// <summary>
			/// Tests if a ray intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestRay3Triangle3(ref Ray3 ray, Vector3 v0, Vector3 v1, Vector3 v2, out IntersectionTypes intersectionType)
			{
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return TestRay3Triangle3(ref ray, ref triangle, out intersectionType);
			}


			/// <summary>
			/// Tests if a ray intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestRay3Triangle3(ref Ray3 ray, ref Triangle3 triangle)
			{
				IntersectionTypes intersectionType;
				return TestRay3Triangle3(ref ray, ref triangle, out intersectionType);
			}

			/// <summary>
			/// Tests if a ray intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestRay3Triangle3(ref Ray3 ray, ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
			{
				IntersectionTypes intersectionType;
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return TestRay3Triangle3(ref ray, ref triangle, out intersectionType);
			}

			/// <summary>
			/// Tests if a ray intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestRay3Triangle3(ref Ray3 ray, Vector3 v0, Vector3 v1, Vector3 v2)
			{
				IntersectionTypes intersectionType;
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return TestRay3Triangle3(ref ray, ref triangle, out intersectionType);
			}


			/// <summary>
			/// Tests if a ray intersects a triangle and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindRay3Triangle3(ref Ray3 ray, ref Triangle3 triangle, out Ray3Triangle3Intr info)
			{
				// Compute the offset origin, edges, and normal.
				Vector3 diff   = ray.Center  - triangle.V0;
				Vector3 edge1  = triangle.V1 - triangle.V0;
				Vector3 edge2  = triangle.V2 - triangle.V0;
				Vector3 normal = edge1.Cross(edge2);

				// Solve Q + t*D = b1*E1 + b2*E2 (Q = kDiff, D = ray direction,
				// E1 = kEdge1, E2 = kEdge2, N = Cross(E1,E2)) by
				//   |Dot(D,N)|*b1 = sign(Dot(D,N))*Dot(D,Cross(Q,E2))
				//   |Dot(D,N)|*b2 = sign(Dot(D,N))*Dot(D,Cross(E1,Q))
				//   |Dot(D,N)|*t = -sign(Dot(D,N))*Dot(Q,N)
				float DdN = ray.Direction.Dot(normal);
				float sign;
				if (DdN > _dotThreshold)
				{
					sign = 1f;
				}
				else if (DdN < -_dotThreshold)
				{
					sign = -1f;
					DdN = -DdN;
				}
				else
				{
					// Ray and triangle are parallel, call it a "no intersection"
					// even if the ray does intersect.
					info = new Ray3Triangle3Intr();
					return false;
				}

				float DdQxE2 = sign * ray.Direction.Dot(diff.Cross(edge2));
				if (DdQxE2 >= -Mathfex.ZeroTolerance)
				{
					float DdE1xQ = sign * ray.Direction.Dot(edge1.Cross(diff));
					if (DdE1xQ >= -Mathfex.ZeroTolerance)
					{
						if (DdQxE2 + DdE1xQ <= DdN + Mathfex.ZeroTolerance)
						{
							// Line intersects triangle, check if ray does.
							float QdN = -sign * diff.Dot(normal);
							if (QdN >= -_intervalThreshold)
							{
								// Ray intersects triangle.
								float inv = 1f / DdN;

								info.IntersectionType = IntersectionTypes.Point;
								info.RayParameter = QdN * inv;
								info.Point = ray.Eval(info.RayParameter);
								info.TriBary1 = DdQxE2 * inv;
								info.TriBary2 = DdE1xQ * inv;
								info.TriBary0 = 1f - info.TriBary1 - info.TriBary2;

								return true;
							}
							// else: t < 0, no intersection
						}
						// else: b1+b2 > 1, no intersection
					}
					// else: b2 < 0, no intersection
				}
				// else: b1 < 0, no intersection

				info = new Ray3Triangle3Intr();

				return false;
			}

			/// <summary>
			/// Tests if a ray intersects a triangle and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindRay3Triangle3(ref Ray3 ray, ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, out Ray3Triangle3Intr info)
			{
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return FindRay3Triangle3(ref ray, ref triangle, out info);
			}

			/// <summary>
			/// Tests if a ray intersects a triangle and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindRay3Triangle3(ref Ray3 ray, Vector3 v0, Vector3 v1, Vector3 v2, out Ray3Triangle3Intr info)
			{
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return FindRay3Triangle3(ref ray, ref triangle, out info);
			}
		}
	}
}
