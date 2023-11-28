using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorThrowTrajectory
    {
        private readonly AnchorThrowConfig _throwConfig;
        private readonly AnchorMotion _anchorMotion;
        private float _currentThrowForce01;
        private float _currentThrowCurveForce01;

        public float ThrowDistance { get; private set; }
        public bool AnchorIsBeingThrown { get; private set; }
        
        public AnchorThrowTrajectory(AnchorThrowConfig throwConfig, AnchorMotion anchorMotion)
        {
            _throwConfig = throwConfig;
            _anchorMotion = anchorMotion;
            ResetThrowForce();
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

            UpdateThrowDistance();
        }

        private void UpdateThrowDistance()
        {
            ThrowDistance = Mathf.Lerp(_throwConfig.MinThrowDistance, _throwConfig.MaxThrowDistance, 
                _currentThrowCurveForce01);
        }


        public async UniTaskVoid ThrowAnchor(Vector3 throwDirection)
        {
            Vector3 moveDisplacement = throwDirection * ThrowDistance;
            float moveDuration = _currentThrowCurveForce01 * _throwConfig.MaxThrowMoveDuration;

            _anchorMotion.MoveByDisplacement(moveDisplacement, moveDuration, Ease.OutQuad);

            await UniTask.Delay(TimeSpan.FromSeconds(moveDuration));
            AnchorIsBeingThrown = true;
        }

        public void InterruptThrow()
        {
            _anchorMotion.CancelMovement();
            AnchorIsBeingThrown = false;
        }
    }
}