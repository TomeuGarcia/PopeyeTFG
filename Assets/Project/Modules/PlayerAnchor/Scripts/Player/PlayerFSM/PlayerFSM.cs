using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using UnityEngine;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PlayerFSM
    {
        private APlayerState _currentState;
        private Dictionary<PlayerStates, APlayerState> _states;


        public void Setup(PlayerStateConfigHelper.ConfigurationsGroup stateConfigurations)
        {
            Spawning_PlayerState spawningState 
                = new Spawning_PlayerState(stateConfigurations.spawning);
            Dead_PlayerState deadState 
                = new Dead_PlayerState(stateConfigurations.dead);
            WithAnchor_PlayerState withAnchorState 
                = new WithAnchor_PlayerState(stateConfigurations.withAnchor);
            WithoutAnchor_PlayerState withoutAnchorState 
                = new WithoutAnchor_PlayerState(stateConfigurations.withoutAnchor);
            
            MovingWithAnchor_PlayerState movingWithAnchor 
                = new MovingWithAnchor_PlayerState(stateConfigurations.movingWithAnchor);
            AimingThrowAnchor_PlayerState aimingThrowAnchor 
                = new AimingThrowAnchor_PlayerState(stateConfigurations.aimingThrowAnchor);
            ThrowingAnchor_PlayerState throwingAnchor 
                = new ThrowingAnchor_PlayerState(stateConfigurations.throwingAnchor);

            MovingWithoutAnchor_PlayerState movingWithoutAnchor 
                = new MovingWithoutAnchor_PlayerState(stateConfigurations.movingWithoutAnchor);
            PickingUpAnchor_PlayerState pickingUpAnchor 
                = new PickingUpAnchor_PlayerState(stateConfigurations.pickingUpAnchor);
            DashingTowardsAnchor_PlayerState dashingTowardsAnchor 
                = new DashingTowardsAnchor_PlayerState(stateConfigurations.dashingTowardsAnchor);
            KickingAnchor_PlayerState kickingAnchor 
                = new KickingAnchor_PlayerState(stateConfigurations.kickingAnchor);
            PullingAnchor_PlayerState pullingAnchor 
                = new PullingAnchor_PlayerState(stateConfigurations.pullingAnchor);
            SpinningAnchor_PlayerState spinningAnchor 
                = new SpinningAnchor_PlayerState(stateConfigurations.spinningAnchor);
            
            
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
                _currentState.Exit();
                _currentState = _states[_currentState.NextState];
                _currentState.Enter();
            }
        }
        
    }
}


