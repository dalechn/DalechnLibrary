using UnityEngine;
using System.Collections.Generic;

namespace Dest.Math
{
	public enum ProjectionPlanes
	{
		XY,
		XZ,
		YZ
	}

	public static class Vector3ex
	{
		internal class Information
		{
			// The intrinsic dimension of the input set.  The parameter 'epsilon'
			// to the GetInformation function is used to provide a tolerance when
			// determining the dimension.
			public int Dimension;

			// Axis-aligned bounding box of the input set.  The maximum range is
			// the larger of max[0]-min[0], max[1]-min[1], and max[2]-min[2].
			public Vector3 Min;
			public Vector3 Max;
			public float   MaxRange;

			// Coordinate system.  The origin is valid for any dimension d.  The
			// unit-length direction vector is valid only for 0 <= i < d.  The
			// extreme index is relative to the array of input points, and is also
			// valid only for 0 <= i < d.  If d = 0, all points are effectively
			// the same, but the use of an epsilon may lead to an extreme index
			// that is not zero.  If d = 1, all points effectively lie on a line
			// segment.  If d = 2, all points effectively line on a plane.  If
			// d = 3, the points are not coplanar.
			public Vector3   Origin;
			public Vector3[] Direction = new Vector3[3];

			// The indices that define the maximum dimensional extents.  The
			// values mExtreme[0] and mExtreme[1] are the indices for the points
			// that define the largest extent in one of the coordinate axis
			// directions.  If the dimension is 2, then mExtreme[2] is the index
			// for the point that generates the largest extent in the direction
			// perpendicular to the line through the points corresponding to
			// mExtreme[0] and mExtreme[1].  Furthermore, if the dimension is 3,
			// then mExtreme[3] is the index for the point that generates the
			// largest extent in the direction perpendicular to the triangle
			// defined by the other extreme points.  The tetrahedron formed by the
			// points V[extreme0], V[extreme1], V[extreme2], V[extreme3]> is
			// clockwise or counterclockwise, the condition stored in mExtremeCCW.
			public int[] Extreme = new int[4];
			public bool  ExtremeCCW;
		}

		public static readonly Vector3 Zero             = new Vector3(0.0f, 0.0f, 0.0f);
		public static readonly Vector3 One              = new Vector3(1.0f, 1.0f, 1.0f);
		public static readonly Vector3 UnitX            = new Vector3(1.0f, 0.0f, 0.0f);
		public static readonly Vector3 UnitY            = new Vector3(0.0f, 1.0f, 0.0f);
		public static readonly Vector3 UnitZ            = new Vector3(0.0f, 0.0f, 1.0f);
		public static readonly Vector3 PositiveInfinity = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		public static readonly Vector3 NegativeInfinity = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
			
