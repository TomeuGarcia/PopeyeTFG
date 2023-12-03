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
        private PlayerMovesetConfig _playerMovesetConfig;

        private TimeStaminaSystem _staminaSystem;
        
        private TransformMotion _playerMotion;
        
        private PopeyeAnchor _anchor;
        private IAnchorThrower _anchorThrower;
        private IAnchorPuller _anchorPuller;
        
        
        public Vector3 Position => _playerController.Position;
        
        
        public void Configure(PlayerFSM stateMachine, PlayerController.PlayerController playerController,
            PlayerMovesetConfig playerMovesetConfig, 
            TimeStaminaSystem staminaSystem, 
            TransformMotion playerMotion,
            PopeyeAnchor anchor, IAnchorThrower anchorThrower, IAnchorPuller anchorPuller)
        {
            _stateMachine = stateMachine;
            _playerController = playerController;
            _playerMovesetConfig = playerMovesetConfig;
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
            SpendStamina(_playerMovesetConfig.AnchorPickUpStaminaCost);
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
            SpendStamina(_playerMovesetConfig.AnchorThrowStaminaCost);
        }

        public void PullAnchor()
        {
            _anchorPuller.PullAnchor();
            SpendStamina(_playerMovesetConfig.AnchorPullStaminaCost);
            LookTowardsAnchorForDuration(0.3f).Forget();
        }

        public void DashTowardsAnchor(float duration)
        {
            Vector3 up = Vector3.up;
            Vector3 toAnchor = Vector3.ProjectOnPlane((_anchor.Position - Position).normalized, up);
            Vector3 right = Vector3.Cross(toAnchor, up).normalized;

            Vector3 extraDisplacement = toAnchor * _playerMovesetConfig.DashExtraDisplacement.z;
            extraDisplacement += right * _playerMovesetConfig.DashExtraDisplacement.x;
            extraDisplacement += up * _playerMovesetConfig.DashExtraDisplacement.y;

            Vector3 dashEndPosition = _anchor.GetDashEndPosition() + extraDisplacement;

            if (_anchor.IsGrabbedBySnapper())
            {
                dashEndPosition += toAnchor * _playerMovesetConfig.SnapExtraDisplacement.z;
                dashEndPosition += right * _playerMovesetConfig.SnapExtraDisplacement.x;
                dashEndPosition += up * _playerMovesetConfig.SnapExtraDisplacement.y;
            }
            
            LookTowardsAnchorForDuration(duration).Forget();
            _playerMotion.MoveToPosition(dashEndPosition, duration, Ease.InOutQuad);
            SpendStamina(_playerMovesetConfig.AnchorDashStaminaCost);
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


        public void SetVulnerable()
        {
            // TODO
            Debug.Log("Invulnerable");
        }

        public void SetInvulnerable()
        {
            // TODO
            Debug.Log("Vulnerable");
        }

        
        public bool HasMaxStamina()
        {
            return _staminaSystem.HasMaxStamina();
        }
        private void SpendStamina(int spendAmount)
        {
            if (spendAmount == 0) return;
            
            _staminaSystem.Spend(spendAmount);
            if (!_staminaSystem.HasStaminaLeft())
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