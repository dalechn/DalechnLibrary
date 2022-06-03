using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public struct QuadraticRoots
		{
			public float X0;
			public float X1;
			public int   RootCount;
			public float this[int rootIndex]
			{
				get
				{
					switch (rootIndex)
					{
						case 0: return X0;
						case 1: return X1;
					}
					return float.NaN;
				}
			}
		}

		public struct CubicRoots
		{
			public float X0;
			public float X1;
			public float X2;
			public int   RootCount;
			public float this[int rootIndex]
			{
				get
				{
					switch (rootIndex)
					{
						case 0: return X0;
						case 1: return X1;
						case 2: return X2;
					}
					return float.NaN;
				}
			}
		}

		public struct QuarticRoots
		{
			public float X0;
			public float X1;
			public float X2;
			public float X3;
			public int   RootCount;
			public float this[int rootIndex]
			{
				get
				{
					switch (rootIndex)
					{
						case 0: return X0;
						case 1: return X1;
						case 2: return X2;
						case 3: return X3;
					}
					return float.NaN;
				}
			}
		}

		public static partial class RootFinder
		{
			private class PolyRootFinder
			{
				private int     _count;
				private int     _maxRoot;
				private float[] _roots;
				private float   _epsilon;

				public float[] Roots { get { return _roots; } }

				public PolyRootFinder(float epsilon)
				{
					_count   = 0;
					_maxRoot = 4;  // default support for degree <= 4
					_roots   = new float[_maxRoot];
					_epsilon = epsilon;
				}

				public bool Bisection(Polynomial poly, float xMin, float xMax, int digits, out float root)
				{
					float p0 = poly.Eval(xMin);
					if (Mathf.Abs(p0) <= Mathfex.ZeroTolerance)
					{
						root = xMin;
						return true;
					}
					float p1 = poly.Eval(xMax);
					if (Mathf.Abs(p1) <= Mathfex.ZeroTolerance)
					{
						root = xMax;
						return true;
					}

					root = float.NaN;

					if (p0 * p1 > (float)0)
					{
						return false;
					}

					// Determine number of iterations to get 'digits' accuracy..
					float tmp0 = Mathf.Log(xMax - xMin);
					float tmp1 = ((float)digits) * Mathf.Log((float)10);
					float arg = (tmp0 + tmp1) / Mathf.Log((float)2);
					int maxIter = (int)(arg + (float)0.5);

					for (int i = 0; i < maxIter; ++i)
					{
						root = ((float)0.5) * (xMin + xMax);
						float p = poly.Eval(root);
						float product = p * p0;
						if (product < (float)0)
						{
							xMax = root;
							p1 = p;
						}
						else if (product > (float)0)
						{
							xMin = root;
							p0 = p;
						}
						else
						{
							break;
						}
					}

					return true;
				}

				public bool Find(Polynomial poly, float xMin, float xMax, int digits)
				{
					// Reallocate root array if necessary.
					if (poly.Degree > _maxRoot)
					{
						_maxRoot = poly.Degree;
						_roots = new float[_maxRoot];
					}

					float root;
					if (poly.Degree == 1)
					{
						if (Bisection(poly, xMin, xMax, digits, out root) && root != float.NaN)
						{
							_count = 1;
							_roots[0] = root;
							return true;
						}
						_count = 0;
						return false;
					}

					// Get roots of derivative polynomial.
					Polynomial deriv = poly.CalcDerivative();
					Find(deriv, xMin, xMax, digits);

					int i, newCount = 0;
					float[] newRoot = new float[_count + 1];

					if (_count > 0)
					{
						// Find root on [xmin,root[0]].
						if (Bisection(poly, xMin, _roots[0], digits, out root))
						{
							newRoot[newCount++] = root;
						}

						// Find root on [root[i],root[i+1]] for 0 <= i <= count-2.
						for (i = 0; i <= _count - 2; ++i)
						{
							if (Bisection(poly, _roots[i], _roots[i + 1], digits, out root))
							{
								newRoot[newCount++] = root;
							}
						}

						// Find root on [root[count-1],xmax].
						if (Bisection(poly, _roots[_count - 1], xMax, digits, out root))
						{
							newRoot[newCount++] = root;
						}
					}
					else
					{
						// Polynomial is monotone on [xmin,xmax], has at most one root.
						if (Bisection(poly, xMin, xMax, digits, out root))
						{
							newRoot[newCount++] = root;
						}
					}

					// Copy to old buffer.
					if (newCount > 0)
					{
						_count = 1;
						_roots[0] = newRoot[0];
						for (i = 1; i < newCount; ++i)
						{
							float rootDiff = newRoot[i] - newRoot[i - 1];
							if (Mathf.Abs(rootDiff) > _epsilon)
							{
								_roots[_count++] = newRoot[i];
							}
						}
					}
					else
					{
						_count = 0;
					}

					return _count > 0;
				}
			}

			private const  float third         = 1f / 3f;
			private const  float twentySeventh = 1f / 27f;
			private static float sqrt3         = Mathf.Sqrt(3f);

			/// <summary>
			/// Linear equations: c1*x+c0 = 0
			/// </summary>
			public static bool Linear(float c0, float c1, out float root, float epsilon = Mathfex.ZeroTolerance)
			{
				if (Mathf.Abs(c1) >= epsilon)
				{
					root = -c0 / c1;
					return true;
				}

				root = float.NaN;
				return false;
			}

			/// <summary>
			/// Quadratic equations: c2*x^2+c1*x+c0 = 0
			/// </summary>
			public static bool Quadratic(float c0, float c1, float c2, out QuadraticRoots roots, float epsilon = Mathfex.ZeroTolerance)
			{
				if (Mathf.Abs(c2) <= epsilon)
				{
					// Polynomial is linear.
					float root;
					bool result = Linear(c0, c1, out root, epsilon);
					if (result)
					{
						roots.X0 = root;
						roots.X1 = float.NaN;
						roots.RootCount = 1;
					}
					else
					{
						roots.X0 = float.NaN;
						roots.X1 = float.NaN;
						roots.RootCount = 0;
					}
					return result;
				}

				float discr = c1 * c1 - 4f * c0 * c2;
				if (Mathf.Abs(discr) <= epsilon)
				{
					discr = 0f;
				}

				if (discr < 0f)
				{
					roots.X0 = float.NaN;
					roots.X1 = float.NaN;
					roots.RootCount = 0;
					return false;
				}

				float tmp = 0.5f / c2;

				if (discr > 0f)
				{
					discr = Mathf.Sqrt(discr);
					roots.X0 = tmp * (-c1 - discr);
					roots.X1 = tmp * (-c1 + discr);
					roots.RootCount = 2;
				}
				else
				{
					roots.X0 = -tmp * c1;
					roots.X1 = float.NaN;
					roots.RootCount = 1;
				}

				return true;
			}

			/// <summary>
			/// Cubic equations: c3*x^3+c2*x^2+c1*x+c0 = 0
			/// </summary>
			public static bool Cubic(float c0, float c1, float c2, float c3, out CubicRoots roots, float epsilon = Mathfex.ZeroTolerance)
			{
				if (Mathf.Abs(c3) <= epsilon)
				{
					// Polynomial is quadratic.
					QuadraticRoots tempRoots;
					bool result = Quadratic(c0, c1, c2, out tempRoots, epsilon);
					if (result)
					{	
						roots.X0 = tempRoots.X0;
						roots.X1 = tempRoots.X1;
						roots.X2 = float.NaN;
						roots.RootCount = tempRoots.RootCount;
					}
					else
					{
						roots.X0 = float.NaN;
						roots.X1 = float.NaN;
						roots.X2 = float.NaN;
						roots.RootCount = 0;
					}
					return result;
				}

				// Make polynomial monic, x^3+c2*x^2+c1*x+c0.
				float invC3 = 1f / c3;
				c2 *= invC3;
				c1 *= invC3;
				c0 *= invC3;

				// Convert to y^3+a*y+b = 0 by x = y-c2/3.
				float offset = third * c2;
				float a = c1 - c2 * offset;
				float b = c0 + c2 * (2f * c2 * c2 - 9f * c1) * twentySeventh;
				float halfB = 0.5f * b;

				float discr = halfB * halfB + a * a * a * twentySeventh;
				if (Mathf.Abs(discr) <= epsilon)
				{
					discr = 0f;
				}

				if (discr > 0f)  // 1 real, 2 complex roots
				{
					discr = Mathf.Sqrt(discr);
					float temp = -halfB + discr;
					if (temp >= 0f)
					{
						roots.X0 = Mathf.Pow(temp, third);
					}
					else
					{
						roots.X0 = -Mathf.Pow(-temp, third);
					}
					temp = -halfB - discr;
					if (temp >= 0f)
					{
						roots.X0 += Mathf.Pow(temp, third);
					}
					else
					{
						roots.X0 -= Mathf.Pow(-temp, third);
					}

					roots.X0 -= offset;
					roots.X1 = float.NaN;
					roots.X2 = float.NaN;
					roots.RootCount = 1;
				}
				else if (discr < 0f)
				{
					float dist = Mathf.Sqrt(-third * a);
					float angle = third * Mathf.Atan2(Mathf.Sqrt(-discr), -halfB);
					float cs = Mathf.Cos(angle);
					float sn = Mathf.Sin(angle);

					roots.X0 = 2f * dist * cs - offset;
					roots.X1 = -dist * (cs + sqrt3 * sn) - offset;
					roots.X2 = -dist * (cs - sqrt3 * sn) - offset;
					roots.RootCount = 3;
				}
				else
				{
					float temp;
					if (halfB >= 0f)
					{
						temp = -Mathf.Pow(halfB, third);
					}
					else
					{
						temp = Mathf.Pow(-halfB, third);
					}

					roots.X0 = 2f * temp - offset;
					roots.X1 = -temp - offset;
					roots.X2 = roots.X1;
					roots.RootCount = 3;
				}

				return true;
			}

			/// <summary>
			/// Quartic equations: c4*x^4+c3*x^3+c2*x^2+c1*x+c0 = 0
			/// </summary>
			public static bool Quartic(float c0, float c1, float c2, float c3, float c4, out QuarticRoots roots, float epsilon = Mathfex.ZeroTolerance)
			{
				roots.X0 = float.NaN;
				roots.X1 = float.NaN;
				roots.X2 = float.NaN;
				roots.X3 = float.NaN;

				if (Mathf.Abs(c4) <= epsilon)
				{
					// Polynomial is cubic.
					CubicRoots tempRoots;
					bool result = Cubic(c0, c1, c2, c3, out tempRoots, epsilon);
					if (result)
					{
						roots.X0 = tempRoots.X0;
						roots.X1 = tempRoots.X1;
						roots.X2 = tempRoots.X2;
						roots.RootCount = tempRoots.RootCount;
					}
					else
					{
						roots.RootCount = 0;
					}
					return result;
				}

				// Make polynomial monic, x^4+c3*x^3+c2*x^2+c1*x+c0.
				float invC4 = ((float)1) / c4;
				c0 *= invC4;
				c1 *= invC4;
				c2 *= invC4;
				c3 *= invC4;

				// Reduction to resolvent cubic polynomial y^3+r2*y^2+r1*y+r0 = 0.
				float r0 = -c3 * c3 * c0 + ((float)4) * c2 * c0 - c1 * c1;
				float r1 = c3 * c1 - ((float)4) * c0;
				float r2 = -c2;

				CubicRoots cubicRoots;
				Cubic(r0, r1, r2, (float)1, out cubicRoots, epsilon);  // always produces at least one root
				float y = cubicRoots.X0;

				roots.RootCount = 0;
				float discr = ((float)0.25) * c3 * c3 - c2 + y;
				if (Mathf.Abs(discr) <= epsilon)
				{
					discr = (float)0;
				}

				if (discr > (float)0)
				{
					float r = Mathf.Sqrt(discr);
					float t1 = ((float)0.75) * c3 * c3 - r * r - ((float)2) * c2;
					float t2 = (((float)4) * c3 * c2 - ((float)8) * c1 - c3 * c3 * c3) / (((float)4.0) * r);

					float tPlus = t1 + t2;
					float tMinus = t1 - t2;
					if (Mathf.Abs(tPlus) <= epsilon)
					{
						tPlus = (float)0;
					}
					if (Mathf.Abs(tMinus) <= epsilon)
					{
						tMinus = (float)0;
					}

					if (tPlus >= (float)0)
					{
						float d = Mathf.Sqrt(tPlus);
						roots.X0 = -((float)0.25) * c3 + ((float)0.5) * (r + d);
						roots.X1 = -((float)0.25) * c3 + ((float)0.5) * (r - d);
						roots.RootCount += 2;
					}
					if (tMinus >= (float)0)
					{
						float e = Mathf.Sqrt(tMinus);
						if (roots.RootCount == 0)
						{
							roots.X0 = -((float)0.25) * c3 + ((float)0.5) * (e - r);
							roots.X1 = -((float)0.25) * c3 - ((float)0.5) * (e + r);
						}
						else
						{
							roots.X2 = -((float)0.25) * c3 + ((float)0.5) * (e - r);
							roots.X3 = -((float)0.25) * c3 - ((float)0.5) * (e + r);
						}
						roots.RootCount += 2;
					}
				}
				else if (discr < (float)0)
				{
					roots.RootCount = 0;
				}
				else
				{
					float t2 = y * y - ((float)4) * c0;
					if (t2 >= -epsilon)
					{
						if (t2 < (float)0) // round to zero
						{
							t2 = (float)0;
						}
						t2 = ((float)2) * Mathf.Sqrt(t2);
						float t1 = ((float)0.75) * c3 * c3 - ((float)2) * c2;
						float tPlus = t1 + t2;
						if (tPlus >= epsilon)
						{
							float d = Mathf.Sqrt(tPlus);
							roots.X0 = -((float)0.25) * c3 + ((float)0.5) * d;
							roots.X1 = -((float)0.25) * c3 - ((float)0.5) * d;
							roots.RootCount += 2;
						}
						float tMinus = t1 - t2;
						if (tMinus >= epsilon)
						{
							float e = Mathf.Sqrt(tMinus);
							if (roots.RootCount == 0)
							{
								roots.X0 = -((float)0.25) * c3 + ((float)0.5) * e;
								roots.X1 = -((float)0.25) * c3 - ((float)0.5) * e;
							}
							else
							{
								roots.X2 = -((float)0.25) * c3 + ((float)0.5) * e;
								roots.X3 = -((float)0.25) * c3 - ((float)0.5) * e;
							}
							roots.RootCount += 2;
						}
					}
				}

				return roots.RootCount > 0;
			}

			/// <summary>
			/// Gets roots bound of the given polynomial or -1 if polynomial is constant.
			/// </summary>
			public static float PolynomialBound(Polynomial poly, float epsilon = Mathfex.ZeroTolerance)
			{
				Polynomial copyPoly = poly.DeepCopy();
				copyPoly.Compress(epsilon);

				int degree = copyPoly.Degree;
				if (degree < 1)
				{
					// Polynomial is constant, return invalid bound.
					return -1f;
				}

				float invCopyDeg = 1f / copyPoly[degree];
				float maxValue = 0f;
				for (int i = 0; i < degree; ++i)
				{
					float tmp = Mathf.Abs(copyPoly[i]) * invCopyDeg;
					if (tmp > maxValue)
					{
						maxValue = tmp;
					}
				}

				return 1f + maxValue;
			}

			/// <summary>
			/// General polynomial equation: Σ(c_i * x^i), where i=[0..degree]. Finds roots in the interval [xMin..xMax].
			/// </summary>
			/// <param name="poly">Polynomial whose roots to be found</param>
			/// <param name="xMin">Interval left border</param>
			/// <param name="xMax">Interval right border</param>
			/// <param name="roots">Roots of the polynomial</param>
			/// <param name="digits">Accuracy</param>
			/// <param name="epsilon">Small positive number</param>
			public static bool Polynomial(Polynomial poly, float xMin, float xMax, out float[] roots, int digits = 6, float epsilon = Mathfex.ZeroTolerance)
			{
				PolyRootFinder finder = new PolyRootFinder(epsilon);
				if (finder.Find(poly, xMin, xMax, digits))
				{
					roots = finder.Roots;
					return true;
				}
				else
				{
					roots = new float[0];
					return false;
				}
			}

			/// <summary>
			/// General polynomial equation: Σ(c_i * x^i), where i=[0..degree].
			/// </summary>
			/// <param name="poly">Polynomial whose roots to be found</param>
			/// <param name="roots">Roots of the polynomial</param>
			/// <param name="digits">Accuracy</param>
			/// <param name="epsilon">Small positive number</param>
			public static bool Polynomial(Polynomial poly, out float[] roots, int digits = 6, float epsilon = Mathfex.ZeroTolerance)
			{
				float bound = PolynomialBound(poly);
				if (bound == -1f)
				{
					roots = new float[0];
					return false;
				}
				return Polynomial(poly, -bound, bound, out roots, digits, epsilon);
			}		
		}
	}
}
