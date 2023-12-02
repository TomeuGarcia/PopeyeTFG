using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using UnityEngine;

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
            _blackboard.PlayerMediator.SetCanRotate(true);
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
            _blackboard.PlayerMediator.PullAnchor();

            await UniTask.WaitUntil(() => !_blackboard.AnchorMediator.IsBeingPulled());
            _anchorPullingFinished = true;
        }
    }
}