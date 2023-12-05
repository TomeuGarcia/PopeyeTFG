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
        
        private AnchorSnapController _anchorSnapController;
        
        
        private float _currentThrowForce01;
        private float _currentThrowCurveForce01;
        
        public float ThrowDistance { get; private set; }
        public Vector3 ThrowDirection { get; private set; }
        private bool _anchorIsBeingThrown;

        public AnchorThrowResult AnchorThrowResult { get; private set; }



        
        public void Configure(IPlayerMediator player, PopeyeAnchor anchor, 
            AnchorTrajectoryMaker anchorTrajectoryMaker, TransformMotion anchorMotion,
            AnchorThrowConfig throwConfig, AnchorSnapController anchorSnapController)
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _anchorMotion = anchorMotion;
            _throwConfig = throwConfig;
            _anchorSnapController = anchorSnapController;

            AnchorThrowResult = new AnchorThrowResult(_throwConfig.MoveInterpolationCurve);
            
            ResetThrowForce();
        }



        public void UpdateThrowTrajectory()
        {
            Vector3 throwStartPoint = _player.GetAnchorThrowStartPosition();
            ThrowDirection = _player.GetFloorAlignedLookDirection();

            Vector3[] trajectoryPath =
                _anchorTrajectoryMaker.UpdateTrajectoryPath(throwStartPoint, ThrowDirection, ThrowDistance,
                    !_anchorSnapController.HasSnapTarget);

            if (_anchorSnapController.CheckForSnapTarget(trajectoryPath))
            {
                IAnchorSnapTarget snapTarget = _anchorSnapController.AnchorSnapTarget;
                _anchorTrajectoryMaker.MakeTrajectoryEndSpotMatchSpot(snapTarget.GetSnapPosition(), snapTarget.GetLookDirection());
            }
        }


        public bool AnchorIsBeingThrown()
        {
            return _anchorIsBeingThrown;
        }

        public Vector3[] AtrajectoryPath;
        public void ThrowAnchor()
        {
            UpdateAnchorThrowResult();

            _anchor.SetThrown(AnchorThrowResult);
            DoThrowAnchor(AnchorThrowResult).Forget();
        }

        private void UpdateAnchorThrowResult()
        {
            float moveDuration = ComputeThrowDuration();
            bool trajectoryEndsOnVoid = false;
            Vector3[] trajectoryPath = null;

            if (_anchorSnapController.HasSnapTarget)
            {
                trajectoryPath = _anchorTrajectoryMaker.ComputeCurvedTrajectory(_anchor.Position, 
                    _anchorSnapController.GetTargetSnapPosition(), 4, out float distance);
                moveDuration = (moveDuration / ThrowDistance) * distance;
                
                _anchorSnapController.ConfirmCurrentTarget(moveDuration);
            }
            else
            {
                trajectoryPath = _anchorTrajectoryMaker.GetCorrectedTrajectoryPath();
                moveDuration = _anchorTrajectoryMaker.GetCorrectedDurationByDistance(ThrowDistance, moveDuration);
                trajectoryEndsOnVoid = _anchorTrajectoryMaker.TrajectoryEndsOnVoid;
            }

            AtrajectoryPath = trajectoryPath;

            Quaternion startLookRotation = _anchorTrajectoryMaker.ComputePathLookRotationBetweenIndices(trajectoryPath, 
                    0, 1);
            Quaternion endLookRotation= _anchorTrajectoryMaker.ComputePathLookRotationBetweenIndices(trajectoryPath, 
                    trajectoryPath.Length-2, trajectoryPath.Length-1);
            
            AnchorThrowResult.Reset(trajectoryPath, ThrowDirection,
                startLookRotation, endLookRotation, moveDuration, trajectoryEndsOnVoid);
        }
        
        private async UniTaskVoid DoThrowAnchor(AnchorThrowResult anchorThrowResult)
        {
            
            _anchorMotion.MoveAlongPath(anchorThrowResult.TrajectoryPathPoints, anchorThrowResult.Duration, 
                anchorThrowResult.InterpolationEaseCurve);
            _anchorMotion.RotateStartToEnd(anchorThrowResult.StartLookRotation,anchorThrowResult.EndLookRotation, 
                anchorThrowResult.Duration, anchorThrowResult.InterpolationEaseCurve);
              /*
            _anchorMotion.MoveAlongPath(anchorThrowResult.TrajectoryPathPoints, anchorThrowResult.Duration);
            _anchorMotion.RotateStartToEnd(anchorThrowResult.StartLookRotation,anchorThrowResult.EndLookRotation, 
                anchorThrowResult.Duration);
            */

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
            }
            else if (_anchorSnapController.HasSnapTarget)
            {
                _anchor.SetGrabbedBySnapper(_anchorSnapController.AnchorSnapTarget);
                _anchorSnapController.ClearState();
            }
            else
            {
                _anchor.SnapToFloor().Forget();
            }
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
            if (_anchorSnapController.HasSnapTarget)
            {
                _anchorSnapController.RemoveCurrentSnapTarget();
            }
        }
        
        public AnchorThrowResult GetLastAnchorThrowResult()
        {
            return AnchorThrowResult;
        }

    }
}