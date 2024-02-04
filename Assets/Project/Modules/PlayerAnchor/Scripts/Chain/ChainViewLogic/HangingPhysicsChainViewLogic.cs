using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class HangingPhysicsChainViewLogic : IChainViewLogic
    {
        private readonly HangingPhysicsChainViewLogicConfig _logicConfig;

        private readonly int _chainBoneCount;
        private readonly int _chainBoneCountMinusOne;

        private readonly Vector3[] _chainPositions;
        
        
        private LayerMask CollisionLayerMask => _logicConfig.CollisionProbingConfig.CollisionLayerMask;
        private float ProbingDistance => _logicConfig.CollisionProbingConfig.ProbeDistance;
        private QueryTriggerInteraction QueryTriggerInteraction => _logicConfig.CollisionProbingConfig.QueryTriggerInteraction;


        private float VerticalOffsetFromFloor => _logicConfig.VerticalOffsetFromFloor;
        private float FullStraightDistance => _logicConfig.FullStraightDistance;
        private AnimationCurve BendingWeightCurve => _logicConfig.BendingWeightCurve;
        
        
        public HangingPhysicsChainViewLogic(HangingPhysicsChainViewLogicConfig logicConfig, int chainBoneCount)
        {
            _logicConfig = logicConfig;
            _chainBoneCount = chainBoneCount;
            _chainBoneCountMinusOne = _chainBoneCount - 1;
            _chainPositions = new Vector3[_chainBoneCount];
        }

        public void OnViewEnter()
        {

        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            _chainPositions[0] = playerBindPosition;
            _chainPositions[^1] = anchorBindPosition;

            
            Vector3 playerToAnchor = anchorBindPosition - playerBindPosition;
            float playerToAnchorDistance = playerToAnchor.magnitude;
            Vector3 playerToAnchorDirection = playerToAnchor / playerToAnchorDistance;

            float distanceStep = playerToAnchorDistance / _chainBoneCountMinusOne;

            float distanceT = Mathf.Min(playerToAnchorDistance / FullStraightDistance, 1.0f);

            for (int i = 1; i < _chainBoneCountMinusOne; ++i)
            {
                Vector3 straightPoint = playerBindPosition + (playerToAnchorDirection * (i * distanceStep));
                Vector3 floorPosition;
                
                if (Physics.Raycast(straightPoint + Vector3.up*2, Vector3.down, out RaycastHit floorHit, 
                        ProbingDistance, CollisionLayerMask, QueryTriggerInteraction))
                {
                    floorPosition = floorHit.point + (Vector3.up * VerticalOffsetFromFloor);
                }
                else
                {
                    floorPosition = straightPoint + (Vector3.down * ProbingDistance);
                }

                float t = i / (float)_chainBoneCountMinusOne;
                _chainPositions[i] = Vector3.Lerp(floorPosition, straightPoint, 
                    BendingWeightCurve.Evaluate(t) * (1-distanceT));
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