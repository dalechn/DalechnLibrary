using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a box intersects another box. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestBox2Box2(ref Box2 box0, ref Box2 box1)
			{
				Vector2 A0 = box0.Axis0;
				Vector2 A1 = box0.Axis1;
				Vector2 B0 = box1.Axis0;
				Vector2 B1 = box1.Axis1;

				float EA0 = box0.Extents.x;
				float EA1 = box0.Extents.y;
				float EB0 = box1.Extents.x;
				float EB1 = box1.Extents.y;

				// Compute difference of box centers, D = C1-C0.
				Vector2 D = box1.Center - box0.Center;

				float AbsAdB00, AbsAdB01, AbsAdB10, AbsAdB11;
				float AbsAdD, RSum;
    
				
				// axis C0+t*A0
				AbsAdB00 = Mathf.Abs(A0.Dot(B0));
				AbsAdB01 = Mathf.Abs(A0.Dot(B1));
				AbsAdD   = Mathf.Abs(A0.Dot(D));
				RSum     = EA0 + EB0 * AbsAdB00 + EB1 * AbsAdB01;
				if (AbsAdD > RSum)
				{
					return false;
				}

				// axis C0+t*A1
				AbsAdB10 = Mathf.Abs(A1.Dot(B0));
				AbsAdB11 = Mathf.Abs(A1.Dot(B1));
				AbsAdD   = Mathf.Abs(A1.Dot(D));
				RSum     = EA1 + EB0 * AbsAdB10 + EB1 * AbsAdB11;
				if (AbsAdD > RSum)
				{
					return false;
				}


				// axis C0+t*B0
				AbsAdD = Mathf.Abs(B0.Dot(D));
				RSum   = EB0 + EA0 * AbsAdB00 + EA1 * AbsAdB10;
				if (AbsAdD > RSum)
				{
					return false;
				}

				// axis C0+t*B1
				AbsAdD = Mathf.Abs(B1.Dot(D));
				RSum   = EB1 + EA0 * AbsAdB01 + EA1 * AbsAdB11;
				if (AbsAdD > RSum)
				{
					return false;
				}

				return true;
			}
		}
	}
}
