namespace Dest
{
	namespace Math
	{
		/// <summary>
		/// Contains various intersection methods.
		/// </summary>
		public static partial class Intersection
		{
			private static float _intervalThreshold;
			private static float _dotThreshold;
			private static float _distanceThreshold;

			/// <summary>
			/// Used in interval comparisons. Default is MathfEx.ZeroTolerance.
			/// </summary>
			public static float IntervalThreshold
			{
				get { return _intervalThreshold; }
				set
				{
					if (value >= 0f)
					{
						_intervalThreshold = value;
						return;
					}
					Logger.LogWarning("Interval threshold must be nonnegative.");
				}
			}

			/// <summary>
			/// Used in dot product comparisons. Default is MathfEx.ZeroTolerance.
			/// </summary>
			public static float DotThreshold
			{
				get { return _dotThreshold; }
				set
				{
					if (value >= 0f)
					{
						_dotThreshold = value;
						return;
					}
					Logger.LogWarning("Dot threshold must be nonnegative.");
				}
			}

			/// <summary>
			/// Used in distance comparisons. Default is MathfEx.ZeroTolerance.
			/// </summary>
			public static float DistanceThreshold
			{
				get { return _distanceThreshold; }
				set
				{
					if (value >= 0f)
					{
						_distanceThreshold = value;
						return;
					}
					Logger.LogWarning("Distance threshold must be nonnegative.");
				}
			}

			static Intersection()
			{
				_intervalThreshold = _dotThreshold = _distanceThreshold = Mathfex.ZeroTolerance;
			}

			/// <summary>
			/// Finds intersection of 1d intervals. Endpoints of the intervals must be sorted,
			/// i.e. seg0Start must be &lt;= seg0End, seg1Start must be &lt;= seg1End. Returns 0 if
			/// intersection is empty, 1 - if intervals intersect in one point, 2 - if intervals
			/// intersect in segment. w0 and w1 will contain intersection point in case intersection occurs.
			/// </summary>
			public static int FindSegment1Segment1(float seg0Start, float seg0End, float seg1Start, float seg1End, out float w0, out float w1)
			{
				w0 = w1 = 0f;
				float eps = _distanceThreshold;

				if (seg0End < (seg1Start - eps) || seg0Start > (seg1End + eps)) return 0;

				if (seg0End > seg1Start + eps)
				{
					if (seg0Start < seg1End - eps)
					{
						if (seg0Start < seg1Start)
						{
							w0 = seg1Start;
						}
						else
						{
							w0 = seg0Start;
						}

						if (seg0End > seg1End)
						{
							w1 = seg1End;
						}
						else
						{
							w1 = seg0End;
						}

						if (w1 - w0 <= eps)
						{
							return 1;
						}

						return 2;
					}
					else
					{
						// u0 == v1
						w0 = seg0Start;
						return 1;
					}
				}
				else
				{
					// u1 == v0
					w0 = seg0End;
					return 1;
				}
			}
		}
	}
}
