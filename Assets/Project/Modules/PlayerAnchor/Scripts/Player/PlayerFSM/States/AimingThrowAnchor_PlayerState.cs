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
            _blackboard.queuedAnchorAim = false;
            
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.AimingMoveSpeed);
            _blackboard.PlayerMediator.SetCanUseRotateInput(true);
            _blackboard.PlayerMediator.SetCanFallOffLedges(false, false);
            
            StartChargingThrow();
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.SetCanUseRotateInput(false);
            _blackboard.PlayerMediator.SetCanFallOffLedges(false, true);
            
            StopChargingThrow();
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.MovesetInputsController.Aim_Released())
            {
                CancelChargingThrow();
                NextState = PlayerStates.MovingWithAnchor;
                return true;
            }
            
            if (_blackboard.MovesetInputsController.Throw_Pressed())
            {
                NextState = PlayerStates.ThrowingAnchor;
                return true;
            }
            
            if (_blackboard.MovesetInputsController.Aim_HeldPressed())
            {
                ChargeThrow(deltaTime);
            }
            
            
            return false;
        }

        
        private void StartChargingThrow()
        {
            _blackboard.PlayerMediator.StartChargingThrow();
        }
        
        private void ChargeThrow(float deltaTime)
        {
            _blackboard.PlayerMediator.ChargeThrow(deltaTime);
        }

        private void StopChargingThrow()
        {
            _blackboard.PlayerMediator.StopChargingThrow();
        }
        
        private void CancelChargingThrow()
        {
            _blackboard.PlayerMediator.CancelChargingThrow();
        }
        
    }
}