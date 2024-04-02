using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class FallingOnVoid_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly Timer _recoverFromFallTimer;

        public FallingOnVoid_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
            _recoverFromFallTimer = new Timer(_blackboard.PlayerStatesConfig.FallingOnVoidDuration);
        }
        
        
        protected override void DoEnter()
        {
            _recoverFromFallTimer.Clear();
            
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.FallingOnVoidMoveSpeed);
            _blackboard.PlayerMediator.DropTargetForCamera();
            _blackboard.PlayerMediator.SetInvulnerable(true);
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            _recoverFromFallTimer.Update(deltaTime);
            if (_recoverFromFallTimer.HasFinished())
            {
                _blackboard.PlayerMediator.SetEnabledFallingPhysics(false);
                _blackboard.PlayerMediator.SetInvulnerable(false);

                bool diedAfterFall = _blackboard.PlayerMediator.TakeFellOnVoidDamage();

                if (diedAfterFall)
                {
                    return false;
                }
                
                _blackboard.PlayerMediator.ResetTargetForCamera();
                _blackboard.PlayerMediator.RespawnToLastSafeGround();
                _blackboard.PlayerMediator.SetInvulnerableForDuration(_blackboard.PlayerStatesConfig.InvulnerableTimeAfterVoidFallRespawn);
                
                if (_blackboard.CameFromState == PlayerStates.MovingWithAnchor)
                {
                    NextState = PlayerStates.MovingWithAnchor;
                }
                else
                {
                    NextState = PlayerStates.PickingUpAnchor;
                }

                return true;
            }

            return false;
        }
    }
}