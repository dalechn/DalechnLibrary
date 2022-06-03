using Dest.Math;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Dest.Math
{
	internal class ConcaveHull2
	{
		private struct Edge
		{
			public int  V0;
			public int  V1;

			public Edge(int v0, int v1)
			{
				V0 = v0;
				V1 = v1;
			}
		}

		private struct InnerPoint
		{
			public float AverageDistance;
			public float Distance0;
			public float Distance1;
			public int   Index;
		}

		private static void Quicksort(InnerPoint[] x, int first, int last)
		{
			int i, j, pivot;
			InnerPoint temp;

			if (first < last)
			{
				pivot = first;
				i = first;
				j = last;

				while (i < j)
				{
					while (x[i].AverageDistance <= x[pivot].AverageDistance && i < last)
						i++;
					
					while (x[j].AverageDistance > x[pivot].AverageDistance)
						j--;

					if (i < j)
					{
						temp = x[i];
						x[i] = x[j];
						x[j] = temp;
					}
				}

				temp = x[pivot];
				x[pivot] = x[j];
				x[j] = temp;
				Quicksort(x, first, j - 1);
				Quicksort(x, j + 1, last);
			}
		}

		private static float CalcDistanceFromPointToEdge(ref Vector2 pointA, ref Vector2 v0, ref Vector2 v1)
		{
			float tmp0, tmp1;

			tmp0 = v0.x - pointA.x;
			tmp1 = v0.y - pointA.y;
			float a = tmp0 * tmp0 + tmp1 * tmp1;

			tmp0 = v1.x - pointA.x;
			tmp1 = v1.y - pointA.y;
			float b = tmp0 * tmp0 + tmp1 * tmp1;

			tmp0 = v0.x - v1.x;
			tmp1 = v0.y - v1.y;
			float c = tmp0 * tmp0 + tmp1 * tmp1;

			if (a < b)
			{
				float tmp = a;
				a = b;
				b = tmp;
			}

			if (a > b + c || c < Mathfex.ZeroTolerance)
			{
				return UnityEngine.Mathf.Sqrt(b);
			}
			else
			{
				float t = v0.x * v1.y - v1.x * v0.y + v1.x * pointA.y - pointA.x * v1.y + pointA.x * v0.y - v0.x * pointA.y;
				return UnityEngine.Mathf.Abs(t) / UnityEngine.Mathf.Sqrt(c);
			}
		}

		public static bool Create(Vector2[] points, out int[] concaveHull, int[] convexHull, float N, float epsilon = Mathfex.ZeroTolerance)
		{
			// Source paper:
			// "A New Concave Hull Algorithm and Concaveness Measure for n-dimensional Datasets"
			// JIN-SEO PARK and SE-JONG OH

			 UnityEngine.Profiling.Profiler.BeginSample("Prepare");
			LinkedList<Edge> hull = new LinkedList<Edge>();
			int convexHullLength = convexHull.Length;
			HashSet<int> availableIndices = new HashSet<int>();
			int i = 0;
			int pointsCount = points.Length;
			for (i = 0; i < pointsCount; ++i)
			{
				availableIndices.Add(i);
			}
			int index;
			for (int i0 = convexHullLength - 1, i1 = 0; i1 < convexHullLength; i0 = i1, ++i1)
			{
				index = convexHull[i1];
				hull.AddLast(new Edge(convexHull[i0], index));
				availableIndices.Remove(index);
			}
			InnerPoint[] innerPoints = new InnerPoint[availableIndices.Count];
			 UnityEngine.Profiling.Profiler.EndSample();

			 UnityEngine.Profiling.Profiler.BeginSample("Dig");
			// Do digging
			LinkedListNode<Edge> hullSegment = hull.First;
			int innerPointsLength;
			float tmpX, tmpY, distance0, distance1, averageDistance;
			while (hullSegment != null)
			{
				if (availableIndices.Count == 0) break; // Got no more candidates for digging

				 UnityEngine.Profiling.Profiler.BeginSample("0");
				int ci0 = hullSegment.Value.V0;
				int ci1 = hullSegment.Value.V1;
				Vector2 v0 = points[ci0];
				Vector2 v1 = points[ci1];

				// Calc average distance from the edge to every available point
				innerPointsLength = 0;
				foreach (int availableIndex in availableIndices)
				{
					Vector2 point = points[availableIndex];

					tmpX = point.x - v0.x;
					tmpY = point.y - v0.y;
					distance0 = UnityEngine.Mathf.Sqrt(tmpX * tmpX + tmpY * tmpY);
					tmpX = point.x - v1.x;
					tmpY = point.y - v1.y;
					distance1 = UnityEngine.Mathf.Sqrt(tmpX * tmpX + tmpY * tmpY);
					averageDistance = (distance0 + distance1) * .5f;

					InnerPoint ip = new InnerPoint();
					ip.Distance0 = distance0;
					ip.Distance1 = distance1;
					ip.AverageDistance = averageDistance;
					ip.Index = availableIndex;
					innerPoints[innerPointsLength] = ip;
					
					++innerPointsLength;
				}
				Quicksort(innerPoints, 0, innerPointsLength - 1);
				 UnityEngine.Profiling.Profiler.EndSample();

				 UnityEngine.Profiling.Profiler.BeginSample("1");
				// As innerPoints is sorted, go from smallest distance to largest
				//Segment2 edgeSegment = new Segment2(ref v0, ref v1);
				InnerPoint nearesPoint = new InnerPoint();
				bool gotPoint = false;
				for (int k = 0, len = innerPointsLength; k < len; ++k)
				{
					InnerPoint innerPoint = innerPoints[k];
					Vector2 point = points[innerPoint.Index];
					int nearestEdgeIndex = innerPoint.Distance0 < innerPoint.Distance1 ? ci0 : ci1;

					// Find adjacent segment
					//TODO rework base data to include adjacency (will also help on sorting stage)
					LinkedListNode<Edge> segment = hull.First;
					LinkedListNode<Edge> adjacent = null;
					while (segment != null)
					{
						if (segment != hullSegment)
						{
							if (segment.Value.V0 == nearestEdgeIndex || segment.Value.V1 == nearestEdgeIndex)
							{
								adjacent = segment;
								break;
							}
						}
						segment = segment.Next;
					}

					// Assert
					//if (adjacent == null)
					//{
					//	Logger.LogError("Can't find adjacent edge to current edge");
					//	concaveHull = null;
					//	return false;
					//}

					float distSqrToEdge = CalcDistanceFromPointToEdge(ref point, ref v0, ref v1);
					float distSqrToNeighbour = CalcDistanceFromPointToEdge(ref point, ref points[adjacent.Value.V0], ref points[adjacent.Value.V1]);

					//Segment2 neigbourSegment = new Segment2(points[adjacent.Value.V0], points[adjacent.Value.V1]);
					//float distSqrToEdge = Distance.SqrPoint2Segment2(ref point, ref edgeSegment);
					//float distSqrToNeighbour = Distance.SqrPoint2Segment2(ref point, ref neigbourSegment);

					if (distSqrToEdge < distSqrToNeighbour)
					{
						nearesPoint = innerPoint;
						gotPoint = true;
						break;
					}
				}
				 UnityEngine.Profiling.Profiler.EndSample();

				//if (nearesPoint == null)
				if (!gotPoint)
				{
					hullSegment = hullSegment.Next;
					continue;
				}

				 UnityEngine.Profiling.Profiler.BeginSample("2");
				float minInnerPointToEdgePointDist = nearesPoint.Distance0 < nearesPoint.Distance1 ? nearesPoint.Distance0 : nearesPoint.Distance1;
				float edgeLength = (v0 - v1).magnitude;

				if (minInnerPointToEdgePointDist > 0 &&
					edgeLength / minInnerPointToEdgePointDist > N)
				{
					LinkedListNode<Edge> edgeToDelete = hullSegment;
					hullSegment = hullSegment.Next;
					hull.Remove(edgeToDelete);
					int k = nearesPoint.Index;
					hull.AddLast(new Edge(ci0, k));
					hull.AddLast(new Edge(k, ci1));
					availableIndices.Remove(k);
				}
				else
				{
					hullSegment = hullSegment.Next;
				}
				 UnityEngine.Profiling.Profiler.EndSample();
			}
			 UnityEngine.Profiling.Profiler.EndSample();

			 UnityEngine.Profiling.Profiler.BeginSample("Result");
			// Sort the hull (connect adjacent edges)
			LinkedListNode<Edge> sortedNode = hull.First;
			bool process;
			do
			{
				process = false;
				LinkedListNode<Edge> node = sortedNode.Next;
				while (node != null)
				{
					if (sortedNode.Value.V1 == node.Value.V0)
					{
						// Found adjacent edges, bring them together
						hull.Remove(node);
						hull.AddAfter(sortedNode, node);
						sortedNode = node;
						process = true;
						break;
					}
					node = node.Next;
				}
			}
			while (process);
			
			// Get indices out of edges
			concaveHull = new int[hull.Count];
			i = 0;
			foreach (Edge edge in hull)
			{
				concaveHull[i] = edge.V0;
				++i;
			}
			 UnityEngine.Profiling.Profiler.EndSample();

			return true;
		}
	}

	public static class ConcaveHull
	{
		public static bool Create2D(Vector2[] points, out int[] concaveHull, out int[] convexHull, float algorithmThreshold, float epsilon = Mathfex.ZeroTolerance)
		{
			if (algorithmThreshold <= 0)
			{
				Logger.LogError("algorithmThreshold must be positive number");
				concaveHull = convexHull = null;
				return false;
			}

            UnityEngine.Profiling.Profiler.BeginSample("CreateConvex");
			int dim;
			bool convexResult = ConvexHull.Create2D(points, out convexHull, out dim, epsilon);
			 UnityEngine.Profiling.Profiler.EndSample();

			if (!convexResult)
			{
				Logger.LogError("Convex hull creation failed, can't create concave hull");
				concaveHull = convexHull = null;
				return false;
			}

			if (dim != 2)
			{
				Logger.LogWarning("Convex hull dimension is less than 2, can't create concave hull");
				concaveHull = convexHull = null;
				return false;
			}

			 UnityEngine.Profiling.Profiler.BeginSample("CreateConcave");
			bool result = ConcaveHull2.Create(points, out concaveHull, convexHull, algorithmThreshold, epsilon);
			 UnityEngine.Profiling.Profiler.EndSample();
			if (!result)
			{
				convexHull = null;
			}
			return result;
		}

		public static bool Create2D(Vector2[] points, out int[] concaveHull, float algorithmThreshold, float epsilon = Mathfex.ZeroTolerance)
		{
			int[] convexHull;
			return Create2D(points, out concaveHull, out convexHull, algorithmThreshold, epsilon);
		}
	}
}
