using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.WorldElements.MovableBlocks.GridMovement;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    public class PullableBlockView
    {
        public bool PlayingMoveFailedAnimation { get; private set; }

        
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
            await gridMovementActor.transform.DOShakePosition(0.3f, movementStep.MoveWorldDisplacement * 0.05f, 2)
                .AsyncWaitForCompletion();

            await UniTask.Delay(TimeSpan.FromSeconds(0.8f));

            PlayingMoveFailedAnimation = false;
        }
    }
}