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
			public static float Line2Line2(ref Line2 line0, ref Line2 line1)
			{
				return Mathf.Sqrt(SqrLine2Line2(ref line0, ref line1));
			}

			/// <summary>
			/// Returns distance between two lines.
			/// </summary>
			/// <param name="closestPoint0">Point on line0 closest to line1</param>
			/// <param name="closestPoint1">Point on line1 closest to line0</param>
			public static float Line2Line2(ref Line2 line0, ref Line2 line1, out Vector2 closestPoint0, out Vector2 closestPoint1)
			{
				return Mathf.Sqrt(SqrLine2Line2(ref line0, ref line1, out closestPoint0, out closestPoint1));
			}


			/// <summary>
			/// Returns squared distance between two lines.
			/// </summary>
			public static float SqrLine2Line2(ref Line2 line0, ref Line2 line1)
			{
				Vector2 diff = line0.Center - line1.Center;
				float a01 = -line0.Direction.Dot(line1.Direction);
				float b0 = diff.Dot(line0.Direction);
				float c = diff.sqrMagnitude;
				float det = Mathf.Abs(1f - a01 * a01);
				float s0, sqrDist;

				if (det >= Mathfex.ZeroTolerance)
				{
					// Lines are not parallel.
					sqrDist = 0f;
				}
				else
				{
					// Lines are parallel, select any closest pair of points.
					s0 = -b0;
					sqrDist = b0 * s0 + c;

					// Account for numerical round-off errors.
					if (sqrDist < 0f)
					{
						sqrDist = 0f;
					}
				}

				return sqrDist;
			}

			/// <summary>
			/// Returns squared distance between two lines.
			/// </summary>
			/// <param name="closestPoint0">Point on line0 closest to line1</param>
			/// <param name="closestPoint1">Point on line1 closest to line0</param>
			public static float SqrLine2Line2(ref Line2 line0, ref Line2 line1, out Vector2 closestPoint0, out Vector2 closestPoint1)
			{
				Vector2 diff = line0.Center - line1.Center;
				float a01 = -line0.Direction.Dot(line1.Direction);
				float b0 = diff.Dot(line0.Direction);
				float c = diff.sqrMagnitude;
				float det = Mathf.Abs(1f - a01 * a01);
				float b1, s0, s1, sqrDist;

				if (det >= Mathfex.ZeroTolerance)
				{
					// Lines are not parallel.
					b1 = -diff.Dot(line1.Direction);
					float invDet = 1f / det;
					s0 = (a01 * b1 - b0) * invDet;
					s1 = (a01 * b0 - b1) * invDet;
					sqrDist = 0f;
				}
				else
				{
					// Lines are parallel, select any closest pair of points.
					s0 = -b0;
					s1 = 0f;
					sqrDist = b0 * s0 + c;

					// Account for numerical round-off errors.
					if (sqrDist < 0f)
					{
						sqrDist = 0f;
					}
				}

				closestPoint0 = line0.Center + s0 * line0.Direction;
				closestPoint1 = line1.Center + s1 * line1.Direction;
				return sqrDist;
			}
		}
	}
}