using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerController.Inputs;
using UnityEngine;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PlayerFSM
    {
        private APlayerState _currentState;
        private Dictionary<PlayerStates, APlayerState> _states;


        public void Setup(PlayerStateConfigHelper.ConfigurationsGroup stateConfigurations,
                          PlayerStatesBlackboard blackboard)
        {
            Spawning_PlayerState spawningState 
                = new Spawning_PlayerState(blackboard, stateConfigurations.spawning);
            Dead_PlayerState deadState 
                = new Dead_PlayerState(blackboard, stateConfigurations.dead);
            WithAnchor_PlayerState withAnchorState 
                = new WithAnchor_PlayerState(blackboard, stateConfigurations.withAnchor);
            WithoutAnchor_PlayerState withoutAnchorState 
                = new WithoutAnchor_PlayerState(blackboard, stateConfigurations.withoutAnchor);
            
            MovingWithAnchor_PlayerState movingWithAnchor 
                = new MovingWithAnchor_PlayerState(blackboard, stateConfigurations.movingWithAnchor);
            AimingThrowAnchor_PlayerState aimingThrowAnchor 
                = new AimingThrowAnchor_PlayerState(blackboard, stateConfigurations.aimingThrowAnchor);
            ThrowingAnchor_PlayerState throwingAnchor 
                = new ThrowingAnchor_PlayerState(blackboard, stateConfigurations.throwingAnchor);

            MovingWithoutAnchor_PlayerState movingWithoutAnchor 
                = new MovingWithoutAnchor_PlayerState(blackboard, stateConfigurations.movingWithoutAnchor);
            PickingUpAnchor_PlayerState pickingUpAnchor 
                = new PickingUpAnchor_PlayerState(blackboard, stateConfigurations.pickingUpAnchor);
            DashingTowardsAnchor_PlayerState dashingTowardsAnchor 
                = new DashingTowardsAnchor_PlayerState(blackboard, stateConfigurations.dashingTowardsAnchor);
            KickingAnchor_PlayerState kickingAnchor 
                = new KickingAnchor_PlayerState(blackboard, stateConfigurations.kickingAnchor);
            PullingAnchor_PlayerState pullingAnchor 
                = new PullingAnchor_PlayerState(blackboard, stateConfigurations.pullingAnchor);
            SpinningAnchor_PlayerState spinningAnchor 
                = new SpinningAnchor_PlayerState(blackboard, stateConfigurations.spinningAnchor);
            
            
            withAnchorState.Setup(movingWithAnchor, aimingThrowAnchor, throwingAnchor);
            withoutAnchorState.Setup(movingWithoutAnchor, pickingUpAnchor, dashingTowardsAnchor, kickingAnchor, 
                pullingAnchor, spinningAnchor);

            _states = new Dictionary<PlayerStates, APlayerState>()
            {
                { PlayerStates.Spawning , spawningState },
                { PlayerStates.Dead , deadState },
                { PlayerStates.WithAnchor , withAnchorState },
                { PlayerStates.WithoutAnchor , withoutAnchorState }
            };
            
            
            
            _currentState = _states[PlayerStates.Spawning];
            _currentState.Enter();
        }

        public void Update(float deltaTime)
        {
            if (_currentState.Update(deltaTime))
            {
                OverwriteState(_currentState.NextState);
            }
        }

        public void OverwriteState(PlayerStates newState)
        {
            _currentState.Exit();
            _currentState = _states[newState];
            _currentState.Enter();
        }
        
    }
}