		internal static Information GetInformation(IList<Vector3> points, float epsilon)
		{
			if (points == null) return null;
			int numPoints = points.Count;
			if (numPoints == 0 || epsilon < 0f) return null;

			Information info = new Information();
			info.ExtremeCCW = false;

			float temp;

			// Compute the axis-aligned bounding box for the input points.  Keep track
			// of the indices into 'points' for the current min and max.
			float minX      = temp = points[0].x;
			float maxX      = temp;
			int   minXIndex = 0;
			int   maxXIndex = 0;

			float minY      = temp = points[0].y;
			float maxY      = temp;
			int   minYIndex = 0;
			int   maxYIndex = 0;

			float minZ      = temp = points[0].z;
			float maxZ      = temp;
			int   minZIndex = 0;
			int   maxZIndex = 0;

			for (int i = 1; i < numPoints; ++i)
			{
				temp = points[i].x;
				if (temp < minX)
				{
					minX      = temp;
					minXIndex = i;
				}
				else if (temp > maxX)
				{
					maxX      = temp;
					maxXIndex = i;
				}

				temp = points[i].y;
				if (temp < minY)
				{
					minY      = temp;
					minYIndex = i;
				}
				else if (temp > maxY)
				{
					maxY      = temp;
					maxYIndex = i;
				}

				temp = points[i].z;
				if (temp < minZ)
				{
					minZ      = temp;
					minZIndex = i;
				}
				else if (temp > maxZ)
				{
					maxZ      = temp;
					maxZIndex = i;
				}
			}

			info.Min.x = minX;
			info.Min.y = minY;
			info.Min.z = minZ;
			info.Max.x = maxX;
			info.Max.y = maxY;
			info.Max.z = maxZ;

			// Determine the maximum range for the bounding box.
			info.MaxRange = maxX - minX;
			info.Extreme[0] = minXIndex;
			info.Extreme[1] = maxXIndex;
			float range = maxY - minY;
			if (range > info.MaxRange)
			{
				info.MaxRange = range;
				info.Extreme[0] = minYIndex;
				info.Extreme[1] = maxYIndex;
			}
			range = maxZ - minZ;
			if (range > info.MaxRange)
			{
				info.MaxRange = range;
				info.Extreme[0] = minZIndex;
				info.Extreme[1] = maxZIndex;
			}

			// The origin is either the point of minimum x-value, point of
			// minimum y-value, or point of minimum z-value.
			info.Origin = points[info.Extreme[0]];

			// Test whether the point set is (nearly) a point.
			if (info.MaxRange < epsilon)
			{
				info.Dimension = 0;
				info.Extreme[1] = info.Extreme[0];
				info.Extreme[2] = info.Extreme[0];
				info.Extreme[3] = info.Extreme[0];
				info.Direction[0] = Vector3ex.Zero;
				info.Direction[1] = Vector3ex.Zero;
				info.Direction[2] = Vector3ex.Zero;
				return info;
			}

			// Test whether the point set is (nearly) a line segment.
			info.Direction[0] = points[info.Extreme[1]] - info.Origin;
			info.Direction[0].Normalize();
			float maxDistance = 0f;
			float distance, dot;
			info.Extreme[2] = info.Extreme[0];
			for (int i = 0; i < numPoints; ++i)
			{
				Vector3 diff = points[i] - info.Origin;
				dot = info.Direction[0].Dot(diff);
				Vector3 proj = diff - dot * info.Direction[0];
				distance = proj.magnitude;
				if (distance > maxDistance)
				{
					maxDistance = distance;
					info.Extreme[2] = i;
				}
			}

			if (maxDistance < epsilon * info.MaxRange)
			{
				info.Dimension = 1;
				info.Extreme[2] = info.Extreme[1];
				info.Extreme[3] = info.Extreme[1];
				return info;
			}

			// Test whether the point set is (nearly) a planar polygon.
			info.Direction[1] = points[info.Extreme[2]] - info.Origin;
			dot = info.Direction[0].Dot(info.Direction[1]);
			info.Direction[1] -= dot * info.Direction[0];
			info.Direction[1].Normalize();
			info.Direction[2] = info.Direction[0].Cross(info.Direction[1]);
			maxDistance = 0f;
			float maxSign = 0f;
			info.Extreme[3] = info.Extreme[0];
			for (int i = 0; i < numPoints; ++i)
			{
				Vector3 diff = points[i] - info.Origin;
				distance = info.Direction[2].Dot(diff);
				float sign = Mathf.Sign(distance);
				distance = Mathf.Abs(distance);
				if (distance > maxDistance)
				{
					maxDistance = distance;
					maxSign = sign;
					info.Extreme[3] = i;
				}
			}

			if (maxDistance < epsilon * info.MaxRange)
			{
				info.Dimension = 2;
				info.Extreme[3] = info.Extreme[2];
				return info;
			}

			info.Dimension = 3;
			info.ExtremeCCW = maxSign > 0f;

			return info;
		}

		/// <summary>
		/// Returns vector length
		/// </summary>
		public static float Length(this Vector3 vector)
		{
			return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
		}

		/// <summary>
		/// Returns vector squared length
		/// </summary>
		public static float LengthSqr(this Vector3 vector)
		{
			return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
		}

		/// <summary>
		/// Vector dot product
		/// </summary>
		public static float Dot(this Vector3 vector, Vector3 value)
		{
			return vector.x * value.x + vector.y * value.y + vector.z * value.z;
		}

		/// <summary>
		/// Vector dot product
		/// </summary>
		public static float Dot(this Vector3 vector, ref Vector3 value)
		{
			return vector.x * value.x + vector.y * value.y + vector.z * value.z;
		}

		/// <summary>
		/// Vector dot product
		/// </summary>
		public static float Dot(ref Vector3 vector, ref Vector3 value)
		{
			return vector.x * value.x + vector.y * value.y + vector.z * value.z;
		}

