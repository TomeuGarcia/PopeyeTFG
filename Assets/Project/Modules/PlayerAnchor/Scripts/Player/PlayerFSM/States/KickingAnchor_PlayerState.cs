using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class KickingAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private bool _finishedKickingAnchor;
        
        public KickingAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.KickingAnchorMoveSpeed);
            StartKickingAnchor().Forget();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_finishedKickingAnchor)
            {
                NextState = PlayerStates.MovingWithoutAnchor;
                return true;
            }

            return false;
        }

        private async UniTaskVoid StartKickingAnchor()
        {
            _finishedKickingAnchor = false;
            
            _blackboard.PlayerMediator.KickAnchor();
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            
            _finishedKickingAnchor = true;
        }
        
    }
}