using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.WorldElements.MovableBlocks.GridMovement;
using Project.Scripts.TweenExtensions;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    public class PullableBlockView
    {
        private readonly PullableBlockViewConfig _config;
        public bool PlayingMoveFailedAnimation { get; private set; }


        public PullableBlockView(PullableBlockViewConfig config)
        {
            _config = config;
        }
        
        public void PlayMoveStartedAnimation()
        {
            
        }
        
        public void PlayMoveFinishedAnimation()
        {
            
        }
        
        public async UniTaskVoid PlayMoveFailedAnimation(GridMovementActorBehaviour gridMovementActor, 
            GridMovementActorBehaviour.MovementStep movementStep)
        {
            PlayingMoveFailedAnimation = true;
            await gridMovementActor.transform
                .DOShakePosition(
                    _config.MoveFailedPositionPunch.Duration, 
                    movementStep.MoveWorldDisplacement * 0.1f, 
                    _config.MoveFailedPositionPunch.Vibrato)
                .AsyncWaitForCompletion();

            await UniTask.Delay(TimeSpan.FromSeconds(_config.DelayAfterFailedMove));

            PlayingMoveFailedAnimation = false;
        }
    }
}