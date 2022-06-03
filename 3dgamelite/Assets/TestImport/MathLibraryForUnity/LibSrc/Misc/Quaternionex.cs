using UnityEngine;

namespace Dest.Math
{
	public static class Quaternionex
	{
		/// <summary>
		/// Calculates difference from this quaternion to given target quaternion. I.e. if you have quaternions Q1 and Q2,
		/// this method will return quaternion Q such that Q2 == Q * Q1 (remember that quaternions are multiplied right-to-left).
		/// </summary>
		public static Quaternion DeltaTo(this Quaternion quat, Quaternion target)
		{
			return target * Quaternion.Inverse(quat);
		}

		/// <summary>
		/// Returns string representation (does not round components as standard Quaternion.ToString() does)
		/// </summary>
		public static string ToStringEx(this Quaternion quat)
		{
			return string.Format("[{0}, {1}, {2}, {3}]", quat.x.ToString(), quat.y.ToString(), quat.z.ToString(), quat.w.ToString());
		}
	}
}
