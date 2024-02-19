using System;
using Popeye.Scripts.Collisions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class GhostVFXChainView : IVFXChainView
    {
        private readonly CollisionProbingConfig _obstacleCollisionProbingConfig;
        private readonly Material _chainSharedMaterial;


        private static readonly Vector3 COLLISION_OFFSET = new (0, 0.15f, 0);
        
        private static readonly Vector3 NOWHERE_POSITION = new (0, -10000, 0);
        private const int NUM_TRACKED_OBSTACLES = 2;
        private readonly Vector3[] _obstacleHitPositions;
        private readonly int[] _obstaclePositionIDs;
        

        private LayerMask CollisionLayerMask => _obstacleCollisionProbingConfig.CollisionLayerMask;
        private QueryTriggerInteraction QueryTriggerInteraction => _obstacleCollisionProbingConfig.QueryTriggerInteraction;

        
        public GhostVFXChainView(CollisionProbingConfig obstacleCollisionProbingConfig, 
            Material chainSharedMaterial)
        {
            _obstacleCollisionProbingConfig = obstacleCollisionProbingConfig;
            _chainSharedMaterial = chainSharedMaterial;

            _obstacleHitPositions = new Vector3[NUM_TRACKED_OBSTACLES * 2];
            Array.Fill(_obstacleHitPositions, NOWHERE_POSITION);
            
            _obstaclePositionIDs = new int[]
            {
                Shader.PropertyToID("_ForwardObstaclePosition1"),
                Shader.PropertyToID("_ForwardObstaclePosition2"),
                Shader.PropertyToID("_BackwardObstaclePosition1"),
                Shader.PropertyToID("_BackwardObstaclePosition2")
            };

            
            UpdateShader();
        }
        
        
        public void Update(Vector3[] chainPositions)
        {
            UpdateObstacleHits(chainPositions);
            UpdateShader();
        }


        private void UpdateObstacleHits(Vector3[] chainPositions)
        {
            int numHitsForward = 0;
            
            for (int i = 1; i < chainPositions.Length; ++i)
            {
                Vector3 origin = chainPositions[i - 1] + COLLISION_OFFSET;
                Vector3 toNext = chainPositions[i] - origin + COLLISION_OFFSET;
                float toNextDistance = toNext.magnitude;
                Vector3 toNextDirection = toNext / toNextDistance;

                if (Physics.Raycast(origin, toNextDirection, out RaycastHit hit,
                        toNextDistance, CollisionLayerMask, QueryTriggerInteraction))
                {
                    _obstacleHitPositions[numHitsForward] = hit.point - COLLISION_OFFSET;
                    ++numHitsForward;
                    if (numHitsForward == NUM_TRACKED_OBSTACLES)
                    {
                        break;
                    }
                }
            }

            
            int numHitsBackward = 0;
            
            for (int i = chainPositions.Length-2; i >= 0 && numHitsBackward < numHitsForward; --i)
            {
                Vector3 origin = chainPositions[i+1] + COLLISION_OFFSET;
                Vector3 toNext = chainPositions[i] - origin + COLLISION_OFFSET;
                float toNextDistance = toNext.magnitude;
                Vector3 toNextDirection = toNext / toNextDistance;

                if (Physics.Raycast(origin, toNextDirection, out RaycastHit hit,
                        toNextDistance, CollisionLayerMask, QueryTriggerInteraction))
                {
                    _obstacleHitPositions[NUM_TRACKED_OBSTACLES + numHitsBackward] = hit.point - COLLISION_OFFSET;
                    ++numHitsBackward;
                }
            }
            
            
            for (int i = numHitsForward; i < NUM_TRACKED_OBSTACLES; ++i)
            {
                _obstacleHitPositions[i] = NOWHERE_POSITION;
            }
            for (int i = NUM_TRACKED_OBSTACLES + numHitsBackward; i < NUM_TRACKED_OBSTACLES * 2; ++i)
            {
                _obstacleHitPositions[i] = NOWHERE_POSITION;
            }
        }

        private void UpdateShader()
        {
            for (int i = 0; i < _obstaclePositionIDs.Length; ++i)
            {
                _chainSharedMaterial.SetVector(_obstaclePositionIDs[i], _obstacleHitPositions[i]);
            }
        }
    }
}