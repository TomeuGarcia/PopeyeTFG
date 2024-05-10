using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PerformingSpecialAttack_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        public PerformingSpecialAttack_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }

        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.PerformingSpecialAttackMoveSpeed);
        }

        public override void Exit()
        {
            if (!_blackboard.PlayerMediator.SpecialAttackHasFinished())
            {
                _blackboard.PlayerMediator.ForceStopSpecialAttack();
            }

            _blackboard.PlayerMediator.OnSpecialAttackPerformFinished();
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.PlayerMediator.SpecialAttackHasFinished())
            {
                NextState = PlayerStates.MovingWithoutAnchor;
                Debug.Log("FINISH");
                
                return true;
            }

            return false;
        }
    }
}