using System;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerMovementChecker
    {
        private IPlayerMediator _player;
        private PlayerController.PlayerController _playerController;
        private bool _isMoving;
        private const float MOVEMENT_SPEED_THRESHOLD = 0.1f;

        public float MovementSpeedRatio { get; private set; }
    
        
        public void Configure(IPlayerMediator player, PlayerController.PlayerController playerController)
        {
            _player = player;
            _playerController = playerController;
            _isMoving = false;
        }

        public void Update()
        {
            UpdateCheckMovingState(_playerController.CurrentSpeedXZ);
        }

        private void UpdateCheckMovingState(float currentMoveSpeed)
        {
            if (!_isMoving && currentMoveSpeed > MOVEMENT_SPEED_THRESHOLD)
            {
                DoStartMoving();
            }
            else if (_isMoving && currentMoveSpeed < MOVEMENT_SPEED_THRESHOLD)
            {
                DoStopMoving();
            }

            MovementSpeedRatio = currentMoveSpeed / _playerController.MaxSpeed;
        }

        private void DoStartMoving()
        {
            _player.OnStartMoving();
            _isMoving = true;
        }
        private void DoStopMoving()
        {
            _player.OnStopMoving();
            _isMoving = false;
        }
        
        
    }
}