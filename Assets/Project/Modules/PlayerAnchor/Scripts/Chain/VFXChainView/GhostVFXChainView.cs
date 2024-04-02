using System;
using Cysharp.Threading.Tasks;
using Popeye.Scripts.Collisions;
using Unity.VisualScripting;
using UnityEngine;
using Timer = Popeye.Timers.Timer;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class GhostVFXChainView : IVFXChainView
    {
        private class ChainObstacleCollisionData
        {
            public Vector3 CollisionNormal { get; private set; }
            public Vector3 Position => Vector3.Lerp(_hiddenPosition, _collisionPosition, _fullCollisionT);
            private Vector3 _collisionPosition = NOWHERE_POSITION;
            private Vector3 _hiddenPosition = NOWHERE_POSITION;
            private float _fullCollisionT = 0f;

            private const float SPEED = 3.0f; 
            private static readonly Vector3 NO_COLLISION_OFFSET = Vector3.down * 2.0f; 
            
            public void UpdateCollision(float deltaTime, Vector3 collisionPosition, Vector3 collisionNormal)
            {
                _fullCollisionT += deltaTime * SPEED;
                _fullCollisionT = Mathf.Min(_fullCollisionT, 1.0f);

                _collisionPosition = collisionPosition;
                _hiddenPosition = _collisionPosition + NO_COLLISION_OFFSET;

                CollisionNormal = collisionNormal;
            }
            public void UpdateNoCollision(float deltaTime)
            {
                _fullCollisionT -= deltaTime * SPEED;
                _fullCollisionT = Mathf.Max(_fullCollisionT, 0.0f);

                if (_fullCollisionT > 0.00001f)
                {
                    _fullCollisionT = 0f;
                    _hiddenPosition = NOWHERE_POSITION;
                }
            }
        }
        
        
        
        private readonly CollisionProbingConfig _obstacleCollisionProbingConfig;
        private readonly Material _chainSharedMaterial;
        private readonly Transform _animationOriginTransform;


        private static readonly Vector3 COLLISION_OFFSET = new (0, 0.15f, 0);
        private static readonly Vector3 NOWHERE_POSITION = new (0, -10000, 0);
        
        private const int NUM_TRACKED_OBSTACLES = 2;
        
        private readonly int[] _obstaclePositionIDs;
        private readonly ChainObstacleCollisionData[] _obstacleCollisionsData;
        private bool _updateObstacleHits = true;


        private LayerMask CollisionLayerMask => _obstacleCollisionProbingConfig.CollisionLayerMask;
        private QueryTriggerInteraction QueryTriggerInteraction => _obstacleCollisionProbingConfig.QueryTriggerInteraction;

        
        public GhostVFXChainView(CollisionProbingConfig obstacleCollisionProbingConfig, 
            Material chainSharedMaterial, Transform animationOriginTransform)
        {
            _obstacleCollisionProbingConfig = obstacleCollisionProbingConfig;
            _chainSharedMaterial = chainSharedMaterial;
            _animationOriginTransform = animationOriginTransform;

            _obstacleCollisionsData = new ChainObstacleCollisionData[NUM_TRACKED_OBSTACLES * 2];
            for (int i = 0; i < _obstacleCollisionsData.Length; ++i)
            {
                _obstacleCollisionsData[i] = new ChainObstacleCollisionData();
            }

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
            if (_updateObstacleHits)
            {
                UpdateObstacleHits(chainPositions);
            }
            
            UpdateShader();
        }

        public void StartOriginAnimation(Vector3 chainPosition, float duration)
        {            
            DisableUpdateObstacleHitsForDuration(chainPosition, duration).Forget();
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
                    UpdateCollision(numHitsForward, hit.point - COLLISION_OFFSET, hit.normal);
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
                    UpdateCollision(NUM_TRACKED_OBSTACLES + numHitsBackward, hit.point - COLLISION_OFFSET, hit.normal);

                    ++numHitsBackward;
                }
            }
            
            
            for (int i = numHitsForward; i < NUM_TRACKED_OBSTACLES; ++i)
            {
                UpdateNoCollision(i);
            }
            for (int i = NUM_TRACKED_OBSTACLES + numHitsBackward; i < NUM_TRACKED_OBSTACLES * 2; ++i)
            {
                UpdateNoCollision(i);
            }
        }
        
        private void UpdateCollision(int index, Vector3 collisionPosition, Vector3 collisionNormal)
        {
            _obstacleCollisionsData[index].UpdateCollision(Time.deltaTime, collisionPosition, collisionNormal);
        }
        private void UpdateNoCollision(int index)
        {
            _obstacleCollisionsData[index].UpdateNoCollision(Time.deltaTime);
        }
        
        

        private void UpdateShader()
        {
            for (int i = 0; i < _obstaclePositionIDs.Length; ++i)
            {
                _chainSharedMaterial.SetVector(_obstaclePositionIDs[i], _obstacleCollisionsData[i].Position);
            }
        }
        
        
        private async UniTaskVoid DisableUpdateObstacleHitsForDuration(Vector3 startPosition, float duration)
        {
            _updateObstacleHits = false;

            Timer timer = new Timer(duration);
            while (!timer.HasFinished())
            {
                Vector3 difference = _animationOriginTransform.position - startPosition;
                Vector3 position = startPosition + difference;
                _obstacleCollisionsData[0].UpdateCollision(1f, position, Vector3.forward);

                timer.Update(Time.deltaTime);
                await UniTask.Yield();
            }
            _obstacleCollisionsData[0].UpdateCollision(1f, NOWHERE_POSITION, Vector3.forward);
                
            _updateObstacleHits = true;
        }

        
    }
    
    
}