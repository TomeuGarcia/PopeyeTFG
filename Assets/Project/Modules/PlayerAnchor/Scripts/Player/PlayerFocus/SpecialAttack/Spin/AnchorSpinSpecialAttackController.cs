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

        private bool _isBeingPerformed;
        private int _numberOfLoops = 2;
        private float _totalDuration = 1.0f;
        
        private float _spinDistance = 7.0f;
        
        private float _startPositioningDuration = 0.2f;
        private float _startPositioningT;

        private float _loopTime;
        private float _fullLoopTime;
        private float _startOffset;
        
        
        public AnchorSpinSpecialAttackController(
            IPlayerFocusSpender focusSpender, 
            PlayerFocusAttackConfig focusAttackConfig,
            IAnchorMediator anchorMediator,
            TransformMotion anchorMotion,
            IPlayerMediator playerMediator)
        {
            _focusSpender = focusSpender;
            _focusAttackConfig = focusAttackConfig;
            _anchorMediator = anchorMediator;
            _anchorMotion = anchorMotion;
            _playerMediator = playerMediator;
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
            //_focusSpender.SpendFocus(_focusAttackConfig.RequiredFocusToPerform);


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
            DOTween.To(
                    () => _loopTime,
                    (loopTime) => _loopTime = loopTime,
                    _fullLoopTime,
                    _totalDuration
                )
                .SetEase(Ease.InOutQuad);
            
            
            _startPositioningT = 0;
            DOTween.To(
                    () => _startPositioningT,
                    (t) => _startPositioningT = t,
                    1f,
                    _startPositioningDuration
                )
                .SetEase(Ease.InQuad);
        }
        

        private void UpdateSpin()
        {
            float spinT = (_loopTime - _startOffset) / (_fullLoopTime - _startOffset);
            spinT = Mathf.Sin(spinT * (Mathf.PI / 2));
            
        
            float cos = Mathf.Cos(_loopTime);
            float sin = Mathf.Sin(_loopTime);
        
            Vector3 spinOffset = new Vector3(cos, 0,sin);
            spinOffset *= _spinDistance;

            Vector3 spinCenter = _playerMediator.Position;

            Vector3 spinPosition = spinCenter + spinOffset;

            spinPosition = Vector3.LerpUnclamped(_anchorMotion.Position, spinPosition, _startPositioningT);

            
            Vector3 tangent = new Vector3(-sin,  0,cos);

            Vector3 anchorLookDirectionStraight = (spinPosition - spinCenter).normalized;
            Vector3 anchorLookDirectionTilted = (anchorLookDirectionStraight - tangent).normalized;
            Vector3 anchorLookDirection =
                Vector3.LerpUnclamped(anchorLookDirectionTilted, anchorLookDirectionStraight, spinT);
            
            
            Quaternion rotation = Quaternion.LookRotation(anchorLookDirection, Vector3.up);
            
            
            _anchorMotion.SetPosition(spinPosition);
            _anchorMotion.SetRotation(rotation);
            
            Vector3 playerLookAtPosition = spinCenter + new Vector3(
                Mathf.Cos(_loopTime + (Mathf.PI /2)),
                0,
                Mathf.Sin(_loopTime + (Mathf.PI /2))
            );
            
            playerLookAtPosition = Vector3.LerpUnclamped(playerLookAtPosition, spinPosition, spinT);
            
            _playerMediator.LookTowardsPosition(playerLookAtPosition);


            Vector3 damagePosition = Vector3.LerpUnclamped(spinCenter, spinPosition, 0.5f);
            Quaternion damageRotation = rotation;
            _anchorMediator.OnKeepSpinning(damagePosition, damageRotation);
        }
        

        public bool SpecialAttackHasFinished()
        {
            return !_isBeingPerformed;
        }
    }
}