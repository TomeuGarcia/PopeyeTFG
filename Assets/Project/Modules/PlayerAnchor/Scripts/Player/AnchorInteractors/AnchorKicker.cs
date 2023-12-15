using System;
using Cysharp.Threading.Tasks;
using Project.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorKicker : IAnchorKicker
    {
        private IPlayerMediator _player;
        private PopeyeAnchor _anchor;
        
        private AnchorKickConfig _anchorKickConfig;
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;


        private AnchorThrowResult AnchorKickResult;
        
        
        public void Configure(IPlayerMediator player, PopeyeAnchor anchor,
            AnchorTrajectoryMaker anchorTrajectoryMaker, AnchorKickConfig anchorKickConfig)
        {
            _player = player;
            _anchor = anchor;
            
            _anchorKickConfig = anchorKickConfig;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;

            AnchorKickResult = new AnchorThrowResult(_anchorKickConfig.MoveInterpolationCurve);
        }


        public void KickAnchor()
        {
            Vector3 startPosition = _anchor.Position;
            Vector3 floorNormal = _player.GetFloorNormal();
            Vector3 direction = _player.GetLookDirectionConsideringSteep();
            float distance = _anchorKickConfig.AnchorKickMoveDistance;

            Vector3[] trajectoryPoints =
            _anchorTrajectoryMaker.ComputeUpdatedTrajectory(startPosition, direction, 
                floorNormal, _anchorKickConfig.HeightDisplacementCurve, distance,
                out float trajectoryDistance, out bool trajectoryEndsOnTheFloor, 
                out RaycastHit obstacleHit, out bool trajectoryHitsObstacle);
            
            float duration = (_anchorKickConfig.AnchorKickMoveDuration / distance) * trajectoryDistance;
            
            AnchorKickResult.Reset(trajectoryPoints, direction, floorNormal, duration, !trajectoryEndsOnTheFloor);

            
            _anchor.SetKicked(AnchorKickResult);
            DoKickAnchor(AnchorKickResult).Forget();
            
        }
        
        private async UniTaskVoid DoKickAnchor(AnchorThrowResult anchorThrowResult)
        {
            _anchorTrajectoryMaker.ShowTrajectoryEndSpot();
            _anchorTrajectoryMaker.DrawDebugLines();
            _anchorTrajectoryMaker.MakeTrajectoryEndSpotMatchSpot(anchorThrowResult.LastTrajectoryPathPoint, 
                Vector3.up, !anchorThrowResult.EndsOnVoid);
            
            await UniTask.Delay(TimeSpan.FromSeconds(anchorThrowResult.Duration));
            
            _anchorTrajectoryMaker.HideTrajectoryEndSpot();

            OnKickCompleted(anchorThrowResult);
        }

        private void OnKickCompleted(AnchorThrowResult anchorThrowResult)
        {
            if (anchorThrowResult.EndsOnVoid)
            {
                _player.OnAnchorEndedInVoid();
                return;
            }
            
            _anchor.SetRestingOnFloor();
        }

        
        
        
    }
}