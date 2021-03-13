using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class GizmosExtensions
    {
        public static void DrawArrow(Vector3 from, Vector3 direction, float headLength = 0.25f, float headAngle = 20.0f)
        {
            Gizmos.DrawRay(from, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + headAngle, 0, 180 + headAngle) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - headAngle, 0, 180 - headAngle) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(from + direction, right * headLength);
            Gizmos.DrawRay(from + direction, left * headLength);
        }
    }
}