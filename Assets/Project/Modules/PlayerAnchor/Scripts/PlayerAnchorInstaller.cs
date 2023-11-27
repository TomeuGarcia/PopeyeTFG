using System;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.PlayerController;
using Popeye.Modules.PlayerController.Inputs;
using UnityEngine;

namespace Project.Modules.PlayerAnchor
{
    public class PlayerAnchorInstaller : MonoBehaviour
    {
        [Header("PLAYER")]
        [SerializeField] private PopeyePlayer _player;
        [SerializeField] private PlayerController _playerController;

        [Header("CONFIGURATION - States")] 
        [SerializeField] private PlayerStateConfigHelper.ConfigurationsGroup _playerStatesConfigurations;

        private void Awake()
        {
            Install();
        }

        private void Install()
        {
            PlayerStatesBlackboard playerStatesBlackboard =
                new PlayerStatesBlackboard(_player, new PlayerAnchorMovesetInputsController());

            PlayerFSM playerStateMachine = new PlayerFSM();
            playerStateMachine.Setup(_playerStatesConfigurations, playerStatesBlackboard);

            _player.Configure(playerStateMachine, _playerController);
        }
    }
}