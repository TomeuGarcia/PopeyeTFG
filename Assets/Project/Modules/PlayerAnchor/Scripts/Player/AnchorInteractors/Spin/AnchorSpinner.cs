using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Anchor;
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
        public Vector3 SpinCircumferencePosition { get; private set; }
        private Vector3 _spinCircumferenceInitialPosition;
        
        private Vector3 _anchorSpinPosition;
        
        private Vector3 _startingStage_SpinPosition;
        private float _spinStartT;
        private float _spinStopT;
        
        private SpinStage _currentSpinStage;

        private float _currentSpinRadius;
        private float _currentSpinSpeed;

        
        private float SpinRadius => _anchorSpinConfig.SpinRadius;
        private float SpinSpeed => _anchorSpinConfig.SpinSpeed;
        private float SpinStartDuration => _anchorSpinConfig.SpinStartDuration;
        private float SpinStopDuration => _anchorSpinConfig.SpinStopDuration;
        private AnimationCurve SpinStartEase => _anchorSpinConfig.SpinStartEase;
        private AnimationCurve SpinStopEase => _anchorSpinConfig.SpinStopEase;
        


        private Action OnSpinStartFinish;
        private Action OnSpinStopFinish;
        private bool _wasInterrupted;


        private enum SpinStage
        {
            Starting,
            Normal,
            Stopping,
            Finished
        }
        

        public void Configure(IPlayerMediator player, IAnchorMediator anchor, AnchorSpinConfig anchorSpinConfig)
        {
            _player = player;
            _anchor = anchor;
            _anchorSpinConfig = anchorSpinConfig;

            _currentSpinStage = SpinStage.Finished;
            _wasInterrupted = false;
        }

        public bool CanSpinningAnchor()
        {
            return _currentSpinStage == SpinStage.Finished && !_wasInterrupted;
        }

        public bool IsLockedIntoSpinningAnchor()
        {
            return _currentSpinStage == SpinStage.Starting;
        }

        public bool SpinningAnchorFinished()
        {
            return _currentSpinStage == SpinStage.Finished;
        }

        public void StartSpinningAnchor(bool startsCarryingAnchor, bool spinToTheRight)
        {
            _anchor.SetSpinning(spinToTheRight);
            
            if (startsCarryingAnchor)
            {
                UpdateSpinDirections(spinToTheRight);
                UpdateSpinStartPosition();
                _player.LookTowardsPosition(_spinCircumferenceInitialPosition);
            }
            else
            {
                _player.LookTowardsPosition(_anchor.Position);
                UpdateSpinDirections(spinToTheRight);
                UpdateSpinStartPosition();
            }
            
            
            
            ResetSpinTime();
            EnterStartingStage(startsCarryingAnchor);
        }

        public void StopSpinningAnchor()
        {
            if (_currentSpinStage == SpinStage.Starting)
            {
                OnSpinStartFinish += QueueStopSpinning;
            }
            else
            {
                OnSpinStopFinish += DoOnStopFinish;
                EnterStoppingStage();
            }
        }
        
        private void QueueStopSpinning()
        {
            OnSpinStartFinish -= QueueStopSpinning;
            StopSpinningAnchor();
        }
        
        private void DoOnStopFinish()
        {
            OnSpinStopFinish -= DoOnStopFinish;
            
            _anchor.SnapToFloor(_player.Position).Forget();
        }

        

        public void SpinAnchor(float deltaTime)
        {
            if (_wasInterrupted)
            {
                return;
            }
            
            UpdateSpinState(deltaTime);
        }

        public void InterruptSpinningAnchor()
        {
            InterruptCooldown().Forget();
            _anchor.SnapToFloor(_player.Position).Forget();
        }

        private async UniTaskVoid InterruptCooldown()
        {
            _wasInterrupted = true;

            float delay = _currentSpinStage == SpinStage.Starting ? 
                SpinStartDuration + 0.1f : 
                SpinStopDuration + 0.1f;
            
            _currentSpinStage = SpinStage.Finished;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            
            _wasInterrupted = false;
        }


        private void UpdateSpinState(float deltaTime)
        {
            UpdateSpinCenterPosition();
            UpdateSpinTime(deltaTime);

            UpdateSpinPosition(deltaTime);
            
            _anchor.OnKeepSpinning();
        }


        private void UpdateSpinDirections(bool spinToTheRight)
        {
            _spinForwardDirection = _player.GetFloorAlignedLookDirection();
            Vector3 floorNormal = _player.GetFloorNormal();

            Vector3 sideDirection = spinToTheRight ? _player.GetRightDirection() : -_player.GetRightDirection();
            _spinSideDirection = Vector3.ProjectOnPlane(sideDirection, floorNormal).normalized;
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
            _spinTime += deltaTime * _currentSpinSpeed;
        }
        
        private void UpdateSpinStartPosition()
        {
            _spinCircumferenceInitialPosition = ComputeSpinPosition(0);
        }
        
        private void UpdateSpinPosition(float deltaTime)
        {
            SpinCircumferencePosition = ComputeSpinPosition(_spinTime);
            
            if (_currentSpinStage != SpinStage.Starting)
            {
                _anchorSpinPosition = SpinCircumferencePosition;
            }
            
            _anchor.SetPosition(_anchorSpinPosition);

            //Vector3 forward = (SpinCircumferencePosition - _player.Position).normalized;
            Vector3 forward = (_anchor.Position - _player.Position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(forward, _player.GetFloorNormal());
            _anchor.SetRotation(lookRotation);
            _player.LookTowardsPosition(SpinCircumferencePosition);
        }

        private Vector3 ComputeSpinPosition(float time)
        {
            return _spinCenterPosition +
                   ((Mathf.Cos(time) * _currentSpinRadius) * _spinForwardDirection) +
                   ((Mathf.Sin(time) * _currentSpinRadius) * _spinSideDirection);
        }



        private void EnterStartingStage(bool startsCarryingAnchor)
        {
            _currentSpinStage = SpinStage.Starting;
            _startingStage_SpinPosition = _anchor.Position;

            float radiusStart = startsCarryingAnchor ? 0 : _player.GetDistanceFromAnchor();
            float startSpinSpeed = startsCarryingAnchor ? SpinSpeed : 0;

            _spinStartT = 0;
            DOTween.To(
                    () => _spinStartT,
                    (t) =>
                    {
                        _spinStartT = t;
                        _currentSpinSpeed = Mathf.Lerp(startSpinSpeed, SpinSpeed, _spinStopT);
                        _anchorSpinPosition = Vector3.Lerp(_startingStage_SpinPosition, SpinCircumferencePosition,
                            _spinStartT);
                        _currentSpinRadius = Mathf.Lerp(radiusStart, SpinRadius, _spinStartT);
                    },
                    1,
                    SpinStartDuration
                )
                .SetEase(SpinStartEase)
                .OnComplete(() =>
                    {
                        if (!_wasInterrupted)
                        {
                            _currentSpinStage = SpinStage.Normal;
                        }
                        OnSpinStartFinish?.Invoke();
                    }
                );
            
            
            DOTween.To(
                    () => _startingStage_SpinPosition,
                    (position) =>
                    {
                        _startingStage_SpinPosition = position;
                    },
                    _spinCircumferenceInitialPosition,
                    SpinStartDuration
                )
                .SetEase(SpinStartEase);
        }

        private void EnterStoppingStage()
        {
            _currentSpinStage = SpinStage.Stopping;

            _spinStopT = 0;
            DOTween.To(
                    () => _spinStopT,
                    (t) =>
                    {
                        _spinStopT = t;
                        _currentSpinSpeed = Mathf.Lerp(SpinSpeed, 0, _spinStopT);
                        UpdateSpinState(Time.deltaTime); // This isn't good, but I can't find a better way to keep
                                                         // updating the state when Stopping, because the player can
                                                         // enter the Tired State
                    },
                    1,
                    SpinStopDuration
                )
                .SetEase(SpinStopEase)
                .OnComplete(() =>
                    {
                        _currentSpinStage = SpinStage.Finished;
                        _anchor.OnStopSpinning();
                        OnSpinStopFinish?.Invoke();
                    }
                );
        }

    }
}