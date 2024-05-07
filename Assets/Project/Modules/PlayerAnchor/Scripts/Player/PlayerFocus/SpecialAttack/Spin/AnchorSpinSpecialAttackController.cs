using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spin
{
    public class AnchorSpinSpecialAttackController : MonoBehaviour, IPlayerSpecialAttackController
    {
        [SerializeField] private AnchorSpinView _view;
        
        private AnchorSpinAttackConfig _config;
        private IPlayerFocusSpender _focusSpender;
        private PlayerFocusAttackConfig _focusAttackConfig;
        private IAnchorMediator _anchorMediator;
        private TransformMotion _anchorMotion;
        private IPlayerMediator _playerMediator;
        private AnchorThrowConfig.RotationCorrection _spinEndFloorRotationCorrection;

        private IEmptyEventChannelDispatcher _attackPerformedEventDispatcher;
        
        private bool _isBeingPerformed;

        private float _startPositioningT;
        private float _endPositioningT;

        private float _loopTime;
        private float _fullLoopTime;
        private float _startOffset;

        private Quaternion _endRotation;
        
        public float PreparationDuration => _config.PreparationDuration;
        public string Name => "Anchor Spin";


        public void Configure(
            AnchorSpinAttackConfig config,
            IPlayerFocusSpender focusSpender, 
            PlayerFocusAttackConfig focusAttackConfig,
            IAnchorMediator anchorMediator,
            TransformMotion anchorMotion,
            IPlayerMediator playerMediator,
            AnchorThrowConfig.RotationCorrection spinEndFloorRotationCorrection,
            IEmptyEventChannelDispatcher attackPerformedEventDispatcher)
        {
            _config = config;
            _focusSpender = focusSpender;
            _focusAttackConfig = focusAttackConfig;
            _anchorMediator = anchorMediator;
            _anchorMotion = anchorMotion;
            _playerMediator = playerMediator;
            _spinEndFloorRotationCorrection = spinEndFloorRotationCorrection;
            _attackPerformedEventDispatcher = attackPerformedEventDispatcher;
        }

        private void Start()
        {
            _view.Configure(_playerMediator.PositionTransform);
        }

        public void OnPreparationStart(float durationToComplete)
        {
            _view.StartPreparationAnimation(_playerMediator.Position);
        }

        public void OnPreparationInterrupted()
        {
            _view.InterruptPreparationAnimation();
        }
        
        public bool CanDoSpecialAttack()
        {
            return _focusSpender.HasEnoughFocus(_focusAttackConfig.RequiredFocusToPerform) && 
                   !SpecialAttackIsBeingPerformed();
        }

        private bool SpecialAttackIsBeingPerformed()
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
            _fullLoopTime = (Mathf.PI * 2 * _config.NumberOfLoops) + _startOffset;
            
            ComputeFinishRotation();
            UpdateLoopTimeAsync().Forget();
            DoStartSpecialAttack().Forget();
            
            _config.PlayPerformSound();
            
            _attackPerformedEventDispatcher.RaiseEvent();
        }

        
        private async UniTaskVoid DoStartSpecialAttack()
        {
            _isBeingPerformed = true;
            _playerMediator.SetCanRotate(false);
            
            _anchorMediator.OnStartSpinning();
            _view.StartAnimation();
            
            while (_loopTime < _fullLoopTime && _isBeingPerformed)
            {
                UpdateSpin();
                await UniTask.Yield();
            }
            
            _anchorMediator.OnStopSpinning();
            _view.FinishAnimation();

            CorrectAnchorEndPosition();
                        
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
                    _config.TotalDuration
                )
                .SetEase(Ease.InOutQuad);
            
            
            DOTween.To(
                    () => _startPositioningT,
                    (t) => _startPositioningT = t,
                    1f,
                    _config.StartPositioningDuration
                )
                .SetEase(Ease.InQuad);


            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(0, _config.TotalDuration - _config.EndPositioningDuration)));
            DOTween.To(
                    () => _endPositioningT,
                    (t) => _endPositioningT = t,
                    1f,
                    _config.EndPositioningDuration
                )
                .SetEase(Ease.OutSine);
        }
        

        private void UpdateSpin()
        {
            float totalSpinT = (_loopTime - _startOffset) / (_fullLoopTime - _startOffset);
            float spinT = Mathf.Sin(totalSpinT * (Mathf.PI / 2));
            
        
            float cos = Mathf.Cos(_loopTime);
            float sin = Mathf.Sin(_loopTime);
        
            Vector3 spinOffset = new Vector3(cos, 0,sin);

            float spinRadius = Mathf.Lerp(_config.StartSpinDistance, _config.EndSpinDistance, spinT);
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
            _view.UpdateAnimation(spinCenter, damageRotation, spinRadius, totalSpinT);
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

        public void ForceStopSpecialAttack()
        {
            _isBeingPerformed = false;
        }
        
        private void CorrectAnchorEndPosition()
        {
            Vector3 playerPosition = _playerMediator.Position;
            Vector3 playerToAnchor = _anchorMediator.Position - playerPosition;
            float playerToAnchorDistance = playerToAnchor.magnitude;
            Vector3 playerToAnchorDirection = playerToAnchor / playerToAnchorDistance;

            if (Physics.Raycast(playerPosition, playerToAnchorDirection,
                    out RaycastHit obstacleHit, playerToAnchorDistance,
                    _config.ObstacleCollisionProbing.CollisionLayerMask,
                    _config.ObstacleCollisionProbing.QueryTriggerInteraction))
            {
                _anchorMotion.SetPosition(obstacleHit.point);
            }

            _anchorMediator.SnapToFloor(_playerMediator.Position);
        }
    }
}