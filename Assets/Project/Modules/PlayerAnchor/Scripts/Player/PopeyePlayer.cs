using System;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PopeyePlayer : MonoBehaviour, IPlayerMediator
    {
        private PlayerFSM _stateMachine;
        
        
        public void Configure(PlayerFSM stateMachine)
        {
            _stateMachine = stateMachine;
        }


        private void Update()
        {
            _stateMachine.Update(Time.deltaTime);
        }
    }
}