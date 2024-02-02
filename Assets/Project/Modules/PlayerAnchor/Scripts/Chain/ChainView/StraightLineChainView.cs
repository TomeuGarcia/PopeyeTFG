using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class StraightLineChainView : IChainView
    {
        private readonly LineRenderer _chainLine;

        public StraightLineChainView(LineRenderer chainLine)
        {
            _chainLine = chainLine;
        }
        
        public void LateUpdate(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            _chainLine.positionCount = 2;
            _chainLine.SetPosition(0, playerBindPosition);
            _chainLine.SetPosition(1, anchorBindPosition);
        }

        public void OnViewSwapped()
        {
            
        }
    }
}