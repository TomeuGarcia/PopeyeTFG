using System;
using Cysharp.Threading.Tasks;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class FallingOnVoid_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly Timer _recoverFromFallTimer;
        private bool _hasFinishedFalling;

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
            //_blackboard.PlayerMediator.SetInvulnerable(true);
            
            _hasFinishedFalling = false;
            UpdateFallTimer().Forget();
        }

        public override void Exit()
        {
            _hasFinishedFalling = true;
        }

        public override bool Update(float deltaTime)
        {
            if (_hasFinishedFalling)
            {                
                _blackboard.PlayerMediator.SetEnabledFallingPhysics(true);    
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

        private async UniTaskVoid UpdateFallTimer()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_blackboard.PlayerStatesConfig.FallingOnVoidDuration));
            if (_hasFinishedFalling) return;
            
            _blackboard.PlayerMediator.SetEnabledFallingPhysics(false);
            //_blackboard.PlayerMediator.SetInvulnerable(false);

            _blackboard.PlayerMediator.ResetTargetForCamera();
            _blackboard.PlayerMediator.RespawnToLastSafeGround();
            //_blackboard.PlayerMediator.SetInvulnerableForDuration(_blackboard.PlayerStatesConfig.InvulnerableTimeAfterVoidFallRespawn);
            
            await UniTask.Yield();
            _blackboard.PlayerMediator.SetEnabledFallingPhysics(true);    
            
            _hasFinishedFalling = true;        
        }
    }
}
