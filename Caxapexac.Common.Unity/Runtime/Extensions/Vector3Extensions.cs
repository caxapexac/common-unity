using System.Collections.Generic;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class Vector3Extensions
    {
        public static Vector3 WithX(this Vector3 self, float x)
        {
            return new Vector3(x, self.y, self.z);
        }

        public static Vector3 WithY(this Vector3 self, float y)
        {
            return new Vector3(self.x, y, self.z);
        }

        public static Vector3 WithZ(this Vector3 self, float z)
        {
            return new Vector3(self.x, self.y, z);
        }

        public static Vector3 OffsetX(this Vector3 self, float x)
        {
            return new Vector3(self.x + x, self.y, self.z);
        }

        public static Vector3 OffsetY(this Vector3 self, float y)
        {
            return new Vector3(self.x, self.y + y, self.z);
        }

        public static Vector3 OffsetZ(this Vector3 self, float z)
        {
            return new Vector3(self.x, self.y, self.z + z);
        }

        public static Vector3 To(this Vector3 self, Vector3 destination)
        {
            return destination - self;
        }

        public static Vector2Int ToVector2Int(this Vector3 self)
        {
            return new Vector2Int(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.z));
        }

        public static Vector3Int ToVector3Int(this Vector3 self)
        {
            return new Vector3Int(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y), Mathf.RoundToInt(self.z));
        }

        public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
        {
            if (!isNormalized) axisDirection.Normalize();
            var d = Vector3.Dot(point, axisDirection);
            return axisDirection * d;
        }

        public static Vector3 NearestPointOnLine(this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
        {
            if (!isNormalized) lineDirection.Normalize();
            var d = Vector3.Dot(point - pointOnLine, lineDirection);
            return pointOnLine + (lineDirection * d);
        }

        public static Vector3 NearestPoint(this Vector3 self, IEnumerable<Vector3> otherPositions)
        {
            var closest = Vector3.zero;
            var shortestDistance = Mathf.Infinity;
            foreach (var otherPosition in otherPositions)
            {
                var distance = (self - otherPosition).sqrMagnitude;

                if (distance < shortestDistance)
                {
                    closest = otherPosition;
                    shortestDistance = distance;
                }
            }
            return closest;
        }

        public static T NearestObjectOfType<T>(this Vector3 self, List<T> objectsToSelectFrom) where T : Component
        {
            T nearestObject = null;
            float currentNearestDistance = float.MaxValue - 1;
            foreach (var item in objectsToSelectFrom)
            {
                float curDistance = (self - item.transform.position).sqrMagnitude;

                if (curDistance < currentNearestDistance)
                {
                    nearestObject = item;
                    currentNearestDistance = curDistance;
                }
            }
            return nearestObject;
        }

        public static bool Approximately(this Vector3 self, Vector3 compared, float threshold = 0.1f)
        {
            var xDiff = Mathf.Abs(self.x - compared.x);
            var yDiff = Mathf.Abs(self.y - compared.y);
            var zDiff = Mathf.Abs(self.z - compared.z);
            return xDiff <= threshold && yDiff <= threshold && zDiff <= threshold;
        }

        public static Vector3 OnUnitCircle(this System.Random self, Vector3 offset, float radius)
        {
            return offset + (Vector3)(Random.insideUnitCircle.normalized) * radius;
        }

        /// <summary>
        /// Fast Vector3 normalization with 0.001 threshold error.
        /// </summary>
        /// <returns>Normalized Vector3.</returns>
        // public static Vector3 NormalizedFast(this Vector3 v)
        // {
        //     var wrapper = new FloatInt();
        //     wrapper.Float = v.x * v.x + v.y * v.y + v.z * v.z;
        //     wrapper.Int = 0x5f3759df - (wrapper.Int >> 1);
        //     v.x *= wrapper.Float;
        //     v.y *= wrapper.Float;
        //     v.z *= wrapper.Float;
        //     return v;
        // }
    }
}