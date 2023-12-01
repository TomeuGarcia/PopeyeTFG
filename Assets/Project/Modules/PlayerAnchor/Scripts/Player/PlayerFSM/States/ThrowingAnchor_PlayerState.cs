using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class ThrowingAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private bool _anchorThrowFinished;
        private bool _anchorThrowLandedOnVoid;

        public ThrowingAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.ThrowingAnchorMoveSpeed);
            _anchorThrowFinished = false;
            _anchorThrowLandedOnVoid = false;
            StartThrowAnchor().Forget();
            
            _blackboard.PlayerMediator.SetCanRotate(false);
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.SetCanRotate(true);
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
            _blackboard.AnchorThrower.ThrowAnchor();

            await UniTask.WaitUntil(() => !_blackboard.AnchorMediator.IsBeingThrown());

            _anchorThrowLandedOnVoid = _blackboard.AnchorThrower.GetLastAnchorThrowResult().EndsOnVoid;
            
            _anchorThrowFinished = true;
        }
    }
}