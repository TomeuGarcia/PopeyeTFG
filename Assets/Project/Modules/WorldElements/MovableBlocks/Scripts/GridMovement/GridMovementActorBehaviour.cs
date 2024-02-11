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
        private class MovementStep
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
        
        [Expandable] [SerializeField] private GridMovementActorConfig _gridMovementActorConfig;
        [SerializeField] private RectangularAreaWrapper _rectangularAreaWrapper;

        private GridMovementArea _associatedMovementArea;
        private Queue<MovementStep> _queuedMoves;
        private bool _processingQueuedMoves;
            

        public RectangularArea RectangularArea => _rectangularAreaWrapper.RectangularArea;
        public Rect AreaBounds => _rectangularAreaWrapper.RectangularArea.AreaBounds;
        private int MoveAmount => _gridMovementActorConfig.MoveAmount;
        private float MoveDuration => _gridMovementActorConfig.MoveDuration;
        private Ease MoveEase => _gridMovementActorConfig.MoveEase;
        private float DelayBetweenMoves => _gridMovementActorConfig.DelayBetweenMoves;


        public delegate void MovementActorEvent();

        public MovementActorEvent OnMoveFinished;
        public MovementActorEvent OnMoveFailed;


        private void OnValidate()
        {
            _rectangularAreaWrapper?.OnValidateUpdateState();
        }
        private void OnDrawGizmos()
        {
            _rectangularAreaWrapper?.DrawGizmos();
        }
        
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
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                QueueMove(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                QueueMove(Vector2.right);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                QueueMove(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                QueueMove(Vector2.down);
            }
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
                    OnMoved();
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(DelayBetweenMoves));
                }
                else
                {
                    MoveFailed();
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
            Vector3 endPosition = transform.position + movementStep.MoveWorldDisplacement;

            await transform.DOMove(endPosition, MoveDuration)
                .SetEase(MoveEase)
                .AsyncWaitForCompletion();
        }
        
        private void OnMoved()
        {
            _rectangularAreaWrapper.RectangularArea.UpdateState();
            OnMoveFinished?.Invoke();
        }
        private void MoveFailed()
        {
            OnMoveFailed?.Invoke();
        }
    }
}