		/// <summary>
		/// Returns angle in degrees between this vector and the target vector. Method normalizes input vectors. Result lies in [0..180] range.
		/// </summary>
		public static float AngleDeg(this Vector3 vector, Vector3 target)
		{
			Normalize(ref vector);
			Normalize(ref target);
			float dot = vector.x * target.x + vector.y * target.y + vector.z * target.z;
			if (dot > 1f) dot = 1f; else if (dot < -1f) dot = -1f;
			return Mathf.Acos(dot) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Returns angle in radians between this vector and the target vector. Method normalizes input vectors. Result lies in [0..PI] range.
		/// </summary>
		public static float AngleRad(this Vector3 vector, Vector3 target)
		{
			Normalize(ref vector);
			Normalize(ref target);
			float dot = vector.x * target.x + vector.y * target.y + vector.z * target.z;
			if (dot > 1f) dot = 1f; else if (dot < -1f) dot = -1f;
			return Mathf.Acos(dot);
		}

		/// <summary>
		/// Returns signed angle in degrees between this vector and the target vector. Method normalizes input vectors. Result lies in [-180..180] range.
		/// </summary>
		/// <param name="normal">Vector which defines world 'up'</param>
		public static float SignedAngleDeg(this Vector3 vector, Vector3 target, Vector3 normal)
		{
			Normalize(ref vector);
			Normalize(ref target);
			float dot = vector.x * target.x + vector.y * target.y + vector.z * target.z;
			if (dot > 1f) dot = 1f; else if (dot < -1f) dot = -1f;
			float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
			Vector3 cross = vector.Cross(target);
			if (normal.Dot(cross) < 0f)
			{
				angle = -angle;
			}
			return angle;
		}

		/// <summary>
		/// Returns signed angle in radians between this vector and the target vector. Method normalizes input vectors. Reults lies in [-PI..PI] range.
		/// </summary>
		/// <param name="normal">Vector which defines world 'up'</param>
		public static float SignedAngleRad(this Vector3 vector, Vector3 target, Vector3 normal)
		{
			Normalize(ref vector);
			Normalize(ref target);
			float dot = vector.x * target.x + vector.y * target.y + vector.z * target.z;
			if (dot > 1f) dot = 1f; else if (dot < -1f) dot = -1f;
			float angle = Mathf.Acos(dot);
			Vector3 cross = vector.Cross(target);
			if (normal.Dot(cross) < 0f)
			{
				angle = -angle;
			}
			return angle;
		}

		/// <summary>
		/// Vector cross product
		/// </summary>
		public static Vector3 Cross(this Vector3 vector, Vector3 value)
		{
			return new Vector3
			(
				vector.y * value.z - vector.z * value.y,
				vector.z * value.x - vector.x * value.z,
				vector.x * value.y - vector.y * value.x
			);
		}

		/// <summary>
		/// Vector cross product
		/// </summary>
		public static Vector3 Cross(this Vector3 vector, ref Vector3 value)
		{
			return new Vector3
			(
				vector.y * value.z - vector.z * value.y,
				vector.z * value.x - vector.x * value.z,
				vector.x * value.y - vector.y * value.x
			);
		}

		/// <summary>
		/// Vector cross product
		/// </summary>
		public static Vector3 Cross(ref Vector3 vector, ref Vector3 value)
		{
			return new Vector3
			(
				vector.y * value.z - vector.z * value.y,
				vector.z * value.x - vector.x * value.z,
				vector.x * value.y - vector.y * value.x
			);
		}

		/// <summary>
		/// Returns normalized cross product of vectors
		/// </summary>
		public static Vector3 UnitCross(this Vector3 vector, Vector3 value)
		{
			Vector3 cross = new Vector3
			(
				vector.y * value.z - vector.z * value.y,
				vector.z * value.x - vector.x * value.z,
				vector.x * value.y - vector.y * value.x
			);
			Normalize(ref cross);
			return cross;
		}

		/// <summary>
		/// Returns normalized cross product of vectors
		/// </summary>
		public static Vector3 UnitCross(this Vector3 vector, ref Vector3 value)
		{
			Vector3 cross = new Vector3
			(
				vector.y * value.z - vector.z * value.y,
				vector.z * value.x - vector.x * value.z,
				vector.x * value.y - vector.y * value.x
			);
			Normalize(ref cross);
			return cross;
		}

		/// <summary>
		/// Returns normalized cross product of vectors
		/// </summary>
		public static Vector3 UnitCross(ref Vector3 vector, ref Vector3 value)
		{
			Vector3 cross = new Vector3
			(
				vector.y * value.z - vector.z * value.y,
				vector.z * value.x - vector.x * value.z,
				vector.x * value.y - vector.y * value.x
			);
			Normalize(ref cross);
			return cross;
		}

		/// <summary>
		/// Normalizes given vector and returns it's length before normalization.
		/// </summary>
		public static float Normalize(ref Vector3 vector, float epsilon = Mathfex.ZeroTolerance)
		{
			float length = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
				
			if (length >= epsilon)
			{
				float invLength = 1f / length;
				vector.x *= invLength;
				vector.y *= invLength;
				vector.z *= invLength;
				return length;
			}
				
			vector.x = 0f;
			vector.y = 0f;
			vector.z = 0f;
			return 0f;
		}

		/// <summary>
		/// Sets vector length to the given value. Returns new vector length or 0 if vector's initial length is less than epsilon.
		/// </summary>
		public static float SetLength(ref Vector3 vector, float lengthValue, float epsilon = Mathfex.ZeroTolerance)
		{
			float length = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);

			if (length >= epsilon)
			{
				float invLength = lengthValue / length;
				vector.x *= invLength;
				vector.y *= invLength;
				vector.z *= invLength;
				return lengthValue;
			}
				
			vector.x = 0f;
			vector.y = 0f;
			vector.z = 0f;
			return 0f;
		}

