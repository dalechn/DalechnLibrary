using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains information about intersection of Line3 and Sphere3
		/// </summary>
		public struct Line3Sphere3Intr
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
			public Vector3 Point0;

			/// <summary>
			/// Second intersection point
			/// </summary>
			public Vector3 Point1;
			
			/// <summary>
			/// Line evaluation parameter of the first intersection point
			/// </summary>
			public float LineParameter0;

			/// <summary>
			/// Line evaluation parameter of the second intersection point
			/// </summary>
			public float LineParameter1;
		}

		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a line intersects a sphere. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestLine3Sphere3(ref Line3 line, ref Sphere3 sphere)
			{
				Vector3 delta = line.Center - sphere.Center;
				float a0 = delta.sqrMagnitude - sphere.Radius * sphere.Radius;

				float a1 = line.Direction.Dot(delta);
				float discr = a1 * a1 - a0;
				
				return discr >= -Mathfex.ZeroTolerance;
			}

			/// <summary>
			/// Tests if a line intersects a sphere and finds intersection parameters. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool FindLine3Sphere3(ref Line3 line, ref Sphere3 sphere, out Line3Sphere3Intr info)
			{
				Vector3 diff = line.Center - sphere.Center;
				float a0 = diff.Dot(diff) - sphere.Radius * sphere.Radius;
				float a1 = line.Direction.Dot(diff);
				float discr = a1 * a1 - a0;

				if (discr < -Mathfex.ZeroTolerance)
				{
					info = new Line3Sphere3Intr();
				}
				else if (discr > Mathfex.ZeroTolerance)
				{
					float root = Mathf.Sqrt(discr);

					info.LineParameter0   = -a1 - root;
					info.LineParameter1   = -a1 + root;
					info.Point0           = line.Center + info.LineParameter0  * line.Direction;
					info.Point1           = line.Center + info.LineParameter1 * line.Direction;
					info.IntersectionType = IntersectionTypes.Segment;
					info.Quantity         = 2;
				}
				else
				{
					info.LineParameter0   = -a1;
					info.LineParameter1   = 0.0f; 
					info.Point0           = line.Center + info.LineParameter0 * line.Direction;
					info.Point1           = Vector3.zero;
					info.IntersectionType = IntersectionTypes.Point;
					info.Quantity         = 1;
				}

				return info.Quantity > 0.0f;
			}
		}
	}
}
