using System;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PopeyePlayer : MonoBehaviour, IPlayerMediator
    {
        private PlayerFSM _stateMachine;
        private PlayerController.PlayerController _playerController;
        
        
        public void Configure(PlayerFSM stateMachine, PlayerController.PlayerController playerController)
        {
            _stateMachine = stateMachine;
            _playerController = playerController;
        }


        private void Update()
        {
            _stateMachine.Update(Time.deltaTime);
        }

        public void SetMaxMovementSpeed(float maxMovementSpeed)
        {
            _playerController.MaxSpeed = maxMovementSpeed;
        }
    }
}