using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class HangingPhysicsChainView : IChainView
    {
        private readonly LineRenderer _chainLine;
        private readonly HangingPhysicsChainViewConfig _config;

        private int _chainBoneCountMinusOne;

        private Transform _chainIK;
        
        private LayerMask CollisionLayerMask => _config.CollisionProbingConfig.CollisionLayerMask;
        private float ProbingDistance => _config.CollisionProbingConfig.ProbeDistance;
        private QueryTriggerInteraction QueryTriggerInteraction => _config.CollisionProbingConfig.QueryTriggerInteraction;


        private int ChainBoneCount => _config.ChainBoneCount;
        private float VerticalOffsetFromFloor => _config.VerticalOffsetFromFloor;
        private float FullStraightDistance => _config.FullStraightDistance;
        private AnimationCurve BendingWeightCurve => _config.BendingWeightCurve;
        
        
        public HangingPhysicsChainView(LineRenderer chainLine, HangingPhysicsChainViewConfig config, Transform chainIK)
        {
            _chainLine = chainLine;
            _config = config;

            _chainIK = chainIK;
            _chainIK.gameObject.SetActive(false);
        }

        public void OnViewEnter()
        {
            _chainLine.positionCount = ChainBoneCount;
            _chainBoneCountMinusOne = ChainBoneCount - 1;

            _chainLine.enabled = false;
            _chainIK.gameObject.SetActive(true);
        }

        public void LateUpdate(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            return;
            
            _chainLine.SetPosition(0, playerBindPosition);
            _chainLine.SetPosition(_chainBoneCountMinusOne, anchorBindPosition);

            
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
                _chainLine.SetPosition(i, Vector3.Lerp(floorPosition, straightPoint, 
                    BendingWeightCurve.Evaluate(t) * (1-distanceT)));
            }
            
        }

        public void OnViewExit()
        {
            _chainLine.enabled = true;
            _chainIK.gameObject.SetActive(false);
        }
        
    }
}