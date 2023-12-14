using System;
using Cysharp.Threading.Tasks;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class DashingTowardsAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private bool _finishedDashing;

        public DashingTowardsAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.queuedDashTowardsAnchor = false;
            
            StartDashing().Forget();
        }

        public override void Exit()
        {
        }

        public override bool Update(float deltaTime)
        {
            if (_finishedDashing)
            {
                NextState = PlayerStates.PickingUpAnchor;
                return true;
            }

            return false;
        }

        private async UniTaskVoid StartDashing()
        {
            _finishedDashing = false;

            float duration = _blackboard.PlayerMediator.GetDistanceFromAnchorRatio01() *
                             _blackboard.PlayerStatesConfig.DashDuration;
            _blackboard.PlayerMediator.DashTowardsAnchor(duration);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            _finishedDashing = true;
        }
    }
}