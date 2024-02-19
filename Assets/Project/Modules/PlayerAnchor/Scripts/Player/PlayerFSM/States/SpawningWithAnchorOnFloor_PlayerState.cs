using System;
using Cysharp.Threading.Tasks;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class SpawningWithAnchorOnFloor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private bool _finishedSpawning;

        public SpawningWithAnchorOnFloor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        
        
        protected override void DoEnter()
        {
            WaitForSpawnToFinish().Forget();
            _blackboard.PlayerMediator.SetMaxMovementSpeed(0);
            _blackboard.PlayerMediator.SetCanRotate(false);
            _blackboard.PlayerMediator.ResetTargetForCamera();
            
            _blackboard.AnchorMediator.SnapToFloor(_blackboard.PlayerMediator.Position).Forget();
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.SetCanRotate(true);
        }

        public override bool Update(float deltaTime)
        {
            if (_finishedSpawning)
            {
                NextState = PlayerStates.MovingWithoutAnchor;
                return true; 
            }

            return false;
        }


        private async UniTaskVoid WaitForSpawnToFinish()
        {
            _finishedSpawning = false;
            await UniTask.Delay(TimeSpan.FromSeconds(_blackboard.PlayerStatesConfig.SpawnDuration));
            _finishedSpawning = true;
        }
        
    }
}