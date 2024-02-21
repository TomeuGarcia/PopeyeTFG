using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor;
using Popeye.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorPuller : IAnchorPuller
    {
        private IPlayerMediator _player;
        private PopeyeAnchor _anchor;
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;
        private AnchorPullConfig _pullConfig;

        private bool _anchorIsBeingPulled;
        
        
        public AnchorThrowResult AnchorPullResult { get; private set; }
        
        
        
        public void Configure(IPlayerMediator player, PopeyeAnchor anchor, 
            AnchorTrajectoryMaker anchorTrajectoryMaker,
            AnchorPullConfig pullConfig)
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _pullConfig = pullConfig;

            AnchorPullResult = new AnchorThrowResult(_pullConfig.MoveInterpolationCurve,
                _pullConfig.RotateInterpolationCurve);
        }


        public bool AnchorIsBeingPulled()
        {
            return _anchorIsBeingPulled;
        }

        public void PullAnchor()
        {
            Vector3 anchorPosition = _anchor.Position;
            Vector3 playerPosition = _player.GetAnchorThrowStartPosition();
            Vector3 pullDirection = -_player.GetFloorAlignedDirectionToAnchor();
            
            Vector3[] trajectoryPath = _anchorTrajectoryMaker.ComputeCurvedTrajectory(anchorPosition, 
                playerPosition, 10, out float trajectoryDistance);
            float duration = ComputePullDuration(trajectoryDistance);

            Vector3 currentForward = _anchor.Forward;
            Vector3 goalForward = -pullDirection;
            
            
            Quaternion startRotation = _anchor.Rotation;

            Vector3 forwardRotationAxis = Vector3.Cross(currentForward, goalForward).normalized;
            float forwardRotationAngle = Mathf.Acos(Vector3.Dot(currentForward, goalForward)) * Mathf.Rad2Deg;
            Quaternion forwardOffsetRotation = Quaternion.AngleAxis(forwardRotationAngle, forwardRotationAxis);
            Quaternion endRotation = forwardOffsetRotation * startRotation;
            

            AnchorPullResult.Reset(trajectoryPath, pullDirection, startRotation, endRotation, 
                duration, false);

            
            _anchor.SetPulled(AnchorPullResult);
            DoPullAnchor(AnchorPullResult).Forget();
        }
        
        private async UniTaskVoid DoPullAnchor(AnchorThrowResult anchorPullResult)
        {
            _anchorIsBeingPulled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(anchorPullResult.Duration));
            
            _anchorIsBeingPulled = false;
        }
        
        
        private float ComputePullDuration(float distance)
        {
            distance = Mathf.Clamp(distance, _pullConfig.MinPullDistance, _pullConfig.MaxPullDistance);
            distance -= _pullConfig.MinPullDistance;
            float maxRatio = _pullConfig.MaxPullDistance - _pullConfig.MinPullDistance;
            
            float distanceRatio01 = distance / maxRatio;
            
            return Mathf.Lerp(_pullConfig.MinPullMoveDuration, _pullConfig.MaxPullMoveDuration,
                distanceRatio01);
        }
        
    }
}