using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor;
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
        private AnchorThrowConfig _verticalThrowConfig;
        
        private AnchorTrajectorySnapController _anchorTrajectorySnapController;

        private IAnchorTrajectoryView _trajectoryView;
        
        
        private float _currentThrowForce01;
        private float _currentThrowCurveForce01;


        private Quaternion _verticalThrowStartRotation;
        private Quaternion _verticalThrowEndRotation;

        
        public float ThrowDistance { get; private set; }
        public Vector3 ThrowDirection { get; private set; }
        private bool _anchorIsBeingThrown;

        public AnchorThrowResult AnchorThrowResult { get; private set; }
        public AnchorThrowResult AnchorVerticalThrowResult { get; private set; }

        
            
        
        public void Configure(IPlayerMediator player, PopeyeAnchor anchor, 
            AnchorTrajectoryMaker anchorTrajectoryMaker,
            AnchorThrowConfig throwConfig, AnchorThrowConfig verticalThrowConfig,
            AnchorTrajectorySnapController anchorTrajectorySnapController,
            IAnchorTrajectoryView trajectoryView)
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _verticalThrowConfig = verticalThrowConfig;
            _throwConfig = throwConfig;
            _anchorTrajectorySnapController = anchorTrajectorySnapController;
            _trajectoryView = trajectoryView;

            AnchorThrowResult = new AnchorThrowResult(_throwConfig.MoveInterpolationCurve,
                _throwConfig.RotateInterpolationCurve);
            AnchorVerticalThrowResult = new AnchorThrowResult(_verticalThrowConfig.MoveInterpolationCurve,
                _verticalThrowConfig.RotateInterpolationCurve);
            
            ResetThrowForce();
            
            _verticalThrowStartRotation = Quaternion.LookRotation(Vector3.up, Vector3.right);
            _verticalThrowEndRotation = Quaternion.LookRotation(Vector3.down, Vector3.left);
        }

        public bool AnchorIsBeingThrown()
        {
            return _anchorIsBeingThrown;
        }


        public void UpdateThrowTrajectory()
        {
            float duration = ComputeThrowDuration();
            
            Vector3 startPosition = _player.GetAnchorThrowStartPosition();
            Vector3 floorNormal = _player.GetFloorNormal();
            Vector3 direction = _player.GetLookDirectionConsideringSteep();
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

            //_anchorTrajectoryMaker.DrawDebugLines(); // TODO remove this line once AnchorTrajectoryView works
            _trajectoryView.DrawTrajectory(trajectoryPoints, obstacleHit, trajectoryHitsObstacle, lastIndexBeforeCollision);
        }
        
        public void ThrowAnchor()
        {
            if (_anchorTrajectorySnapController.HasAutoAimTarget)
            {
                AnchorThrowResult.EndLookRotation = _anchorTrajectorySnapController.GetTargetRotation();
                _anchorTrajectorySnapController.UseCurrentTarget(AnchorThrowResult.Duration);
            }

            _anchor.SetThrown(AnchorThrowResult);
            DoThrowAnchor(AnchorThrowResult).Forget();
            
            _trajectoryView.Hide();
        }

        public void ThrowAnchorVertically()
        {
            float distance = _verticalThrowConfig.MaxThrowDistance;
            float duration = _verticalThrowConfig.MaxThrowMoveDuration;
            
            Vector3[] throwTrajectory = _anchorTrajectoryMaker.ComputeUpAndDownTrajectory(_anchor.Position, distance,
                out RaycastHit floorHit);
            
            AnchorVerticalThrowResult.Reset(throwTrajectory, Vector3.up, 
                _verticalThrowStartRotation, _verticalThrowEndRotation, duration, false);
            
            _anchor.SetThrownVertically(AnchorVerticalThrowResult, floorHit).Forget();
            DoThrowAnchor(AnchorVerticalThrowResult).Forget();
        }

        private async UniTaskVoid DoThrowAnchor(AnchorThrowResult anchorThrowResult)
        {
            _anchorIsBeingThrown = true;
            await UniTask.Delay(TimeSpan.FromSeconds(anchorThrowResult.Duration));
            _anchorIsBeingThrown = false;

            OnThrowCompleted(anchorThrowResult);
        }

        private void OnThrowCompleted(AnchorThrowResult anchorThrowResult)
        {
            if (anchorThrowResult.EndsOnVoid)
            {
                _player.OnAnchorEndedInVoid();
                return;
            }
            
            if (_anchorTrajectorySnapController.HasAutoAimTarget)
            {
                _anchor.SetGrabbedBySnapper(_anchorTrajectorySnapController.AnchorSnapTarget);
                _anchorTrajectorySnapController.ClearState();
                return;
            }
            
            _anchor.SetRestingOnFloor();
        }
        

        public void ResetThrowForce()
        {
            _currentThrowForce01 = 0.0f;
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
            return Mathf.Lerp(_throwConfig.MinThrowDistance, _throwConfig.MaxThrowDistance, 
                _currentThrowCurveForce01);
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
        }
        
        public AnchorThrowResult GetLastAnchorThrowResult()
        {
            return AnchorThrowResult;
        }

    }
}