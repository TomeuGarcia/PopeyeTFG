using System;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.PlayerController;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Modules.Camera;
using Project.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Project.Modules.PlayerAnchor
{
    public class PlayerAnchorInstaller : MonoBehaviour
    {
        [Header("CAMERA")] 
        [SerializeField] private OrbitingCamera _isometricCamera;
        
        
        [Space(20)]
        [Header("PLAYER")]
        [SerializeField] private PopeyePlayer _player;
        [SerializeField] private PlayerController _playerController;

        [Header("Player States")] 
        [SerializeField] private PlayerStateConfigHelper.ConfigurationsGroup _playerStatesConfigurations;

        
        [Space(20)] 
        [Header("ANCHOR")] 
        [SerializeField] private PopeyeAnchor _anchor;
        [SerializeField] private AnchorThrowConfig _anchorThrowConfig;
        [SerializeField] private Transform _anchorMoveTransform;
        

        private void Awake()
        {
            Install();
        }

        private void Install()
        {
            // Player FSM
            PlayerStatesBlackboard playerStatesBlackboard =
                new PlayerStatesBlackboard(_player, new PlayerAnchorMovesetInputsController(), _anchor);
            PlayerFSM playerStateMachine = new PlayerFSM();
            playerStateMachine.Setup(_playerStatesConfigurations, playerStatesBlackboard);

            // PlayerController
            _playerController.MovementInputHandler = new CameraAxisMovementInput(_isometricCamera.CameraTransform);
            
            // Player
            _player.Configure(playerStateMachine, _playerController, _anchor);
            
            
            // Anchor
            AnchorMotion anchorMotion = new AnchorMotion(_anchorMoveTransform);
            AnchorThrowTrajectory anchorThrowTrajectory = new AnchorThrowTrajectory(_anchorThrowConfig, anchorMotion);
            _anchor.Configure(anchorThrowTrajectory);
        }
    }
}