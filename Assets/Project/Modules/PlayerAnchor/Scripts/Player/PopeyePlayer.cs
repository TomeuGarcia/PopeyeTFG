using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.ValueStatSystem;
using Project.Modules.PlayerAnchor;
using Project.Modules.PlayerAnchor.Anchor;
using Project.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PopeyePlayer : MonoBehaviour, IPlayerMediator
    {
        [SerializeField] private Transform _anchorCarryHolder;
        [SerializeField] private Transform _anchorGrabToThrowHolder;
        [SerializeField] private Transform _targetForEnemies;
        [SerializeField] private PlayerRespawner _playerRespawner;
        
        public Transform AnchorCarryHolder => _anchorCarryHolder;
        public Transform AnchorGrabToThrowHolder => _anchorGrabToThrowHolder;
        
        private PlayerFSM _stateMachine;
        private PlayerController.PlayerController _playerController;
        private PlayerGeneralConfig _playerGeneralConfig;
        private AnchorGeneralConfig _anchorGeneralConfig;

        private IPlayerView _playerView;
        private PlayerHealth _playerHealth;
        private TimeStaminaSystem _staminaSystem;
        
        private TransformMotion _playerMotion;
        
        private PopeyeAnchor _anchor;
        private IAnchorThrower _anchorThrower;
        private IAnchorPuller _anchorPuller;
        private IAnchorKicker _anchorKicker;

        private bool _pullingAnchorFromTheVoid;
        
        public Vector3 Position => _playerController.Position;
        
        
        public void Configure(PlayerFSM stateMachine, PlayerController.PlayerController playerController,
            PlayerGeneralConfig playerGeneralConfig, AnchorGeneralConfig anchorGeneralConfig,
            IPlayerView playerView, PlayerHealth playerHealth, TimeStaminaSystem staminaSystem, 
            TransformMotion playerMotion,
            PopeyeAnchor anchor, 
            IAnchorThrower anchorThrower, IAnchorPuller anchorPuller, IAnchorKicker anchorKicker)
        {
            _stateMachine = stateMachine;
            _playerController = playerController;
            _playerGeneralConfig = playerGeneralConfig;
            _anchorGeneralConfig = anchorGeneralConfig; 
            _playerView = playerView;
            _playerHealth = playerHealth;
            _staminaSystem = staminaSystem;
            _playerMotion = playerMotion;
            _anchor = anchor;
            _anchorThrower = anchorThrower;
            _anchorPuller = anchorPuller;
            _anchorKicker = anchorKicker;

            _staminaSystem.OnValueExhausted += OnStaminaExhausted;
        }

        private void OnDestroy()
        {
            _staminaSystem.OnValueExhausted -= OnStaminaExhausted;
        }

        private void Update()
        {
            _stateMachine.Update(Time.deltaTime);
        }


        private void ResetAnchor()
        {
            _anchor.ResetState();
        }
        

        public void SetMaxMovementSpeed(float maxMovementSpeed)
        {
            _playerController.MaxSpeed = maxMovementSpeed;
        }

        public void SetCanRotate(bool canRotate)
        {
            _playerController.CanRotate = canRotate;
        }

        
        public float GetDistanceFromAnchor()
        {
            return Vector3.Distance(Position, _anchor.Position);
        }

        public float GetDistanceFromAnchorRatio01()
        {
            return Mathf.Min(1.0f, GetDistanceFromAnchor() / _anchorGeneralConfig.ChainConfig.MaxChainLength);
        }

        public Vector3 GetFloorAlignedDirectionToAnchor()
        {
            return Vector3.ProjectOnPlane((_anchor.Position - Position).normalized, Vector3.up).normalized;
        }

        public Vector3 GetLookDirection()
        {
            return _playerController.LookDirection;
        }
        public Vector3 GetFloorAlignedLookDirection()
        {
            return _playerController.GetFloorAlignedLookDirection();
        }

        public Vector3 GetLookDirectionConsideringSteep()
        {
            Vector3 playerLookDirection = GetLookDirection();

            bool isInSteepLookingUp = Vector3.Dot(GetFloorNormal(), playerLookDirection) < 0;
            return isInSteepLookingUp
                ? GetFloorAlignedLookDirection()
                : playerLookDirection;
        }

        public Vector3 GetFloorNormal()
        {
            return _playerController.GroundNormal;
        }

        public Vector3 GetAnchorThrowStartPosition()
        {
            return _anchorGrabToThrowHolder.position;
        }


        
        public void PickUpAnchor()
        {
            _anchor.SetCarried();
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorPickUpStaminaCost);
        }
        
        public void StartChargingThrow()
        {
            _anchorThrower.ResetThrowForce();
            _anchor.SetGrabbedToThrow();
            _anchor.OnStartChargingThrow();
        }

        public void ChargeThrow(float deltaTime)
        {
            _anchorThrower.IncrementThrowForce(deltaTime);
            _anchor.OnKeepChargingThrow();
        }

        public void StopChargingThrow()
        {
            _anchor.OnStopChargingThrow();
        }

        public void CancelChargingThrow()
        {
            _anchorThrower.CancelChargingThrow();
            _anchor.SetCarried();
        }

        public void ThrowAnchor()
        {
            _anchorThrower.ThrowAnchor();
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorThrowStaminaCost);
            
            _playerView.PlayThrowAnimation();
        }

        public void PullAnchor()
        {
            _anchorPuller.PullAnchor();
            LookTowardsAnchorForDuration(0.3f).Forget();
            
            _playerView.PlayPullAnimation(0.3f);
        }

        public void OnPullAnchorComplete()
        {
            if (_pullingAnchorFromTheVoid)
            {
                _anchor.SetCarried();
                _pullingAnchorFromTheVoid = false;
                return;
            }
            
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorPullStaminaCost);
            if (HasStaminaLeft())
            {
                _anchor.SetCarried();
            }
            else
            {
                _anchor.SnapToFloor().Forget();
            }
        }
        
        

        public void DashTowardsAnchor(float duration)
        {
            LookTowardsAnchorForDuration(duration).Forget();
            _playerMotion.MoveToPosition(ComputeDashEndPosition(), duration, Ease.InOutQuad);
            
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorDashStaminaCost);

            SetInvulnerableForDuration(_playerGeneralConfig.StatesConfig.DashInvulnerableDuration);
            DropTargetForEnemies(_playerGeneralConfig.StatesConfig.DashInvulnerableDuration).Forget();
            
            _playerView.PlayDashAnimation(duration);
        }

        private Vector3 ComputeDashEndPosition()
        {
            Vector3 up = Vector3.up;
            Vector3 toAnchor = Vector3.ProjectOnPlane((_anchor.Position - Position).normalized, up);
            Vector3 right = Vector3.Cross(toAnchor, up).normalized;

            Vector3 dashExtraDisplacement = _playerGeneralConfig.MovesetConfig.DashExtraDisplacement;
            
            Vector3 extraDisplacement = toAnchor * dashExtraDisplacement.z;
            extraDisplacement += right * dashExtraDisplacement.x;
            extraDisplacement += up * dashExtraDisplacement.y;

            Vector3 dashEndPosition = _anchor.GetDashEndPosition() + extraDisplacement;

            if (_anchor.IsGrabbedBySnapper())
            {
                Vector3 snapExtraDisplacement = _playerGeneralConfig.MovesetConfig.SnapExtraDisplacement;
                dashEndPosition += toAnchor * snapExtraDisplacement.z;
                dashEndPosition += right * snapExtraDisplacement.x;
                dashEndPosition += up * snapExtraDisplacement.y;
            }

            return dashEndPosition;
        }
        

        public void KickAnchor()
        {
            _anchorKicker.KickAnchor();
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorKickStaminaCost);

            _playerView.PlayKickAnimation();
        }

        public async UniTaskVoid StartSpinningAnchor()
        {
            _anchor.SetSpinning();
            
            await _staminaSystem.SpendProgressively();
            
            _anchor.SnapToFloor().Forget();
        }
        public void StopSpinningAnchor()
        {
            _staminaSystem.StopSpendingProgressively();
        }
        


        public void OnAnchorEndedInVoid()
        {
            _stateMachine.OverwriteState(PlayerStates.PlayerStates.PullingAnchor);
            _pullingAnchorFromTheVoid = true;
        }

        public async UniTaskVoid LookTowardsAnchorForDuration(float duration)
        {
            _playerController.CanRotate = false;
            _playerController.LookTowardsPosition(_anchor.Position);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            _playerController.CanRotate = true;
        }

        
        public Transform GetTargetForEnemies()
        {
            return _targetForEnemies;
        }

        public void Respawn()
        {
            _playerMotion.MoveToPosition(_playerRespawner.RespawnPosition, 0.01f);  
            _playerHealth.HealToMax();
            ResetAnchor();
        }

        private async UniTaskVoid DropTargetForEnemies(float duration)
        {
            _targetForEnemies.SetParent(null);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            _targetForEnemies.SetParent(_playerController.Transform);
            _targetForEnemies.localPosition = Vector3.zero;
        }
        

        private void SetInvulnerableForDuration(float duration)
        {
            _playerHealth.SetInvulnerableForDuration(duration);
        }
        
        public bool HasStaminaLeft()
        {
            return _staminaSystem.HasStaminaLeft();
        }
        public bool HasMaxStamina()
        {
            return _staminaSystem.HasMaxStamina();
        }

        public bool CanHeal()
        {
            // For now there are no number of heal limits
            return !_playerHealth.IsMaxHealth();
        }

        public async UniTask UseHeal()
        {
            _playerHealth.UseHeal();
            await _playerView.PlayHealAnimation();
        }

        public void HealToMax()
        {
            _playerHealth.HealToMax();
        }

        public void OnDamageTaken()
        {
            _playerView.PlayTakeDamageAnimation();
            SetInvulnerableForDuration(_playerGeneralConfig.InvulnerableDurationAfterHit);
        }

        public void OnKilledByDamageTaken()
        {
            _stateMachine.OverwriteState(PlayerStates.PlayerStates.Dead);
            _playerView.PlayDeathAnimation();
        }

        public void OnHealed()
        {
            _playerView.PlayHealAnimation();
        }
        

        private void SpendStamina(int spendAmount)
        {
            if (spendAmount == 0) return;
            
            _staminaSystem.Spend(spendAmount);
        }

        private void OnStaminaExhausted()
        {
            EnterTiredState();
        }

        private void EnterTiredState()
        {
            _stateMachine.OverwriteState(PlayerStates.PlayerStates.Tired);
        }
    }
}