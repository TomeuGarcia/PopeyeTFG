using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class SpinningAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private bool _stoppingSpinning;
        private bool _obstacleHit;
        
        public SpinningAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.SpinningAnchorMoveSpeed);
            _blackboard.PlayerMediator.SetCanRotate(false);

            _blackboard.PlayerMediator.StartSpinningAnchor(_blackboard.cameFromState == PlayerStates.MovingWithAnchor,
                _blackboard.spinAttackTowardsRight);

            _blackboard.spinAttackTowardsRight = false;
            _stoppingSpinning = false;
            _obstacleHit = false;
            
            _blackboard.AnchorMediator.SubscribeToOnObstacleHit(OnAnchorHitObstacle);
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.SetCanRotate(true);

            if (!_obstacleHit)
            {
                _blackboard.AnchorMediator.UnsubscribeToOnObstacleHit(OnAnchorHitObstacle);
            }
        }

        public override bool Update(float deltaTime)
        {
            if (PlayerStillSpinning())
            {
                _blackboard.PlayerMediator.SpinAnchor(deltaTime);
            }
            else if (!_stoppingSpinning && PlayerStoppingSpinning())
            {
                _blackboard.PlayerMediator.StopSpinningAnchor();
                _stoppingSpinning = true;
            }

            if (_blackboard.PlayerMediator.SpinningAnchorFinished())
            {
                NextState = PlayerStates.MovingWithoutAnchor;
                return true;
            }

            return false;
        }

        private bool PlayerStillSpinning()
        {
            return _blackboard.MovesetInputsController.SpinAttack_HeldPressed() ||
                   _blackboard.PlayerMediator.IsLockedIntoSpinningAnchor();
        }
        private bool PlayerStoppingSpinning()
        {
            return _blackboard.MovesetInputsController.SpinAttack_Released();
        }

        private void OnAnchorHitObstacle(Collider obstacle)
        {
            if (!_obstacleHit)
            {
                _blackboard.AnchorMediator.UnsubscribeToOnObstacleHit(OnAnchorHitObstacle);
                _obstacleHit = true;
            }
            
            _blackboard.PlayerMediator.InterruptSpinningAnchor();
        }
        
    }
}