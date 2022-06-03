using UnityEngine;

namespace Dest.Math
{
	/// <summary>
	/// Implements presudo-random number generator using Xorshift128 algorithm.
	/// </summary>
	public class Rand
	{
		// http://www.jstatsoft.org/v08/i14/paper

		private const int a = 5;
		private const int b = 14;
		private const int c = 1;

		private const uint DefaultY = 273326509;
		private const uint DefaultZ = 3579807591;
		private const uint DefaultW = 842502087;

		private const uint PositiveMask   = 0x7FFFFFFF;
		private const uint BoolModuloMask = 0x1;
		private const uint ByteModuloMask = 0xFF;

		private const double One_div_uintMaxValuePlusOne = 1.0 / ((double)uint.MaxValue + 1.0);
		private const double TwoPi = System.Math.PI * 2.0;

		private static Rand _seedGenerator;

		private uint _x;
		private uint _y;
		private uint _z;
		private uint _w;

		public static Rand Instance;
		
		static Rand()
		{
			_seedGenerator = new Rand(System.Environment.TickCount);
			Instance = new Rand();
		}

		/// <summary>
		/// Creates random number generator using randomized seed.
		/// </summary>
		public Rand()
		{
			ResetSeed(_seedGenerator.NextInt());
		}

		/// <summary>
		/// Creates random number generator using specified seed.
		/// </summary>
		public Rand(int seed)
		{
			ResetSeed(seed);
		}

		/// <summary>
		/// Resets generator using specified seed.
		/// </summary>
		public void ResetSeed(int seed)
		{
			_x = (uint)((seed * 1183186591) + (seed * 1431655781) + (seed * 338294347) + (seed * 622729787));
			_y = DefaultY;
			_z = DefaultZ;
			_w = DefaultW;
		}

		/// <summary>
		/// Gets generator inner state represented by four uints. Can be used for generator serialization.
		/// </summary>
		public void GetState(out uint x, out uint y, out uint z, out uint w)
		{
			x = _x;
			y = _y;
			z = _z;
			w = _w;
		}

		/// <summary>
		/// Sets generator inner state from four uints. Can be used for generator deserialization.
		/// </summary>
		public void SetState(uint x, uint y, uint z, uint w)
		{
			_x = x;
			_y = y;
			_z = z;
			_w = w;
		}

		/// <summary>
		/// Generates a random integer in the range [int.MinValue,int.MaxValue].
		/// </summary>
		public int NextInt()
		{
			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
			return (int)(_w);
		}

		/// <summary>
		/// Generates a random integer in the range [0,max)
		/// </summary>
		public int NextInt(int max)
		{
			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
			return (int)((double)_w * One_div_uintMaxValuePlusOne * (double)max);
		}

		/// <summary>
		/// Generates a random integer in the range [min,max). max must be >= min.
		/// </summary>
		public int NextInt(int min, int max)
		{
			if (min > max)
			{
				Logger.LogError("max must be >= min");
				return 0;
			}

			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));

			int range = unchecked(max - min);
			if (range >= 0) // No overflow
			{
				return min + (int)((double)_w * One_div_uintMaxValuePlusOne * (double)range);
			}

