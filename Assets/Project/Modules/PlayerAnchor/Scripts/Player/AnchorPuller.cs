using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Modules.PlayerAnchor;
using Project.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorPuller : IAnchorPuller
    {
        private IPlayerMediator _player;
        private PopeyeAnchor _anchor;
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;
        private TransformMotion _anchorMotion;
        private AnchorPullConfig _pullConfig;

        private bool _anchorIsBeingPulled;
        
        
        public AnchorThrowResult AnchorPullResult { get; private set; }
        
        
        
        public void Configure(IPlayerMediator player, PopeyeAnchor anchor, 
            AnchorTrajectoryMaker anchorTrajectoryMaker, TransformMotion anchorMotion,
            AnchorPullConfig pullConfig)
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _anchorMotion = anchorMotion;
            _pullConfig = pullConfig;

            AnchorPullResult = new AnchorThrowResult();
        }


        public bool AnchorIsBeingPulled()
        {
            return _anchorIsBeingPulled;
        }

        public void PullAnchor()
        {
            _anchor.SetPulled();

            
            Vector3 anchorPosition = _anchor.Position;
            Vector3 playerPosition = _player.GetAnchorThrowStartPosition();
            Vector3[] trajectoryPath = _anchorTrajectoryMaker.ComputeCurvedTrajectory(anchorPosition, 
                playerPosition, 10, out float trajectoryDistance);
            float duration = ComputePullDuration(trajectoryDistance);
            AnchorPullResult.Reset(trajectoryPath,  Quaternion.identity, Quaternion.identity, 
                duration, false);

            DoPullAnchor(AnchorPullResult).Forget();
        }
        
        private async UniTaskVoid DoPullAnchor(AnchorThrowResult anchorPullResult)
        {
            _anchorMotion.MoveAlongPath(anchorPullResult.TrajectoryPathPoints, anchorPullResult.Duration, Ease.InOutQuad);
            
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