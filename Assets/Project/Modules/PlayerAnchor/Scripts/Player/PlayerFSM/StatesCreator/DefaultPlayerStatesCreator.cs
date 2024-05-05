using System.Collections.Generic;
using Popeye.Modules.PlayerController.Inputs;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class DefaultPlayerStatesCreator : IPlayerStatesCreator
    {
        public PlayerStates StartState => PlayerStates.Spawning;
        
        public Dictionary<PlayerStates, APlayerState> CreateStatesDictionary(PlayerStatesBlackboard blackboard)
        {
            Spawning_PlayerState spawningState 
                = new Spawning_PlayerState(blackboard);
            SpawningWithAnchorOnFloor_PlayerState spawningWithAnchorOnFloorState 
                = new SpawningWithAnchorOnFloor_PlayerState(blackboard);
            Dead_PlayerState deadState 
                = new Dead_PlayerState(blackboard);

            MovingWithAnchor_PlayerState movingWithAnchor 
                = new MovingWithAnchor_PlayerState(blackboard);
            AimingThrowAnchor_PlayerState aimingThrowAnchor 
                = new AimingThrowAnchor_PlayerState(blackboard);
            ThrowingAnchor_PlayerState throwingAnchor 
                = new ThrowingAnchor_PlayerState(blackboard);
            DashingDroppingAnchor_PlayerState dashingDroppingAnchor
                = new DashingDroppingAnchor_PlayerState(blackboard);

            MovingWithoutAnchor_PlayerState movingWithoutAnchor 
                = new MovingWithoutAnchor_PlayerState(blackboard);
            PickingUpAnchor_PlayerState pickingUpAnchor 
                = new PickingUpAnchor_PlayerState(blackboard);
            DashingTowardsAnchor_PlayerState dashingTowardsAnchor 
                = new DashingTowardsAnchor_PlayerState(blackboard);
            KickingAnchor_PlayerState kickingAnchor 
                = new KickingAnchor_PlayerState(blackboard);
            PullingAnchor_PlayerState pullingAnchor 
                = new PullingAnchor_PlayerState(blackboard);

            Tired_PlayerState tired
                = new Tired_PlayerState(blackboard);
            TiredPickingUpAnchor_PlayerState tiredPickingUpAnchor
                = new TiredPickingUpAnchor_PlayerState(blackboard);

            Healing_PlayerState healing
                = new Healing_PlayerState(blackboard);


            SpecialAttackInput anchorSpinAttackInput = new SpecialAttackInput(
                blackboard.MovesetInputsController.SpinAttack_Pressed,
                blackboard.MovesetInputsController.SpinAttack_HeldPressed,
                blackboard.MovesetInputsController.SpinAttack_Released
            );
            EnteringSpecialAttack_PlayerState enteringAnchorSpinAttack
                = new EnteringSpecialAttack_PlayerState(blackboard, blackboard.AnchorSpinAttackController,
                    anchorSpinAttackInput, PlayerStates.PerformingAnchorSpinAttack);
            PerformingSpecialAttack_PlayerState performingAnchorSpinAttack
                = new PerformingSpecialAttack_PlayerState(blackboard);
            
            
            SpecialAttackInput chainSpikesAttackInput = new SpecialAttackInput(
                blackboard.MovesetInputsController.SpikesAttack_Pressed,
                blackboard.MovesetInputsController.SpikesAttack_HeldPressed,
                blackboard.MovesetInputsController.SpikesAttack_Released
            );
            EnteringSpecialAttack_PlayerState enteringChainSpikesAttack
                = new EnteringSpecialAttack_PlayerState(blackboard, blackboard.ChainSpikesAttackController,
                    chainSpikesAttackInput, PlayerStates.PerformingChainSpikesAttack);
            PerformingSpecialAttack_PlayerState performingChainSpikesAttack
                = new PerformingSpecialAttack_PlayerState(blackboard);

            
            
            
            
            FallingOnVoid_PlayerState fallingOnVoid
                = new FallingOnVoid_PlayerState(blackboard);
            
            
            
            Dictionary<PlayerStates, APlayerState> states = new Dictionary<PlayerStates, APlayerState>()
            {
                { PlayerStates.Spawning , spawningState },
                { PlayerStates.SpawningWithAnchorOnFloor, spawningWithAnchorOnFloorState },
                { PlayerStates.Dead , deadState },
                
                { PlayerStates.MovingWithAnchor , movingWithAnchor },
                { PlayerStates.AimingThrowAnchor , aimingThrowAnchor },
                { PlayerStates.ThrowingAnchor , throwingAnchor },
                { PlayerStates.DashingDroppingAnchor , dashingDroppingAnchor },
                
                { PlayerStates.MovingWithoutAnchor , movingWithoutAnchor },
                { PlayerStates.PickingUpAnchor , pickingUpAnchor },
                { PlayerStates.DashingTowardsAnchor , dashingTowardsAnchor },
                { PlayerStates.KickingAnchor , kickingAnchor },
                { PlayerStates.PullingAnchor , pullingAnchor },
                
                { PlayerStates.Tired , tired },
                { PlayerStates.TiredPickingUpAnchor , tiredPickingUpAnchor },
                
                { PlayerStates.Healing , healing },
                
                { PlayerStates.EnteringAnchorSpinAttack , enteringAnchorSpinAttack },
                { PlayerStates.PerformingAnchorSpinAttack , performingAnchorSpinAttack },
                
                { PlayerStates.EnteringChainSpikesAttack , enteringChainSpikesAttack },
                { PlayerStates.PerformingChainSpikesAttack , performingChainSpikesAttack },
                
                { PlayerStates.FallingOnVoid , fallingOnVoid },
            };

            return states;
        }
    }
}