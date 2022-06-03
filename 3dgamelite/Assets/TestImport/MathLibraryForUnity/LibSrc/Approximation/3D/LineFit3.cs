using UnityEngine;
using System.Collections.Generic;

namespace Dest
{
	namespace Math
	{
		public static partial class Approximation
		{
			/// <summary>
			/// Producing a line using least-squares fitting. A set must contain at least one point!
			/// </summary>
			public static Line3 LeastsSquaresLineFit3(IList<Vector3> points)
			{
				Line3 line = new Line3();
				int numPoints = points.Count;

				// Compute the mean of the points.
				line.Center = points[0];
				int i;
				for (i = 1; i < numPoints; i++)
				{
					line.Center += points[i];
				}
				float invNumPoints = ((float)1) / numPoints;
				line.Center *= invNumPoints;

				// Compute the covariance matrix of the points.
				float sumXX = (float)0, sumXY = (float)0, sumXZ = (float)0;
				float sumYY = (float)0, sumYZ = (float)0, sumZZ = (float)0;
				for (i = 0; i < numPoints; i++)
				{
					Vector3 diff = points[i] - line.Center;
					sumXX += diff.x * diff.x;
					sumXY += diff.x * diff.y;
					sumXZ += diff.x * diff.z;
					sumYY += diff.y * diff.y;
					sumYZ += diff.y * diff.z;
					sumZZ += diff.z * diff.z;
				}

				sumXX *= invNumPoints;
				sumXY *= invNumPoints;
				sumXZ *= invNumPoints;
				sumYY *= invNumPoints;
				sumYZ *= invNumPoints;
				sumZZ *= invNumPoints;

				// Set up the eigensolver.
				float[,] matrix = new float[3, 3];
				matrix[0, 0] = sumYY + sumZZ;
				matrix[0, 1] = -sumXY;
				matrix[0, 2] = -sumXZ;
				matrix[1, 0] = matrix[0, 1];
				matrix[1, 1] = sumXX + sumZZ;
				matrix[1, 2] = -sumYZ;
				matrix[2, 0] = matrix[0, 2];
				matrix[2, 1] = matrix[1, 2];
				matrix[2, 2] = sumXX + sumYY;

				// Compute eigenstuff, smallest eigenvalue is in last position.
				EigenData eigenData = EigenDecomposition.Solve(matrix, false);

				// Unit-length direction for best-fit line.
				line.Direction = eigenData.GetEigenvector3(2);

				return line;
			}
		}
	}
}
