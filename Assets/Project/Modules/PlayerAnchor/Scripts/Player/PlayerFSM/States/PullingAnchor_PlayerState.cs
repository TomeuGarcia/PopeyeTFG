using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PullingAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private bool _anchorPullingFinished;
        
        
        public PullingAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _anchorPullingFinished = false;
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.PullingAnchorMoveSpeed);
            StartPulling().Forget();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_anchorPullingFinished)
            {
                NextState = PlayerStates.WithAnchor;
                return true;
            }
            
            return false;
        }

        private async UniTaskVoid StartPulling()
        {
            _blackboard.AnchorPuller.PullAnchor();

            await UniTask.WaitUntil(() => !_blackboard.AnchorMediator.IsBeingThrown());
            
            _anchorPullingFinished = true;
        }
    }
}