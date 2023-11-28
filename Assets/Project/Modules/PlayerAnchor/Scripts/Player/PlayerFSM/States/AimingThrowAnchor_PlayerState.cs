using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class AimingThrowAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly AimingThrowAnchor_PlayerStateConfig _config;

        private bool _heldThrowButtonLongEnough;
        
        public AimingThrowAnchor_PlayerState(PlayerStatesBlackboard blackboard, AimingThrowAnchor_PlayerStateConfig config)
        {
            _blackboard = blackboard;
            _config = config;
        }
        
        protected override void DoEnter()
        {
            _heldThrowButtonLongEnough = false;
            
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_config.MovementSpeed);
            _blackboard.AnchorMediator.ResetThrowForce();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.MovesetInputsController.Aim_Released())
            {
                CancelChargingThrow();
                NextState = PlayerStates.MovingWithAnchor;
                return true;
            }
            
            if (_blackboard.MovesetInputsController.Throw_HeldPressed())
            {
                _heldThrowButtonLongEnough = true;
                ChargeThrow(deltaTime);
            }
            else if (_blackboard.MovesetInputsController.Throw_Released())
            {
                if (_heldThrowButtonLongEnough)
                {
                    NextState = PlayerStates.ThrowingAnchor;
                }
                else
                {
                    CancelChargingThrow();
                    NextState = PlayerStates.MovingWithAnchor;
                }
                
                return true;
            }
            
            return false;
        }

        private void ChargeThrow(float deltaTime)
        {
            _blackboard.AnchorMediator.IncrementThrowForce(deltaTime);
        }
        
        private void CancelChargingThrow()
        {
            _blackboard.AnchorMediator.CancelChargingThrow();
        }
        
    }
}