		/// <summary>
		/// Changes vector length adding given value. Returns new vector length or 0 if vector's initial length is less than epsilon.
		/// </summary>
		public static float GrowLength(ref Vector3 vector, float lengthDelta, float epsilon = Mathfex.ZeroTolerance)
		{
			float length = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
				
			if (length >= epsilon)
			{
				float newLength = length + lengthDelta;
				float invLength = newLength / length;
				vector.x *= invLength;
				vector.y *= invLength;
				vector.z *= invLength;
				return newLength;
			}

			vector.x = 0f;
			vector.y = 0f;
			vector.z = 0f;
			return 0f;
		}

		/// <summary>
		/// Creates a vector with all components equal to value
		/// </summary>
		public static Vector3 Replicate(float value)
		{
			return new Vector3(value, value, value);
		}

		/// <summary>
		/// Input W must be a unit-length vector. The output vectors {U,V} are
		/// unit length and mutually perpendicular, and {U,V,W} is an orthonormal basis.
		/// </summary>
		public static void CreateOrthonormalBasis(out Vector3 u, out Vector3 v, ref Vector3 w)
		{
			if (Mathf.Abs(w.x) >= Mathf.Abs(w.y))
			{
				// W.x or W.z is the largest magnitude component, swap them
				float invLength = Mathfex.InvSqrt(w.x * w.x + w.z * w.z);
					
				u.x = -w.z * invLength;
				u.y = 0.0f;
				u.z = w.x * invLength;
					
				v.x =  w.y * u.z;
				v.y =  w.z * u.x - w.x * u.z;
				v.z = -w.y * u.x;
			}
			else
			{
				// W.y or W.z is the largest magnitude component, swap them
				float invLength = Mathfex.InvSqrt(w.y * w.y + w.z * w.z);

				u.x = 0.0f;
				u.y =  w.z * invLength;
				u.z = -w.y * invLength;
					
				v.x =  w.y * u.z - w.z * u.y;
				v.y = -w.x * u.z;
				v.z =  w.x * u.y;
			}
		}

		/// <summary>
		/// Returns true if Dot(value0,value1) > 0
		/// </summary>
		public static bool SameDirection(Vector3 value0, Vector3 value1)
		{
			return value0.Dot(value1) > Mathfex.ZeroTolerance;
		}

		/// <summary>
		/// Converts Vector3 to Vector2, copying x and y components of Vector3 to x and y components of Vector2 respectively.
		/// </summary>
		public static Vector2 ToVector2XY(this Vector3 vector)
		{
			return new Vector2(vector.x, vector.y);
		}

		/// <summary>
		/// Converts Vector3 to Vector2, copying x and z components of Vector3 to x and y components of Vector2 respectively.
		/// </summary>
		public static Vector2 ToVector2XZ(this Vector3 vector)
		{
			return new Vector2(vector.x, vector.z);
		}

		/// <summary>
		/// Converts Vector3 to Vector2, copying y and z components of Vector3 to x and y components of Vector2 respectively.
		/// </summary>
		public static Vector2 ToVector2YZ(this Vector3 vector)
		{
			return new Vector2(vector.y, vector.z);
		}

		/// <summary>
		/// Converts Vector3 to Vector2 using specified projection plane.
		/// </summary>
		public static Vector2 ToVector2(this Vector3 vector, ProjectionPlanes projectionPlane)
		{
			if (projectionPlane == ProjectionPlanes.XY)
			{
				return new Vector2(vector.x, vector.y);
			}
			else if (projectionPlane == ProjectionPlanes.XZ)
			{
				return new Vector2(vector.x, vector.z);
			}
			else // YZ
			{
				return new Vector2(vector.y, vector.z);
			}
		}

		/// <summary>
		/// Returns most appropriate projection plane considering vector as a normal (e.g. if y component is largest, then XZ plane is returned).
		/// </summary>
		public static ProjectionPlanes GetProjectionPlane(this Vector3 vector)
		{
			ProjectionPlanes result = ProjectionPlanes.YZ;
			float fmax = Mathf.Abs(vector.x);
			float absMax = Mathf.Abs(vector.y);
			if (absMax > fmax)
			{
				result = ProjectionPlanes.XZ;
				fmax = absMax;
			}
			absMax = Mathf.Abs(vector.z);
			if (absMax > fmax)
			{
				result = ProjectionPlanes.XY;
			}
			return result;
		}

		/// <summary>
		/// Returns string representation (does not round components as standard Vector3.ToString() does)
		/// </summary>
		public static string ToStringEx(this Vector3 vector)
		{
			return string.Format("({0}, {1}, {2})", vector.x.ToString(), vector.y.ToString(), vector.z.ToString());
		}
	}
}
