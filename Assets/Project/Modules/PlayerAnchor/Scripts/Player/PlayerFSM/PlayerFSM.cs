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


        public void Setup(PlayerStatesBlackboard blackboard)
        {
            Spawning_PlayerState spawningState 
                = new Spawning_PlayerState(blackboard);
            Dead_PlayerState deadState 
                = new Dead_PlayerState(blackboard);
            WithAnchor_PlayerState withAnchorState 
                = new WithAnchor_PlayerState(blackboard);
            WithoutAnchor_PlayerState withoutAnchorState 
                = new WithoutAnchor_PlayerState(blackboard);
            
            MovingWithAnchor_PlayerState movingWithAnchor 
                = new MovingWithAnchor_PlayerState(blackboard);
            AimingThrowAnchor_PlayerState aimingThrowAnchor 
                = new AimingThrowAnchor_PlayerState(blackboard);
            ThrowingAnchor_PlayerState throwingAnchor 
                = new ThrowingAnchor_PlayerState(blackboard);

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


