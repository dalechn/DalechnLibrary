using UnityEngine;
using System.Collections.Generic;

namespace Dest
{
	namespace Math
	{
		public static partial class Approximation
		{
			/// <summary>
			/// Fitting to a line using least-squares method and using distance
			/// measurements in the y-direction. The result is a line represented by
			/// y = A*x + B. If a line cannot be constructed method returns false and
			/// A and B are returned as float.MaxValue.
			/// </summary>
			internal static bool HeightLineFit2(IList<Vector2> points, out float a, out float b)
			{
				// You need at least two points to determine the line.  Even so, if
				// the points are on a vertical line, there is no least-squares fit in
				// the 'height' sense.  This will be trapped by the determinant of the
				// coefficient matrix being (nearly) zero.

				// Compute sums for linear system.
				float sumX = (float)0, sumY = (float)0;
				float sumXX = (float)0, sumXY = (float)0;
				int numPoints = points.Count;
				int i;

				for (i = 0; i < numPoints; ++i)
				{
					sumX += points[i].x;
					sumY += points[i].y;
					sumXX += points[i].x * points[i].x;
					sumXY += points[i].x * points[i].y;
				}

				float[,] A =
				{
					{sumXX, sumX},
					{sumX, (float)numPoints}
				};

				float[] B =
				{
					sumXY,
					sumY
				};

				float[] X;
				bool nonsingular = LinearSystem.Solve2(A, B, out X);

				if (nonsingular)
				{
					a = X[0];
					b = X[1];
				}
				else
				{
					a = float.MaxValue;
					b = float.MaxValue;
				}

				return nonsingular;
			}

			/// <summary>
			/// Producing a line using least-squares fitting. A set must contain at least one point!
			/// </summary>
			public static Line2 LeastSquaresLineFit2(IList<Vector2> points)
			{
				Line2 line = new Line2();
				int numPoints = points.Count;

				// Compute the mean of the points.
				line.Center = points[0];
				int i;
				for (i = 1; i < numPoints; ++i)
				{
					line.Center += points[i];
				}
				float invNumPoints = ((float)1) / numPoints;
				line.Center *= invNumPoints;

				// Compute the covariance matrix of the points.
				float sumXX = (float)0, sumXY = (float)0, sumYY = (float)0;
				for (i = 0; i < numPoints; ++i)
				{
					Vector2 diff = points[i] - line.Center;
					sumXX += diff.x * diff.x;
					sumXY += diff.x * diff.y;
					sumYY += diff.y * diff.y;
				}

				sumXX *= invNumPoints;
				sumXY *= invNumPoints;
				sumYY *= invNumPoints;

				// Set up the eigensolver.
				float[,] matrix =
				{
					{ sumYY, -sumXY },
					{ sumXY,  sumXX }
				};
				// Compute eigenstuff, smallest eigenvalue is in last position.
				EigenData eigenData = EigenDecomposition.Solve(matrix, false);

				// Unit-length direction for best-fit line.
				line.Direction = eigenData.GetEigenvector2(1);

				return line;
			}
		}
	}
}
