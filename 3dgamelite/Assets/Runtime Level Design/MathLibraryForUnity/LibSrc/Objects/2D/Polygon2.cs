using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Polygon2 edge
		/// </summary>
		public struct Edge2
		{
			/// <summary>
			/// Edge start vertex
			/// </summary>
			public Vector2 Point0;

			/// <summary>
			/// Edge end vertex
			/// </summary>
			public Vector2 Point1;

			/// <summary>
			/// Unit length direction vector
			/// </summary>
			public Vector2 Direction;

			/// <summary>
			/// Unit length normal vector
			/// </summary>
			public Vector2 Normal;

			/// <summary>
			/// Edge length
			/// </summary>
			public float Length;
		}



		/// <summary>
		/// Represents 2d polygon (vertex count must be >= 3).
		/// </summary>
		public class Polygon2
		{
			private Vector2[] _vertices;
			private Edge2[]   _edges;

			/// <summary>
			/// Gets vertices array (do not change the data, use only for traversal)
			/// </summary>
			public Vector2[] Vertices { get { return _vertices; } }

			/// <summary>
			/// Gets edges array (do not change the data, use only for traversal)
			/// </summary>
			public Edge2[] Edges { get { return _edges; } }

			/// <summary>
			/// Polygon vertex count
			/// </summary>
			public int VertexCount { get { return _vertices.Length; } }

			/// <summary>
			/// Gets or sets polygon vertex
			/// </summary>
			public Vector2 this[int vertexIndex] { get { return _vertices[vertexIndex]; } set { _vertices[vertexIndex] = value; } }


			private Polygon2()
			{
			}
			
			/// <summary>
			/// Creates polygon from an array of vertices (array is copied)
			/// </summary>
			public Polygon2(Vector2[] vertices)
			{
				_vertices = new Vector2[vertices.Length];
				_edges = new Edge2[vertices.Length];
				System.Array.Copy(vertices, _vertices, vertices.Length);
				UpdateEdges();
			}

			/// <summary>
			/// Creates polygon setting number of vertices. Vertices then
			/// can be filled using indexer.
			/// </summary>
			public Polygon2(int vertexCount)
			{
				_vertices = new Vector2[vertexCount];
				_edges = new Edge2[vertexCount];
			}

			/// <summary>
			/// Creates Polygon2 instance from Polygon3 instance by projecting
			/// Polygon3 vertices onto one of three base planes (on practice just
			/// dropping one of the coordinates).
			/// </summary>
			public static Polygon2 CreateProjected(Polygon3 polygon, ProjectionPlanes projectionPlane)
			{
				Polygon2 result = new Polygon2(polygon.VertexCount);
				if (projectionPlane == ProjectionPlanes.XY)
				{
					for (int i = 0, len = polygon.VertexCount; i < len; ++i)
					{
						result._vertices[i] = polygon[i].ToVector2XY();
					}
				}
				else if (projectionPlane == ProjectionPlanes.XZ)
				{
					for (int i = 0, len = polygon.VertexCount; i < len; ++i)
					{
						result._vertices[i] = polygon[i].ToVector2XZ();
					}
				}
				else // YZ
				{
					for (int i = 0, len = polygon.VertexCount; i < len; ++i)
					{
						result._vertices[i] = polygon[i].ToVector2YZ();
					}
				}
				result.UpdateEdges();
				return result;
			}

			
			/// <summary>
			/// Returns polygon edge
			/// </summary>
			public Edge2 GetEdge(int edgeIndex)
			{
				return _edges[edgeIndex];
			}

			/// <summary>
			/// Updates all polygon edges. Use after vertex change.
			/// </summary>
			public void UpdateEdges()
			{
				int vertexCount = _vertices.Length;
				for (int i0 = vertexCount - 1, i1 = 0; i1 < vertexCount; i0 = i1, ++i1)
				{
					Vector2 direction =
						(_edges[i0].Point1 = _vertices[i1]) -
						(_edges[i0].Point0 = _vertices[i0]);
					_edges[i0].Length = Vector2ex.Normalize(ref direction);
					_edges[i0].Direction = direction;
					_edges[i0].Normal = direction.Perp();
				}
			}

			/// <summary>
			/// Updates certain polygon edge. Use after vertex change.
			/// </summary>
			public void UpdateEdge(int edgeIndex)
			{
				Vector2 direction = 
					(_edges[edgeIndex].Point1 = _vertices[(edgeIndex + 1) % _vertices.Length]) - 
					(_edges[edgeIndex].Point0 = _vertices[edgeIndex]);
				_edges[edgeIndex].Length = Vector2ex.Normalize(ref direction);
				_edges[edgeIndex].Direction = direction;
				_edges[edgeIndex].Normal = direction.Perp();
			}

			/// <summary>
			/// Returns polygon center
			/// </summary>
			public Vector2 CalcCenter()
			{
				Vector2 average = _vertices[0];
				int vertexCount = _vertices.Length;
				for (int i = 1; i < vertexCount; ++i)
				{
					average += _vertices[i];
				}
				average /= (float)vertexCount;
				return average;
			}

			/// <summary>
			/// Returns polygon perimeter length
			/// </summary>
			public float CalcPerimeter()
			{
				float perimeterLength = 0f;
				for (int i = 0, len = _edges.Length; i < len; ++i)
				{
					perimeterLength += _edges[i].Length;
				}
				return perimeterLength;
			}

			/// <summary>
			/// Returns polygon area (polygon must be simple, i.e. without self-intersections).
			/// </summary>
			public float CalcArea()
			{
				int last = _vertices.Length - 1;
				float area =
					_vertices[0   ][0] * (_vertices[1][1] - _vertices[last  ][1]) +
					_vertices[last][0] * (_vertices[0][1] - _vertices[last-1][1]);

				for (int im1 = 0, i = 1, ip1 = 2; i < last; ++im1, ++i, ++ip1)
				{
					area += _vertices[i][0] * (_vertices[ip1][1] - _vertices[im1][1]);
				}

				area *= 0.5f;

				return Mathf.Abs(area);
			}

			/// <summary>
			/// Tests if the polygon is convex and returns orientation
			/// </summary>
			public bool IsConvex(out Orientations orientation, float threshold = Mathfex.ZeroTolerance)
			{
				orientation = Orientations.None;
				int edgesLength = _edges.Length;
				Vector2 edge0, edge1;
				int sign = 0;
				
				for (int i0 = edgesLength - 1, i1 = 0; i1 < edgesLength; i0 = i1, ++i1)
				{
					edge0 = -_edges[i0].Direction;
					edge1 = _edges[i1].Direction;

					float cross = edge0.DotPerp(edge1);
					int cornerSign = (cross < -threshold || cross > threshold) ? (cross > 0f ? 1 : -1) : 0;

					if (cornerSign != 0)
					{
						if (sign != 0)
						{
							if ((sign > 0 && cornerSign < 0) || (sign < 0 && cornerSign > 0))
							{
								return false;
							}
						}
						else
						{
							sign += cornerSign;
						}
					}
				}

				orientation = sign == 0 ? Orientations.None : (sign > 0 ? Orientations.CW : Orientations.CCW);

				return orientation != Orientations.None;
			}

			/// <summary>
			/// Tests if the polygon is convex
			/// </summary>
			public bool IsConvex(float threshold = Mathfex.ZeroTolerance)
			{
				Orientations orientation;
				return IsConvex(out orientation);
			}

			/// <summary>
			/// Returns true if polygon contains some edges which have zero angle between them.
			/// </summary>
			public bool HasZeroCorners(float threshold = Mathfex.ZeroTolerance)
			{
				int edgesLength = _edges.Length;
				Vector2 edge0, edge1;
				float thr = 1f - threshold;

				for (int i0 = edgesLength - 1, i1 = 0; i1 < edgesLength; i0 = i1, ++i1)
				{
					edge0 = -_edges[i0].Direction;
					edge1 = _edges[i1].Direction;

					float dot = Vector2.Dot(edge0, edge1);
					if (dot >= thr)
					{
						return true;
					}
				}

				return false;
			}

			/// <summary>
			/// Reverses polygon vertex order
			/// </summary>
			public void ReverseVertices()
			{
				int vertexCount = _vertices.Length;
				int limit = vertexCount / 2;
				--vertexCount;
				int index;
				Vector2 temp;
				for (int i = 0; i < limit; ++i)
				{
					temp = _vertices[i];
					index = vertexCount - i;
					_vertices[i] = _vertices[index];
					_vertices[index] = temp;
				}
				UpdateEdges();
			}

			/// <summary>
			/// Tests whether a point is contained by the convex CCW 4-sided polygon (the caller must ensure that polygon is indeed CCW ordered)
			/// </summary>
			public bool ContainsConvexQuadCCW(ref Vector2 point)
			{
				if (_vertices.Length != 4) return false;

				float nx = _vertices[2].y - _vertices[0].y;
				float ny = _vertices[0].x - _vertices[2].x;
				float dx = point.x - _vertices[0].x;
				float dy = point.y - _vertices[0].y;

				if (nx * dx + ny * dy > 0.0f)
				{
					// P potentially in <V0,V1,V2>
					nx = _vertices[1].y - _vertices[0].y;
					ny = _vertices[0].x - _vertices[1].x;
					if (nx * dx + ny * dy > 0.0f)
					{
						return false;
					}

					nx = _vertices[2].y - _vertices[1].y;
					ny = _vertices[1].x - _vertices[2].x;
					dx = point.x - _vertices[1].x;
					dy = point.y - _vertices[1].y;
					if (nx * dx + ny * dy > 0.0f)
					{
						return false;
					}
				}
				else
				{
					// P potentially in <V0,V2,V3>
					nx = _vertices[0].y - _vertices[3].y;
					ny = _vertices[3].x - _vertices[0].x;
					if (nx * dx + ny * dy > 0.0f)
					{
						return false;
					}

					nx = _vertices[3].y - _vertices[2].y;
					ny = _vertices[2].x - _vertices[3].x;
					dx = point.x - _vertices[3].x;
					dy = point.y - _vertices[3].y;
					if (nx * dx + ny * dy > 0.0f)
					{
						return false;
					}
				}
				return true;
			}

			/// <summary>
			/// Tests whether a point is contained by the convex CCW 4-sided polygon (the caller must ensure that polygon is indeed CCW ordered)
			/// </summary>
			public bool ContainsConvexQuadCCW(Vector2 point)
			{
				return ContainsConvexQuadCCW(ref point);
			}

			/// <summary>
			/// Tests whether a point is contained by the convex CW 4-sided polygon (the caller must ensure that polygon is indeed CW ordered)
			/// </summary>
			public bool ContainsConvexQuadCW(ref Vector2 point)
			{
				if (_vertices.Length != 4) return false;

				float nx = _vertices[2].y - _vertices[0].y;
				float ny = _vertices[0].x - _vertices[2].x;
				float dx = point.x - _vertices[0].x;
				float dy = point.y - _vertices[0].y;

				if (nx * dx + ny * dy < 0.0f)
				{
					// P potentially in <V0,V1,V2>
					nx = _vertices[1].y - _vertices[0].y;
					ny = _vertices[0].x - _vertices[1].x;
					if (nx * dx + ny * dy < 0.0f)
					{
						return false;
					}

					nx = _vertices[2].y - _vertices[1].y;
					ny = _vertices[1].x - _vertices[2].x;
					dx = point.x - _vertices[1].x;
					dy = point.y - _vertices[1].y;
					if (nx * dx + ny * dy < 0.0f)
					{
						return false;
					}
				}
				else
				{
					// P potentially in <V0,V2,V3>
					nx = _vertices[0].y - _vertices[3].y;
					ny = _vertices[3].x - _vertices[0].x;
					if (nx * dx + ny * dy < 0.0f)
					{
						return false;
					}

					nx = _vertices[3].y - _vertices[2].y;
					ny = _vertices[2].x - _vertices[3].x;
					dx = point.x - _vertices[3].x;
					dy = point.y - _vertices[3].y;
					if (nx * dx + ny * dy < 0.0f)
					{
						return false;
					}
				}
				return true;
			}

			/// <summary>
			/// Tests whether a point is contained by the convex CW 4-sided polygon (the caller must ensure that polygon is indeed CW ordered)
			/// </summary>
			public bool ContainsConvexQuadCW(Vector2 point)
			{
				return ContainsConvexQuadCW(ref point);
			}

			/// <summary>
			/// Tests whether a point is contained by the convex CCW polygon (the caller must ensure that polygon is indeed CCW ordered)
			/// </summary>
			public bool ContainsConvexCCW(ref Vector2 point)
			{
				return SubContainsPointCCW(ref point, 0, 0);
			}

			/// <summary>
			/// Tests whether a point is contained by the convex CCW polygon (the caller must ensure that polygon is indeed CCW ordered)
			/// </summary>
			public bool ContainsConvexCCW(Vector2 point)
			{
				return ContainsConvexCCW(ref point);
			}

			private bool SubContainsPointCCW(ref Vector2 p, int i0, int i1)
			{
				int numPoints = _vertices.Length;
				float nx, ny, dx, dy;

				int diff = i1 - i0;
				if (diff == 1 || (diff < 0 && diff + numPoints == 1))
				{
					nx = _vertices[i1].y - _vertices[i0].y;
					ny = _vertices[i0].x - _vertices[i1].x;
					dx = p.x - _vertices[i0].x;
					dy = p.y - _vertices[i0].y;
					return nx * dx + ny * dy <= 0.0f;
				}

				// Bisect the index range.
				int mid;
				if (i0 < i1)
				{
					mid = (i0 + i1) >> 1;
				}
				else
				{
					mid = ((i0 + i1 + numPoints) >> 1);
					if (mid >= numPoints)
					{
						mid -= numPoints;
					}
				}

				// Determine which side of the splitting line contains the point.
				nx = _vertices[mid].y - _vertices[i0].y;
				ny = _vertices[i0].x - _vertices[mid].x;
				dx = p.x - _vertices[i0].x;
				dy = p.y - _vertices[i0].y;
				if (nx * dx + ny * dy > 0.0f)
				{
					// P potentially in <V(i0),V(i0+1),...,V(mid-1),V(mid)>
					return SubContainsPointCCW(ref p, i0, mid);
				}
				else
				{
					// P potentially in <V(mid),V(mid+1),...,V(i1-1),V(i1)>
					return SubContainsPointCCW(ref p, mid, i1);
				}
			}

			/// <summary>
			/// Tests whether a point is contained by the convex CW polygon (the caller must ensure that polygon is indeed CW ordered)
			/// </summary>
			public bool ContainsConvexCW(ref Vector2 point)
			{
				return SubContainsPointCW(ref point, 0, 0);
			}

			/// <summary>
			/// Tests whether a point is contained by the convex CW polygon (the caller must ensure that polygon is indeed CW ordered)
			/// </summary>
			public bool ContainsConvexCW(Vector2 point)
			{
				return ContainsConvexCW(ref point);
			}

			private bool SubContainsPointCW(ref Vector2 p, int i0, int i1)
			{
				int numPoints = _vertices.Length;
				float nx, ny, dx, dy;

				int diff = i1 - i0;
				if (diff == 1 || (diff < 0 && diff + numPoints == 1))
				{
					nx = _vertices[i1].y - _vertices[i0].y;
					ny = _vertices[i0].x - _vertices[i1].x;
					dx = p.x - _vertices[i0].x;
					dy = p.y - _vertices[i0].y;
					return nx * dx + ny * dy >= 0.0f;
				}

				// Bisect the index range.
				int mid;
				if (i0 < i1)
				{
					mid = (i0 + i1) >> 1;
				}
				else
				{
					mid = ((i0 + i1 + numPoints) >> 1);
					if (mid >= numPoints)
					{
						mid -= numPoints;
					}
				}

				// Determine which side of the splitting line contains the point.
				nx = _vertices[mid].y - _vertices[i0].y;
				ny = _vertices[i0].x - _vertices[mid].x;
				dx = p.x - _vertices[i0].x;
				dy = p.y - _vertices[i0].y;
				if (nx * dx + ny * dy < 0.0f)
				{
					// P potentially in <V(i0),V(i0+1),...,V(mid-1),V(mid)>
					return SubContainsPointCW(ref p, i0, mid);
				}
				else
				{
					// P potentially in <V(mid),V(mid+1),...,V(i1-1),V(i1)>
					return SubContainsPointCW(ref p, mid, i1);
				}
			}

			/// <summary>
			/// Tests whether a point is contained by the simple polygon (i.e. without self intersection). Non-convex polygons are allowed, orientation is irrelevant.
			/// Note that points which are on border may be classified differently depending on the point position.
			/// </summary>
			public bool ContainsSimple(ref Vector2 point)
			{
				bool inside = false;
				int numPoints = _vertices.Length;
				for (int i = 0, j = numPoints - 1; i < numPoints; j = i, ++i)
				{
					Vector2 U0 = _vertices[i];
					Vector2 U1 = _vertices[j];
					float rhs, lhs;

					if (point.y < U1.y)			// U1 above ray
					{
						if (U0.y <= point.y)	// U0 on or below ray
						{
							lhs = (point.y - U0.y) * (U1.x - U0.x);
							rhs = (point.x - U0.x) * (U1.y - U0.y);
							if (lhs > rhs)
							{
								inside = !inside;
							}
						}
					}
					else if (point.y < U0.y)	// U1 on or below ray, U0 above ray
					{
						lhs = (point.y - U0.y) * (U1.x - U0.x);
						rhs = (point.x - U0.x) * (U1.y - U0.y);
						if (lhs < rhs)
						{
							inside = !inside;
						}
					}
				}
				return inside;
			}

			/// <summary>
			/// Tests whether a point is contained by the simple polygon (i.e. without self intersection). Non-convex polygons are allowed, orientation is irrelevant.
			/// Note that points which are on border may be classified differently depending on the point position.
			/// </summary>
			public bool ContainsSimple(Vector2 point)
			{
				return ContainsSimple(ref point);
			}

			/// <summary>
			/// Converts the polygon to segment array
			/// </summary>
			public Segment2[] ToSegmentArray()
			{
				Segment2[] result = new Segment2[_edges.Length];
				for (int i = 0, len = result.Length; i < len; ++i)
				{
					result[i] = new Segment2(_edges[i].Point0, _edges[i].Point1);
				}
				return result;
			}

			/// <summary>
			/// Returns string representation.
			/// </summary>
			public override string ToString()
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append("[VertexCount: " + _vertices.Length.ToString());
				for (int i = 0, len = _vertices.Length; i < len; ++i)
				{
					sb.Append(string.Format(" V{0}: {1}", i.ToString(), _vertices[i].ToStringEx()));
				}
				sb.Append("]");
				return sb.ToString();
			}
		}
	}
}
