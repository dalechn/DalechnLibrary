using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public enum Sphere3Sphere3IntrTypes
		{
			/// <summary>
			/// Spheres are disjoint/separated
			/// </summary>
			Empty,
			
			/// <summary>
			/// Spheres touch at point, each sphere outside the other
			/// </summary>
			Point,
			
			/// <summary>
			/// Spheres intersect in a circle
			/// </summary>
			Circle,
			
			/// <summary>
			/// Sphere0 strictly contained in sphere1
			/// </summary>
			Sphere0,
			
			/// <summary>
			/// Sphere0 contained in sphere1, share common point
			/// </summary>
			Sphere0Point,
			
			/// <summary>
			/// Sphere1 strictly contained in sphere0
			/// </summary>
			Sphere1,
			
			/// <summary>
			/// Sphere1 contained in sphere0, share common point
			/// </summary>
			Sphere1Point,

			/// <summary>
			/// Spheres are the same
			/// </summary>
			Same
		}

		/// <summary>
		/// Contains information about intersection of Sphere3 and Sphere3
		/// </summary>
		public struct Sphere3Sphere3Intr
		{
			/// <summary>
			/// Equals to:
			/// Sphere3Sphere3IntersectionTypes.Empty if no intersection occurs;
			/// Sphere3Sphere3IntersectionTypes.Point if spheres are touching in a point and outside of each other;
			/// Sphere3Sphere3IntersectionTypes.Circle is spheres intersect (common case);
			/// Sphere3Sphere3IntersectionTypes.Sphere0 or Sphere3Sphere3IntersectionTypes.Sphere1 if sphere0 is strictly contained inside sphere1, or
			/// sphere1 is strictly contained in sphere0 respectively;
			/// Sphere3Sphere3IntersectionTypes.Sphere0Point or Sphere3Sphere3IntersectionTypes.Sphere1Point if sphere0 is contained inside sphere1 and share common point or
			/// sphere1 is contained inside sphere0 and share common point;
			/// Sphere3Sphere3IntersectionTypes.Same if spheres are esssentialy the same.
			/// </summary>
			public Sphere3Sphere3IntrTypes IntersectionType;

			/// <summary>
			/// Circle of intersection in case of Sphere3Sphere3IntersectionTypes.Circle
			/// </summary>
			public Circle3 Circle;

			/// <summary>
			/// Contact point in case of Sphere3Sphere3IntersectionTypes.Point,
			/// Sphere3Sphere3IntersectionTypes.Sphere0Point, Sphere3Sphere3IntersectionTypes.Sphere1Point
			/// </summary>
			public Vector3 ContactPoint;
		}

		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a sphere intersects another sphere. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestSphere3Sphere3(ref Sphere3 sphere0, ref Sphere3 sphere1)
			{
				Vector3 diff = sphere1.Center - sphere0.Center;
				float rSum = sphere0.Radius + sphere1.Radius;
				return diff.sqrMagnitude <= rSum * rSum;
			}

			/// <summary>
			/// Tests if a sphere intersects another sphere and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindSphere3Sphere3(ref Sphere3 sphere0, ref Sphere3 sphere1, out Sphere3Sphere3Intr info)
			{
				// Plane of intersection must have N as its normal.
				Vector3 C1mC0 = sphere1.Center - sphere0.Center;
				float sqrLen = C1mC0.sqrMagnitude;
				float r0 = sphere0.Radius, r1 = sphere1.Radius;

				float rDif = r0 - r1;

				if (C1mC0.sqrMagnitude < Mathfex.ZeroToleranceSqr &&
					Mathf.Abs(rDif) < Mathfex.ZeroTolerance)
				{
					// Spheres are the same

					info.IntersectionType = Sphere3Sphere3IntrTypes.Same;
					info.ContactPoint = Vector3ex.Zero;
					info.Circle = new Circle3();

					return true;
				}

				float rSum = r0 + r1;
				float rSumSqr = rSum * rSum;

				if (sqrLen > rSumSqr)
				{
					// Spheres are disjoint/separated.
					info = new Sphere3Sphere3Intr();

					return false;
				}
				if (sqrLen == rSumSqr)
				{
					// Spheres are just touching.  The caller must call
					// GetIntersectionType() to determine what type of intersection has
					// occurred.  In this case, GetContactPoint() should be called, not
					// GetCircle().  The circle parameters are set just in case the caller
					// does not test for intersection type.
					C1mC0.Normalize();

					info.ContactPoint = sphere0.Center + r0 * C1mC0;
					info.Circle = new Circle3();
					info.IntersectionType = Sphere3Sphere3IntrTypes.Point;

					return true;
				}
								
				float rDifSqr = rDif * rDif;
				if (sqrLen < rDifSqr)
				{
					// One sphere is strictly contained in the other.  The caller must
					// call GetIntersectionType() to determine what type of intersection
					// has occurred.  In this case, neither GetCircle() nor
					// GetContactPoint() should be called.  The circle and contact
					// parameters are set just in case the caller does not test for
					// intersection type, but the choices are arbitrary.
					C1mC0.Normalize();

					info.ContactPoint = 0.5f * (sphere0.Center + sphere1.Center);
					info.Circle = new Circle3();
					info.IntersectionType = rDif <= 0f ? Sphere3Sphere3IntrTypes.Sphere0 : Sphere3Sphere3IntrTypes.Sphere1;

					return true;
				}
				if (sqrLen == rDifSqr)
				{
					// One sphere is contained in the other sphere but with a single point
					// of contact.  The caller must call GetIntersectionType() to
					// determine what type of intersection has occurred.  In this case,
					// GetContactPoint() should be called.  The circle parameters are set
					// just in case the caller does not test for intersection type.
					C1mC0.Normalize();

					if (rDif <= 0f)
					{
						info.IntersectionType = Sphere3Sphere3IntrTypes.Sphere0Point;
						info.ContactPoint = sphere1.Center + r1 * C1mC0;
					}
					else
					{
						info.IntersectionType = Sphere3Sphere3IntrTypes.Sphere1Point;
						info.ContactPoint = sphere0.Center + r0 * C1mC0;
					}
					info.Circle = new Circle3();

					return true;
				}

				// Compute t for which the circle of intersection has center
				// K = C0 + t*(C1 - C0).
				float t = 0.5f * (1f + rDif * rSum / sqrLen);

				// Center and radius of circle of intersection.
				Vector3 center = sphere0.Center + t * C1mC0;
				float radius = Mathf.Sqrt(Mathf.Abs(r0 * r0 - t * t * sqrLen));

				// Compute N, U, and V for plane of circle.
				C1mC0.Normalize();

				info.Circle = new Circle3(ref center, ref C1mC0, radius);

				// The intersection is a circle.
				info.IntersectionType = Sphere3Sphere3IntrTypes.Circle;
				info.ContactPoint = Vector3ex.Zero;

				return true;
			}
		}
	}
}
