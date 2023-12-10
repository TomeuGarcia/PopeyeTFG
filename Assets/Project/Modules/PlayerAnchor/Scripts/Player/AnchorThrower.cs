using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Modules.PlayerAnchor;
using Project.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorThrower : IAnchorThrower
    {
        private IPlayerMediator _player;
        private PopeyeAnchor _anchor;
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;
        private TransformMotion _anchorMotion;
        private AnchorThrowConfig _throwConfig;
        
        private AnchorAutoAimController _anchorAutoAimController;
        
        
        private float _currentThrowForce01;
        private float _currentThrowCurveForce01;
        
        public float ThrowDistance { get; private set; }
        public Vector3 ThrowDirection { get; private set; }
        private bool _anchorIsBeingThrown;

        public AnchorThrowResult AnchorThrowResult { get; private set; }

        
            
        
        public void Configure(IPlayerMediator player, PopeyeAnchor anchor, 
            AnchorTrajectoryMaker anchorTrajectoryMaker, TransformMotion anchorMotion,
            AnchorThrowConfig throwConfig, AnchorAutoAimController anchorAutoAimController)
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _anchorMotion = anchorMotion;
            _throwConfig = throwConfig;
            _anchorAutoAimController = anchorAutoAimController;

            AnchorThrowResult = new AnchorThrowResult(_throwConfig.MoveInterpolationCurve);
            
            ResetThrowForce();
        }

        public bool AnchorIsBeingThrown()
        {
            return _anchorIsBeingThrown;
        }


        public void UpdateThrowTrajectory()
        {
            float duration = ComputeThrowDuration();
            
            Vector3 startPosition = _player.GetAnchorThrowStartPosition();
            Vector3 direction = _player.GetLookDirection(); //_player.GetFloorAlignedLookDirection();
            Vector3 floorNormal = _player.GetFloorNormal();
            float distance = ThrowDistance;


            Vector3[] trajectoryPoints =
                _anchorTrajectoryMaker.ComputeUpdatedTrajectory(startPosition, direction, floorNormal, distance,
                    out float finalTrajectoryDistance, out bool trajectoryEndsOnFloor, 
                    out IAutoAimTarget autoAimTarget, out bool validAutoAimTarget, 
                    out RaycastHit obstacleHit, out bool trajectoryHitsObstacle);

            duration = (duration / ThrowDistance) * finalTrajectoryDistance;
            
            
            Vector3 right = Vector3.Cross(direction, floorNormal).normalized;
            Quaternion startLookRotation = _anchorTrajectoryMaker.ComputePathLookRotationBetweenIndices(trajectoryPoints, 
                0, 1, right);
            Quaternion endLookRotation = _anchorTrajectoryMaker.ComputePathLookRotationBetweenIndices(trajectoryPoints, 
                trajectoryPoints.Length-2, trajectoryPoints.Length-1, right);
            
            AnchorThrowResult.Reset(trajectoryPoints, direction, startLookRotation, endLookRotation, 
                duration, !trajectoryEndsOnFloor);

            
            
            if (validAutoAimTarget)
            {
                _anchorAutoAimController.ManageAutoAimTargetFound(autoAimTarget);
                
                _anchorTrajectoryMaker.MakeTrajectoryEndSpotMatchSpot(autoAimTarget.GetAimLockPosition(), 
                    autoAimTarget.GetLookDirectionForAimedTargeter(), trajectoryEndsOnFloor);
            }
            else
            {
                _anchorAutoAimController.ManageNoAutoAimTargetFound();

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

            _anchorTrajectoryMaker.DrawDebugLines();
        }
        
        public void ThrowAnchor()
        {
            if (_anchorAutoAimController.HasAutoAimTarget)
            {
                _anchorAutoAimController.UseCurrentTarget(AnchorThrowResult.Duration);
            }

            _anchor.SetThrown(AnchorThrowResult);
            DoThrowAnchor(AnchorThrowResult).Forget();
        }
        
        private async UniTaskVoid DoThrowAnchor(AnchorThrowResult anchorThrowResult)
        {
            _anchorMotion.MoveAlongPath(anchorThrowResult.TrajectoryPathPoints, anchorThrowResult.Duration, 
                anchorThrowResult.InterpolationEaseCurve);
            _anchorMotion.RotateStartToEnd(anchorThrowResult.StartLookRotation,anchorThrowResult.EndLookRotation, 
                anchorThrowResult.Duration, anchorThrowResult.InterpolationEaseCurve);

            
            _anchorIsBeingThrown = true;
            await UniTask.Delay(TimeSpan.FromSeconds(anchorThrowResult.Duration));
            _anchorIsBeingThrown = false;

            OnThrowCompleted(anchorThrowResult);
        }

        private void OnThrowCompleted(AnchorThrowResult anchorThrowResult)
        {
            if (anchorThrowResult.EndsOnVoid)
            {
                _player.OnAnchorThrowEndedInVoid();
                return;
            }
            
            if (_anchorAutoAimController.HasAutoAimTarget)
            {
                _anchor.SetGrabbedBySnapper(_anchorAutoAimController.AnchorAutoAimTarget);
                _anchorAutoAimController.ClearState();
                return;
            }
            
            _anchor.SetRestingOnFloor();
        }
        
        
        public void InterruptThrow()
        {
            _anchorMotion.CancelMovement();
            _anchorIsBeingThrown = false;
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
            if (_anchorAutoAimController.HasAutoAimTarget)
            {
                _anchorAutoAimController.RemoveCurrentAutoAimTarget();
            }
        }
        
        public AnchorThrowResult GetLastAnchorThrowResult()
        {
            return AnchorThrowResult;
        }

    }
}