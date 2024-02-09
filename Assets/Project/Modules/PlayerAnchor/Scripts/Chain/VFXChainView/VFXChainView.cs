using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class VFXChainView : IVFXChainView
    {
        private CollisionProbingConfig _obstacleCollisionProbingConfig;
        private readonly Material _chainSharedMaterial;

        private LayerMask CollisionLayerMask => _obstacleCollisionProbingConfig.CollisionLayerMask;
        private QueryTriggerInteraction QueryTriggerInteraction => _obstacleCollisionProbingConfig.QueryTriggerInteraction;


        private Vector3 _nowherePosition;
        private int _forwardObstaclePosition1_ID;
        private int _backwardObstaclePosition1_ID;

        
        public VFXChainView(CollisionProbingConfig obstacleCollisionProbingConfig, 
            Material chainSharedMaterial)
        {
            _obstacleCollisionProbingConfig = obstacleCollisionProbingConfig;
            _chainSharedMaterial = chainSharedMaterial;

            _nowherePosition = new Vector3(0, -10000, 0);
            _forwardObstaclePosition1_ID = Shader.PropertyToID("_ForwardObstaclePosition1");
            _backwardObstaclePosition1_ID = Shader.PropertyToID("_BackwardObstaclePosition1");
        }
        
        
        public void Update(Vector3[] chainPositions)
        {
            bool hitObstacle = false;
            
            
            for (int i = 1; i < chainPositions.Length; ++i)
            {
                Vector3 origin = chainPositions[i - 1];
                Vector3 toNext = chainPositions[i] - origin;
                float toNextDistance = toNext.magnitude;
                Vector3 toNextDirection = toNext / toNextDistance;

                if (Physics.Raycast(origin, toNextDirection, out RaycastHit hit,
                        toNextDistance, CollisionLayerMask, QueryTriggerInteraction))
                {
                    hitObstacle = true;
                    
                    _chainSharedMaterial.SetVector(_forwardObstaclePosition1_ID, hit.point);
                    break;
                }
            }

            if (!hitObstacle)
            {
                _chainSharedMaterial.SetVector(_forwardObstaclePosition1_ID, _nowherePosition);
            }
            
            
            
            for (int i = chainPositions.Length-2; i >= 0; --i)
            {
                Vector3 origin = chainPositions[i+1];
                Vector3 toNext = chainPositions[i] - origin;
                float toNextDistance = toNext.magnitude;
                Vector3 toNextDirection = toNext / toNextDistance;

                if (Physics.Raycast(origin, toNextDirection, out RaycastHit hit,
                        toNextDistance, CollisionLayerMask, QueryTriggerInteraction))
                {
                    hitObstacle = true;
                    
                    _chainSharedMaterial.SetVector(_backwardObstaclePosition1_ID, hit.point);
                    break;
                }
            }
            if (!hitObstacle)
            {
                _chainSharedMaterial.SetVector(_backwardObstaclePosition1_ID, _nowherePosition);
            }
            
        }
    }
}