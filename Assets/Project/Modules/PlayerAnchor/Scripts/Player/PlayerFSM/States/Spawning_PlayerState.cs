using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class Spawning_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private bool _finishedSpawning;

        public Spawning_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        
        
        protected override void DoEnter()
        {
            _finishedSpawning = false;
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
            if (_finishedSpawning)
            {
                NextState = PlayerStates.WithAnchor;
                return true; 
            }

            return false;
        }


        private async UniTaskVoid WaitForSpawnToFinish()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_blackboard.PlayerStatesConfig.SpawnDuration));
            _finishedSpawning = true;
        }
        
    }
}