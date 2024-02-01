using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class Tired_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        
        public Tired_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }


        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.TiredMoveSpeed);
            _blackboard.PlayerMediator.SetCanRotate(true);
            _blackboard.PlayerView.StartTired();
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.SetCanRotate(true);
            _blackboard.PlayerView.EndTired();
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.PlayerMediator.HasMaxStamina())
            {
                NextState = PlayerStates.MovingWithoutAnchor;
                return true;
            }

            return false;
        }
    }
}