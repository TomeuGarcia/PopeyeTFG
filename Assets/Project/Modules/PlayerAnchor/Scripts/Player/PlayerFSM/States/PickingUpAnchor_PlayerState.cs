using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PickingUpAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        public PickingUpAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.PlayerView.PlayPickUpAnchorAnimation();
            StartPickingUpAnchor();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            NextState = PlayerStates.MovingWithAnchor;
            return true;
        }

        private void StartPickingUpAnchor()
        {
            _blackboard.PlayerMediator.PickUpAnchor();
        }
    }
}