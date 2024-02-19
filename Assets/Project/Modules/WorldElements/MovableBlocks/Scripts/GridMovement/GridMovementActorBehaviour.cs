using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.WorldElements.MovableBlocks.GridMovement
{
    public class GridMovementActorBehaviour : MonoBehaviour, IGridMovementActor
    {
        public class MovementStep
        {
            public Vector2 Direction { get; private set; }
            public Vector2 MoveDisplacement { get; private set; }
            public Vector3 MoveWorldDisplacement { get; private set; }
            
            public MovementStep(Vector2 direction, float moveAmount)
            {
                Direction = direction;
                MoveDisplacement = Direction * moveAmount;
                MoveWorldDisplacement = new Vector3(MoveDisplacement.x, 0, MoveDisplacement.y);
            }
        }
        
        [Required("Assign - ScriptableObject")] 
        [Expandable] [SerializeField] private GridMovementActorConfig _gridMovementActorConfig;
        [SerializeField] private RectangularAreaWrapper _rectangularAreaWrapper;

        private GridMovementArea _associatedMovementArea;
        private Queue<MovementStep> _queuedMoves;
        private bool _processingQueuedMoves;
        
        
        public bool IsMoving { get; private set; }

        public RectangularArea RectangularArea => _rectangularAreaWrapper.RectangularArea;
        public Rect AreaBounds => _rectangularAreaWrapper.RectangularArea.AreaBounds;
        private int MoveAmount => _gridMovementActorConfig.MoveAmount;
        private float MoveDuration => _gridMovementActorConfig.MoveDuration;
        private Ease MoveEase => _gridMovementActorConfig.MoveEase;
        private float DelayBetweenMoves => _gridMovementActorConfig.DelayBetweenMoves;


        public delegate void MovementActorEvent(MovementStep movementStep);

        public MovementActorEvent OnMoveStarted;
        public MovementActorEvent OnMoveFinished;
        public MovementActorEvent OnMoveFailed;


        private void OnValidate()
        {
            _rectangularAreaWrapper?.OnValidateUpdateState();
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _rectangularAreaWrapper?.DrawGizmos();
        }
#endif
        
        [Button("Reset Area")]
        private void ResetArea()
        {
            RectangularArea rectangularArea = new RectangularArea(transform);
            _rectangularAreaWrapper = new RectangularAreaWrapper(rectangularArea);
        }


        public void Configure(GridMovementArea associatedMovementArea)
        {
            _associatedMovementArea = associatedMovementArea;
            _queuedMoves = new Queue<MovementStep>();
            RectangularArea.UpdateState();
        }


        public void QueueMove(Vector2 direction)
        {
            _queuedMoves.Enqueue(new MovementStep(direction, MoveAmount));
            
            if (!_processingQueuedMoves)
            {
                ProcessQueuedMoves().Forget();                
            }
        }

        private async UniTaskVoid ProcessQueuedMoves()
        {
            _processingQueuedMoves = true;
            

            while (_queuedMoves.Count > 0)
            {
                MovementStep movementStep = _queuedMoves.Dequeue();

                if (CanMove(movementStep))
                {
                    await Move(movementStep);
                    OnMoved(movementStep);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(DelayBetweenMoves));
                }
                else
                {
                    MoveFailed(movementStep);
                }
            }

            _processingQueuedMoves = false;
        }

        private bool CanMove(MovementStep movementStep)
        {
            return _associatedMovementArea.CanMoveAfterDisplacement(this, movementStep.MoveDisplacement);
        }
        
        private async UniTask Move(MovementStep movementStep)
        {
            IsMoving = true;
            OnMoveStarted?.Invoke(movementStep);
            
            Vector3 endPosition = transform.position + movementStep.MoveWorldDisplacement;

            await transform.DOMove(endPosition, MoveDuration)
                .SetEase(MoveEase)
                .AsyncWaitForCompletion();

            IsMoving = false;
        }
        
        private void OnMoved(MovementStep movementStep)
        {
            _rectangularAreaWrapper.RectangularArea.UpdateState();
            OnMoveFinished?.Invoke(movementStep);
        }
        private void MoveFailed(MovementStep movementStep)
        {
            OnMoveFailed?.Invoke(movementStep);
        }
    }
}