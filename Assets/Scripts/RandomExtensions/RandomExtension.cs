using UnityEngine;

namespace Extensions
{
    public class RandomExtension
    {
        public static Vector2 RandomPointInBounds(BoundsInt bounds)
        {
            return new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );
        }
        public static Vector2 RandomPointInBounds(Bounds bounds)
        {
            return new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );
        }
    }
}
