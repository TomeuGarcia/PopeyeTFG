using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorSpinner : IAnchorSpinner
    {
        private IPlayerMediator _player;
        private IAnchorMediator _anchor;
        private AnchorSpinConfig _anchorSpinConfig;

        private float _spinTime;
        private Vector3 _spinForwardDirection;
        private Vector3 _spinSideDirection;
        private Vector3 _spinCenterPosition;
        public Vector3 SpinPosition { get; private set; }
        private Vector3 _spinStartPosition;
        private Vector3 _anchorSpinPosition;
        private float _spinStartT;

        private bool _isReadyingSpin;


        private float SpinRadius => _anchorSpinConfig.SpinRadius;
        private float SpinMaxSpeed => _anchorSpinConfig.SpinMaxSpeed;
        private float SpinReadyDuration => _anchorSpinConfig.SpinReadyDuration;


        private Action OnSpinReadyFinish;
        

        public void Configure(IPlayerMediator player, IAnchorMediator anchor, AnchorSpinConfig anchorSpinConfig)
        {
            _player = player;
            _anchor = anchor;
            _anchorSpinConfig = anchorSpinConfig;
        }

        public bool CanSpinningAnchor()
        {
            return !_isReadyingSpin;
        }

        public void StartSpinningAnchor(bool startsCarryingAnchor)
        {
            _player.SetCanRotate(false);
            
            _anchor.SetSpinning();
            
            if (startsCarryingAnchor)
            {
                UpdateSpinDirections();
                UpdateSpinStartPosition();
                _player.LookTowardsPosition(_spinStartPosition);
            }
            else
            {
                _player.LookTowardsPosition(_anchor.Position);
                UpdateSpinDirections();
                UpdateSpinStartPosition();
            }
            
            
            
            ResetSpinTime();
            ReadySpin();
        }

        public void StopSpinningAnchor()
        {
            if (_isReadyingSpin)
            {
                OnSpinReadyFinish += QueueStopSpinning;
            }
            else
            {
                _player.SetCanRotate(true);
                _anchor.SnapToFloor().Forget();
            }
        }

        public void SpinAnchor(float deltaTime)
        {
            if (_isReadyingSpin)
            {
                //return;
            }
            
            UpdateSpinCenterPosition();
            UpdateSpinTime(deltaTime);

            UpdateSpinPosition(deltaTime);
        }


        private void UpdateSpinDirections()
        {
            _spinForwardDirection = _player.GetFloorAlignedLookDirection();
            Vector3 floorNormal = _player.GetFloorNormal();
            _spinSideDirection = Vector3.ProjectOnPlane(_player.GetRightDirection(), floorNormal).normalized;
        }

        private void UpdateSpinCenterPosition()
        {
            _spinCenterPosition = _player.Position;
        }

        private void ResetSpinTime()
        {
            _spinTime = 0;
        }
        private void UpdateSpinTime(float deltaTime)
        {
            _spinTime += deltaTime * SpinMaxSpeed;
        }
        
        private void UpdateSpinStartPosition()
        {
            _spinStartPosition = ComputeSpinPosition(0);
        }
        
        private void UpdateSpinPosition(float deltaTime)
        {
            SpinPosition = ComputeSpinPosition(_spinTime);
            UpdateAnchorSpinPosition(deltaTime);
            
            _anchor.SetPosition(_anchorSpinPosition);

            Vector3 forward = (SpinPosition - _player.Position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(forward, _player.GetFloorNormal());
            _anchor.SetRotation(lookRotation);
            _player.LookTowardsPosition(SpinPosition);
        }

        private Vector3 ComputeSpinPosition(float time)
        {
            return _spinCenterPosition +
                   ((Mathf.Cos(time) * SpinRadius) * _spinForwardDirection) +
                   ((Mathf.Sin(time) * SpinRadius) * _spinSideDirection);
        }



        private void ReadySpin()
        {
            _isReadyingSpin = true;
            
            SpinPosition = _anchor.Position;

            DOTween.To(
                    () => SpinPosition,
                    (position) =>
                    {
                        SpinPosition = position;
                    },
                    _spinStartPosition,
                    SpinReadyDuration
                )
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                    {
                        OnSpinReadyFinish?.Invoke();
                        _isReadyingSpin = false;
                    }
                );
        }

        private void QueueStopSpinning()
        {
            OnSpinReadyFinish -= QueueStopSpinning;
            StopSpinningAnchor();
        }

        private void UpdateAnchorSpinPosition(float deltaTime)
        {
            _spinStartT += deltaTime * SpinReadyDuration;

            if (_spinStartT < 1)
            {
                _anchorSpinPosition = Vector3.Lerp(_spinStartPosition, SpinPosition, _spinStartT);
            }
            else
            {
                _anchorSpinPosition = SpinPosition;
            }
        }
        
    }
}