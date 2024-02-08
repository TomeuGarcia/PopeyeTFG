using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class LineRendererChainView : IChainView
    {
        private readonly LineRenderer _lineRenderer;

        public LineRendererChainView(LineRenderer lineRenderer)
        {
            _lineRenderer = lineRenderer;
        }
        
        
        public void Update(Vector3[] positions)
        {
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
        }
        
        public void DrawGizmos()
        {
            for (int i = 0; i < _lineRenderer.positionCount-1; ++i)
            {
                Gizmos.color = Color.Lerp(Color.green, Color.red, i / (float)(_lineRenderer.positionCount - 1));
                Gizmos.DrawLine(_lineRenderer.GetPosition(i), _lineRenderer.GetPosition(i+1));
            }
        }
    }
}