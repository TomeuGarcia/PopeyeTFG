using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorThrower : IAnchorThrower
    {
        private IPlayerMediator _player;
        private PopeyeAnchor _anchor;
        private AnchorThrowTrajectory _anchorThrowTrajectory;
        private AnchorMotion _anchorMotion;
        private AnchorThrowConfig _throwConfig;
        
        private float _currentThrowForce01;
        private float _currentThrowCurveForce01;
        
        public float ThrowDistance { get; private set; }
        private bool _anchorIsBeingThrown;

        
        public void Configure(IPlayerMediator player, PopeyeAnchor anchor, 
            AnchorThrowTrajectory anchorThrowTrajectory, AnchorMotion anchorMotion,
            AnchorThrowConfig throwConfig)
        {
            _player = player;
            _anchor = anchor;
            _anchorThrowTrajectory = anchorThrowTrajectory;
            _anchorMotion = anchorMotion;
            _throwConfig = throwConfig;
            
            ResetThrowForce();
        }



        public void UpdateThrowTrajectory()
        {
            Vector3 throwStartPoint = _player.GetAnchorThrowStartPosition();
            Vector3 throwDirection = _player.GetFloorAlignedLookDirection();

            _anchorThrowTrajectory.UpdateTrajectoryPath(throwStartPoint, throwDirection, ThrowDistance);
        }


        public bool AnchorIsBeingThrown()
        {
            return _anchorIsBeingThrown;
        }

        public void ThrowAnchor()
        {
            _anchor.SetThrown();
            
            Vector3 throwDirection = _player.GetFloorAlignedLookDirection();
            float moveDuration = ComputeThrowDuration();

            DoThrowAnchor(throwDirection, ThrowDistance, moveDuration).Forget();
        }

        private async UniTaskVoid DoThrowAnchor(Vector3 direction, float distance, float duration)
        {
            Vector3[] trajectoryPath = _anchorThrowTrajectory.GetCorrectedTrajectoryPath();
            duration = _anchorThrowTrajectory.GetCorrectedDurationByDistance(distance, duration);
            bool trajectoryEndsOnVoid = _anchorThrowTrajectory.TrajectoryEndsOnVoid;
            
            _anchorMotion.MoveAlongPath(trajectoryPath, duration, Ease.Linear);

            _anchorIsBeingThrown = true;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            
            _anchorIsBeingThrown = false;

            if (trajectoryEndsOnVoid)
            {
                _player.OnAnchorThrowEndedInVoid();
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
            // TODO
        }
        
    }
}