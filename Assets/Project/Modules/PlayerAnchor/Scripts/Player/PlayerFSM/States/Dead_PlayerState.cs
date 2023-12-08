using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class Dead_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        
        private bool _finishedDying;

        public Dead_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        
        protected override void DoEnter()
        {
            WaitForSpawnToFinish().Forget();
            _blackboard.PlayerMediator.SetMaxMovementSpeed(0);
            _blackboard.PlayerMediator.SetCanRotate(false);
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.SetCanRotate(true);
        }

        public override bool Update(float deltaTime)
        {
            if (_finishedDying)
            {
                _blackboard.PlayerMediator.Respawn();
                NextState = PlayerStates.Spawning;
                return true;
            }

            return false;
        }
        
        
        private async UniTaskVoid WaitForSpawnToFinish()
        {
            _finishedDying = false;
            await UniTask.Delay(TimeSpan.FromSeconds(_blackboard.PlayerStatesConfig.BeforeRespawnDuration));
            _finishedDying = true;
        }
        
    }
}