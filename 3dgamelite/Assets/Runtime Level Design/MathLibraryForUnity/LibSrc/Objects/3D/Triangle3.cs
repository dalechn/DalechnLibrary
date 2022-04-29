using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public struct Triangle3
		{
			/// <summary>
			/// First triangle vertex
			/// </summary>
			public Vector3 V0;

			/// <summary>
			/// Second triangle vertex
			/// </summary>
			public Vector3 V1;

			/// <summary>
			/// Third triangle vertex
			/// </summary>
			public Vector3 V2;

			/// <summary>
			/// Gets or sets triangle vertex by index: 0, 1 or 2
			/// </summary>
			public Vector3 this[int index]
			{
				get { switch (index) { case 0: return V0; case 1: return V1; case 2: return V2; default: return Vector3.zero; } }
				set { switch (index) { case 0: V0 = value; return; case 1: V1 = value; return; case 2: V2 = value; return; } }
			}


			/// <summary>
			/// Creates Triangle3 from 3 vertices
			/// </summary>
			public Triangle3(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
			{
				V0 = v0;
				V1 = v1;
				V2 = v2;
			}

			/// <summary>
			/// Creates Triangle3 from 3 vertices
			/// </summary>
			public Triangle3(Vector3 v0, Vector3 v1, Vector3 v2)
			{
				V0 = v0;
				V1 = v1;
				V2 = v2;
			}			

			
			/// <summary>
			/// Returns triangle edge by index 0, 1 or 2
			/// Edge[i] = V[i+1]-V[i]
			/// </summary>
			public Vector3 CalcEdge(int edgeIndex)
			{
				switch (edgeIndex)
				{
					case 0: return V1 - V0;
					case 1: return V2 - V1;
					case 2: return V0 - V2;
				}
				return Vector2.zero;
			}

			/// <summary>
			/// Returns triangle normal as (V1-V0)x(V2-V0)
			/// </summary>
			public Vector3 CalcNormal()
			{
				return Vector3.Cross(V1 - V0, V2 - V0);
			}

			/// <summary>
			/// Returns triangle area as 0.5*Abs(Length((V1-V0)x(V2-V0)))
			/// </summary>
			public float CalcArea()
			{
				return 0.5f * Vector3.Cross(V1 - V0, V2 - V0).magnitude;
			}

			/// <summary>
			/// Returns triangle area defined by 3 points.
			/// </summary>
			public static float CalcArea(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
			{
				return 0.5f * Vector3.Cross(v1 - v0, v2 - v0).magnitude;
			}

			/// <summary>
			/// Returns triangle area defined by 3 points.
			/// </summary>
			public static float CalcArea(Vector3 v0, Vector3 v1, Vector3 v2)
			{
				return 0.5f * Vector3.Cross(v1 - v0, v2 - v0).magnitude;
			}

			/// <summary>
			/// Calculates angles of the triangle in degrees.
			/// Angles are returned in the instance of Vector3 following way: (angle of vertex V0, angle of vertex V1, angle of vertex V2)
			/// </summary>
			public Vector3 CalcAnglesDeg()
			{
				float sideX = V2.x - V1.x;
				float sideY = V2.y - V1.y;
				float sideZ = V2.z - V1.z;
				float aSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;

				sideX = V2.x - V0.x;
				sideY = V2.y - V0.y;
				sideZ = V2.z - V0.z;
				float bSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;

				sideX = V1.x - V0.x;
				sideY = V1.y - V0.y;
				sideZ = V1.z - V0.z;
				float cSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;
				float two_c = 2f * Mathf.Sqrt(cSqr);

				Vector3 result;
				result.x = Mathf.Acos((bSqr + cSqr - aSqr) / (Mathf.Sqrt(bSqr) * two_c)) * Mathf.Rad2Deg;
				result.y = Mathf.Acos((aSqr + cSqr - bSqr) / (Mathf.Sqrt(aSqr) * two_c)) * Mathf.Rad2Deg;
				result.z = 180f - result.x - result.y;

				return result;
			}

			/// <summary>
			/// Calculates angles of the triangle defined by 3 points in degrees.
			/// Angles are returned in the instance of Vector3 following way: (angle of vertex V0, angle of vertex V1, angle of vertex V2)
			/// </summary>
			public static Vector3 CalcAnglesDeg(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
			{
				float sideX = v2.x - v1.x;
				float sideY = v2.y - v1.y;
				float sideZ = v2.z - v1.z;
				float aSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;

				sideX = v2.x - v0.x;
				sideY = v2.y - v0.y;
				sideZ = v2.z - v0.z;
				float bSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;

				sideX = v1.x - v0.x;
				sideY = v1.y - v0.y;
				sideZ = v1.z - v0.z;
				float cSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;
				float two_c = 2f * Mathf.Sqrt(cSqr);

				Vector3 result;
				result.x = Mathf.Acos((bSqr + cSqr - aSqr) / (Mathf.Sqrt(bSqr) * two_c)) * Mathf.Rad2Deg;
				result.y = Mathf.Acos((aSqr + cSqr - bSqr) / (Mathf.Sqrt(aSqr) * two_c)) * Mathf.Rad2Deg;
				result.z = 180f - result.x - result.y;

				return result;
			}

			/// <summary>
			/// Calculates angles of the triangle defined by 3 points in degrees.
			/// Angles are returned in the instance of Vector3 following way: (angle of vertex V0, angle of vertex V1, angle of vertex V2)
			/// </summary>
			public static Vector3 CalcAnglesDeg(Vector3 v0, Vector3 v1, Vector3 v2)
			{
				return CalcAnglesDeg(ref v0, ref v1, ref v2);
			}

			/// <summary>
			/// Calculates angles of the triangle in radians.
			/// Angles are returned in the instance of Vector3 following way: (angle of vertex V0, angle of vertex V1, angle of vertex V2)
			/// </summary>
			public Vector3 CalcAnglesRad()
			{
				float sideX = V2.x - V1.x;
				float sideY = V2.y - V1.y;
				float sideZ = V2.z - V1.z;
				float aSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;

				sideX = V2.x - V0.x;
				sideY = V2.y - V0.y;
				sideZ = V2.z - V0.z;
				float bSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;

				sideX = V1.x - V0.x;
				sideY = V1.y - V0.y;
				sideZ = V1.z - V0.z;
				float cSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;
				float two_c = 2f * Mathf.Sqrt(cSqr);

				Vector3 result;
				result.x = Mathf.Acos((bSqr + cSqr - aSqr) / (Mathf.Sqrt(bSqr) * two_c));
				result.y = Mathf.Acos((aSqr + cSqr - bSqr) / (Mathf.Sqrt(aSqr) * two_c));
				result.z = Mathfex.Pi - result.x - result.y;

				return result;
			}

			/// <summary>
			/// Calculates angles of the triangle defined by 3 points in radians.
			/// Angles are returned in the instance of Vector3 following way: (angle of vertex V0, angle of vertex V1, angle of vertex V2)
			/// </summary>
			public static Vector3 CalcAnglesRad(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
			{
				float sideX = v2.x - v1.x;
				float sideY = v2.y - v1.y;
				float sideZ = v2.z - v1.z;
				float aSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;

				sideX = v2.x - v0.x;
				sideY = v2.y - v0.y;
				sideZ = v2.z - v0.z;
				float bSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;

				sideX = v1.x - v0.x;
				sideY = v1.y - v0.y;
				sideZ = v1.z - v0.z;
				float cSqr = sideX * sideX + sideY * sideY + sideZ * sideZ;
				float two_c = 2f * Mathf.Sqrt(cSqr);

				Vector3 result;
				result.x = Mathf.Acos((bSqr + cSqr - aSqr) / (Mathf.Sqrt(bSqr) * two_c));
				result.y = Mathf.Acos((aSqr + cSqr - bSqr) / (Mathf.Sqrt(aSqr) * two_c));
				result.z = Mathfex.Pi - result.x - result.y;

				return result;
			}

			/// <summary>
			/// Calculates angles of the triangle defined by 3 points in radians.
			/// Angles are returned in the instance of Vector3 following way: (angle of vertex V0, angle of vertex V1, angle of vertex V2)
			/// </summary>
			public static Vector3 CalcAnglesRad(Vector3 v0, Vector3 v1, Vector3 v2)
			{
				return CalcAnglesRad(ref v0, ref v1, ref v2);
			}

			/// <summary>
			/// Gets point on the triangle using barycentric coordinates.
			/// The result is c0*V0 + c1*V1 + c2*V2, 0 &lt;= c0,c1,c2 &lt;= 1, c0+c1+c2=1, c2 is calculated as 1-c0-c1.
			/// </summary>
			public Vector3 EvalBarycentric(float c0, float c1)
			{
				float c2 = 1f - c0 - c1;
				return c0 * V0 + c1 * V1 + c2 * V2;
			}

			/// <summary>
			/// Gets point on the triangle using barycentric coordinates. baryCoords parameter is (c0,c1,c2).
			/// The result is c0*V0 + c1*V1 + c2*V2, 0 &lt;= c0,c1,c2 &lt;= 1, c0+c1+c2=1
			/// </summary>
			public Vector3 EvalBarycentric(ref Vector3 baryCoords)
			{
				return baryCoords.x * V0 + baryCoords.y * V1 + baryCoords.z * V2;
			}

			/// <summary>
			/// Gets point on the triangle using barycentric coordinates. baryCoords parameter is (c0,c1,c2).
			/// The result is c0*V0 + c1*V1 + c2*V2, 0 &lt;= c0,c1,c2 &lt;= 1, c0+c1+c2=1
			/// </summary>
			public Vector3 EvalBarycentric(Vector3 baryCoords)
			{
				return baryCoords.x * V0 + baryCoords.y * V1 + baryCoords.z * V2;
			}

			/// <summary>
			/// Calculate barycentric coordinates for the input point with regarding to triangle defined by 3 points.
			/// </summary>
			public static void CalcBarycentricCoords(ref Vector3 point, ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, out Vector3 baryCoords)
			{
				// http://gamedev.stackexchange.com/questions/23743/whats-the-most-efficient-way-to-find-barycentric-coordinates
				
				Vector3 e0 = v1 - v0;
				Vector3 e1 = v2 - v0;
				Vector3 e2 = point - v0;
				float d00 = Vector3ex.Dot(ref e0, ref e0);
				float d01 = Vector3ex.Dot(ref e0, ref e1);
				float d11 = Vector3ex.Dot(ref e1, ref e1);
				float d20 = Vector3ex.Dot(ref e2, ref e0);
				float d21 = Vector3ex.Dot(ref e2, ref e1);
				float denomInv = 1f / (d00 * d11 - d01 * d01);

				baryCoords.y = (d11 * d20 - d01 * d21) * denomInv;
				baryCoords.z = (d00 * d21 - d01 * d20) * denomInv;
				baryCoords.x = 1.0f - baryCoords.y - baryCoords.z;
			}

			/// <summary>
			/// Calculate barycentric coordinates for the input point regarding to the triangle.
			/// </summary>
			public Vector3 CalcBarycentricCoords(ref Vector3 point)
			{
				Vector3 result;
				CalcBarycentricCoords(ref point, ref V0, ref V1, ref V2, out result);
				return result;
			}

			/// <summary>
			/// Calculate barycentric coordinates for the input point regarding to the triangle.
			/// </summary>
			public Vector3 CalcBarycentricCoords(Vector3 point)
			{
				Vector3 result;
				CalcBarycentricCoords(ref point, ref V0, ref V1, ref V2, out result);
				return result;
			}

			/// <summary>
			/// Returns string representation.
			/// </summary>
			public override string ToString()
			{
				return string.Format("[V0: {0} V1: {1} V2: {2}]", V0.ToStringEx(), V1.ToStringEx(), V2.ToStringEx());
			}
		}
	}
}
