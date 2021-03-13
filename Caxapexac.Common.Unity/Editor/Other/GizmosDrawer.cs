using System;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Editor.Other
{
    [Serializable]
    public enum GizmoType
    {
        Cube,
        WireCube,
        Sphere,
        WireSphere,
        LineToCamera,
    }


    [ExecuteAlways]
    public class GizmosDrawer : MonoBehaviour
    {
        public bool DrawOnlySelected = false;
        public GizmoType Type = GizmoType.Cube;
        public Color Color = Color.magenta;
        public float Size = 1;

        private void OnDrawGizmos()
        {
            if (DrawOnlySelected) return;
            Draw();
        }

        private void OnDrawGizmosSelected()
        {
            if (!DrawOnlySelected) return;
            Draw();
        }

        private void Draw()
        {
            Gizmos.color = Color;
            switch (Type)
            {
                case GizmoType.Cube:
                    Gizmos.DrawCube(transform.position, Vector3.one * Size);
                    break;
                case GizmoType.WireCube:
                    Gizmos.DrawWireCube(transform.position, Vector3.one * Size);
                    break;
                case GizmoType.Sphere:
                    Gizmos.DrawSphere(transform.position, Size);
                    break;
                case GizmoType.WireSphere:
                    Gizmos.DrawWireSphere(transform.position, Size);
                    break;
                case GizmoType.LineToCamera:
                    Transform cam = Camera.main.transform;
                    Gizmos.DrawLine(transform.position, cam.position + cam.forward * Size);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}