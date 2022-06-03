using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// The plane is represented as Dot(N,X) = c where N is a unit-length
		/// normal vector, c is the plane constant, and X is any point on the
		/// plane.  The user must ensure that the normal vector is unit length.
		/// </summary>
		public struct Plane3
		{
			/// <summary>
			/// Plane normal. Must be unit length!
			/// </summary>
			public Vector3 Normal;

			/// <summary>
			/// Plane constant c from the equation Dot(N,X) = c
			/// </summary>
			public float Constant;


			/// <summary>
			/// Creates the plane by specifying N and c directly.
			/// </summary>
			/// <param name="normal">Must be unit length!</param>
			public Plane3(ref Vector3 normal, float constant)
			{
				Normal = normal;
				Constant = constant;
			}

			/// <summary>
			/// Creates the plane by specifying N and c directly.
			/// </summary>
			/// <param name="normal">Must be unit length!</param>
			public Plane3(Vector3 normal, float constant)
			{
				Normal = normal;
				Constant = constant;
			}

			/// <summary>
			/// N is specified, c = Dot(N,P) where P is a point on the plane.
			/// </summary>
			/// <param name="normal">Must be unit length!</param>
			public Plane3(ref Vector3 normal, ref Vector3 point)
			{
				Normal = normal;
				Constant = normal.Dot(point);
			}

			/// <summary>
			/// N is specified, c = Dot(N,P) where P is a point on the plane.
			/// </summary>
			/// <param name="normal">Must be unit length!</param>
			public Plane3(Vector3 normal, Vector3 point)
			{
				Normal = normal;
				Constant = normal.Dot(point);
			}

			/// <summary>
			/// Creates the plane from 3 points.
			/// N = Cross(P1-P0,P2-P0)/Length(Cross(P1-P0,P2-P0)), c = Dot(N,P0) where
			/// P0, P1, P2 are points on the plane.
			/// </summary>
			public Plane3(ref Vector3 p0, ref Vector3 p1, ref Vector3 p2)
			{
				Vector3 edge1 = p1 - p0;
				Vector3 edge2 = p2 - p0;
				Normal = edge1.UnitCross(edge2);
				Constant = Normal.Dot(p0);
			}

			/// <summary>
			/// Creates the plane from 3 points.
			/// N = Cross(P1-P0,P2-P0)/Length(Cross(P1-P0,P2-P0)), c = Dot(N,P0) where
			/// P0, P1, P2 are points on the plane.
			/// </summary>
			public Plane3(Vector3 p0, Vector3 p1, Vector3 p2)
			{
				Vector3 edge1 = p1 - p0;
				Vector3 edge2 = p2 - p0;
				Normal = edge1.UnitCross(edge2);
				Constant = Normal.Dot(p0);
			}			

			/// <summary>
			/// Converts Plane3 to UnityEngine.Plane
			/// </summary>
			public static implicit operator Plane(Plane3 value)
			{
				return new Plane(value.Normal, -value.Constant);
			}

			/// <summary>
			/// Converts UnityEngine.Plane to Plane3
			/// </summary>
			public static implicit operator Plane3(Plane value)
			{
				return new Plane3() { Normal = value.normal, Constant = -value.distance };
			}

			
			/// <summary>
			/// Returns N*c
			/// </summary>
			public Vector3 CalcOrigin()
			{
				return Normal * Constant;
			}

			/// <summary>
			/// Creates orthonormal basis from plane. In the output n - is the plane normal.
			/// </summary>
			public void CreateOrthonormalBasis(out Vector3 u, out Vector3 v, out Vector3 n)
			{
				n = Normal;

				if (Mathf.Abs(n.x) >= Mathf.Abs(n.y))
				{
					// N.x or N.z is the largest magnitude component
					float invLength = Mathfex.InvSqrt(n.x * n.x + n.z * n.z);

					u.x = n.z * invLength;
					u.y = 0.0f;
					u.z = -n.x * invLength;
				}
				else
				{
					// N.y or N.z is the largest magnitude component
					float invLength = Mathfex.InvSqrt(n.y * n.y + n.z * n.z);

					u.x = 0.0f;
					u.y = n.z * invLength;
					u.z = -n.y * invLength;
				}

				v = Vector3.Cross(n, u); // automatically unit length
			}

			/// <summary>
			/// Compute d = Dot(N,P)-c where N is the plane normal and c is the plane
			/// constant.  This is a signed distance.  The sign of the return value is
			/// positive if the point is on the positive side of the plane, negative if
			/// the point is on the negative side, and zero if the point is on the plane.
			/// </summary>
			internal float SignedDistanceTo(ref Vector3 point)
			{
				return Normal.Dot(point) - Constant;
			}

			/// <summary>
			/// Compute d = Dot(N,P)-c where N is the plane normal and c is the plane
			/// constant.  This is a signed distance.  The sign of the return value is
			/// positive if the point is on the positive side of the plane, negative if
			/// the point is on the negative side, and zero if the point is on the plane.
			/// </summary>
			public float SignedDistanceTo(Vector3 point)
			{
				return Normal.Dot(point) - Constant;
			}

			/// <summary>
			/// Returns distance to a point, distance is >= 0f.
			/// </summary>
			public float DistanceTo(Vector3 point)
			{
				return Mathf.Abs(Normal.Dot(point) - Constant);
			}

			/// <summary>
			/// Determines on which side of the plane a point is. Returns +1 if a point
			/// is on the positive side of the plane, 0 if it's on the plane, -1 if it's on the negative side.
			/// The positive side of the plane is the half-space to which the plane normal points.
			/// </summary>
			public int  QuerySide(Vector3 point, float epsilon = Mathfex.ZeroTolerance)
			{
				float signedDistance = Normal.Dot(point) - Constant;
				if (signedDistance < -epsilon)
				{
					return -1;
				}
				else if (signedDistance > epsilon)
				{
					return 1;
				}
				return 0;
			}

			/// <summary>
			/// Returns true if a point is on the negative side of the plane, false otherwise.
			/// </summary>
			public bool QuerySideNegative(Vector3 point, float epsilon = Mathfex.ZeroTolerance)
			{
				float signedDistance = Normal.Dot(point) - Constant;
				return signedDistance <= epsilon;
			}

			/// <summary>
			/// Returns true if a point is on the positive side of the plane, false otherwise.
			/// </summary>
			public bool QuerySidePositive(Vector3 point, float epsilon = Mathfex.ZeroTolerance)
			{
				float signedDistance = Normal.Dot(point) - Constant;
				return signedDistance >= -epsilon;
			}

			/// <summary>
			/// Determines on which side of the plane a box is. Returns +1 if a box
			/// is on the positive side of the plane, 0 if it's intersecting the plane, -1 if it's on the negative side.
			/// The positive side of the plane is the half-space to which the plane normal points.
			/// </summary>
			public int  QuerySide(ref Box3 box, float epsilon = Mathfex.ZeroTolerance)
			{
				float tmp0 = box.Extents.x * (Normal.Dot(box.Axis0));
				float tmp1 = box.Extents.y * (Normal.Dot(box.Axis1));
				float tmp2 = box.Extents.z * (Normal.Dot(box.Axis2));

				float radius = Mathf.Abs(tmp0) + Mathf.Abs(tmp1) + Mathf.Abs(tmp2);
				float signedDistance = Normal.Dot(box.Center) - Constant;
				return signedDistance < -radius + epsilon ? -1 : (signedDistance > radius - epsilon ? 1 : 0);
			}

			/// <summary>
			/// Returns true if a box is on the negative side of the plane, false otherwise.
			/// </summary>
			public bool QuerySideNegative(ref Box3 box, float epsilon = Mathfex.ZeroTolerance)
			{
				float tmp0 = box.Extents.x * (Normal.Dot(box.Axis0));
				float tmp1 = box.Extents.y * (Normal.Dot(box.Axis1));
				float tmp2 = box.Extents.z * (Normal.Dot(box.Axis2));

				float radius = Mathf.Abs(tmp0) + Mathf.Abs(tmp1) + Mathf.Abs(tmp2);
				float signedDistance = Normal.Dot(box.Center) - Constant;
				return signedDistance <= -radius + epsilon;
			}

			/// <summary>
			/// Returns true if a box is on the positive side of the plane, false otherwise.
			/// </summary>
			public bool QuerySidePositive(ref Box3 box, float epsilon = Mathfex.ZeroTolerance)
			{
				float tmp0 = box.Extents.x * (Normal.Dot(box.Axis0));
				float tmp1 = box.Extents.y * (Normal.Dot(box.Axis1));
				float tmp2 = box.Extents.z * (Normal.Dot(box.Axis2));

				float radius = Mathf.Abs(tmp0) + Mathf.Abs(tmp1) + Mathf.Abs(tmp2);
				float signedDistance = Normal.Dot(box.Center) - Constant;
				return signedDistance >= radius - epsilon;
			}

			/// <summary>
			/// Determines on which side of the plane a box is. Returns +1 if a box
			/// is on the positive side of the plane, 0 if it's intersecting the plane, -1 if it's on the negative side.
			/// The positive side of the plane is the half-space to which the plane normal points.
			/// </summary>
			public int  QuerySide(ref AAB3 box, float epsilon = Mathfex.ZeroTolerance)
			{
				Vector3 dMin, dMax;

				if (Normal.x >= 0.0f)
				{
					dMin.x = box.Min.x;
					dMax.x = box.Max.x;
				}
				else
				{
					dMin.x = box.Max.x;
					dMax.x = box.Min.x;
				}

				if (Normal.y >= 0.0f)
				{
					dMin.y = box.Min.y;
					dMax.y = box.Max.y;
				}
				else
				{
					dMin.y = box.Max.y;
					dMax.y = box.Min.y;
				}

				if (Normal.z >= 0.0f)
				{
					dMin.z = box.Min.z;
					dMax.z = box.Max.z;
				}
				else
				{
					dMin.z = box.Max.z;
					dMax.z = box.Min.z;
				}

				if ((Normal.Dot(dMin) - Constant) > -epsilon)
				{
					return 1;
				}
				else if ((Normal.Dot(dMax) - Constant) < epsilon)
				{
					return -1;
				}
				return 0;
			}

			/// <summary>
			/// Returns true if a box is on the negative side of the plane, false otherwise.
			/// </summary>
			public bool QuerySideNegative(ref AAB3 box, float epsilon = Mathfex.ZeroTolerance)
			{
				Vector3 dMax;

				if (Normal.x >= 0.0f)
				{
					dMax.x = box.Max.x;
				}
				else
				{
					dMax.x = box.Min.x;
				}

				if (Normal.y >= 0.0f)
				{
					dMax.y = box.Max.y;
				}
				else
				{
					dMax.y = box.Min.y;
				}

				if (Normal.z >= 0.0f)
				{
					dMax.z = box.Max.z;
				}
				else
				{
					dMax.z = box.Min.z;
				}

				return (Normal.Dot(dMax) - Constant) <= epsilon;
			}

			/// <summary>
			/// Returns true if a box is on the positive side of the plane, false otherwise.
			/// </summary>
			public bool QuerySidePositive(ref AAB3 box, float epsilon = Mathfex.ZeroTolerance)
			{
				Vector3 dMin;

				if (Normal.x >= 0.0f)
				{
					dMin.x = box.Min.x;
				}
				else
				{
					dMin.x = box.Max.x;
				}

				if (Normal.y >= 0.0f)
				{
					dMin.y = box.Min.y;
				}
				else
				{
					dMin.y = box.Max.y;
				}

				if (Normal.z >= 0.0f)
				{
					dMin.z = box.Min.z;
				}
				else
				{
					dMin.z = box.Max.z;
				}

				return (Normal.Dot(dMin) - Constant) >= -epsilon;
			}

			/// <summary>
			/// Determines on which side of the plane a sphere is. Returns +1 if a sphere
			/// is on the positive side of the plane, 0 if it's intersecting the plane, -1 if it's on the negative side.
			/// The positive side of the plane is the half-space to which the plane normal points.
			/// </summary>
			public int  QuerySide(ref Sphere3 sphere, float epsilon = Mathfex.ZeroTolerance)
			{
				float signedDistance = Normal.Dot(sphere.Center) - Constant;
				return signedDistance > sphere.Radius - epsilon ? 1 : (signedDistance < -sphere.Radius + epsilon ? -1 : 0);
			}

			/// <summary>
			/// Returns true if a sphere is on the negative side of the plane, false otherwise.
			/// </summary>
			public bool QuerySideNegative(ref Sphere3 sphere, float epsilon = Mathfex.ZeroTolerance)
			{
				float signedDistance = Normal.Dot(sphere.Center) - Constant;
				return signedDistance <= -sphere.Radius + epsilon;
			}

			/// <summary>
			/// Returns true if a sphere is on the positive side of the plane, false otherwise.
			/// </summary>
			public bool QuerySidePositive(ref Sphere3 sphere, float epsilon = Mathfex.ZeroTolerance)
			{
				float signedDistance = Normal.Dot(sphere.Center) - Constant;
				return signedDistance >= sphere.Radius - epsilon;
			}

			/// <summary>
			/// Returns projected point
			/// </summary>
			public Vector3 Project(Vector3 point)
			{
				Vector3 result;
				Distance.SqrPoint3Plane3(ref point, ref this, out result);
				return result;
			}

			/// <summary>
			/// Returns projected vector
			/// </summary>
			public Vector3 ProjectVector(Vector3 vector)
			{
				return vector - Normal.Dot(vector) * Normal;
			}

			/// <summary>
			/// Returns angle in radians between plane normal and line direction which is: arccos(dot(normal,direction))
			/// </summary>
			public float AngleBetweenPlaneNormalAndLine(Line3 line)
			{
				float dot = Normal.Dot(line.Direction);
				if (dot > 1f) dot = 1f; else if (dot < -1f) dot = -1f;
				return Mathf.Acos(dot);
			}

			/// <summary>
			/// Returns angle in radians between plane normal and line direction which is: arccos(dot(normal,direction)). Direction will be normalized.
			/// </summary>
			public float AngleBetweenPlaneNormalAndLine(Vector3 direction)
			{
				Vector3ex.Normalize(ref direction);
				float dot = Normal.Dot(direction);
				if (dot > 1f) dot = 1f; else if (dot < -1f) dot = -1f;
				return Mathf.Acos(dot);
			}

			/// <summary>
			/// Returns angle between plane itself and line direction which is: pi/2 - arccos(dot(normal,direction))
			/// </summary>
			public float AngleBetweenPlaneAndLine(Line3 line)
			{
				float dot = Normal.Dot(line.Direction);
				if (dot > 1f) dot = 1f; else if (dot < -1f) dot = -1f;
				return Mathfex.HalfPi - Mathf.Acos(dot);
			}

			/// <summary>
			/// Returns angle in radians between plane itself and direction which is: pi/2 - arccos(dot(normal,direction)).  Direction will be normalized.
			/// </summary>
			public float AngleBetweenPlaneAndLine(Vector3 direction)
			{
				Vector3ex.Normalize(ref direction);
				float dot = Normal.Dot(direction);
				if (dot > 1f) dot = 1f; else if (dot < -1f) dot = -1f;
				return Mathfex.HalfPi - Mathf.Acos(dot);
			}

			/// <summary>
			/// Returns angle in radians between this plane's normal and another plane's normal as: arccos(dot(this.Normal,another.Normal))
			/// </summary>
			public float AngleBetweenTwoPlanes(Plane3 anotherPlane)
			{
				float dot = Normal.Dot(anotherPlane.Normal);
				if (dot > 1f) dot = 1f; else if (dot < -1f) dot = -1f;
				return Mathf.Acos(dot);
			}

			/// <summary>
			/// Returns string representation.
			/// </summary>
			public override string ToString()
			{
				return string.Format("[Normal: {0} Constant: {1}]", Normal.ToStringEx(), Constant.ToString());
			}
		}
	}
}
