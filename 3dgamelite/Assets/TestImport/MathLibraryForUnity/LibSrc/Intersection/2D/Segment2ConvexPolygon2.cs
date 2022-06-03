using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Segment2 and convex ccw ordered Polygon2
		/// </summary>
		public struct Segment2ConvexPolygon2Intr
		{
			/// <summary>
			/// Equals to IntersectionTypes.Point or IntersectionTypes.Segment if intersection occured otherwise IntersectionTypes.Empty
			/// </summary>
			public IntersectionTypes IntersectionType;

			/// <summary>
			/// Number of intersection points.
			/// IntersectionTypes.Empty: 0;
			/// IntersectionTypes.Point: 1;
			/// IntersectionTypes.Segment: 2.
			/// </summary>
			public int Quantity;

			/// <summary>
			/// First intersection point
			/// </summary>
			public Vector2 Point0;

			/// <summary>
			/// Second intersection point
			/// </summary>
			public Vector2 Point1;

			/// <summary>
			/// Segment evaluation parameter of the first intersection point
			/// </summary>
			public float Parameter0;

			/// <summary>
			/// Segment evaluation parameter of the second intersection point
			/// </summary>
			public float Parameter1;
		}

		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a ray intersects a convex ccw ordered polygon. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment2ConvexPolygon2(ref Segment2 segment, Polygon2 convexPolygon)
			{
				Segment2ConvexPolygon2Intr info;
				return FindSegment2ConvexPolygon2(ref segment, convexPolygon, out info);
			}

			/// <summary>
			/// Tests if a ray intersects a convex ccw ordered polygon and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindSegment2ConvexPolygon2(ref Segment2 segment, Polygon2 convexPolygon, out Segment2ConvexPolygon2Intr info)
			{
				// http://geomalgorithms.com/a13-_intersect-4.html

				Edge2[] edges = convexPolygon.Edges;
				int edgeCount = edges.Length;
				float tE = 0;							// the maximum entering segment parameter
				float tL = 1;							// the minimum leaving segment parameter
				float t, N, D;							// intersect parameter t = N / D
				Vector2 dS = segment.P1 - segment.P0;	// the  segment direction vector
				Vector2 e;								// edge vector
				Vector2 ne;								// edge outward normal

				for (int i = 0; i < edgeCount; ++i)
				{
					e = edges[i].Point1 - edges[i].Point0;
					ne = new Vector2(e.y, -e.x);
					N = ne.Dot(edges[i].Point0 - segment.P0);	// dot(ne, V[i] - S.P0)
					D = ne.Dot(dS);								// dot(ne, dS)
					
					if (Mathf.Abs(D) < Mathfex.ZeroTolerance)
					{
						// S is nearly parallel to this edge

						if (N < 0)
						{
							// P0 is outside this edge, so S is outside the polygon
							info = new Segment2ConvexPolygon2Intr();
							return false;
						}
						else
						{
							// S cannot cross this edge, so ignore this edge
							continue;
						}
					}

					t = N / D;
					if (D < 0)
					{
						// segment S is entering across this edge

						if (t > tE)
						{       
							// new max tE
							tE = t;
							if (tE > tL)
							{
								// S enters after leaving polygon
								info = new Segment2ConvexPolygon2Intr();
								return false;
							}
						}
					}
					else
					{
						// segment S is leaving across this edge

						if (t < tL)
						{       
							// new min tL
							tL = t;
							if (tL < tE)
							{
								// S leaves before entering polygon
								info = new Segment2ConvexPolygon2Intr();
								return false;
							}
						}
					}
				}

				// tE <= tL implies that there is a valid intersection subsegment
				// P(tE) = point where S enters polygon
				// P(tL) = point where S leaves polygon

				if (tL - tE > Mathfex.ZeroTolerance)
				{
					info.IntersectionType = IntersectionTypes.Segment;
					info.Quantity = 2;
					info.Point0 = segment.P0 + tE * dS;
					info.Point1 = segment.P0 + tL * dS;
					info.Parameter0 = tE;
					info.Parameter1 = tL;
				}
				else
				{
					info.IntersectionType = IntersectionTypes.Point;
					info.Quantity = 1;
					info.Point0 = segment.P0 + tE * dS;
					info.Point1 = Vector2ex.Zero;
					info.Parameter0 = tE;
					info.Parameter1 = 0f;
				}

				return true;
			}
		}
	}
}
