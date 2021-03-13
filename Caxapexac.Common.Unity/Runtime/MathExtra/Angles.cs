using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.MathExtra
{
    public static class Angles
    {
        /// <summary>
        /// Get angle between two points, without Y axis.
        /// </summary>
        /// <param name="from">First point.</param>
        /// <param name="to">Second point.</param>
        /// <returns>Angle between points.</returns>
        public static float AngleXZ(Vector3 from, Vector3 to)
        {
            Vector2 p1 = new Vector2(from.x, from.z);
            Vector2 p2 = new Vector2(to.x, to.z);
            return Angle(p1, p2);
        }

        /// <summary>
        /// Get angle between two 2d points.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns>Angle between points.</returns>
        public static float Angle(Vector2 p1, Vector2 p2)
        {
            return Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
        }

        /// <summary>
        /// Get avarage between two angles.
        /// Note: this also represent the angle directly between two other angles.
        /// </summary>
        /// <returns>Average value between two angles.</returns>
        public static float AnglesAvg(float a, float b)
        {
            return Mathf.LerpAngle(a, b, 0.5f);
        }

        public static float AngleBetween(float x, float y, float angleOffset = 0f)
        {
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg + angleOffset;
            if (angle < 0f)
            {
                angle += 360f;
            }
            return angle % 360f;
        }
    }
}