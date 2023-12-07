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
            
            _anchorTrajectoryMaker.SuperDuperSetup(_throwConfig, 20);
        }

        public bool AnchorIsBeingThrown()
        {
            return _anchorIsBeingThrown;
        }


        public void UpdateThrowTrajectory()
        {
            float duration = ComputeThrowDuration();
            
            Vector3 startPosition = _player.GetAnchorThrowStartPosition();
            Vector3 direction = _player.GetFloorAlignedLookDirection();
            Vector3 floorNormal = _player.GetFloorNormal();
            float distance = ThrowDistance;
            float finalTrajectoryDistance;
            bool trajectoryEndsOnFloor;
            
            IAutoAimTarget autoAimTarget;
            bool validAutoAimTarget;

            Vector3[] trajectoryPoints =
                _anchorTrajectoryMaker.ComputeUpdatedTrajectory(startPosition, direction, floorNormal, distance,
                    out finalTrajectoryDistance, out trajectoryEndsOnFloor, 
                    out autoAimTarget, out validAutoAimTarget);
            
            
            Vector3 right = Vector3.Cross(direction, floorNormal).normalized;
            Quaternion startLookRotation = _anchorTrajectoryMaker.ComputePathLookRotationBetweenIndices(trajectoryPoints, 
                0, 1, right);
            Quaternion endLookRotation = _anchorTrajectoryMaker.ComputePathLookRotationBetweenIndices(trajectoryPoints, 
                trajectoryPoints.Length-2, trajectoryPoints.Length-1, right);
            
            AnchorThrowResult.Reset(trajectoryPoints, direction, startLookRotation, endLookRotation, 
                duration, !trajectoryEndsOnFloor);


            if (validAutoAimTarget)
            {
                _anchorSnapController.ManageAutoAimTargetFound(autoAimTarget);
            }
            else
            {
                _anchorSnapController.ManageNoAutoAimTargetFound();
            }
        }
        
        public void ThrowAnchor()
        {
            if (_anchorSnapController.HasAutoAimTarget)
            {
                _anchorSnapController.UseCurrentTarget(AnchorThrowResult.Duration);
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
            
            if (_anchorSnapController.HasAutoAimTarget)
            {
                _anchor.SetGrabbedBySnapper(_anchorSnapController.AnchorAutoAimTarget);
                _anchorSnapController.ClearState();
                return;
            }
            
            _anchor.SnapToFloor().Forget();
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
            if (_anchorSnapController.HasAutoAimTarget)
            {
                _anchorSnapController.RemoveCurrentAutoAimTarget();
            }
        }
        
        public AnchorThrowResult GetLastAnchorThrowResult()
        {
            return AnchorThrowResult;
        }

    }
}