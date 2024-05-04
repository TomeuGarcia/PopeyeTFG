using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class FoldedCompletelyChainViewLogic : IChainViewLogic
    {
        private readonly int _chainBoneCount;
        private readonly Vector3[] _chainPositions;

        public float PositionsExtraDistance => 0;

        public FoldedCompletelyChainViewLogic(int chainBoneCount)
        {
            _chainBoneCount = chainBoneCount;
            _chainPositions = new Vector3[_chainBoneCount];
        }
        
        
        public void OnViewEnter(Vector3[] previousStateChainPositions, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            for (int i = 0; i < _chainBoneCount; ++i)
            {
                _chainPositions[i] = anchorBindPosition;
            }
        }

        public void OnViewExit()
        {
        }

        public Vector3[] GetChainPositions()
        {
           return _chainPositions;
        }
    }
}