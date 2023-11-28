using System;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Project.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PopeyePlayer : MonoBehaviour, IPlayerMediator
    {
        private PlayerFSM _stateMachine;
        private PlayerController.PlayerController _playerController;
        private PopeyeAnchor _anchor;
        
        
        public void Configure(PlayerFSM stateMachine, PlayerController.PlayerController playerController,
            PopeyeAnchor anchor)
        {
            _stateMachine = stateMachine;
            _playerController = playerController;
            _anchor = anchor;
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

        
        public void ThrowAnchor()
        {
            Vector3 throwDirection = _playerController.GetFloorAlignedLookDirection(); 
            _anchor.GetThrown(throwDirection);
        }
    }
}