using UnityEngine;

namespace Gameplay
{
    public class CircleRenderer : MonoBehaviour
    {
        [SerializeField] private int _vertexCount = 50;
        [SerializeField] private float _lineWidth = 0.2f;
        private float _radius = 1f;

        private LineRenderer lineRenderer;

        public void Initialize(float radius)
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startWidth = _lineWidth;
            lineRenderer.endWidth = _lineWidth;
            _radius = radius;

            CreateCircle();
        }

        private void CreateCircle()
        {
            lineRenderer.positionCount = _vertexCount + 1;
            lineRenderer.useWorldSpace = false;

            float deltaTheta = (2f * Mathf.PI) / _vertexCount;
            float theta = 0f;

            for (int i = 0; i <= _vertexCount; i++)
            {
                float x = _radius * Mathf.Cos(theta);
                float y = _radius * Mathf.Sin(theta);

                Vector3 pos = new Vector3(x, y, 0f);
                lineRenderer.SetPosition(i, pos);

                theta += deltaTheta;
            }
        }
    }
}