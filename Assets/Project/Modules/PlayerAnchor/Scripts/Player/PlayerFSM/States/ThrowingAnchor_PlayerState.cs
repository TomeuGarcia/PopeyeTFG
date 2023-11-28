using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class ThrowingAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly ThrowingAnchor_PlayerStateConfig _config;

        private bool _anchorThrowFinished;

        public ThrowingAnchor_PlayerState(PlayerStatesBlackboard blackboard, ThrowingAnchor_PlayerStateConfig config)
        {
            _blackboard = blackboard;
            _config = config;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_config.MovementSpeed);
            _anchorThrowFinished = false;
            StartThrowAnchor().Forget();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_anchorThrowFinished)
            {
                NextState = PlayerStates.WithoutAnchor;
                return true;
            }
            
            return false;
        }

        private async UniTaskVoid StartThrowAnchor()
        {
            _blackboard.PlayerMediator.ThrowAnchor();

            await UniTask.WaitUntil(() => !_blackboard.AnchorMediator.IsBeingThrown());
            
            _anchorThrowFinished = true;
        }
    }
}