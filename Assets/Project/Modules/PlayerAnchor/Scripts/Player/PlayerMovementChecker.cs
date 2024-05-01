using System;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerMovementChecker
    {
        private IPlayerMediator _player;
        private PlayerController.PlayerController _playerController;
        private float _maxMovementSpeed;
        private const float MOVEMENT_SPEED_THRESHOLD = 0.1f;

        public bool IsMoving { get; private set; }
        public float MovementSpeedRatio { get; private set; }
        public float MaxMovementSpeed
        {
            get => _maxMovementSpeed; 
            set => _maxMovementSpeed = Mathf.Max(0.01f, value); 
        }
        
        
        public void Configure(IPlayerMediator player, PlayerController.PlayerController playerController)
        {
            _player = player;
            _playerController = playerController;
            IsMoving = false;
            MaxMovementSpeed = 0.0f;
        }

        public void Update()
        {
            UpdateCheckMovingState(_playerController.CurrentSpeedXZ);
        }

        private void UpdateCheckMovingState(float currentMoveSpeed)
        {
            if (!IsMoving && currentMoveSpeed > MOVEMENT_SPEED_THRESHOLD)
            {
                DoStartMoving();
            }
            else if (IsMoving && currentMoveSpeed < MOVEMENT_SPEED_THRESHOLD)
            {
                DoStopMoving();
            }

            MovementSpeedRatio = currentMoveSpeed / MaxMovementSpeed;
        }

        private void DoStartMoving()
        {
            _player.OnStartMoving();
            IsMoving = true;
        }
        private void DoStopMoving()
        {
            _player.OnStopMoving();
            IsMoving = false;
        }
        
        
    }
}