using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.ValueStatSystem;
using Project.Modules.PlayerAnchor;
using Project.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PopeyePlayer : MonoBehaviour, IPlayerMediator
    {
        [SerializeField] private Transform _anchorCarryHolder;
        [SerializeField] private Transform _anchorGrabToThrowHolder;
        public Transform AnchorCarryHolder => _anchorCarryHolder;
        public Transform AnchorGrabToThrowHolder => _anchorGrabToThrowHolder;
        
        private PlayerFSM _stateMachine;
        private PlayerController.PlayerController _playerController;
        private PlayerGeneralConfig _playerGeneralConfig;

        private IPlayerView _playerView;
        private PlayerHealth _playerHealth;
        private TimeStaminaSystem _staminaSystem;
        
        private TransformMotion _playerMotion;
        
        private PopeyeAnchor _anchor;
        private IAnchorThrower _anchorThrower;
        private IAnchorPuller _anchorPuller;
        
        
        public Vector3 Position => _playerController.Position;
        
        
        public void Configure(PlayerFSM stateMachine, PlayerController.PlayerController playerController,
            PlayerGeneralConfig playerGeneralConfig, 
            IPlayerView playerView, PlayerHealth playerHealth, TimeStaminaSystem staminaSystem, 
            TransformMotion playerMotion,
            PopeyeAnchor anchor, IAnchorThrower anchorThrower, IAnchorPuller anchorPuller)
        {
            _stateMachine = stateMachine;
            _playerController = playerController;
            _playerGeneralConfig = playerGeneralConfig;
            _playerView = playerView;
            _playerHealth = playerHealth;
            _staminaSystem = staminaSystem;
            _playerMotion = playerMotion;
            _anchor = anchor;
            _anchorThrower = anchorThrower;
            _anchorPuller = anchorPuller;
        }


        private void Update()
        {
            _stateMachine.Update(Time.deltaTime);
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
        public Vector3 GetFloorNormal()
        {
            return _playerController.ContactNormal;
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
        }

        public void PullAnchor()
        {
            _anchorPuller.PullAnchor();
            LookTowardsAnchorForDuration(0.3f).Forget();
        }

        public void OnPullAnchorComplete()
        {
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
            
            LookTowardsAnchorForDuration(duration).Forget();
            _playerMotion.MoveToPosition(dashEndPosition, duration, Ease.InOutQuad);
            SpendStamina(_playerGeneralConfig.MovesetConfig.AnchorDashStaminaCost);

            SetInvulnerableForDuration(_playerGeneralConfig.StatesConfig.DashInvulnerableDuration);
        }


        public void OnAnchorThrowEndedInVoid()
        {
            _stateMachine.OverwriteState(PlayerStates.PlayerStates.PullingAnchor);
        }

        public async UniTaskVoid LookTowardsAnchorForDuration(float duration)
        {
            _playerController.CanRotate = false;
            _playerController.LookTowardsPosition(_anchor.Position);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            _playerController.CanRotate = true;
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
        }

        public void OnKilledByDamageTaken()
        {
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
            if (!HasStaminaLeft())
            {
                EnterTiredState();
            }
        }

        private void EnterTiredState()
        {
            _stateMachine.OverwriteState(PlayerStates.PlayerStates.Tired);
        }
    }
}