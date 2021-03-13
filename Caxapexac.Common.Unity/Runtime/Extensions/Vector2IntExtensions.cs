using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class Vector2IntExtensions
    {
        public static Vector3 ToVector3(this Vector2Int self, float height = 0)
        {
            return new Vector3(self.x, height, self.y);
        }
    }
}