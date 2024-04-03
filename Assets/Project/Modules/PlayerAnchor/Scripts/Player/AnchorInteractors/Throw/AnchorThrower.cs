
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
        
        
        private float _currentThrowForce01;
        private float _currentThrowCurveForce01;
        

        
        public float ThrowDistance { get; private set; }
        public AnchorThrowResult AnchorThrowResult { get; private set; }

        
            
        
        public void Configure(
            IPlayerMediator player, 
            PopeyeAnchor anchor, 
            AnchorTrajectoryMaker anchorTrajectoryMaker,
            IThrowDistanceComputer throwDistanceComputer,
            AnchorThrowConfig throwConfig, 
            AnchorTrajectorySnapController anchorTrajectorySnapController,
            IAnchorTrajectoryView trajectoryView,
            AnchorThrowController anchorThrowController)
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;

            _throwDistanceComputer = throwDistanceComputer;
            
            _throwConfig = throwConfig;
            _anchorTrajectorySnapController = anchorTrajectorySnapController;
            _trajectoryView = trajectoryView;

            _anchorThrowController = anchorThrowController;

            AnchorThrowResult = new AnchorThrowResult(_throwConfig.MoveInterpolationCurve,
                _throwConfig.RotateInterpolationCurve);

            ResetThrowForce();
        }

        public bool AnchorIsBeingThrown()
        {
            return _anchorThrowController.AnchorIsBeingThrown;
        }


        public void UpdateThrowTrajectory()
        {
            float duration = ComputeThrowDuration();
            
            Vector3 startPosition = _player.GetAnchorThrowStartPosition();
            Vector3 floorNormal = _player.GetFloorNormal();
            Vector3 direction = _player.GetLookDirection();
            float distance = ThrowDistance;


            Vector3[] trajectoryPoints =
                _anchorTrajectoryMaker.ComputeUpdatedTrajectoryWithAutoAim(startPosition, direction,  
                    floorNormal, _throwConfig.HeightDisplacementCurve, distance,
                    out float finalTrajectoryDistance, out bool trajectoryEndsOnFloor, 
                    out IAnchorTrajectorySnapTarget snapTarget, out bool validSnapTarget, 
                    out RaycastHit obstacleHit, out bool trajectoryHitsObstacle, out int lastIndexBeforeCollision);

            float correctedDuration = (duration / ThrowDistance) * finalTrajectoryDistance;
            float correctedDurationHitObstacle = trajectoryHitsObstacle ? 
                duration * (Vector3.Distance(obstacleHit.point, trajectoryPoints[0]) / ThrowDistance) 
                : correctedDuration;
            
            AnchorThrowResult.Reset(trajectoryPoints, direction, floorNormal, 
                correctedDuration, correctedDurationHitObstacle, !trajectoryEndsOnFloor, trajectoryHitsObstacle);
            
            
            if (validSnapTarget)
            {
                _anchorTrajectorySnapController.ManageAutoAimTargetFound(snapTarget);
                
                _anchorTrajectoryMaker.MakeTrajectoryEndSpotMatchSpot(snapTarget.GetAimLockPosition(), 
                    snapTarget.GetLookDirectionForAimedTargeter(), trajectoryEndsOnFloor);
            }
            else
            {
                _anchorTrajectorySnapController.ManageNoAutoAimTargetFound();

                if (trajectoryHitsObstacle && trajectoryEndsOnFloor)
                {
                    _anchorTrajectoryMaker.MakeTrajectoryEndSpotMatchSpot(obstacleHit.point, 
                        obstacleHit.normal, true);
                }
                else
                {
                    _anchorTrajectoryMaker.MakeTrajectoryEndSpotMatchSpot(AnchorThrowResult.LastTrajectoryPathPoint, 
                        Vector3.up, false);
                }
                
            }

            _trajectoryView.DrawTrajectory(trajectoryPoints, trajectoryHitsObstacle, lastIndexBeforeCollision);
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
            
            _trajectoryView.Hide();
        }
        

        public void ResetThrowForce()
        {
            _currentThrowForce01 = 0.0f;
            _throwDistanceComputer.ClearState();
        }

        public void IncrementThrowForce(float deltaTime)
        {
            _currentThrowForce01 += deltaTime / _throwConfig.MaxThrowForceChargeDuration;
            _currentThrowForce01 = Mathf.Min(1.0f, _currentThrowForce01);
            
            _currentThrowCurveForce01 = _throwConfig.ThrowForceCurve.Evaluate(_currentThrowForce01);
            
            ThrowDistance = ComputeThrowDistance();
        }
        

        private float ComputeThrowDistance()
        {
            return _throwDistanceComputer.ComputeThrowDistance(_currentThrowCurveForce01);
        }
        private float ComputeThrowDuration()
        {
            return Mathf.Lerp(_throwConfig.MinThrowMoveDuration, _throwConfig.MaxThrowMoveDuration,
                _currentThrowCurveForce01);
        }
        
        
        public void CancelChargingThrow()
        {
            if (_anchorTrajectorySnapController.HasAutoAimTarget)
            {
                _anchorTrajectorySnapController.RemoveCurrentAutoAimTarget();
            }
            
            _trajectoryView.Hide();
        }

    }
}