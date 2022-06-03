using UnityEngine;
using System.Collections.Generic;

namespace Dest
{
	namespace Math
	{
		public static partial class Approximation
		{
			/// <summary>
			/// Fits points with a Gaussian distribution. Produces box as the result.
			/// Box center is average of a point set. Box axes are eigenvectors of the
			/// covariance matrix, box extents are eigenvalues.
			/// A set must contain at least one point!
			/// </summary>
			public static Box3 GaussPointsFit3(IList<Vector3> points)
			{
				Box3 box = new Box3(Vector3ex.Zero,Vector3ex.UnitX, Vector3ex.UnitY, Vector3ex.UnitZ, Vector3ex.One);
				int numPoints = points.Count;

				// Compute the mean of the points.
				box.Center = points[0];
				for (int i = 1; i < numPoints; ++i)
				{
					box.Center += points[i];
				}
				float invNumPoints = 1.0f / numPoints;
				box.Center *= invNumPoints;

				// Compute the covariance matrix of the points.
				float sumXX = 0f, sumXY = 0f, sumXZ = 0f;
				float sumYY = 0f, sumYZ = 0f, sumZZ = 0f;
				for (int i = 0; i < numPoints; ++i)
				{
					Vector3 diff = points[i] - box.Center;
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

				// Setup the eigensolver.
				float[,] matrix = 
				{
					{ sumXX, sumXY, sumXZ },
					{ sumXY, sumYY, sumYZ },
					{ sumXZ, sumYZ, sumZZ }
				};
				EigenData eigenData = EigenDecomposition.Solve(matrix, true);

				box.Extents.x = eigenData.GetEigenvalue(0);
				box.Axis0 = eigenData.GetEigenvector3(0);

				box.Extents.y = eigenData.GetEigenvalue(1);
				box.Axis1 = eigenData.GetEigenvector3(1);

				box.Extents.z = eigenData.GetEigenvalue(2);
				box.Axis2 = eigenData.GetEigenvector3(2);

				return box;
			}
		}
	}
}
