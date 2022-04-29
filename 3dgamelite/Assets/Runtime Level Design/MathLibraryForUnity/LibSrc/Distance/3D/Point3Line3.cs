using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Distance
		{
			/// <summary>
			/// Returns distance between a point and a line
			/// </summary>
			public static float Point3Line3(ref Vector3 point, ref Line3 line)
			{
				return Mathf.Sqrt(SqrPoint3Line3(ref point, ref line));
			}

			/// <summary>
			/// Returns distance between a point and a line
			/// </summary>
			/// <param name="closestPoint">Point projected on a line</param>
			public static float Point3Line3(ref Vector3 point, ref Line3 line, out Vector3 closestPoint)
			{
				return Mathf.Sqrt(SqrPoint3Line3(ref point, ref line, out closestPoint));
			}


			/// <summary>
			/// Returns squared distance between a point and a line
			/// </summary>
			public static float SqrPoint3Line3(ref Vector3 point, ref Line3 line)
			{
				Vector3 diff = point - line.Center;
				float param = line.Direction.Dot(diff);
				Vector3 closestPoint = line.Center + param * line.Direction;
				diff = closestPoint - point;
				return diff.sqrMagnitude;
			}

			/// <summary>
			/// Returns squared distance between a point and a line
			/// </summary>
			/// <param name="closestPoint">Point projected on a line</param>
			public static float SqrPoint3Line3(ref Vector3 point, ref Line3 line, out Vector3 closestPoint)
			{
				Vector3 diff = point - line.Center;
				float param = line.Direction.Dot(diff);
				closestPoint = line.Center + param * line.Direction;
				diff = closestPoint - point;
				return diff.sqrMagnitude;
			}
		}
	}
}