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
            _blackboard.PlayerMediator.SetInstantRotation(true);
            _blackboard.PlayerMediator.SetCanUseRotateInput(true);
            _blackboard.PlayerMediator.SetCanFallOffLedges(false, false);
            
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetBreakOverTimeMode();
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetEnabled(true);

            _blackboard.PlayerMediator.PlayerView.PlayEnterAimingAnimation();
            
            StartChargingThrow();
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.SetInstantRotation(false);
            _blackboard.PlayerMediator.SetCanUseRotateInput(false);
            _blackboard.PlayerMediator.SetCanFallOffLedges(false, true);
            
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetEnabled(false);
            
            StopChargingThrow();
        }

        public override bool Update(float deltaTime)
        {
            _blackboard.PlayerMediator.UpdateSafeGroundChecking(deltaTime, out bool playerIsOnVoid, out bool anchorIsOnVoid);
            if (playerIsOnVoid)
            {
                CancelChargingThrow();
                _blackboard.PlayerMediator.OnPlayerFellOnVoid();
                NextState = PlayerStates.FallingOnVoid;
                return true;
            }
            
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