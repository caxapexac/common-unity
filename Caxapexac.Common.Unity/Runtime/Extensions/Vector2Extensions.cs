using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class Vector2Extensions
    {
        public static Vector2 WithX(this Vector2 self, float x)
        {
            return new Vector2(x, self.y);
        }

        public static Vector2 WithY(this Vector2 self, float y)
        {
            return new Vector2(self.x, y);
        }

        public static Vector3 WithZ(this Vector2 self, float z)
        {
            return new Vector3(self.x, self.y, z);
        }

        public static Vector2 OffsetX(this Vector2 self, float x)
        {
            return new Vector2(self.x + x, self.y);
        }

        public static Vector2 OffsetY(this Vector2 self, float y)
        {
            return new Vector2(self.x, self.y + y);
        }

        public static Vector2 InvertX(this Vector2 self)
        {
            return new Vector2(-self.x, self.y);
        }

        public static Vector2 InvertY(this Vector2 self)
        {
            return new Vector2(self.x, -self.y);
        }

        public static Vector2 To(this Vector2 self, Vector2 destination)
        {
            return destination - self;
        }

        public static Vector2Int ToVector2Int(this Vector2 self)
        {
            return new Vector2Int(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y));
        }

        public static float[] ToFloats(this Vector2 self)
        {
            return new[] {self.x, self.y};
        }

        public static Vector2 Clamp(this Vector2 self, Vector2 min, Vector2 max)
        {
            self.x = Mathf.Clamp(self.x, min.x, max.x);
            self.y = Mathf.Clamp(self.y, min.y, max.y);
            return self;
        }

        public static bool IsEqual(this Vector2 self, Vector2 other)
        {
            return Mathf.Approximately(self.x, other.x) && Mathf.Approximately(self.y, other.y);
        }

        public static Vector2 AspectRatio(this Vector2 self, float maxSize)
        {
            float max = Mathf.Max(self.x, self.y);
            if (!(max > maxSize)) return self;
            float ratio = maxSize / max;
            self *= ratio;
            return self;
        }

        public static bool Approximately(this Vector2 self, Vector2 compared, float threshold = 0.1f)
        {
            var xDiff = Mathf.Abs(self.x - compared.x);
            var yDiff = Mathf.Abs(self.y - compared.y);
            return xDiff <= threshold && yDiff <= threshold;
        }
        
        public static Vector2 ToVector2(this float[] self)
        {
            return new Vector2(self[0], self[1]);
        }
        
        /// <summary>
        /// Fast Vector2 normalization with 0.001 threshold error.
        /// </summary>
        /// <returns>Normalized Vector2.</returns>
        // public static Vector2 NormalizedFast(this Vector2 v)
        // {
        //     var wrapper = new FloatInt();
        //     wrapper.Float = v.x * v.x + v.y * v.y;
        //     wrapper.Int = 0x5f3759df - (wrapper.Int >> 1);
        //     v.x *= wrapper.Float;
        //     v.y *= wrapper.Float;
        //     return v;
        // }

    }
}