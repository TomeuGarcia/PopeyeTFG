using System;
using Cysharp.Threading.Tasks;
using Popeye.CollisionNotifiers;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorCollisions : MonoBehaviour
    {
        [SerializeField] private ColliderOnlyTriggerNotifierBehaviour _obstacleHitNotifierBehaviour;
        [SerializeField] private BoxCollider _obstructionBox;
        
        private ObstacleProbingConfig _obstacleProbingConfig;
        private LayerMask ObstacleLayerMask => _obstacleProbingConfig.ObstaclesLayerMask;
        
        

        
        public void Configure(ObstacleProbingConfig obstacleProbingConfig)
        {
            _obstacleProbingConfig = obstacleProbingConfig;
            _obstructionBox.enabled = false;
        }
        
        public bool IsObstructedByObstacles(Vector3 position, Quaternion orientation)
        {
            position += _obstructionBox.center;
            Vector3 halfExtents = _obstructionBox.size / 2;
            
            if (Physics.BoxCast(position, halfExtents, Vector3.up, orientation, 1,
                    ObstacleLayerMask, QueryTriggerInteraction.Ignore))
            {
                return true;
            }

            return false;
        }
        

        public void SubscribeToOnObstacleHit(Action<Collider> callback)
        {
            _obstacleHitNotifierBehaviour.OnEnter += callback;
        }
        public void UnsubscribeToOnObstacleHit(Action<Collider> callback)
        {
            _obstacleHitNotifierBehaviour.OnEnter -= callback;
        }

        public async UniTaskVoid EnableObstacleHitForDuration(float duration)
        {
            DisableObstacleHit();
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            EnableObstacleHit();
        }
        public void EnableObstacleHit()
        {
            _obstacleHitNotifierBehaviour.EnableCollider();
        }
        public void DisableObstacleHit()
        {
            _obstacleHitNotifierBehaviour.DisableCollider();
        }
        
    }
}