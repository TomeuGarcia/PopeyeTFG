using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.OnVoid;
using Project.Modules.WorldElements.DestructiblePlatforms;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PopeyePlayer : MonoBehaviour, IPlayerMediator
    {
        [SerializeField] private Transform _anchorCarryHolder;
        [SerializeField] private Transform _anchorGrabToThrowHolder;
        [SerializeField] private Transform _targetForEnemies;
        [SerializeField] private Transform _targetForCamera;
        [SerializeField] private InterfaceReference<ISafeGroundChecker, MonoBehaviour> _respawnCheckpointChecker;
        [SerializeField] private DestructiblePlatformBreaker _destructiblePlatformBreaker;
        
        private IPlayerAudio _playerAudio;
        
        public Transform AnchorCarryHolder => _anchorCarryHolder;
        public Transform AnchorGrabToThrowHolder => _anchorGrabToThrowHolder;
        
        private PlayerFSM _stateMachine;
        private PlayerController.PlayerController _playerController;
        private PlayerGeneralConfig _playerGeneralConfig;
        private AnchorGeneralConfig _anchorGeneralConfig;

        public IPlayerView PlayerView { get; private set; }
        private PlayerHealth _playerHealth;
        private TimeStaminaSystem _staminaSystem;
        
        private PlayerMovementChecker _playerMovementChecker;
        private TransformMotion _playerMotion;
        private PlayerDasher _playerDasher;
        
        private PopeyeAnchor _anchor;
        private IAnchorThrower _anchorThrower;
        private IAnchorPuller _anchorPuller;
        private IAnchorKicker _anchorKicker;
        private IAnchorSpinner _anchorSpinner;

        private ISafeGroundChecker _safeGroundChecker;
        private IOnVoidChecker _onVoidChecker;
        
        private bool _pullingAnchorFromTheVoid;
        
        public Vector3 Position => _playerController.Position;
        public Transform PositionTransform => _playerController.Transform;
        public DestructiblePlatformBreaker DestructiblePlatformBreaker => _destructiblePlatformBreaker;

        public void Configure(PlayerFSM stateMachine, PlayerController.PlayerController playerController,
            PlayerGeneralConfig playerGeneralConfig, AnchorGeneralConfig anchorGeneralConfig,
            IPlayerView playerView, IPlayerAudio playerAudio, 
            PlayerHealth playerHealth, TimeStaminaSystem staminaSystem, 
            PlayerMovementChecker playerMovementChecker, TransformMotion playerMotion, PlayerDasher playerDasher,
            PopeyeAnchor anchor, 
            IAnchorThrower anchorThrower, IAnchorPuller anchorPuller, IAnchorKicker anchorKicker,
            IAnchorSpinner anchorSpinner,
            ISafeGroundChecker safeGroundChecker, IOnVoidChecker onVoidChecker)
        {
            _stateMachine = stateMachine;
            _playerController = playerController;
            _playerGeneralConfig = playerGeneralConfig;
            _anchorGeneralConfig = anchorGeneralConfig; 
            PlayerView = playerView;
            _playerHealth = playerHealth;
            _staminaSystem = staminaSystem;
            _playerMovementChecker = playerMovementChecker;
            _playerMotion = playerMotion;
            _playerDasher = playerDasher;
            _anchor = anchor;
            _anchorThrower = anchorThrower;
            _anchorPuller = anchorPuller;
            _anchorKicker = anchorKicker;
            _anchorSpinner = anchorSpinner;

            _playerAudio = playerAudio;

            _safeGroundChecker = safeGroundChecker;
            _onVoidChecker = onVoidChecker;
            
            SetCanUseRotateInput(false);
            SetCanFallOffLedges(false);
            SetInstantRotation(false);
            
            _staminaSystem.OnValueExhausted += OnStaminaExhausted;
        }

        private void OnDestroy()
        {
            _staminaSystem.OnValueExhausted -= OnStaminaExhausted;
        }

        private void Update()
        {
            _stateMachine.Update(Time.deltaTime);
            _playerMovementChecker.Update();
        }

        private void ResetAnchor()
        {
            _anchor.ResetState(Position);
        }
        

        public void SetMaxMovementSpeed(float maxMovementSpeed)
        {
            _playerController.MaxSpeed = maxMovementSpeed;
        }

        public void SetCanUseRotateInput(bool canUseRotateInput)
        {
            _playerController.useLookInput = canUseRotateInput;
        }

        public void SetInstantRotation(bool instantRotation)
        {
            if (instantRotation)
            {
                _playerController.SetInstantRotationMode();
            }
            else
            {
                _playerController.SetOverTimeRotationMode();
            }
        }

        public void SetCanRotate(bool canRotate)
        {
            _playerController.CanRotate = canRotate;
        }

        public void SetCanFallOffLedges(bool canFallOffLedges, bool checkingIgnoreLedges = true)
        {
            _playerController.SetCheckLedges(!canFallOffLedges, checkingIgnoreLedges);
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
            return _anchorGrabToThrowHolder.position + _anchorCarryHolder.TransformDirection(Vector3.back) * 1.4f;
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
            
            PlayerView.PlayThrowAnimation();
        }

        public void PullAnchor()
        {
            _anchorPuller.PullAnchor();
            LookTowardsAnchorForDuration(0.3f).Forget();
            
            PlayerView.PlayPullAnimation(0.3f).Forget();
        }

        public void OnPullAnchorComplete()
        {
            if (_pullingAnchorFromTheVoid)
            {
                _pullingAnchorFromTheVoid = false;

                SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorAutoPullStaminaCost);
                if (!HasStaminaLeft())
                {
                    _anchor.SnapToFloor(Position).Forget();
                    EnterTiredState();
                }
                else
                {
                    _anchor.SetCarried();
                }
                
                return;
            }
            
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorPullStaminaCost);
            if (HasStaminaLeft())
            {
                _anchor.SetCarried();
            }
            else
            {
                _anchor.SnapToFloor(Position).Forget();
            }
        }
        
        

        public async UniTask DashTowardsAnchor()
        {
            float minDuration = _anchor.IsGrabbedBySnapper() ? 
                _playerGeneralConfig.StatesConfig.MinUtilityDashDuration : 
                _playerGeneralConfig.StatesConfig.MinDashDuration;

            float duration = Mathf.Lerp(minDuration, 
                                        _playerGeneralConfig.StatesConfig.MaxDashDuration,
                                        GetDistanceFromAnchorRatio01());
            
            LookTowardsAnchorForDuration(duration).Forget();
            _playerDasher.DashTowardsAnchor(duration);
            
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorDashStaminaCost);

            SetInvulnerableForDuration(_playerGeneralConfig.StatesConfig.DashInvulnerableDuration);
            DropTargetForEnemies(_playerGeneralConfig.StatesConfig.DashInvulnerableDuration).Forget();
            
            PlayerView.PlayDashAnimation(duration);

            await UniTask.Delay(TimeSpan.FromSeconds(duration + 0.1f));
        }

        public async UniTask DashForward()
        {
            _playerDasher.DashForward(_playerGeneralConfig.StatesConfig.MinRollDuration,
                                        _playerGeneralConfig.StatesConfig.MaxRollDuration, 
                            out float duration);
            
            SpendStamina(_playerGeneralConfig.MovesetConfig.RollStaminaCost);
            
            _anchorThrower.ThrowAnchorVertically();

            float invulnerableDuration = _playerGeneralConfig.StatesConfig.RollInvulnerableDuration;
            SetInvulnerableForDuration(invulnerableDuration);
            DropTargetForEnemies(invulnerableDuration).Forget();
            
            PlayerView.PlayDashAnimation(duration);
            
            _playerController.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            _playerController.enabled = true;
        }

        public void KickAnchor()
        {
            _anchorKicker.KickAnchor();
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorKickStaminaCost);

            PlayerView.PlayKickAnimation();
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
            _anchor.OnVoidChecker.ClearState();
            _stateMachine.OverwriteState(PlayerStates.PlayerStates.PullingAnchor);
            _pullingAnchorFromTheVoid = true;
        }

        public void OnPlayerFellOnVoid()
        {
            _onVoidChecker.ClearState();
        }

        public bool TakeFellOnVoidDamage()
        {
            _playerHealth.TakeVoidFallDamage();
            return _playerHealth.IsDead();
        }

        public void OnTryUsingObstructedAnchor()
        {
            LookTowardsAnchor();
            PlayerView.PlayAnchorObstructedAnimation();
            _anchor.OnTryUsingWhenObstructed();
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

        public void RespawnToLastSafeGround()
        {
            _playerController.ResetRigidbody();
            Vector3 respawnPosition = _safeGroundChecker.BestSafePosition + _playerGeneralConfig.RespawnFromVoidPositionOffset;
            _playerMotion.SetPosition(respawnPosition);
        }
        public void RespawnFromDeath()
        {
            _playerController.ResetRigidbody();
            _playerMotion.SetPosition(_respawnCheckpointChecker.Value.BestSafePosition);  
            _playerMotion.SetRotation(Quaternion.identity);
            _playerHealth.HealToMax();
            ResetAnchor();
            
            _playerController.DisableForDuration(0.3f).Forget();

            PlayerView.PlayRespawnAnimation();
        }

        public void OnStartMoving()
        {
            _playerAudio.StartPlayingStepsSounds();
        }

        public void OnStopMoving()
        {
            _playerAudio.StopPlayingStepsSounds();
        }

        public void UpdateSafeGroundChecking(float deltaTime, out bool playerIsOnVoid, out bool anchorIsOnVoid)
        {
            _safeGroundChecker.UpdateChecking(deltaTime);
            _onVoidChecker.UpdateChecking(deltaTime);
            _anchor.OnVoidChecker.UpdateChecking(deltaTime);

            playerIsOnVoid = _onVoidChecker.IsOnVoid;
            anchorIsOnVoid = _anchor.OnVoidChecker.IsOnVoid;
        }

        private async UniTaskVoid DropTargetForEnemies(float duration)
        {
            _targetForEnemies.SetParent(null);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            _targetForEnemies.SetParent(_playerController.Transform);
            _targetForEnemies.localPosition = Vector3.zero;
        }
        public void DropTargetForCamera()
        {
            _targetForCamera.SetParent(null);
        }
        public void ResetTargetForCamera()
        {
            _targetForCamera.SetParent(_playerController.Transform);
            _targetForCamera.localPosition = Vector3.zero;
        }


        public void SetInvulnerable(bool isInvulnerable)
        {
            _playerHealth.SetInvulnerable(isInvulnerable);
        }
        public void SetInvulnerableForDuration(float duration)
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

        public void UseHeal()
        {
            _playerHealth.UseHeal();
            PlayerView.PlayHealAnimation();
        }

        public void HealToMax()
        {
            _playerHealth.HealToMax();
        }

        public void OnDamageTaken()
        {
            PlayerView.PlayTakeDamageAnimation();
            SetInvulnerableForDuration(_playerGeneralConfig.InvulnerableDurationAfterHit);
        }

        public void OnKilledByDamageTaken()
        {
            _stateMachine.OverwriteState(PlayerStates.PlayerStates.Dead);
        }

        public void OnHealed()
        {
            PlayerView.PlayHealAnimation();
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