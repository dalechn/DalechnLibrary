using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between two lines.
			/// </summary>
			public static float Line3Line3(ref Line3 line0, ref Line3 line1)
			{
				Vector3 closestPoint0, closestPoint1;
				return Mathf.Sqrt(SqrLine3Line3(ref line0, ref line1, out closestPoint0, out closestPoint1));
			}

			/// <summary>
			/// Returns distance between two lines.
			/// </summary>
			/// <param name="closestPoint0">Point on line0 closest to line1</param>
			/// <param name="closestPoint1">Point on line1 closest to line0</param>
			public static float Line3Line3(ref Line3 line0, ref Line3 line1, out Vector3 closestPoint0, out Vector3 closestPoint1)
			{
				return Mathf.Sqrt(SqrLine3Line3(ref line0, ref line1, out closestPoint0, out closestPoint1));
			}


			/// <summary>
			/// Returns squared distance between two lines.
			/// </summary>
			public static float SqrLine3Line3(ref Line3 line0, ref Line3 line1)
			{
				Vector3 closestPoint0, closestPoint1;
				return SqrLine3Line3(ref line0, ref line1, out closestPoint0, out closestPoint1);
			}

			/// <summary>
			/// Returns squared distance between two lines.
			/// </summary>
			/// <param name="closestPoint0">Point on line0 closest to line1</param>
			/// <param name="closestPoint1">Point on line1 closest to line0</param>
			public static float SqrLine3Line3(ref Line3 line0, ref Line3 line1, out Vector3 closestPoint0, out Vector3 closestPoint1)
			{
				Vector3 diff = line0.Center - line1.Center;
				float a01 = -line0.Direction.Dot(line1.Direction);
				float b0 = diff.Dot(line0.Direction);
				float c = diff.sqrMagnitude;
				float det = Mathf.Abs((float)1 - a01 * a01);
				float b1, s0, s1, sqrDist;

				if (det >= Mathfex.ZeroTolerance)
				{
					// Lines are not parallel.
					b1 = -diff.Dot(line1.Direction);
					float invDet = ((float)1) / det;
					s0 = (a01 * b1 - b0) * invDet;
					s1 = (a01 * b0 - b1) * invDet;
					sqrDist = s0 * (s0 + a01 * s1 + ((float)2) * b0) +
						s1 * (a01 * s0 + s1 + ((float)2) * b1) + c;
				}
				else
				{
					// Lines are parallel, select any closest pair of points.
					s0 = -b0;
					s1 = (float)0;
					sqrDist = b0 * s0 + c;
				}

				closestPoint0 = line0.Center + s0 * line0.Direction;
				closestPoint1 = line1.Center + s1 * line1.Direction;

				// Account for numerical round-off errors.
				if (sqrDist < (float)0)
				{
					sqrDist = (float)0;
				}
				return sqrDist;
			}
		}
	}
}