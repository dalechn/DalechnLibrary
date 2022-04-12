using UnityEngine;
using System.Collections.Generic;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// A box has center C, axis directions A0 and A1 (perpendicular and
		/// unit-length vectors), and extents e0 and e1 (nonnegative numbers).
		/// A point X = C + y0*A0 + y1*A1 is inside or on the box whenever
		/// |y[i]| &lt;= e[i] for all i.
		/// </summary>
		public struct Box2
		{
			/// <summary>
			/// Box center
			/// </summary>
			public Vector2 Center;

			/// <summary>
			/// First box axis. Must be unit length!
			/// </summary>
			public Vector2 Axis0;

			/// <summary>
			/// Second box axis. Must be unit length!
			/// </summary>
			public Vector2 Axis1;

			/// <summary>
			/// Extents (half sizes) along Axis0 and Axis1. Must be non-negative!
			/// </summary>
			public Vector2 Extents;


			/// <summary>
			/// Creates new Box2 instance.
			/// </summary>
			/// <param name="center">Box center</param>
			/// <param name="axis0">First box axis. Must be unit length!</param>
			/// <param name="axis1">Second box axis. Must be unit length!</param>
			/// <param name="extents">Extents (half sizes) along Axis0 and Axis1. Must be non-negative!</param>
			public Box2(ref Vector2 center, ref Vector2 axis0, ref Vector2 axis1, ref Vector2 extents)
			{
				Center = center;
				Axis0 = axis0;
				Axis1 = axis1;
				Extents = extents;
			}

			/// <summary>
			/// Creates new Box2 instance.
			/// </summary>
			/// <param name="center">Box center</param>
			/// <param name="axis0">First box axis. Must be unit length!</param>
			/// <param name="axis1">Second box axis. Must be unit length!</param>
			/// <param name="extents">Extents (half sizes) along Axis0 and Axis1. Must be non-negative!</param>
			public Box2(Vector2 center, Vector2 axis0, Vector2 axis1, Vector2 extents)
			{
				Center = center;
				Axis0 = axis0;
				Axis1 = axis1;
				Extents = extents;
			}

			/// <summary>
			/// Creates Box2 from AxisAlignedBox2
			/// </summary>
			public Box2(ref AAB2 box)
			{
				box.CalcCenterExtents(out Center, out Extents);
				Axis0 = Vector2ex.UnitX;
				Axis1 = Vector2ex.UnitY;
			}

			/// <summary>
			/// Creates Box2 from AxisAlignedBox2
			/// </summary>
			public Box2(AAB2 box)
			{
				box.CalcCenterExtents(out Center, out Extents);
				Axis0 = Vector2ex.UnitX;
				Axis1 = Vector2ex.UnitY;
			}

			/// <summary>
			/// Computes oriented bounding box from a set of points.
			/// If a set is empty returns new Box2().
			/// </summary>
			public static Box2 CreateFromPoints(IList<Vector2> points)
			{
				int numPoints = points.Count;
				if (numPoints == 0)
				{
					return new Box2();
				}

				Box2 box = Approximation.GaussPointsFit2(points);

				// Let C be the box center and let U0 and U1 be the box axes.  Each
				// input point is of the form X = C + y0*U0 + y1*U1.  The following code
				// computes min(y0), max(y0), min(y1), max(y1), min(y2), and max(y2).
				// The box center is then adjusted to be
				//   C' = C + 0.5*(min(y0)+max(y0))*U0 + 0.5*(min(y1)+max(y1))*U1

				Vector2 diff = points[0] - box.Center;
				Vector2 pmin = new Vector2(diff.Dot(box.Axis0), diff.Dot(box.Axis1));
				Vector2 pmax = pmin;
			

				for (int i = 1; i < numPoints; ++i)
				{
					diff = points[i] - box.Center;
					for (int j = 0; j < 2; ++j)
					{
						float dot = diff.Dot(box.GetAxis(j));
						if (dot < pmin[j])
						{
							pmin[j] = dot;
						}
						else if (dot > pmax[j])
						{
							pmax[j] = dot;
						}
					}
				}

				box.Center +=
					(0.5f * (pmin[0] + pmax[0])) * box.Axis0 +
					(0.5f * (pmin[1] + pmax[1])) * box.Axis1;

				box.Extents.x = 0.5f * (pmax[0] - pmin[0]);
				box.Extents.y = 0.5f * (pmax[1] - pmin[1]);

				return box;
			}


			/// <summary>
			/// Returns axis by index (0, 1)
			/// </summary>
			public Vector2 GetAxis(int index)
			{
				if (index == 0) return Axis0;
				if (index == 1) return Axis1;
				return Vector2ex.Zero;
			}

			/// <summary>
			/// Calculates 4 box corners. extAxis[i] is Axis[i]*Extent[i], i=0,1.
			/// </summary>
			/// <param name="vertex0">Center - extAxis0 - extAxis1</param>
			/// <param name="vertex1">Center + extAxis0 - extAxis1</param>
			/// <param name="vertex2">Center + extAxis0 + extAxis1</param>
			/// <param name="vertex3">Center - extAxis0 + extAxis1</param>
			public void CalcVertices(out Vector2 vertex0, out Vector2 vertex1, out Vector2 vertex2, out Vector2 vertex3)
			{
				Vector2 extAxis0 = Axis0 * Extents.x;
				Vector2 extAxis1 = Axis1 * Extents.y;

				vertex0 = Center - extAxis0 - extAxis1;
				vertex1 = Center + extAxis0 - extAxis1;
				vertex2 = Center + extAxis0 + extAxis1;
				vertex3 = Center - extAxis0 + extAxis1;
			}

			/// <summary>
			/// Calculates 4 box corners and returns them in an allocated array.
			/// See array-less overload for the description.
			/// </summary>
			public Vector2[] CalcVertices()
			{
				Vector2 extAxis0 = Axis0 * Extents.x;
				Vector2 extAxis1 = Axis1 * Extents.y;

				Vector2[] result =
				{
					Center - extAxis0 - extAxis1,
					Center + extAxis0 - extAxis1,
					Center + extAxis0 + extAxis1,
					Center - extAxis0 + extAxis1,
				};
				return result;
			}

			/// <summary>
			/// Calculates 4 box corners and fills the input array with them (array length must be 4).
			/// See array-less overload for the description.
			/// </summary>
			public void CalcVertices(Vector2[] array)
			{
				Vector2 extAxis0 = Axis0 * Extents.x;
				Vector2 extAxis1 = Axis1 * Extents.y;

				array[0] = Center - extAxis0 - extAxis1;
				array[1] = Center + extAxis0 - extAxis1;
				array[2] = Center + extAxis0 + extAxis1;
				array[3] = Center - extAxis0 + extAxis1;
			}

			/// <summary>
			/// Returns area of the box as Extents.x * Extents.y * 4
			/// </summary>
			public float CalcArea()
			{
				return 4f * Extents.x * Extents.y;
			}

			/// <summary>
			/// Returns distance to a point, distance is >= 0f.
			/// </summary>
			public float DistanceTo(Vector2 point)
			{
				return Distance.Point2Box2(ref point, ref this);
			}			

			/// <summary>
			/// Returns projected point
			/// </summary>
			public Vector2 Project(Vector2 point)
			{
				Vector2 result;
				Distance.SqrPoint2Box2(ref point, ref this, out result);
				return result;
			}

			/// <summary>
			/// Tests whether a point is contained by the box
			/// </summary>
			public bool Contains(ref Vector2 point)
			{
				Vector2 diff;
				diff.x = point.x - Center.x;
				diff.y = point.y - Center.y;
				float proj;
				proj = diff.Dot(Axis0);
				if (proj < -Extents.x) return false;
				if (proj > Extents.x) return false;
				proj = diff.Dot(Axis1);
				if (proj < -Extents.y) return false;
				if (proj > Extents.y) return false;
				return true;
			}

			/// <summary>
			/// Tests whether a point is contained by the box
			/// </summary>
			public bool Contains(Vector2 point)
			{
				Vector2 diff;
				diff.x = point.x - Center.x;
				diff.y = point.y - Center.y;
				float proj;
				proj = diff.Dot(Axis0);
				if (proj < -Extents.x) return false;
				if (proj > Extents.x) return false;
				proj = diff.Dot(Axis1);
				if (proj < -Extents.y) return false;
				if (proj > Extents.y) return false;
				return true;
			}

			/// <summary>
			/// Enlarges the box so it includes another box.
			/// </summary>
			public void Include(ref Box2 box)
			{
				// Construct a box that contains the input boxes.
				Box2 result = new Box2();

				// The first guess at the box center.  This value will be updated later
				// after the input box vertices are projected onto axes determined by an
				// average of box axes.
				result.Center = 0.5f * (this.Center + box.Center);

				// The merged box axes are the averages of the input box axes.  The
				// axes of the second box are negated, if necessary, so they form acute
				// angles with the axes of the first box.
				if (this.Axis0.Dot(box.Axis0) >= 0f)
				{
					result.Axis0 = 0.5f * (this.Axis0 + box.Axis0);
					result.Axis0.Normalize();
				}
				else
				{
					result.Axis0 = 0.5f * (this.Axis0 - box.Axis0);
					result.Axis0.Normalize();
				}
				result.Axis1 = -result.Axis0.Perp();

				// Project the input box vertices onto the merged-box axes.  Each axis
				// D[i] containing the current center C has a minimum projected value
				// min[i] and a maximum projected value max[i].  The corresponding end
				// points on the axes are C+min[i]*D[i] and C+max[i]*D[i].  The point C
				// is not necessarily the midpoint for any of the intervals.  The actual
				// box center will be adjusted from C to a point C' that is the midpoint
				// of each interval,
				//   C' = C + sum_{i=0}^1 0.5*(min[i]+max[i])*D[i]
				// The box extents are
				//   e[i] = 0.5*(max[i]-min[i])

				int i, j;
				float dot;
				Vector2 diff;
				Vector2 pmin = Vector2ex.Zero;
				Vector2 pmax = Vector2ex.Zero;
				Vector2[] vertex = this.CalcVertices();

				for (i = 0; i < 4; ++i)
				{
					diff = vertex[i] - result.Center;
					for (j = 0; j < 2; ++j)
					{
						dot = diff.Dot(result.GetAxis(j));
						if (dot > pmax[j])
						{
							pmax[j] = dot;
						}
						else if (dot < pmin[j])
						{
							pmin[j] = dot;
						}
					}
				}

				box.CalcVertices(out vertex[0], out vertex[1], out vertex[2], out vertex[3]);
				for (i = 0; i < 4; ++i)
				{
					diff = vertex[i] - result.Center;
					for (j = 0; j < 2; ++j)
					{
						dot = diff.Dot(result.GetAxis(j));
						if (dot > pmax[j])
						{
							pmax[j] = dot;
						}
						else if (dot < pmin[j])
						{
							pmin[j] = dot;
						}
					}
				}

				// [min,max] is the axis-aligned box in the coordinate system of the
				// merged box axes.  Update the current box center to be the center of
				// the new box.  Compute the extents based on the new center.
				for (j = 0; j < 2; ++j)
				{
					result.Center += result.GetAxis(j) * (0.5f * (pmax[j] + pmin[j]));
					result.Extents[j] = 0.5f * (pmax[j] - pmin[j]);
				}

				this = result;
			}

			/// <summary>
			/// Enlarges the box so it includes another box.
			/// </summary>
			public void Include(Box2 box)
			{
				Include(ref box);
			}

			/// <summary>
			/// Returns string representation.
			/// </summary>
			public override string ToString()
			{
				return string.Format("[Center: {0} Axis0: {1} Axis1: {2} Extents: {3}]", Center.ToStringEx(), Axis0.ToStringEx(), Axis1.ToStringEx(), Extents.ToStringEx());
			}
		}
	}
}
