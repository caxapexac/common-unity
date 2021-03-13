using Caxapexac.Common.Sharp.Runtime.Extensions.Unity;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.MonoHelpers
{
    [ExecuteAlways]
    public class DrawLine : MonoBehaviour
    {
        public Transform lineStart;
        public Transform lineEnd;
        public float lineWidth = 0.1f;
        public Material customMaterial;

        private LineRenderer _lineRenderer;

        private void Start()
        {
            if (!lineStart || lineEnd == null) return;
            _lineRenderer = gameObject.EnsureComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.startWidth = lineWidth;
            _lineRenderer.endWidth = lineWidth;
            _lineRenderer.material = customMaterial == null ? new Material(Shader.Find("UI/Unlit/Text")) : customMaterial;
        }

        private void Update()
        {
            if (!lineStart || lineEnd == null) return;
            Vector3[] positions = new Vector3[2];
            positions[0] = lineStart.position;
            positions[1] = lineEnd.position;
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
        }
    }
}