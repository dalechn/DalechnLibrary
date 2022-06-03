using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static class Mathfex
		{
			/// <summary>
			/// 1e-5f
			/// </summary>
			public const float ZeroTolerance = 1e-5f;

			/// <summary>
			/// -1e-5f
			/// </summary>
			public const float NegativeZeroTolerance = -ZeroTolerance;

			/// <summary>
			/// (1e-5f)^2
			/// </summary>
			public const float ZeroToleranceSqr = ZeroTolerance * ZeroTolerance;

			/// <summary>
			/// π
			/// </summary>
			public const float Pi = Mathf.PI;

			/// <summary>
			/// π/2
			/// </summary>
			public const float HalfPi = 0.5f * Pi;

			/// <summary>
			/// 2*π
			/// </summary>
			public const float TwoPi = 2f * Pi;

			
			/// <summary>
			/// Evaluates x^2
			/// </summary>
			public static float EvalSquared(float x)
			{
				return x * x;
			}

			/// <summary>
			/// Evaluates x^(1/2)
			/// </summary>
			public static float EvalInvSquared(float x)
			{
				return Mathf.Sqrt(x);
			}

			/// <summary>
			/// Evaluates x^3
			/// </summary>
			public static float EvalCubic(float x)
			{
				return x * x * x;
			}

			/// <summary>
			/// Evaluates x^(1/3)
			/// </summary>
			public static float EvalInvCubic(float x)
			{
				return Mathf.Pow(x, 1f / 3f);
			}

			/// <summary>
			/// Evaluates quadratic equation a*x^2 + b*x + c
			/// </summary>
			public static float EvalQuadratic(float x, float a, float b, float c)
			{
				return a * x * x + b * x + c;
			}

			/// <summary>
			/// Evaluates sigmoid function (used for smoothing values).
			/// Formula: x^2 * (3 - 2*x)
			/// </summary>
			public static float EvalSigmoid(float x)
			{
				return x * x * (3f - 2f * x);
			}

			/// <summary>
			/// Evaluates overlapped step function. Useful for animating several objects
			/// (stepIndex parameter is number of the objects), where animations follow one after
			/// another with some overlapping in time (overlap parameter).
			/// </summary>
			/// <param name="x">Evaluation parameter, makes sence in [0,1] range</param>
			/// <param name="overlap">Overlapping between animations (must be greater or equal to zero),
			/// where 0 means that animations do not overlap and follow one after another.</param>
			/// <param name="objectIndex">Index of object beeing animated</param>
			/// <param name="objectCount">Number of objects beeing animated</param>
			public static float EvalOverlappedStep(float x, float overlap, int objectIndex, int objectCount)
			{
				float result = (x - (1f - overlap) * objectIndex / (objectCount - 1f)) / overlap;
				if (result < 0f)
				{
					result = 0f;
				}
				else if (result > 1f)
				{
					result = 1f;
				}
				return result;
			}

			/// <summary>
			/// Evaluates overlapped step function and applies sigmoid to smooth the result. Useful for animating several objects
			/// (stepIndex parameter is number of the objects), where animations follow one after
			/// another with some overlapping in time (overlap parameter).
			/// </summary>
			/// <param name="x">Evaluation parameter, makes sence in [0,1] range</param>
			/// <param name="overlap">Overlapping between animations (must be greater or equal to zero),
			/// where 0 means that animations do not overlap and follow one after another.</param>
			/// <param name="objectIndex">Index of object beeing animated</param>
			/// <param name="objectCount">Number of objects beeing animated</param>
			public static float EvalSmoothOverlappedStep(float x, float overlap, int objectIndex, int objectCount)
			{
				float result = (x - (1f - overlap) * objectIndex / (objectCount - 1f)) / overlap;
				if (result < 0f)
				{
					result = 0f;
				}
				else if (result > 1f)
				{
					result = 1f;
				}
				return result * result * (3f - 2f * result);
			}

			/// <summary>
			/// Evaluates scalar gaussian function. The formula is:
			/// a * e^(-(x-b)^2 / 2*c^2)
			/// </summary>
			/// <param name="x">Function parameter</param>
			public static float EvalGaussian(float x, float a, float b, float c)
			{
				float x_min_b = x - b;
				return a * Mathf.Exp(x_min_b * x_min_b / (-2f * c * c));
			}

			/// <summary>
			/// Evaluates 2-dimensional gaussian function. The formula is:
			/// A * e^(-(a*(x - x0)^2 + 2*b*(x - x0)*(y - y0) + c*(y - y0)^2))
			/// </summary>
			/// <param name="x">First function parameter</param>
			/// <param name="y">Second function parameter</param>
			public static float EvalGaussian2D(float x, float y, float x0, float y0, float A, float a, float b, float c)
			{
				float x_min_x0 = x - x0;
				float y_min_y0 = y - y0;
				return A * Mathf.Exp(-(a * x_min_x0 * x_min_x0 + 2f * b * x_min_x0 * y_min_y0 + c * y_min_y0 * y_min_y0));
			}


			/// <summary>
			/// Linearly interpolates between 'value0' and 'value1'.
			/// </summary>
			/// <param name="factor">Interpolation factor in range [0..1] (will be clamped)</param>
			public static float Lerp(float value0, float value1, float factor)
			{
				if (factor < 0f) factor = 0f; else if (factor > 1f) factor = 1f;
				return value0 + (value1 - value0) * factor;
			}

			/// <summary>
			/// Linearly interpolates between 'value0' and 'value1'.
			/// </summary>
			/// <param name="factor">Interpolation factor in range [0..1] (will NOT be clamped, i.e. interpolation can overshoot)</param>
			public static float LerpUnclamped(float value0, float value1, float factor)
			{
				return value0 + (value1 - value0) * factor;
			}

			/// <summary>
			/// Interpolates between 'value0' and 'value1' using sigmoid as interpolation function.
			/// </summary>
			/// <param name="factor">Interpolation factor in range [0..1] (will be clamped)</param>
			public static float SigmoidInterp(float value0, float value1, float factor)
			{
				if (factor < 0f) factor = 0f; else if (factor > 1f) factor = 1f;
				factor = factor * factor * (3f - 2f * factor);
				return value0 + (value1 - value0) * factor;
			}

			/// <summary>
			/// Interpolates between 'value0' and 'value1' using sine function easing at the end.
			/// </summary>
			/// <param name="factor">Interpolation factor in range [0..1] (will be clamped)</param>
			public static float SinInterp(float value0, float value1, float factor)
			{
				if (factor < 0f) factor = 0f; else if (factor > 1f) factor = 1f;
				factor = Mathf.Sin(factor * Mathfex.HalfPi);
				return value0 + (value1 - value0) * factor;
			}

			/// <summary>
			/// Interpolates between 'value0' and 'value1' using cosine function easing in the start.
			/// </summary>
			/// <param name="factor">Interpolation factor in range [0..1] (will be clamped)</param>
			public static float CosInterp(float value0, float value1, float factor)
			{
				if (factor < 0f) factor = 0f; else if (factor > 1f) factor = 1f;
				factor = 1.0f - Mathf.Cos(factor * Mathfex.HalfPi);
				return value0 + (value1 - value0) * factor;
			}

			/// <summary>
			/// Interpolates between 'value0' and 'value1' in using special function which overshoots first, then waves back and forth gradually declining towards the end.
			/// </summary>
			/// <param name="factor">Interpolation factor in range [0..1] (will be clamped)</param>
			public static float WobbleInterp(float value0, float value1, float factor)
			{
				if (factor < 0f) factor = 0f; else if (factor > 1f) factor = 1f;
				factor = (Mathf.Sin(factor * Mathf.PI * (0.2f + 2.5f * factor * factor * factor)) * Mathf.Pow(1f - factor, 2.2f) + factor) * (1f + (1.2f * (1f - factor)));
				return value0 + (value1 - value0) * factor;
			}

			/// <summary>
			/// Interpolates between 'value0' and 'value1' using provided animation curve (curve will be sampled in [0..1] range]).
			/// </summary>
			/// <param name="factor">Interpolation factor in range [0..1] (will be clamped)</param>
			public static float CurveInterp(float value0, float value1, float factor, AnimationCurve curve)
			{
				if (factor < 0f) factor = 0f; else if (factor > 1f) factor = 1f;
				factor = curve.Evaluate(factor);
				return value0 + (value1 - value0) * factor;
			}

			/// <summary>
			/// Interpolates between 'value0' and 'value1' using provided function (function will be sampled in [0..1] range]).
			/// </summary>
			/// <param name="factor">Interpolation factor in range [0..1] (will be clamped)</param>
			public static float FuncInterp(float value0, float value1, float factor, System.Func<float, float> func)
			{
				if (factor < 0f) factor = 0f; else if (factor > 1f) factor = 1f;
				float t = func(factor);
				return value0 * (1f - t) + value1 * t;
			}

			
			/// <summary>
			/// Returns 1/Sqrt(value) if value != 0, otherwise returns 0.
			/// </summary>
			public static float InvSqrt(float value)
			{
				if (value != 0.0f)
				{
					return 1f / Mathf.Sqrt(value);
				}
				else
				{
					return 0.0f;
				}
			}

			/// <summary>
			/// Returns abs(v0-v1)&lt;eps
			/// </summary>
			public static bool Near(float value0, float value1, float epsilon = Mathfex.ZeroTolerance)
			{
				return Mathf.Abs(value0 - value1) < epsilon;
			}

			/// <summary>
			/// Returns abs(v)&lt;eps
			/// </summary>
			public static bool NearZero(float value, float epsilon = Mathfex.ZeroTolerance)
			{
				return Mathf.Abs(value) < epsilon;
			}


			/// <summary>
			/// Converts cartesian coordinates to polar coordinates.
			/// Resulting vector contains rho (length) in x coordinate and phi (angle) in y coordinate; rho >= 0, 0 &lt;= phi &lt; 2pi.
			/// If cartesian coordinates are (0,0) resulting coordinates are (0,0).
			/// </summary>
			public static Vector2 CartesianToPolar(Vector2 cartesianCoordinates)
			{
				float x = cartesianCoordinates.x;
				float y = cartesianCoordinates.y;
				Vector2 result;

				result.x = Mathf.Sqrt(x * x + y * y);

				if (x > 0f)
				{
					if (y >= 0)
					{
						result.y = Mathf.Atan(y / x);
					}
					else // y < 0
					{
						result.y = Mathf.Atan(y / x) + TwoPi;
					}
				}
				else if (x < 0f)
				{
					result.y = Mathf.Atan(y / x) + Pi;
				}
				else // x == 0
				{
					if (y > 0f)
					{
						result.y = HalfPi;
					}
					else if (y < 0f)
					{
						result.y = HalfPi + Pi;
					}
					else // y == 0
					{
						result.x = 0f;
						result.y = 0f;
					}
				}

				return result;
			}

			/// <summary>
			/// Converts polar coordinates to cartesian coordinates.
			/// Input vector contains rho (length) in x coordinate and phi (angle) in y coordinate; rho >= 0, 0 &lt;= phi &lt; 2pi.
			/// </summary>
			public static Vector2 PolarToCartesian(Vector2 polarCoordinates)
			{
				Vector2 result;
				result.x = polarCoordinates.x * Mathf.Cos(polarCoordinates.y);
				result.y = polarCoordinates.x * Mathf.Sin(polarCoordinates.y);
				return result;
			}

			/// <summary>
			/// Converts cartesian coordinates to spherical coordinates.
			/// Resulting vector contains rho (length) in x coordinate, theta (azimutal angle in XZ plane from X axis) in y coordinate,
			/// phi (zenith angle from positive Y axis) in z coordinate; rho >= 0, 0 &lt;= theta &lt; 2pi, 0 &lt;= phi &lt; pi.
			/// If cartesian coordinates are (0,0,0) resulting coordinates are (0,0,0).
			/// </summary>
			/// <param name="cartesianCoordinates"></param>
			/// <returns></returns>
			public static Vector3 CartesianToSpherical(Vector3 cartesianCoordinates)
			{
				float x = cartesianCoordinates.x;
				float y = cartesianCoordinates.y;
				float z = cartesianCoordinates.z;
				
				float rho = Mathf.Sqrt(x * x + y * y + z * z);
				float theta;
				float phi;

				if (rho != 0f)
				{
					phi = Mathf.Acos(y / rho);

					if (x > 0f)
					{
						if (z >= 0)
						{
							theta = Mathf.Atan(z / x);
						}
						else // z < 0
						{
							theta = Mathf.Atan(z / x) + TwoPi;
						}
					}
					else if (x < 0f)
					{
						theta = Mathf.Atan(z / x) + Pi;
					}
					else // x == 0
					{
						if (z > 0f)
						{
							theta = HalfPi;
						}
						else if (z < 0f)
						{
							theta = HalfPi + Pi;
						}
						else // z == 0
						{
							theta = 0f;
						}
					}
				}
				else
				{
					rho   = 0f;
					theta = 0f;
					phi   = 0f;
				}

				Vector3 result;
				result.x = rho;
				result.y = theta;
				result.z = phi;

				return result;
			}

			/// <summary>
			/// Converts spherical coordinates to cartesian coordinates.
			/// Input vector contains rho (length) in x coordinate, theta (azimutal angle in XZ plane from X axis) in y coordinate,
			/// phi (zenith angle from positive Y axis) in z coordinate; rho >= 0, 0 &lt;= theta &lt; 2pi, 0 &lt;= phi &lt; pi.
			/// </summary>
			/// <param name="sphericalCoordinates"></param>
			/// <returns></returns>
			public static Vector3 SphericalToCartesian(Vector3 sphericalCoordinates)
			{
				float rho    = sphericalCoordinates.x;
				float theta  = sphericalCoordinates.y;
				float phi    = sphericalCoordinates.z;

				float sinPhi = Mathf.Sin(phi);

				Vector3 result;
				result.x = rho * Mathf.Cos(theta) * sinPhi;
				result.y = rho * Mathf.Cos(phi);
				result.z = rho * Mathf.Sin(theta) * sinPhi;

				return result;
			}

			/// <summary>
			/// Converts cartesian coordinates to cylindrical coordinates.
			/// Resulting vector contains rho (length) in x coordinate, phi (polar angle in XZ plane) in y coordinate,
			/// height (height from XZ plane to the point) in z coordinate.
			/// </summary>
			public static Vector3 CartesianToCylindrical(Vector3 cartesianCoordinates)
			{
				float x = cartesianCoordinates.x;
				float z = cartesianCoordinates.z;
				
				float rho = Mathf.Sqrt(x * x + z * z);
				float phi;

				if (x > 0f)
				{
					if (z >= 0)
					{
						phi = Mathf.Atan(z / x);
					}
					else // z < 0
					{
						phi = Mathf.Atan(z / x) + TwoPi;
					}
				}
				else if (x < 0f)
				{
					phi = Mathf.Atan(z / x) + Pi;
				}
				else // x == 0
				{
					if (z > 0f)
					{
						phi = HalfPi;
					}
					else if (z < 0f)
					{
						phi = HalfPi + Pi;
					}
					else // z == 0
					{
						phi = 0;
					}
				}

				Vector3 result;
				result.x = rho;
				result.y = phi;
				result.z = cartesianCoordinates.y;

				return result;
			}

			/// <summary>
			/// Converts cylindrical coordinates to cartesian coordinates.
			/// Input vector contains rho (length) in x coordinate, phi (polar angle in XZ plane) in y coordinate,
			/// height (height from XZ plane to the point) in z coordinate.
			/// </summary>
			public static Vector3 CylindricalToCartesian(Vector3 cylindricalCoordinates)
			{
				Vector3 result;
				result.x = cylindricalCoordinates.x * Mathf.Cos(cylindricalCoordinates.y);
				result.y = cylindricalCoordinates.z;
				result.z = cylindricalCoordinates.x * Mathf.Sin(cylindricalCoordinates.y);
				return result;
			}
		}
	}
}
