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


        private static readonly Vector3 NOWHERE_POSITION = new (0, -10000, 0);
        private const int NUM_TRACKED_OBSTACLES = 2;
        private Vector3[] _obstacleHitPositions;
        private int[] _obstaclePositionIDs;

        
        public VFXChainView(CollisionProbingConfig obstacleCollisionProbingConfig, 
            Material chainSharedMaterial)
        {
            _obstacleCollisionProbingConfig = obstacleCollisionProbingConfig;
            _chainSharedMaterial = chainSharedMaterial;

            _obstacleHitPositions = new Vector3[NUM_TRACKED_OBSTACLES * 2];
            _obstaclePositionIDs = new int[]
            {
                Shader.PropertyToID("_ForwardObstaclePosition1"),
                Shader.PropertyToID("_BackwardObstaclePosition1"),
                Shader.PropertyToID("_ForwardObstaclePosition2"),
                Shader.PropertyToID("_BackwardObstaclePosition2")
            };
        }
        
        
        public void Update(Vector3[] chainPositions)
        {
            int numHitsForward = 0;
            
            for (int i = 1; i < chainPositions.Length; ++i)
            {
                Vector3 origin = chainPositions[i - 1];
                Vector3 toNext = chainPositions[i] - origin;
                float toNextDistance = toNext.magnitude;
                Vector3 toNextDirection = toNext / toNextDistance;

                if (Physics.Raycast(origin, toNextDirection, out RaycastHit hit,
                        toNextDistance, CollisionLayerMask, QueryTriggerInteraction))
                {
                    _obstacleHitPositions[numHitsForward] = hit.point;
                    ++numHitsForward;
                    if (numHitsForward == NUM_TRACKED_OBSTACLES)
                    {
                        break;
                    }
                }
            }

            int numHits = numHitsForward;
            
            for (int i = chainPositions.Length-2; i >= 0; --i)
            {
                Vector3 origin = chainPositions[i+1];
                Vector3 toNext = chainPositions[i] - origin;
                float toNextDistance = toNext.magnitude;
                Vector3 toNextDirection = toNext / toNextDistance;

                if (Physics.Raycast(origin, toNextDirection, out RaycastHit hit,
                        toNextDistance, CollisionLayerMask, QueryTriggerInteraction))
                {
                    _obstacleHitPositions[(NUM_TRACKED_OBSTACLES*2) - numHitsForward] = hit.point;
                    --numHitsForward;
                    if (numHitsForward == 0)
                    {
                        break;
                    }
                }
            }
            

            // Apply changes
            for (int i = 0; i < numHits; ++i)
            {
                _chainSharedMaterial.SetVector(_obstaclePositionIDs[i], _obstacleHitPositions[i]);
            }
            for (int i = NUM_TRACKED_OBSTACLES; i < NUM_TRACKED_OBSTACLES + numHits; ++i)
            {
                _chainSharedMaterial.SetVector(_obstaclePositionIDs[i], _obstacleHitPositions[i]);
            }
            
            for (int i = numHits; i < NUM_TRACKED_OBSTACLES; ++i)
            {
                _chainSharedMaterial.SetVector(_obstaclePositionIDs[i], NOWHERE_POSITION);
            }
            for (int i = NUM_TRACKED_OBSTACLES + numHits; i < NUM_TRACKED_OBSTACLES + numHits; ++i)
            {
                _chainSharedMaterial.SetVector(_obstaclePositionIDs[i], NOWHERE_POSITION);
            }
            
        }
    }
}