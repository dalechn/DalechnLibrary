using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of two Ray2
		/// </summary>
		public struct Ray2Ray2Intr
		{
			/// <summary>
			/// Equals to IntersectionTypes.Point or IntersectionTypes.Ray (rays are collinear and overlap in more than one point)
			/// if intersection occured otherwise IntersectionTypes.Empty
			/// </summary>
			public IntersectionTypes IntersectionType;

			/// <summary>
			/// In case of IntersectionTypes.Point constains single point of intersection.
			/// In case of IntersectionTypes.Ray contains second ray's origin.
			/// Otherwise Vector2.zero.
			/// </summary>
			public Vector2 Point;

			/// <summary>
			/// In case of IntersectionTypes.Point contains evaluation parameter of single
			/// intersection point according to first ray.
			/// In case of IntersectionTypes.Ray contains evaluation parameter of the
			/// second ray's origin according to first ray.
			/// Otherwise 0.
			/// </summary>
			public float Parameter;
		}

		public static partial class Intersection
		{
			private static IntersectionTypes Classify(ref Ray2 ray0, ref Ray2 ray1, out float s0, out float s1)
			{
				// The intersection of two lines is a solution to P0+s0*D0 = P1+s1*D1.
				// Rewrite this as s0*D0 - s1*D1 = P1 - P0 = Q.  If D0.Dot(Perp(D1)) = 0,
				// the lines are parallel.  Additionally, if Q.Dot(Perp(D1)) = 0, the
				// lines are the same.  If D0.Dot(Perp(D1)) is not zero, then
				//   s0 = Q.Dot(Perp(D1))/D0.Dot(Perp(D1))
				// produces the point of intersection.  Also,
				//   s1 = Q.Dot(Perp(D0))/D0.Dot(Perp(D1))

				Vector2 originDiff = ray1.Center - ray0.Center;

				s0 = s1 = 0f;

				float D0DotPerpD1 = ray0.Direction.DotPerp(ray1.Direction);

				if (Mathf.Abs(D0DotPerpD1) > _dotThreshold)
				{
					// Lines intersect in a single point.
					float invD0DotPerpD1 = 1f / D0DotPerpD1;

					float diffDotPerpD1 = originDiff.DotPerp(ray1.Direction);
					s0 = diffDotPerpD1 * invD0DotPerpD1;

					float diffDotPerpD0 = originDiff.DotPerp(ray0.Direction);
					s1 = diffDotPerpD0 * invD0DotPerpD1;

					return IntersectionTypes.Point;
				}

				// Lines are parallel.
				originDiff.Normalize();

				float diffNDotPerpD1 = originDiff.DotPerp(ray1.Direction);
				if (Mathf.Abs(diffNDotPerpD1) <= _dotThreshold)
				{
					s0 = Vector2.Dot(ray1.Center - ray0.Center, ray0.Direction);

					// Lines are colinear.
					return IntersectionTypes.Ray;
				}

				// Lines are parallel, but distinct.
				return IntersectionTypes.Empty;
			}

			/// <summary>
			/// Tests whether two rays intersect.
			/// Returns true if intersection occurs (IntersectionTypes.Point, IntersectionTypes.Ray),
			/// or false if rays do not intersect (IntersectionTypes.Empty).
			/// </summary>
			public static bool TestRay2Ray2(ref Ray2 ray0, ref Ray2 ray1, out IntersectionTypes intersectionType)
			{
				float parameter0, parameter1;
				intersectionType = Classify(ref ray0, ref ray1, out parameter0, out parameter1);

				if (intersectionType == IntersectionTypes.Point)
				{
					// Test whether the line-line intersection is on the rays.
					if (parameter0 >= -_intervalThreshold && parameter1 >= -_intervalThreshold)
					{
						// OK
					}
					else
					{
						intersectionType = IntersectionTypes.Empty;
					}
				}
				else if (intersectionType == IntersectionTypes.Ray)
				{
					if (Mathf.Abs(parameter0) == 0f)
					{
						float dot = Vector2.Dot(ray0.Direction, ray1.Direction);
						if (dot < 0)
						{
							intersectionType = IntersectionTypes.Point;
						}
					}
					else if (parameter0 < 0f)
					{
						float dot = Vector2.Dot(ray0.Direction, ray1.Direction);
						if (dot < 0)
						{
							// Rays look into opposite directions
							intersectionType = IntersectionTypes.Empty;
						}
					}
				}

				return intersectionType != IntersectionTypes.Empty;
			}

			/// <summary>
			/// Tests whether two rays intersect.
			/// Returns true if intersection occurs (IntersectionTypes.Point, IntersectionTypes.Ray),
			/// or false if rays do not intersect (IntersectionTypes.Empty).
			/// </summary>
			public static bool TestRay2Ray2(ref Ray2 ray0, ref Ray2 ray1)
			{
				IntersectionTypes intersectionType;
				return TestRay2Ray2(ref ray0, ref ray1, out intersectionType);
			}

			/// <summary>
			/// Tests whether two rays intersect and finds actual intersection parameters.
			/// Returns true if intersection occurs (IntersectionTypes.Point, IntersectionTypes.Ray),
			/// or false if rays do not intersect (IntersectionTypes.Empty).
			/// </summary>
			public static bool FindRay2Ray2(ref Ray2 ray0, ref Ray2 ray1, out Ray2Ray2Intr info)
			{
				float parameter0, parameter1;
				info.IntersectionType = Classify(ref ray0, ref ray1, out parameter0, out parameter1);
				info.Point = Vector2.zero;
				info.Parameter = 0f;

				if (info.IntersectionType == IntersectionTypes.Point)
				{
					// Test whether the line-line intersection is on the rays.
					if (parameter0 >= -_intervalThreshold && parameter1 >= -_intervalThreshold)
					{
						if (parameter0 < 0) parameter0 = 0f;
						info.Point = ray0.Center + parameter0 * ray0.Direction;
						info.Parameter = parameter0;
					}
					else
					{
						info.IntersectionType = IntersectionTypes.Empty;
					}
				}
				else if (info.IntersectionType == IntersectionTypes.Ray)
				{
					if (Mathf.Abs(parameter0) == 0f)
					{
						float dot = Vector2.Dot(ray0.Direction, ray1.Direction);
						if (dot < 0)
						{
							info.IntersectionType= IntersectionTypes.Point;
						}
						info.Point = ray1.Center;
					}
					else if (parameter0 < 0f)
					{
						float dot = Vector2.Dot(ray0.Direction, ray1.Direction);
						if (dot < 0)
						{
							// Rays look into opposite directions
							info.IntersectionType = IntersectionTypes.Empty;
						}
						else
						{
							info.Point = ray1.Center;
							info.Parameter = parameter0;
						}
					}
					else
					{
						info.Point = ray1.Center;
						info.Parameter = parameter0;
					}
				}

				return info.IntersectionType != IntersectionTypes.Empty;
			}
		}
	}
}
