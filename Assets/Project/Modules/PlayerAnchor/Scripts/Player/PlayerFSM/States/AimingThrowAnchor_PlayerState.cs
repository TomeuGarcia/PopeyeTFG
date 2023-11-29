using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class AimingThrowAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        
        public AimingThrowAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.AimingMoveSpeed);
            _blackboard.AnchorThrower.ResetThrowForce();
            
            StartChargingThrow();
        }

        public override void Exit()
        {
            _blackboard.AnchorMediator.OnStopChargingThrow();
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
                ChargeThrow(deltaTime);
            }
            else if (_blackboard.MovesetInputsController.Throw_Released())
            {
                NextState = PlayerStates.ThrowingAnchor;
                return true;
            }
            
            return false;
        }

        
        private void StartChargingThrow()
        {
            _blackboard.PlayerMediator.AimAnchor();
            _blackboard.AnchorMediator.OnStartChargingThrow();
        }
        
        private void ChargeThrow(float deltaTime)
        {
            _blackboard.AnchorThrower.IncrementThrowForce(deltaTime);
            _blackboard.AnchorMediator.OnKeepChargingThrow();
        }
        
        private void CancelChargingThrow()
        {
            _blackboard.AnchorThrower.CancelChargingThrow();
            _blackboard.PlayerMediator.CarryAnchor();
        }
        
    }
}