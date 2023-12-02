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
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;
        private AnchorMotion _anchorMotion;
        private AnchorThrowConfig _throwConfig;
        
        private float _currentThrowForce01;
        private float _currentThrowCurveForce01;
        
        public float ThrowDistance { get; private set; }
        private bool _anchorIsBeingThrown;

        public AnchorThrowResult AnchorThrowResult { get; private set; }



        
        public void Configure(IPlayerMediator player, PopeyeAnchor anchor, 
            AnchorTrajectoryMaker anchorTrajectoryMaker, AnchorMotion anchorMotion,
            AnchorThrowConfig throwConfig)
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _anchorMotion = anchorMotion;
            _throwConfig = throwConfig;

            AnchorThrowResult = new AnchorThrowResult();
            
            ResetThrowForce();
        }



        public void UpdateThrowTrajectory()
        {
            Vector3 throwStartPoint = _player.GetAnchorThrowStartPosition();
            Vector3 throwDirection = _player.GetFloorAlignedLookDirection();

            _anchorTrajectoryMaker.UpdateTrajectoryPath(throwStartPoint, throwDirection, ThrowDistance);
        }


        public bool AnchorIsBeingThrown()
        {
            return _anchorIsBeingThrown;
        }

        public void ThrowAnchor()
        {
            Vector3[] trajectoryPath = _anchorTrajectoryMaker.GetCorrectedTrajectoryPath();
            float moveDuration = ComputeThrowDuration();
            moveDuration = _anchorTrajectoryMaker.GetCorrectedDurationByDistance(ThrowDistance, moveDuration);
            bool trajectoryEndsOnVoid = _anchorTrajectoryMaker.TrajectoryEndsOnVoid;
            
            _anchor.SetThrown(trajectoryEndsOnVoid);

            AnchorThrowResult.Reset(trajectoryPath, moveDuration, trajectoryEndsOnVoid);
            DoThrowAnchor(AnchorThrowResult).Forget();
        }

        private async UniTaskVoid DoThrowAnchor(AnchorThrowResult anchorThrowResult)
        {
            _anchorMotion.MoveAlongPath(anchorThrowResult.TrajectoryPathPoints, anchorThrowResult.Duration, Ease.Linear);

            _anchorIsBeingThrown = true;
            await UniTask.Delay(TimeSpan.FromSeconds(anchorThrowResult.Duration));
            
            _anchorIsBeingThrown = false;

            if (anchorThrowResult.EndsOnVoid)
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
        
        public AnchorThrowResult GetLastAnchorThrowResult()
        {
            return AnchorThrowResult;
        }
        
    }
}