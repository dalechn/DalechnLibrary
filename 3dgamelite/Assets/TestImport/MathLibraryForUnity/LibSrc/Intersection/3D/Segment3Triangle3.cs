using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Segment3 and Triangle3
		/// </summary>
		public struct Segment3Triangle3Intr
		{
			/// <summary>
			/// Equals to IntersectionTypes.Point if intersection occured otherwise IntersectionTypes.Empty (even when a segment lies in a triangle plane)
			/// </summary>
			public IntersectionTypes IntersectionType;

			/// <summary>
			/// Intersection point (in case of IntersectionTypes.Point)
			/// </summary>
			public Vector3 Point;

			/// <summary>
			/// Segment evaluation parameter of the intersection point (in case of IntersectionTypes.Point)
			/// </summary>
			public float SegmentParameter;

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
			/// Tests if a segment intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment3Triangle3(ref Segment3 segment, ref Triangle3 triangle, out IntersectionTypes intersectionType)
			{
				// Compute the offset origin, edges, and normal.
				Vector3 diff   = segment.Center - triangle.V0;
				Vector3 edge1  = triangle.V1    - triangle.V0;
				Vector3 edge2  = triangle.V2    - triangle.V0;
				Vector3 normal = edge1.Cross(edge2);

				// Solve Q + t*D = b1*E1 + b2*E2 (Q = diff, D = segment direction,
				// E1 = edge1, E2 = edge2, N = Cross(E1,E2)) by
				//   |Dot(D,N)|*b1 = sign(Dot(D,N))*Dot(D,Cross(Q,E2))
				//   |Dot(D,N)|*b2 = sign(Dot(D,N))*Dot(D,Cross(E1,Q))
				//   |Dot(D,N)|*t = -sign(Dot(D,N))*Dot(Q,N)
				float DdN = segment.Direction.Dot(normal);
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
					// Segment and triangle are parallel, call it a "no intersection"
					// even if the segment does intersect.
					intersectionType = IntersectionTypes.Empty;
					return false;
				}

				float DdQxE2 = sign * segment.Direction.Dot(diff.Cross(edge2));
				if (DdQxE2 >= -Mathfex.ZeroTolerance)
				{
					float DdE1xQ = sign * segment.Direction.Dot(edge1.Cross(diff));
					if (DdE1xQ >= -Mathfex.ZeroTolerance)
					{
						if (DdQxE2 + DdE1xQ <= DdN + Mathfex.ZeroTolerance)
						{
							// Line intersects triangle, check if segment does.
							float QdN = -sign*diff.Dot(normal);
							float extDdN = segment.Extent * DdN;
							if ((-extDdN - Mathfex.ZeroTolerance) <= QdN && QdN <= (extDdN + Mathfex.ZeroTolerance))
							{
								// Segment intersects triangle.
								intersectionType = IntersectionTypes.Point;
								return true;
							}
							// else: |t| > extent, no intersection
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
			/// Tests if a segment intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment3Triangle3(ref Segment3 segment, ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, out IntersectionTypes intersectionType)
			{
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return TestSegment3Triangle3(ref segment, ref triangle, out intersectionType);
			}

			/// <summary>
			/// Tests if a segment intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment3Triangle3(ref Segment3 segment, Vector3 v0, Vector3 v1, Vector3 v2, out IntersectionTypes intersectionType)
			{
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return TestSegment3Triangle3(ref segment, ref triangle, out intersectionType);
			}


			/// <summary>
			/// Tests if a segment intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment3Triangle3(ref Segment3 segment, ref Triangle3 triangle)
			{
				IntersectionTypes intersectionType;
				return TestSegment3Triangle3(ref segment, ref triangle, out intersectionType);
			}

			/// <summary>
			/// Tests if a segment intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment3Triangle3(ref Segment3 segment, ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
			{
				IntersectionTypes intersectionType;
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return TestSegment3Triangle3(ref segment, ref triangle, out intersectionType);
			}

			/// <summary>
			/// Tests if a segment intersects a triangle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment3Triangle3(ref Segment3 segment, Vector3 v0, Vector3 v1, Vector3 v2)
			{
				IntersectionTypes intersectionType;
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return TestSegment3Triangle3(ref segment, ref triangle, out intersectionType);
			}


			/// <summary>
			/// Tests if a segment intersects a triangle and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindSegment3Triangle3(ref Segment3 segment, ref Triangle3 triangle, out Segment3Triangle3Intr info)
			{
				// Compute the offset origin, edges, and normal.
				Vector3 diff   = segment.Center - triangle.V0;
				Vector3 edge1  = triangle.V1    - triangle.V0;
				Vector3 edge2  = triangle.V2    - triangle.V0;
				Vector3 normal = edge1.Cross(edge2);

				// Solve Q + t*D = b1*E1 + b2*E2 (Q = diff, D = segment direction,
				// E1 = edge1, E2 = edge2, N = Cross(E1,E2)) by
				//   |Dot(D,N)|*b1 = sign(Dot(D,N))*Dot(D,Cross(Q,E2))
				//   |Dot(D,N)|*b2 = sign(Dot(D,N))*Dot(D,Cross(E1,Q))
				//   |Dot(D,N)|*t = -sign(Dot(D,N))*Dot(Q,N)
				float DdN = segment.Direction.Dot(normal);
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
					// Segment and triangle are parallel, call it a "no intersection"
					// even if the segment does intersect.
					info = new Segment3Triangle3Intr();
					return false;
				}

				float DdQxE2 = sign * segment.Direction.Dot(diff.Cross(edge2));
				if (DdQxE2 >= -Mathfex.ZeroTolerance)
				{
					float DdE1xQ = sign * segment.Direction.Dot(edge1.Cross(diff));
					if (DdE1xQ >= -Mathfex.ZeroTolerance)
					{
						if (DdQxE2 + DdE1xQ <= DdN + Mathfex.ZeroTolerance)
						{
							// Line intersects triangle, check if segment does.
							float QdN = -sign*diff.Dot(normal);
							float extDdN = segment.Extent*DdN;
							if ((-extDdN - Mathfex.ZeroTolerance) <= QdN && QdN <= (extDdN + Mathfex.ZeroTolerance))
							{
								// Segment intersects triangle.
								float inv = 1f / DdN;

								info.IntersectionType = IntersectionTypes.Point;
								info.SegmentParameter = QdN * inv;
								info.Point = segment.Center + info.SegmentParameter * segment.Direction;
								info.SegmentParameter = (info.SegmentParameter + segment.Extent) / (2f * segment.Extent);
								info.TriBary1 = DdQxE2 * inv;
								info.TriBary2 = DdE1xQ * inv;
								info.TriBary0 = 1f - info.TriBary1 - info.TriBary2;
								
								return true;
							}
							// else: |t| > extent, no intersection
						}
						// else: b1+b2 > 1, no intersection
					}
					// else: b2 < 0, no intersection
				}
				// else: b1 < 0, no intersection

				info = new Segment3Triangle3Intr();
				return false;
			}

			/// <summary>
			/// Tests if a segment intersects a triangle and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindSegment3Triangle3(ref Segment3 segment, ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, out Segment3Triangle3Intr info)
			{
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return FindSegment3Triangle3(ref segment, ref triangle, out info);
			}

			/// <summary>
			/// Tests if a segment intersects a triangle and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindSegment3Triangle3(ref Segment3 segment, Vector3 v0, Vector3 v1, Vector3 v2, out Segment3Triangle3Intr info)
			{
				Triangle3 triangle = new Triangle3() { V0 = v0, V1 = v1, V2 = v2 };
				return FindSegment3Triangle3(ref segment, ref triangle, out info);
			}
		}
	}
}
