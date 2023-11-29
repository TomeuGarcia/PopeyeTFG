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
            _finishedDying = false;
            WaitForSpawnToFinish().Forget();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_finishedDying)
            {
                NextState = PlayerStates.Spawning;
                return true;
            }

            return false;
        }
        
        
        private async UniTaskVoid WaitForSpawnToFinish()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_blackboard.PlayerStatesConfig.BeforeRespawnDuration));
            _finishedDying = true;
        }
        
    }
}