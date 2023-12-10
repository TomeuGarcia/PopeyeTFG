using Cysharp.Threading.Tasks;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class Healing_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly PlayerStates _healEndNextState;
        private bool _finishedHealing;
        
        
        public Healing_PlayerState(PlayerStatesBlackboard blackboard, PlayerStates healEndNextState)
        {
            _blackboard = blackboard;
            _healEndNextState = healEndNextState;
        }
        
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.HealingMoveSpeed);
            StartHealing().Forget();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_finishedHealing)
            {
                NextState = _healEndNextState;
                return true;
            }

            return false;
        }

        private async UniTaskVoid StartHealing()
        {
            _finishedHealing = false;
            await _blackboard.PlayerMediator.UseHeal();
            _finishedHealing = true;
        }
    }
}