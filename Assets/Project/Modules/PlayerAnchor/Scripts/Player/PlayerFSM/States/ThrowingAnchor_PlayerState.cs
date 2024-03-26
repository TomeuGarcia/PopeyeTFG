using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class ThrowingAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private bool _anchorThrowFinished;

        public ThrowingAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.ThrowingAnchorMoveSpeed);
            _anchorThrowFinished = false;
            StartThrowAnchor().Forget();
            
            _blackboard.PlayerMediator.SetCanRotate(false);
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.SetCanRotate(true);
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.MovesetInputsController.Dash_Pressed())
            {
                _blackboard.QueuedDashTowardsAnchor = true;
            }
            if (_blackboard.MovesetInputsController.Pull_Pressed())
            {
                _blackboard.QueuedAnchorPull = true;
            }
            
            if (_anchorThrowFinished)
            {
                NextState = PlayerStates.MovingWithoutAnchor;
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