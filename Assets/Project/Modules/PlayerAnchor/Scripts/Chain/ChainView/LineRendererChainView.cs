using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class LineRendererChainView : IChainView
    {
        private readonly LineRenderer _lineRenderer;
        private Vector3[] _updatedPositions;
        
        public LineRendererChainView(LineRenderer lineRenderer)
        {
            _lineRenderer = lineRenderer;
        }
        
        
        public void Update(Vector3[] positions)
        {
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
            _updatedPositions = positions;
        }

        public Vector3[] GetUpdatedPositions()
        {
            return _updatedPositions;
        }
    }
}