			long longMin = (long)min;
			return (int)(longMin + (long)((double)_w * One_div_uintMaxValuePlusOne * (double)((long)max - longMin)));			
		}

		/// <summary>
		/// Generates a random integer in the range [min,max]. max must be >= min.
		/// The method simply calls NextInt(min,max+1), thus largest allowable value for max is int.MaxValue-1.
		/// </summary>
		public int NextIntInclusive(int min, int max)
		{
			return NextInt(min, max + 1);
		}

		/// <summary>
		/// Generates a random integer in the range [0,int.MaxValue].
		/// </summary>
		public int NextPositiveInt()
		{
			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
			return (int)(_w & PositiveMask);
		}

		/// <summary>
		/// Generates a random unsigned integer in the range [0,uint.MaxValue].
		/// </summary>
		public uint NextUInt()
		{
			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
			return _w;
		}

		/// <summary>
		/// Generates a random double in the range [0,1).
		/// </summary>
		public double NextDouble()
		{
			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
			return (double)_w * One_div_uintMaxValuePlusOne;
		}

		/// <summary>
		/// Generates a random double in the range [min,max).
		/// </summary>
		public double NextDouble(double min, double max)
		{
			if (min > max)
			{
				Logger.LogError("max must be >= min");
				return 0.0;
			}

			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));

			return min + (max - min) * ((double)_w * One_div_uintMaxValuePlusOne);
		}

		/// <summary>
		/// Generates a random float in the range [0,1).
		/// </summary>
		public float NextFloat()
		{
			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
			return (float)((double)_w * One_div_uintMaxValuePlusOne);
		}

		/// <summary>
		/// Generates a random float in the range [min,max).
		/// </summary>
		public float NextFloat(float min, float max)
		{
			if (min > max)
			{
				Logger.LogError("max must be >= min");
				return 0.0f;
			}

			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));

			return min + (max - min) * (float)((double)_w * One_div_uintMaxValuePlusOne);
		}

		/// <summary>
		/// Generates a random bool.
		/// </summary>
		public bool NextBool()
		{
			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
			return (_w & BoolModuloMask) == 0;
		}

		/// <summary>
		/// Generates a random byte.
		/// </summary>
		public byte NextByte()
		{
			uint t = _x ^ (_x << a);
			_x = _y;
			_y = _z;
			_z = _w;
			_w = (_w ^ (_w >> c)) ^ (t ^ (t >> b));
			return (byte)(_w & ByteModuloMask);
		}

		/// <summary>
		/// Generates a random opaque color.
		/// </summary>
		public Color RandomColorOpaque()
		{
			return new Color(NextFloat(), NextFloat(), NextFloat());
		}

		/// <summary>
		/// Generates a random color with randomized alpha.
		/// </summary>
		public Color RandomColorTransparent()
		{
			return new Color(NextFloat(), NextFloat(), NextFloat(), NextFloat());
		}

		/// <summary>
		/// Generates a random opaque color.
		/// </summary>
		public Color32 RandomColor32Opaque()
		{
			return new Color32(NextByte(), NextByte(), NextByte(), 255);
		}

		/// <summary>
		/// Generates a random color with randomized alpha.
		/// </summary>
		public Color32 RandomColor32Transparent()
		{
			return new Color32(NextByte(), NextByte(), NextByte(), NextByte());
		}
	
		/// <summary>
		/// Generates a random angle [0,2*pi)
		/// </summary>
		public float RandomAngleRadians()
		{
			return NextFloat() * Mathfex.TwoPi;
		}

		/// <summary>
		/// Generates a random angle [0,360)
		/// </summary>
		public float RandomAngleDegrees()
		{
			return NextFloat() * 360f;
		}

		/// <summary>
		/// Generates a random point inside the square with specified side size.
		/// </summary>
		public Vector2 InSquare(float side = 1f)
		{
			float halfPos = side * .5f;
			float halfNeg = -halfPos;
			return new Vector2(NextFloat(halfNeg, halfPos), NextFloat(halfNeg, halfPos));
		}

		/// <summary>
		/// Generates a random point on the border of the square with specified side size.
		/// </summary>
		public Vector2 OnSquare(float side = 1f)
		{
			float halfSidePos = side * .5f;
			float halfSideNeg = -halfSidePos;
			int sideIndex = NextInt(0, 4);
			switch (sideIndex)
			{
				case 0: return new Vector2(NextFloat(halfSideNeg, halfSidePos), halfSidePos);
				case 1: return new Vector2(NextFloat(halfSideNeg, halfSidePos), halfSideNeg);
				case 2: return new Vector2(halfSidePos, NextFloat(halfSideNeg, halfSidePos));
				case 3: return new Vector2(halfSideNeg, NextFloat(halfSideNeg, halfSidePos));
			}
			Logger.LogError("Should not get here!");
			return Vector2ex.Zero;
		}

		/// <summary>
		/// Generates a random point inside the cube with specified side size.
		/// </summary>
		public Vector3 InCube(float side = 1f)
		{
			float halfPos = side * .5f;
			float halfNeg = -halfPos;
			return new Vector3(NextFloat(halfNeg, halfPos), NextFloat(halfNeg, halfPos), NextFloat(halfNeg, halfPos));
		}

		/// <summary>
		/// Generates a random point on the surface of the cube with specified side size.
		/// </summary>
		public Vector3 OnCube(float side = 1f)
		{
			float halfSidePos = side * .5f;
			float halfSideNeg = -halfSidePos;
			int sideIndex = NextInt(0, 6);
			switch (sideIndex)
			{
				case 0: return new Vector3(NextFloat(halfSideNeg, halfSidePos), NextFloat(halfSideNeg, halfSidePos), halfSidePos);
				case 1: return new Vector3(NextFloat(halfSideNeg, halfSidePos), NextFloat(halfSideNeg, halfSidePos), halfSideNeg);
				case 2: return new Vector3(NextFloat(halfSideNeg, halfSidePos), halfSidePos, NextFloat(halfSideNeg, halfSidePos));
				case 3: return new Vector3(NextFloat(halfSideNeg, halfSidePos), halfSideNeg, NextFloat(halfSideNeg, halfSidePos));
				case 4: return new Vector3(halfSidePos, NextFloat(halfSideNeg, halfSidePos), NextFloat(halfSideNeg, halfSidePos));
				case 5: return new Vector3(halfSideNeg, NextFloat(halfSideNeg, halfSidePos), NextFloat(halfSideNeg, halfSidePos));
			}
			Logger.LogError("Should not get here!");
			return Vector3ex.Zero;
		}

		/// <summary>
		/// Generates a random point inside the circle with specified radius.
		/// </summary>
		public Vector2 InCircle(float radius = 1f)
		{
			// http://www.anderswallin.net/2009/05/uniform-random-points-in-a-circle-using-polar-coordinates/
			float r = radius * Mathf.Sqrt(NextFloat());
			float theta = RandomAngleRadians();
			return new Vector2(r * Mathf.Cos(theta), r * Mathf.Sin(theta));
		}

		/// <summary>
		/// Generates a random point inside the ring with specified radia.
		/// </summary>
		public Vector2 InCircle(float radiusMin, float radiusMax)
		{
			// http://stackoverflow.com/questions/9048095/create-random-number-within-an-annulus
			float A = 2.0f / (radiusMax * radiusMax - radiusMin * radiusMin);
			float r = Mathf.Sqrt(2.0f * NextFloat() / A + radiusMin * radiusMin);
			float theta = RandomAngleRadians();
			return new Vector2(r * Mathf.Cos(theta), r * Mathf.Sin(theta));
		}

		/// <summary>
		/// Generates a random point on the border of the circle with specified radius.
		/// </summary>
		public Vector2 OnCircle(float radius = 1f)
		{
			// http://mathworld.wolfram.com/CirclePointPicking.html
			float angle = RandomAngleRadians();
			return new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
		}

		/// <summary>
		/// Generates a random point inside the sphere with specified radius.
		/// </summary>
		public Vector3 InSphere(float radius = 1f)
		{
			// http://math.stackexchange.com/questions/87230/picking-random-points-in-the-volume-of-sphere-with-uniform-probability
			float twoRadius = radius * 2.0f;
			float radiusSquared = radius * radius;
			while (true)
			{
				float x = NextFloat() * twoRadius - radius;
				float y = NextFloat() * twoRadius - radius;
				float z = NextFloat() * twoRadius - radius;
				if (x * x + y * y + z * z <= radiusSquared)
				{
					return new Vector3(x, y, z);
				}
			}
		}

		/// <summary>
		/// Generates a random point on the surface of the sphere with specified radius.
		/// </summary>
		public Vector3 OnSphere(float radius = 1f)
		{
			// http://www.altdevblogaday.com/2012/05/03/generating-uniformly-distributed-points-on-sphere/
			float z = NextFloat() * 2.0f - 1.0f;
			float t = NextFloat() * Mathfex.TwoPi;
			float r = Mathf.Sqrt(1.0f - z * z) * radius;
			return new Vector3(r * Mathf.Cos(t), r * Mathf.Sin(t), z * radius);
		}

		/// <summary>
		/// Generates a random point inside the triangle.
		/// </summary>
		public Vector3 InTriangle(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
		{
			// http://math.stackexchange.com/questions/18686/uniform-random-point-in-triangle
			double r1sqrt = System.Math.Sqrt(NextDouble());
			double r2 = NextDouble();
			Vector3 t0 = ((float)(1.0 - r1sqrt)) * v0;
			Vector3 t1 = ((float)(r1sqrt * (1.0 - r2))) * v1;
			Vector3 t2 = ((float)(r2 * r1sqrt)) * v2;
			return new Vector3(t0.x + t1.x + t2.x, t0.y + t1.y + t2.y, t0.z + t1.z + t2.z);
		}

		/// <summary>
		/// Generates a random point inside the triangle.
		/// </summary>
		public Vector3 InTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
		{
			// http://math.stackexchange.com/questions/18686/uniform-random-point-in-triangle
			double r1sqrt = System.Math.Sqrt(NextDouble());
			double r2 = NextDouble();
			Vector3 t0 = ((float)(1.0 - r1sqrt)) * v0;
			Vector3 t1 = ((float)(r1sqrt * (1.0 - r2))) * v1;
			Vector3 t2 = ((float)(r2 * r1sqrt)) * v2;
			return new Vector3(t0.x + t1.x + t2.x, t0.y + t1.y + t2.y, t0.z + t1.z + t2.z);
		}

		/// <summary>
		/// Generates a random rotation.
		/// </summary>
		public Quaternion RandomRotation()
		{
			// http://hub.jmonkeyengine.org/forum/topic/random-rotation/
			// http://planning.cs.uiuc.edu/node198.html

			double u1 = NextDouble();
			double u2 = NextDouble();
			double u3 = NextDouble();

			double u1sqrt   = System.Math.Sqrt(u1);
			double u1m1sqrt = System.Math.Sqrt(1 - u1);
			double twoPimu2 = TwoPi * u2;
			double twoPimu3 = TwoPi * u3;

			return new Quaternion(
				(float)(u1m1sqrt * System.Math.Sin(twoPimu2)),
				(float)(u1m1sqrt * System.Math.Cos(twoPimu2)),
				(float)(u1sqrt   * System.Math.Sin(twoPimu3)),
				(float)(u1sqrt   * System.Math.Cos(twoPimu3))
				);
		}
	}
}
