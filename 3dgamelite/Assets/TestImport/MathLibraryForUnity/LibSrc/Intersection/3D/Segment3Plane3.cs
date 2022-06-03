using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Segment3 and Plane3
		/// </summary>
		public struct Segment3Plane3Intr
		{
			/// <summary>
			/// Equals to IntersectionTypes.Point or IntersectionTypes.Segment (a segment lies in a plane) if intersection occured otherwise IntersectionTypes.Empty
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
		}

		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a segment intersects a plane. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment3Plane3(ref Segment3 segment, ref Plane3 plane, out IntersectionTypes intersectionType)
			{
				Vector3 P0 = segment.P0;
				float sdistance0 = plane.SignedDistanceTo(ref P0);
				if (Mathf.Abs(sdistance0) <= _distanceThreshold)
				{
					sdistance0 = 0f;
				}

				Vector3 P1 = segment.P1;
				float sdistance1 = plane.SignedDistanceTo(ref P1);
				if (Mathf.Abs(sdistance1) <= _distanceThreshold)
				{
					sdistance1 = 0f;
				}

				float prod = sdistance0 * sdistance1;
				if (prod < 0f)
				{
					// The segment passes through the plane.
					intersectionType = IntersectionTypes.Point;
					return true;
				}

				if (prod > 0f)
				{
					// The segment is on one side of the plane.
					intersectionType = IntersectionTypes.Empty;
					return false;
				}

				if (sdistance0 != 0f || sdistance1 != 0f)
				{
					// A segment end point touches the plane.
					intersectionType = IntersectionTypes.Point;
					return true;
				}

				// The segment is coincident with the plane.
				intersectionType = IntersectionTypes.Segment;
				return true;
			}

			/// <summary>
			/// Tests if a segment intersects a plane. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSegment3Plane3(ref Segment3 segment, ref Plane3 plane)
			{
				IntersectionTypes intersectionType;
				return TestSegment3Plane3(ref segment, ref plane, out intersectionType);
			}

			/// <summary>
			/// Tests if a segment intersects a plane and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindSegment3Plane3(ref Segment3 segment, ref Plane3 plane, out Segment3Plane3Intr info)
			{
				float DdN = segment.Direction.Dot(plane.Normal);
				float signedDistance = plane.SignedDistanceTo(ref segment.Center);
				if (Mathf.Abs(DdN) > _dotThreshold)
				{
					float parameter = -signedDistance / DdN;
					if (Mathf.Abs(parameter) <= segment.Extent + _intervalThreshold)
					{
						info.IntersectionType = IntersectionTypes.Point;
						info.Point = segment.Center + parameter * segment.Direction;
						info.SegmentParameter = (parameter + segment.Extent) / (segment.Extent * 2f);
						return true;
					}
					else
					{
						info.IntersectionType = IntersectionTypes.Empty;
						info.Point = Vector3.zero;
						info.SegmentParameter = 0f;
						return false;
					}
				}

				// The Line and plane are parallel.  Determine if they are numerically
				// close enough to be coincident.
				if (Mathf.Abs(signedDistance) <= _distanceThreshold)
				{
					// The line is coincident with the plane, so choose t = 0 for the parameter.
					info.IntersectionType = IntersectionTypes.Segment;
					info.Point = Vector3.zero;
					info.SegmentParameter = 0f;
					return true;
				}

				info.IntersectionType = IntersectionTypes.Empty;
				info.Point = Vector3.zero;
				info.SegmentParameter = 0f;
				return false;
			}
		}
	}
}
