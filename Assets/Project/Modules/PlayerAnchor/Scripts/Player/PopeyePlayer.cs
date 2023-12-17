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
        private PlayerDasher _playerDasher;
        
        private PopeyeAnchor _anchor;
        private IAnchorThrower _anchorThrower;
        private IAnchorPuller _anchorPuller;
        private IAnchorKicker _anchorKicker;
        private IAnchorSpinner _anchorSpinner;

        private bool _pullingAnchorFromTheVoid;
        
        public Vector3 Position => _playerController.Position;
        
        
        public void Configure(PlayerFSM stateMachine, PlayerController.PlayerController playerController,
            PlayerGeneralConfig playerGeneralConfig, AnchorGeneralConfig anchorGeneralConfig,
            IPlayerView playerView, PlayerHealth playerHealth, TimeStaminaSystem staminaSystem, 
            TransformMotion playerMotion, PlayerDasher playerDasher,
            PopeyeAnchor anchor, 
            IAnchorThrower anchorThrower, IAnchorPuller anchorPuller, IAnchorKicker anchorKicker,
            IAnchorSpinner anchorSpinner)
        {
            _stateMachine = stateMachine;
            _playerController = playerController;
            _playerGeneralConfig = playerGeneralConfig;
            _anchorGeneralConfig = anchorGeneralConfig; 
            _playerView = playerView;
            _playerHealth = playerHealth;
            _staminaSystem = staminaSystem;
            _playerMotion = playerMotion;
            _playerDasher = playerDasher;
            _anchor = anchor;
            _anchorThrower = anchorThrower;
            _anchorPuller = anchorPuller;
            _anchorKicker = anchorKicker;
            _anchorSpinner = anchorSpinner;

            SetCanUseRotateInput(false);
            SetCanFallOffLedges(false);
            
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

        public void SetCanUseRotateInput(bool canUseRotateInput)
        {
            _playerController.useLookInput = canUseRotateInput;
        }

        public void SetCanRotate(bool canRotate)
        {
            _playerController.CanRotate = canRotate;
        }

        public void SetCanFallOffLedges(bool canFallOffLedges)
        {
            _playerController.SetCheckLedges(!canFallOffLedges);
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

        public Vector3 GetRightDirection()
        {
            return _playerController.RightDirection;
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
        
        

        public async UniTask DashTowardsAnchor()
        {
            float duration = Mathf.Lerp(_playerGeneralConfig.StatesConfig.MinDashDuration, 
                                        _playerGeneralConfig.StatesConfig.MaxDashDuration,
                                        GetDistanceFromAnchorRatio01());
            
            LookTowardsAnchorForDuration(duration).Forget();
            _playerDasher.DashTowardsAnchor(duration);
            
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorDashStaminaCost);

            SetInvulnerableForDuration(_playerGeneralConfig.StatesConfig.DashInvulnerableDuration);
            DropTargetForEnemies(_playerGeneralConfig.StatesConfig.DashInvulnerableDuration).Forget();
            
            _playerView.PlayDashAnimation(duration);

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        public async UniTask DashForward()
        {
            float duration = _playerGeneralConfig.StatesConfig.MaxRollDuration;
            
            _playerDasher.DashForward(duration, out float distanceChangeRatio01);
            duration *= distanceChangeRatio01;
            
            SpendStamina(_playerGeneralConfig.MovesetConfig.RollStaminaCost);
            
            _anchorThrower.ThrowAnchorVertically();

            float invulnerableDuration = _playerGeneralConfig.StatesConfig.RollInvulnerableDuration * distanceChangeRatio01;
            SetInvulnerableForDuration(invulnerableDuration);
            DropTargetForEnemies(invulnerableDuration).Forget();
            
            _playerView.PlayDashAnimation(duration);
            
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        public void KickAnchor()
        {
            _anchorKicker.KickAnchor();
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorKickStaminaCost);

            _playerView.PlayKickAnimation();
        }

        public bool CanSpinAnchor()
        {
            return _anchorSpinner.CanSpinningAnchor();
        }

        public bool IsLockedIntoSpinningAnchor()
        {
            return _anchorSpinner.IsLockedIntoSpinningAnchor();
        }


        public void StartSpinningAnchor(bool startsCarryingAnchor, bool spinToTheRight)
        {
            _anchorSpinner.StartSpinningAnchor(startsCarryingAnchor, spinToTheRight);
            
            _staminaSystem.SetProgressiveSpendPerSecond(_playerGeneralConfig.MovesetConfig.AnchorSpinPerSecondStaminaCost);
            _staminaSystem.SpendProgressively().Forget();

            _staminaSystem.OnValueExhausted += StopSpinningAnchor;
        }

        public void SpinAnchor(float deltaTime)
        {
            _anchorSpinner.SpinAnchor(deltaTime);
        }

        public void StopSpinningAnchor()
        {
            _anchorSpinner.StopSpinningAnchor();
            
            _staminaSystem.StopSpendingProgressively();
            _staminaSystem.OnValueExhausted -= StopSpinningAnchor;
        }

        public void InterruptSpinningAnchor()
        {
            _anchorSpinner.InterruptSpinningAnchor();
            
            _staminaSystem.StopSpendingProgressively();
            _staminaSystem.OnValueExhausted -= StopSpinningAnchor;
        }

        public bool SpinningAnchorFinished()
        {
            return _anchorSpinner.SpinningAnchorFinished();
        }


        public void OnAnchorEndedInVoid()
        {
            _stateMachine.OverwriteState(PlayerStates.PlayerStates.PullingAnchor);
            _pullingAnchorFromTheVoid = true;
        }

        public void LookTowardsPosition(Vector3 position)
        {
            _playerController.LookTowardsPosition(position);
        }

        public void LookTowardsAnchor()
        {
            LookTowardsPosition(_anchor.Position);
        }

        public async UniTaskVoid LookTowardsAnchorForDuration(float duration)
        {
            _playerController.CanRotate = false;
            LookTowardsAnchor();
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            _playerController.CanRotate = true;
        }

        
        public Transform GetTargetForEnemies()
        {
            return _targetForEnemies;
        }

        public void Respawn()
        {
            _playerMotion.SetPosition(_playerRespawner.RespawnPosition);  
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