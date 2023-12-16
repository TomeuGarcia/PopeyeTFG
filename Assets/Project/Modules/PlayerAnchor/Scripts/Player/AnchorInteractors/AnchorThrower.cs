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
        private AnchorThrowConfig _throwConfig;
        private AnchorThrowConfig _verticalThrowConfig;
        
        private AnchorAutoAimController _anchorAutoAimController;
        
        
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
            AnchorAutoAimController anchorAutoAimController)
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _verticalThrowConfig = verticalThrowConfig;
            _throwConfig = throwConfig;
            _anchorAutoAimController = anchorAutoAimController;

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
                    out IAutoAimTarget autoAimTarget, out bool validAutoAimTarget, 
                    out RaycastHit obstacleHit, out bool trajectoryHitsObstacle);

            duration = (duration / ThrowDistance) * finalTrajectoryDistance;
            
            AnchorThrowResult.Reset(trajectoryPoints, direction, floorNormal, duration, !trajectoryEndsOnFloor);

            
            
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
                AnchorThrowResult.EndLookRotation = _anchorAutoAimController.GetTargetRotation();
                _anchorAutoAimController.UseCurrentTarget(AnchorThrowResult.Duration);
            }

            _anchor.SetThrown(AnchorThrowResult);
            DoThrowAnchor(AnchorThrowResult).Forget();
        }

        public void ThrowAnchorVertically()
        {
            float distance = _verticalThrowConfig.MaxThrowDistance;
            float duration = _verticalThrowConfig.MaxThrowMoveDuration;
            
            Vector3[] throwTrajectory = _anchorTrajectoryMaker.ComputeUpAndDownTrajectory(_anchor.Position, distance,
                out RaycastHit floorHit);
            
            AnchorVerticalThrowResult.Reset(throwTrajectory, Vector3.up, 
                _verticalThrowStartRotation, _verticalThrowEndRotation, duration, false);
            
            _anchor.SetThrownVertically(AnchorVerticalThrowResult, floorHit);
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
            
            if (_anchorAutoAimController.HasAutoAimTarget)
            {
                _anchor.SetGrabbedBySnapper(_anchorAutoAimController.AnchorAutoAimTarget);
                _anchorAutoAimController.ClearState();
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