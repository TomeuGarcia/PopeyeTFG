using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using Popeye.Modules.PlayerAnchor;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.Modules.Utilities.Scripts.Collisions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerDasher
    {
        private IPlayerMediator _player;
        private IAnchorMediator _anchor;
        private PlayerGeneralConfig _playerGeneralConfig;
        private TransformMotion _playerMotion;
        private ObstacleProbingConfig _obstacleProbingConfig;

        private LayerMask ObstacleLayerMask => _obstacleProbingConfig.ObstaclesLayerMask;
        

        private QuickMotionFloorPlatformChecker _floorPlatformChecker;
        

        public void Configure(IPlayerMediator playerMediator, IAnchorMediator anchorMediator,
            PlayerGeneralConfig playerGeneralConfig, TransformMotion playerMotion,
            ObstacleProbingConfig obstacleProbingConfig,
            CollisionProbingConfig floorPlatformsProbingConfig)
        {
            _player = playerMediator;
            _anchor = anchorMediator;
            _playerGeneralConfig = playerGeneralConfig;
            _playerMotion = playerMotion;
            _obstacleProbingConfig = obstacleProbingConfig;

            _floorPlatformChecker = new QuickMotionFloorPlatformChecker(floorPlatformsProbingConfig,
                1.0f, 1.0f);
        }



        public void DashTowardsAnchor(float duration)
        {
            Ease ease = Ease.InOutQuad;
            _playerMotion.MoveToPosition(ComputeDashTowardsAnchorPosition(), duration, ease);
            _anchor.OnDashedAt(duration, ease);
        }
        
        public void DashForward(float minDuration, float maxDuration, out float resultDuration)
        {
            Vector3 direction = _player.GetFloorAlignedLookDirection();
            Vector3 dashEndPosition = _player.Position + (direction * _playerGeneralConfig.MovesetConfig.RollDistance);

            dashEndPosition = ComputeEndPositionCheckingForObstacles(_player.Position, dashEndPosition,
                out float distanceChangeRatio01_Obstacle);

            dashEndPosition = _floorPlatformChecker.ComputeEndPosition_FrontRear(_player.Position, 
                dashEndPosition, out float distanceChangeRatio01_NoFloor);

            float distanceChangeRatio01 = distanceChangeRatio01_Obstacle * distanceChangeRatio01_NoFloor;
            
            resultDuration = Mathf.Lerp(minDuration, maxDuration, distanceChangeRatio01);

            _playerMotion.MoveToPosition(dashEndPosition, resultDuration, Ease.InOutQuad);
        }
        
        
        
        private Vector3 ComputeDashTowardsAnchorPosition()
        {
            Vector3 up = Vector3.up;
            Vector3 toAnchor = Vector3.ProjectOnPlane((_anchor.Position - _player.Position).normalized, up);
            Vector3 right = Vector3.Cross(toAnchor, up).normalized;

            Vector3 dashEndPosition = ComputeDashEndAnchorPosition(toAnchor, right, up);

            /*
            dashEndPosition = ComputeEndPositionCheckingForObstacles(_player.Position, dashEndPosition,
                out float distanceChangeRatio01);
                */

            return dashEndPosition;
        }


        private Vector3 ComputeDashEndAnchorPosition(Vector3 toAnchorDirection, Vector3 right, Vector3 up)
        {
            if (_anchor.IsGrabbedBySnapper())
            {
                return _anchor.CurrentTrajectorySnapTarget.GetDashEndPosition();
            }
            
            Vector3 dashExtraDisplacement = _playerGeneralConfig.MovesetConfig.DashExtraDisplacement;
            
            Vector3 extraDisplacement = toAnchorDirection * dashExtraDisplacement.z;
            extraDisplacement += right * dashExtraDisplacement.x;
            extraDisplacement += up * dashExtraDisplacement.y;
            
            Vector3 dashEndPosition = _anchor.Position + extraDisplacement;

            return dashEndPosition;
        }

        private Vector3 ComputeEndPositionCheckingForObstacles(Vector3 startPosition, Vector3 endPosition,
            out float distanceChangeRatio01)
        {
            distanceChangeRatio01 = 1;

            Vector3 startToEnd = endPosition - startPosition;
            float originalStartToEndDistance = startToEnd.magnitude;

            if (Physics.Raycast(startPosition, startToEnd.normalized, out RaycastHit obstacleHit,
                    originalStartToEndDistance, ObstacleLayerMask, QueryTriggerInteraction.Ignore))
            {
                endPosition = obstacleHit.point + (obstacleHit.normal * 0.5f);
                distanceChangeRatio01 = Vector3.Distance(startPosition, endPosition) / originalStartToEndDistance;
            }
            
            
            return endPosition;
        }
    }
}