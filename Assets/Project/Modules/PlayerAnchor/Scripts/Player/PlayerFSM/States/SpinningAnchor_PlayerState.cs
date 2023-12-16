using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class SpinningAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        public SpinningAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.StartSpinningAnchor(_blackboard.cameFromState == PlayerStates.MovingWithAnchor);
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.StopSpinningAnchor();
        }

        public override bool Update(float deltaTime)
        {
            if (StillSpinning())
            {
                _blackboard.PlayerMediator.SpinAnchor(deltaTime);
            }
            else
            {
                NextState = PlayerStates.MovingWithoutAnchor;
                return true;
            }

            return false;
        }

        private bool StillSpinning()
        {
            return _blackboard.MovesetInputsController.SpinAttack_HeldPressed();
        }
    }
}