
using Popeye.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorThrower : IAnchorThrower
    {
        private IPlayerMediator _player;
        private PopeyeAnchor _anchor;
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;
        private AnchorThrowConfig _throwConfig;
        private IThrowDistanceComputer _throwDistanceComputer;
        
        private AnchorTrajectorySnapController _anchorTrajectorySnapController;

        private IAnchorTrajectoryView _trajectoryView;

        private AnchorThrowController _anchorThrowController;

        private AnchorTrajectoryEndSpot _trajectoryEndSpot;
        
        private float MaxThrowDistance => _throwConfig.MaxThrowDistance;
        
        public AnchorThrowResult AnchorThrowResult { get; private set; }

        
            
        
        public void Configure(
            IPlayerMediator player, 
            PopeyeAnchor anchor, 
            AnchorTrajectoryMaker anchorTrajectoryMaker,
            IThrowDistanceComputer throwDistanceComputer,
            AnchorThrowConfig throwConfig, 
            AnchorTrajectorySnapController anchorTrajectorySnapController,
            IAnchorTrajectoryView trajectoryView,
            AnchorThrowController anchorThrowController,
            AnchorTrajectoryEndSpot trajectoryEndSpot)
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;

            _throwDistanceComputer = throwDistanceComputer;
            
            _throwConfig = throwConfig;
            _anchorTrajectorySnapController = anchorTrajectorySnapController;
            _trajectoryView = trajectoryView;

            _anchorThrowController = anchorThrowController;

            _trajectoryEndSpot = trajectoryEndSpot;

            AnchorThrowResult = new AnchorThrowResult(_throwConfig.MoveInterpolationCurve,
                _throwConfig.RotateInterpolationCurve);

            _throwDistanceComputer.ClearState();
        }

        public bool AnchorIsBeingThrown()
        {
            return _anchorThrowController.AnchorIsBeingThrown;
        }


        public void UpdateThrowTrajectory()
        {
            float duration = _throwConfig.MaxThrowMoveDuration;
            
            Vector3 startPosition = _player.GetAnchorThrowStartPosition();
            Vector3 floorNormal = _player.GetFloorNormal();
            Vector3 direction = _player.GetLookDirection();
            float distance = ComputeThrowDistance();


            Vector3[] trajectoryPoints =
                _anchorTrajectoryMaker.ComputeUpdatedTrajectoryWithAutoAim(startPosition, direction,  
                    floorNormal, _throwConfig.HeightDisplacementCurve, distance,
                    out float finalTrajectoryDistance, out bool trajectoryEndsOnFloor, 
                    out IAnchorTrajectorySnapTarget snapTarget, out bool validSnapTarget, 
                    out RaycastHit obstacleHit, out bool trajectoryHitsObstacle, out int lastIndexBeforeCollision);

            float correctedDuration = (duration / MaxThrowDistance) * finalTrajectoryDistance;
            float correctedDurationHitObstacle = trajectoryHitsObstacle ? 
                duration * (Vector3.Distance(obstacleHit.point, trajectoryPoints[0]) / MaxThrowDistance) 
                : correctedDuration;
            
            AnchorThrowResult.Reset(trajectoryPoints, direction, floorNormal, 
                correctedDuration, correctedDurationHitObstacle, !trajectoryEndsOnFloor, trajectoryHitsObstacle);


            if (validSnapTarget)
            {
                OnTrajectoryFindsSnapTarget(snapTarget, trajectoryEndsOnFloor);
            }
            else
            {
                OnTrajectoryDoesntFindSnapTarget(trajectoryEndsOnFloor, trajectoryHitsObstacle, obstacleHit);
            }

            _trajectoryView.DrawTrajectory(trajectoryPoints, trajectoryHitsObstacle, lastIndexBeforeCollision, !trajectoryEndsOnFloor);
        }
        
        public void ThrowAnchor()
        {
            if (_anchorTrajectorySnapController.HasAutoAimTarget)
            {
                AnchorThrowResult.EndLookRotation = _anchorTrajectorySnapController.GetTargetRotation();
                _anchorTrajectorySnapController.UseCurrentTarget(AnchorThrowResult.Duration);
            }
            else
            {
                AnchorThrowUtilities.CorrectEndRotationForVisibility(AnchorThrowResult, _throwConfig);
            }

            _anchor.SetThrown(AnchorThrowResult).Forget();
            _anchorThrowController.DoThrowAnchor(AnchorThrowResult).Forget();
        }
        

        public void StartThrow()
        {
            _throwDistanceComputer.ClearState();
            _trajectoryEndSpot.Show();
            _trajectoryView.Show();
        }
        public void FinishThrow()
        {
            _trajectoryEndSpot.Hide();
            _trajectoryView.Hide();
        }
        

        private float ComputeThrowDistance()
        {
            return _throwDistanceComputer.ComputeThrowDistance(1f);
        }

        
        
        public void CancelThrow()
        {
            if (_anchorTrajectorySnapController.HasAutoAimTarget)
            {
                _anchorTrajectorySnapController.RemoveCurrentAutoAimTarget();
            }
        }

        private void OnTrajectoryFindsSnapTarget(IAnchorTrajectorySnapTarget snapTarget, bool trajectoryEndsOnFloor)
        {
            _anchorTrajectorySnapController.ManageAutoAimTargetFound(snapTarget);
                
            _trajectoryEndSpot.MatchSpot(snapTarget.GetAimLockPosition(), 
                snapTarget.GetLookDirectionForAimedTargeter(), trajectoryEndsOnFloor);
        }
        
        private void OnTrajectoryDoesntFindSnapTarget(bool trajectoryEndsOnFloor, bool trajectoryHitsObstacle, RaycastHit obstacleHit)
        {
            _anchorTrajectorySnapController.ManageNoAutoAimTargetFound();
            if (trajectoryHitsObstacle && trajectoryEndsOnFloor)
            {
                _trajectoryEndSpot.MatchSpot(obstacleHit.point, obstacleHit.normal, true);
            }
            else
            {
                _trajectoryEndSpot.MatchSpot(AnchorThrowResult.LastTrajectoryPathPoint, Vector3.up, false);
            }
        }
        
    }
}