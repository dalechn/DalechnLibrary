using UnityEngine;

namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// The system is y'(t) = F(t,y). The dimension of y is passed to the constructor of OdeSolver.
		/// </summary>
		public delegate void OdeFunction(
			float   t,	// t
			float[] y,	// y
			float[] F);	// F(t,y)

		public abstract class OdeSolver
		{
			protected int         _dim;
			protected float       _step;
			protected OdeFunction _function;
			protected float[]     _FValue;

			public virtual float Step { get { return _step; } set { _step = value; } }

			public OdeSolver(int dim, float step, OdeFunction function)
			{
				_dim      = dim;
				_step     = step;
				_function = function;
				_FValue   = new float[_dim];
			}

			public abstract void Update(float tIn, float[] yIn, ref float tOut, float[] yOut);
		}

		public class OdeEuler : OdeSolver
		{
			public OdeEuler(int dim, float step, OdeFunction function)
				: base(dim, step, function)
			{
			}

			public override void Update(float tIn, float[] yIn, ref float tOut, float[] yOut)
			{
				_function(tIn, yIn, _FValue);

				for (int i = 0; i < _dim; ++i)
				{
					yOut[i] = yIn[i] + _step * _FValue[i];
				}

				tOut = tIn + _step;
			}
		}

		public class OdeMidpoint : OdeSolver
		{
			private float   _halfStep;
			private float[] _yTemp;

			public override float Step { get { return base.Step; } set { _step = value; _halfStep = _step * .5f; } }

			public OdeMidpoint(int dim, float step, OdeFunction function)
				: base(dim, step, function)
			{
				_halfStep = _step * 0.5f;
				_yTemp = new float[_dim];
			}

			public override void Update(float tIn, float[] yIn, ref float tOut, float[] yOut)
			{
				// first step
				_function(tIn, yIn, _FValue);
				int i;
				for (i = 0; i < _dim; ++i)
				{
					_yTemp[i] = yIn[i] + _halfStep * _FValue[i];
				}

				// second step
				float halfT = tIn + _halfStep;
				_function(halfT, _yTemp, _FValue);
				for (i = 0; i < _dim; ++i)
				{
					yOut[i] = yIn[i] + _step * _FValue[i];
				}

				tOut = tIn + _step;
			}
		}

		public class OdeRungeKutta4 : OdeSolver
		{
			private float   _halfStep;
			private float   _sixthStep;
			private float[] _temp1;
			private float[] _temp2;
			private float[] _temp3;
			private float[] _temp4;
			private float[] _yTemp;

			public override float Step { get { return base.Step; } set { _step = value; _halfStep = _step * .5f; _sixthStep = _step / 6.0f; } }

			public OdeRungeKutta4(int dim, float step, OdeFunction function)
				: base(dim, step, function)
			{
				_halfStep = 0.5f * step;
				_sixthStep = step / 6.0f;

				_temp1 = new float[_dim];
				_temp2 = new float[_dim];
				_temp3 = new float[_dim];
				_temp4 = new float[_dim];
				_yTemp = new float[_dim];
			}

			public override void Update(float tIn, float[] yIn, ref float tOut, float[] yOut)
			{
				// first step
				_function(tIn, yIn, _temp1);
				int i;
				for (i = 0; i < _dim; ++i)
				{
					_yTemp[i] = yIn[i] + _halfStep * _temp1[i];
				}

				// second step
				float halfT = tIn + _halfStep;
				_function(halfT, _yTemp, _temp2);
				for (i = 0; i < _dim; ++i)
				{
					_yTemp[i] = yIn[i] + _halfStep * _temp2[i];
				}

				// third step
				_function(halfT, _yTemp, _temp3);
				for (i = 0; i < _dim; ++i)
				{
					_yTemp[i] = yIn[i] + _step * _temp3[i];
				}

				// fourth step
				tOut = tIn + _step;
				_function(tOut, _yTemp, _temp4);
				for (i = 0; i < _dim; ++i)
				{
					yOut[i] = yIn[i] + _sixthStep * (_temp1[i] + 2f * (_temp2[i] + _temp3[i]) + _temp4[i]);
				}
			}
		}
	}
}
