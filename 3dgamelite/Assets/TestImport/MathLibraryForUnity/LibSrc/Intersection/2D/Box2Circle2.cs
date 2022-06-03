using UnityEngine;

namespace Dest
{
	namespace Math
	{
		public static partial class Intersection
		{
			/// <summary>
			/// Tests if a box intersects a circle. Returns true if intersection occurs false otherwise.
			/// </summary>
			public static bool TestBox2Circle2(ref Box2 box, ref Circle2 circle)
			{
				float distSquared = 0f;
				float delta;
				float proj;
				float extent;

				Vector2 diff = circle.Center - box.Center;

				proj = diff.Dot(box.Axis0);
				extent = box.Extents.x;
				if (proj < -extent)
				{
					delta = proj + extent;
					distSquared += delta * delta;
				}
				else if (proj > extent)
				{
					delta = proj - extent;
					distSquared += delta * delta;
				}

				proj = diff.Dot(box.Axis1);
				extent = box.Extents.y;
				if (proj < -extent)
				{
					delta = proj + extent;
					distSquared += delta * delta;
				}
				else if (proj > extent)
				{
					delta = proj - extent;
					distSquared += delta * delta;
				}

				return distSquared <= circle.Radius * circle.Radius;
			}
		}
	}
}
