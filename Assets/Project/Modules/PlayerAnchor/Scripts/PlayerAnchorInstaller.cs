using System;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using UnityEngine;

namespace Project.Modules.PlayerAnchor
{
    public class PlayerAnchorInstaller : MonoBehaviour
    {
        [Header("PLAYER")]
        [SerializeField] private PopeyePlayer _player;

        [Header("CONFIGURATION - States")] 
        [SerializeField] private PlayerStateConfigHelper.ConfigurationsGroup _playerStatesConfigurations;

        private void Awake()
        {
            Install();
        }

        private void Install()
        {

            PlayerFSM playerStateMachine = new PlayerFSM();
            playerStateMachine.Setup(_playerStatesConfigurations);

            _player.Configure(playerStateMachine);
        }
    }
}