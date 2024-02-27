using System.Collections.Generic;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class DefaultPlayerStatesCreator : IPlayerStatesCreator
    {
        public PlayerStates StartState => PlayerStates.Spawning;
        
        public Dictionary<PlayerStates, APlayerState> CreateStatesDictionary(PlayerStatesBlackboard blackboard)
        {
            Spawning_PlayerState spawningState 
                = new Spawning_PlayerState(blackboard);
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
            SpinningAnchor_PlayerState spinningAnchor 
                = new SpinningAnchor_PlayerState(blackboard);
            
            Tired_PlayerState tired
                = new Tired_PlayerState(blackboard);
            TiredPickingUpAnchor_PlayerState tiredPickingUpAnchor
                = new TiredPickingUpAnchor_PlayerState(blackboard);

            Healing_PlayerState healing
                = new Healing_PlayerState(blackboard);

            FallingOnVoid_PlayerState fallingOnVoid
                = new FallingOnVoid_PlayerState(blackboard);
            
            
            
            Dictionary<PlayerStates, APlayerState> states = new Dictionary<PlayerStates, APlayerState>()
            {
                { PlayerStates.Spawning , spawningState },
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
                { PlayerStates.SpinningAnchor , spinningAnchor },
                
                { PlayerStates.Tired , tired },
                { PlayerStates.TiredPickingUpAnchor , tiredPickingUpAnchor },
                
                { PlayerStates.Healing , healing },
                { PlayerStates.FallingOnVoid , fallingOnVoid },
            };

            return states;
        }
    }
}