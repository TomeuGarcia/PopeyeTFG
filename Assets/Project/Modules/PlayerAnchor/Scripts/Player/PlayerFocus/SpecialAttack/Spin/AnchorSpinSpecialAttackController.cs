using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spin
{
    public class AnchorSpinSpecialAttackController : IPlayerSpecialAttackController
    {
        private readonly IPlayerFocusSpender _focusSpender;
        private readonly PlayerFocusAttackConfig _focusAttackConfig;
        private readonly IAnchorMediator _anchorMediator;
        private readonly TransformMotion _anchorMotion;
        private readonly IPlayerMediator _playerMediator;
        private readonly AnchorThrowConfig.RotationCorrection _spinEndFloorRotationCorrection;

        private bool _isBeingPerformed;
        private int _numberOfLoops = 2;
        private float _totalDuration = 1.0f;
        
        private float _startSpinDistance = 4.0f;
        private float _endSpinDistance = 7.0f;
        
        private float _startPositioningDuration = 0.2f;
        private float _startPositioningT;
        
        private float _endPositioningDuration = 0.15f;
        private float _endPositioningT;

        private float _loopTime;
        private float _fullLoopTime;
        private float _startOffset;

        private Quaternion _endRotation;
        
        
        public AnchorSpinSpecialAttackController(
            IPlayerFocusSpender focusSpender, 
            PlayerFocusAttackConfig focusAttackConfig,
            IAnchorMediator anchorMediator,
            TransformMotion anchorMotion,
            IPlayerMediator playerMediator,
            AnchorThrowConfig.RotationCorrection spinEndFloorRotationCorrection)
        {
            _focusSpender = focusSpender;
            _focusAttackConfig = focusAttackConfig;
            _anchorMediator = anchorMediator;
            _anchorMotion = anchorMotion;
            _playerMediator = playerMediator;
            _spinEndFloorRotationCorrection = spinEndFloorRotationCorrection;
        }
        
        
        public bool CanDoSpecialAttack()
        {
            return _focusSpender.HasEnoughFocus(_focusAttackConfig.RequiredFocusToPerform) && 
                   !SpecialAttackIsBeingPerformed();/* &&
                   !_anchorMediator.IsBeingCarried();*/
        }

        public bool SpecialAttackIsBeingPerformed()
        {
            return _isBeingPerformed;
        }
        
        public void StartSpecialAttack()
        {
            _focusSpender.SpendFocus(_focusAttackConfig.RequiredFocusToPerform);


            _anchorMediator.OnStartSpinning();

            Vector3 toAnchor;
            if (_anchorMediator.IsBeingCarried())
            {
                toAnchor = _playerMediator.GetLookDirection();
            }
            else
            {
                toAnchor = _anchorMediator.Position - _playerMediator.Position;
            }
            toAnchor = Vector3.ProjectOnPlane(toAnchor, Vector3.up).normalized;
            

            float xDot = Vector3.Dot(toAnchor, Vector3.right);
            _startOffset = Mathf.Acos(xDot);
            
            float zDot = Vector3.Dot(toAnchor, Vector3.forward);
            if (zDot < 0f)
            {
                _startOffset = (Mathf.PI * 2) - _startOffset;
            }

            _loopTime = _startOffset;
            _fullLoopTime = (Mathf.PI * 2 * _numberOfLoops) + _startOffset;
            
            ComputeFinishRotation();
            UpdateLoopTimeAsync().Forget();
            DoStartSpecialAttack().Forget();
        }

        
        private async UniTaskVoid DoStartSpecialAttack()
        {
            _isBeingPerformed = true;
            _playerMediator.SetCanRotate(false);
            
            _anchorMediator.OnStartSpinning();
            
            while (_loopTime < _fullLoopTime)
            {
                UpdateSpin();
                await UniTask.Yield();
            }
            
            _anchorMediator.OnStopSpinning();

            _anchorMediator.SnapToFloor(_playerMediator.Position);
            
            _playerMediator.SetCanRotate(true);
            _isBeingPerformed = false;
        }

        private async UniTaskVoid UpdateLoopTimeAsync()
        {
            _loopTime = _startOffset;
            _startPositioningT = 0;
            _endPositioningT = 0;

            DOTween.To(
                    () => _loopTime,
                    (loopTime) => _loopTime = loopTime,
                    _fullLoopTime,
                    _totalDuration
                )
                .SetEase(Ease.InOutQuad);
            
            
            DOTween.To(
                    () => _startPositioningT,
                    (t) => _startPositioningT = t,
                    1f,
                    _startPositioningDuration
                )
                .SetEase(Ease.InQuad);


            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(0, _totalDuration - _endPositioningDuration)));
            DOTween.To(
                    () => _endPositioningT,
                    (t) => _endPositioningT = t,
                    1f,
                    _endPositioningDuration
                )
                .SetEase(Ease.OutSine);
        }
        

        private void UpdateSpin()
        {
            float spinT = (_loopTime - _startOffset) / (_fullLoopTime - _startOffset);
            spinT = Mathf.Sin(spinT * (Mathf.PI / 2));
            
        
            float cos = Mathf.Cos(_loopTime);
            float sin = Mathf.Sin(_loopTime);
        
            Vector3 spinOffset = new Vector3(cos, 0,sin);

            float spinRadius = Mathf.Lerp(_startSpinDistance, _endSpinDistance, spinT);
            spinOffset *= spinRadius;

            Vector3 spinCenter = _playerMediator.Position;
            Vector3 spinPosition = spinCenter + spinOffset;
            spinPosition = Vector3.LerpUnclamped(_anchorMotion.Position, spinPosition, _startPositioningT);

            
            Vector3 tangent = new Vector3(-sin,  0,cos);

            Vector3 anchorLookDirectionStraight = (spinPosition - spinCenter).normalized;
            Vector3 anchorLookDirectionTilted = (anchorLookDirectionStraight - tangent).normalized;
            Vector3 anchorLookDirection =
                Vector3.LerpUnclamped(anchorLookDirectionTilted, anchorLookDirectionStraight, spinT);
            Quaternion rotation = Quaternion.LookRotation(anchorLookDirection, Vector3.up);
            
            
            rotation = Quaternion.SlerpUnclamped(rotation, _endRotation, _endPositioningT);
            
            _anchorMotion.SetPosition(spinPosition);
            _anchorMotion.SetRotation(rotation);
            
            
            Vector3 playerLookAtPosition = spinCenter + new Vector3(
                Mathf.Cos(_loopTime + (Mathf.PI /2)),
                0,
                Mathf.Sin(_loopTime + (Mathf.PI /2))
            );
            playerLookAtPosition = Vector3.LerpUnclamped(playerLookAtPosition, spinPosition, spinT);
            _playerMediator.LookTowardsPosition(playerLookAtPosition);


            Quaternion damageRotation = Quaternion.LookRotation(anchorLookDirectionStraight, Vector3.up);
            _anchorMediator.OnKeepSpinning(spinCenter, damageRotation, spinRadius);
        }


        private void ComputeFinishRotation()
        {
            float cos = Mathf.Cos(_fullLoopTime);
            float sin = Mathf.Sin(_fullLoopTime);
        
            Vector3 endSpinDirection = new Vector3(cos, 0,sin);
            
            Quaternion rotation = Quaternion.LookRotation(Vector3.down, endSpinDirection);
            _endRotation = AnchorThrowUtilities.CorrectRotationForVisibility(rotation, _spinEndFloorRotationCorrection);
        }
        
        public bool SpecialAttackHasFinished()
        {
            return !_isBeingPerformed;
        }
    }
}