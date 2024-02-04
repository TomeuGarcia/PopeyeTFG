using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class StraightLineChainViewLogic : IChainViewLogic
    {
        private readonly int _chainBoneCount;
        private readonly int _chainBoneCountMinusOne;

        private readonly Vector3[] _chainPositions;

        public StraightLineChainViewLogic(int chainBoneCount)
        {
            _chainBoneCount = chainBoneCount;
            _chainBoneCountMinusOne = _chainBoneCount - 1;

            _chainPositions = new Vector3[_chainBoneCount];
        }

        public void OnViewEnter()
        {
            
        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            Vector3 playerToAnchor = anchorBindPosition - playerBindPosition;
            float playerToAnchorDistance = playerToAnchor.magnitude;
            Vector3 playerToAnchorDirection = playerToAnchor / playerToAnchorDistance;
            float distanceStep = playerToAnchorDistance / _chainBoneCountMinusOne;

            
            _chainPositions[0] = playerBindPosition;
            _chainPositions[^1] = anchorBindPosition;

            for (int i = 1; i < _chainBoneCount - 1; ++i)
            {
                _chainPositions[i] = playerBindPosition + (playerToAnchorDirection * (i * distanceStep));
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