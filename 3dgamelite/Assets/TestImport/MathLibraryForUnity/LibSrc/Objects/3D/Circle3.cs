using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// The plane containing the circle is Dot(N,X-C) = 0, where X is any point
		/// in the plane.  Vectors U, V, and N form an orthonormal set
		/// (matrix [U V N] is orthonormal and has determinant 1).  The circle
		/// within the plane is parameterized by X = C + R*(cos(t)*U + sin(t)*V),
		/// where t is an angle in [0,2*pi).
		/// </summary>
		public struct Circle3
		{
			/// <summary>
			/// Circle center.
			/// </summary>
			public Vector3 Center;

			/// <summary>
			/// First circle axis. Must be unit length!
			/// </summary>
			public Vector3 Axis0;

			/// <summary>
			/// Second circle axis. Must be unit length!
			/// </summary>
			public Vector3 Axis1;

			/// <summary>
			/// Circle normal which is Cross(Axis0, Axis1). Must be unit length!
			/// </summary>
			public Vector3 Normal;

			/// <summary>
			/// Circle radius.
			/// </summary>
			public float Radius;


			/// <summary>
			/// Creates new circle instance from center, axes and radius. Normal is calculated as cross product of the axes.
			/// </summary>
			/// <param name="axis0">Must be unit length!</param>
			/// <param name="axis1">Must be unit length!</param>
			public Circle3(ref Vector3 center, ref Vector3 axis0, ref Vector3 axis1, float radius)
			{
				Center = center;
				Axis0  = axis0;
				Axis1  = axis1;
				Normal = axis0.Cross(axis1);
				Radius = radius;
			}

			/// <summary>
			/// Creates new circle instance from center, axes and radius. Normal is calculated as cross product of the axes.
			/// </summary>
			public Circle3(Vector3 center, Vector3 axis0, Vector3 axis1, float radius)
			{
				Center = center;
				Axis0  = axis0;
				Axis1  = axis1;
				Normal = axis0.Cross(axis1);
				Radius = radius;
			}

			/// <summary>
			/// Creates new circle instance. Computes axes from specified normal.
			/// </summary>
			/// <param name="normal">Must be unit length!</param>
			public Circle3(ref Vector3 center, ref Vector3 normal, float radius)
			{
				Center = center;
				Normal = normal;
				Vector3ex.CreateOrthonormalBasis(out Axis0, out Axis1, ref Normal);
				Radius = radius;
			}

			/// <summary>
			/// Creates new circle instance. Computes axes from specified normal.
			/// </summary>
			/// <param name="normal">Must be unit length!</param>
			public Circle3(Vector3 center, Vector3 normal, float radius)
			{
				Center = center;
				Normal = normal;
				Vector3ex.CreateOrthonormalBasis(out Axis0, out Axis1, ref Normal);
				Radius = radius;
			}

			/// <summary>
			/// Creates circle which is circumscribed around triangle.
			/// Returns 'true' if circle has been constructed, 'false' otherwise (input points are linearly dependent).
			/// </summary>
			public static bool CreateCircumscribed(Vector3 v0, Vector3 v1, Vector3 v2, out Circle3 circle)
			{
				Vector3 E02 = v0 - v2;
				Vector3 E12 = v1 - v2;
				float e02e02 = E02.Dot(E02);
				float e02e12 = E02.Dot(E12);
				float e12e12 = E12.Dot(E12);
				float det = e02e02 * e12e12 - e02e12 * e02e12;

				if (Mathf.Abs(det) < Mathfex.ZeroTolerance)
				{
					circle = new Circle3();
					return false;
				}

				float halfInvDet = 0.5f / det;
				float u0 = halfInvDet * e12e12 * (e02e02 - e02e12);
				float u1 = halfInvDet * e02e02 * (e12e12 - e02e12);
				Vector3 tmp = u0 * E02 + u1 * E12;

				circle.Center = v2 + tmp;
				circle.Radius = tmp.magnitude;

				circle.Normal = E02.UnitCross(E12);

				if (Mathf.Abs(circle.Normal.x) >= Mathf.Abs(circle.Normal.y) &&
					Mathf.Abs(circle.Normal.x) >= Mathf.Abs(circle.Normal.z))
				{
					circle.Axis0.x = -circle.Normal.y;
					circle.Axis0.y = circle.Normal.x;
					circle.Axis0.z = 0f;
				}
				else
				{
					circle.Axis0.x = 0f;
					circle.Axis0.y = circle.Normal.z;
					circle.Axis0.z = -circle.Normal.y;
				}

				circle.Axis0.Normalize();
				circle.Axis1 = circle.Normal.Cross(circle.Axis0);

				return true;
			}

			/// <summary>
			/// Creates circle which is insribed into triangle.
			/// Returns 'true' if circle has been constructed, 'false' otherwise (input points are linearly dependent).
			/// </summary>
			public static bool CreateInscribed(Vector3 v0, Vector3 v1, Vector3 v2, out Circle3 circle)
			{
				// Edges.
				Vector3 E0 = v1 - v0;
				Vector3 E1 = v2 - v1;
				Vector3 E2 = v0 - v2;

				// Plane normal.
				circle.Normal = E1.Cross(E0);

				// Edge normals within the plane.
				Vector3 N0 = circle.Normal.UnitCross(E0);
				Vector3 N1 = circle.Normal.UnitCross(E1);
				Vector3 N2 = circle.Normal.UnitCross(E2);

				float a0 = N1.Dot(E0);
				if (Mathf.Abs(a0) < Mathfex.ZeroTolerance)
				{
					circle = new Circle3();
					return false;
				}

				float a1 = N2.Dot(E1);
				if (Mathf.Abs(a1) < Mathfex.ZeroTolerance)
				{
					circle = new Circle3();
					return false;
				}

				float a2 = N0.Dot(E2);
				if (Mathf.Abs(a2) < Mathfex.ZeroTolerance)
				{
					circle = new Circle3();
					return false;
				}

				float invA0 = 1f / a0;
				float invA1 = 1f / a1;
				float invA2 = 1f / a2;

				circle.Radius = 1f / (invA0 + invA1 + invA2);
				circle.Center = circle.Radius * (invA0 * v0 + invA1 * v1 + invA2 * v2);

				circle.Normal.Normalize();
				circle.Axis0 = N0;
				circle.Axis1 = circle.Normal.Cross(circle.Axis0);

				return true;
			}
						
						
			/// <summary>
			/// Returns circle perimeter
			/// </summary>
			public float CalcPerimeter()
			{
				return Mathfex.TwoPi * Radius;
			}

			/// <summary>
			/// Returns circle area
			/// </summary>
			public float CalcArea()
			{
				return Mathf.PI * Radius * Radius;
			}

			/// <summary>
			/// Evaluates circle using formula X = C + R*cos(t)*U + R*sin(t)*V
			/// where t is an angle in [0,2*pi).
			/// </summary>
			/// <param name="t">Evaluation parameter</param>
			public Vector3 Eval(float t)
			{
				return Center + Radius * (Mathf.Cos(t) * Axis0 + Mathf.Sin(t) * Axis1);
			}

			/// <summary>
			/// Evaluates disk using formula X = C + radius*cos(t)*U + radius*sin(t)*V
			/// where t is an angle in [0,2*pi).
			/// </summary>
			/// <param name="t">Evaluation parameter</param>
			/// <param name="radius">Evaluation radius</param>
			public Vector3 Eval(float t, float radius)
			{
				return Center + radius * (Mathf.Cos(t) * Axis0 + Mathf.Sin(t) * Axis1);
			}

			/// <summary>
			/// Returns distance to a point, distance is >= 0f.
			/// </summary>
			public float DistanceTo(Vector3 point, bool solid = true)
			{
				return Distance.Point3Circle3(ref point, ref this, solid);
			}

			/// <summary>
			/// Returns projected point
			/// </summary>
			public Vector3 Project(Vector3 point, bool solid = true)
			{
				Vector3 result;
				Distance.SqrPoint3Circle3(ref point, ref this, out result, solid);
				return result;
			}

			/// <summary>
			/// Returns string representation.
			/// </summary>
			public override string ToString()
			{
				return string.Format("[Center: {0} Axis0: {1} Axis1: {2} Normal: {3} Radius: {4}]", Center.ToStringEx(), Axis0.ToStringEx(), Axis1.ToStringEx(), Normal.ToStringEx(), Radius.ToString());
			}
		}
	}
}
