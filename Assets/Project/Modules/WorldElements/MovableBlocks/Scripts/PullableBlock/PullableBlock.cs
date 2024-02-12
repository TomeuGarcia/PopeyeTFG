using System;
using AYellowpaper;
using DG.Tweening;
using Popeye.Modules.WorldElements.MovableBlocks.GridMovement;
using UnityEngine;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    [RequireComponent(typeof(GridMovementActorBehaviour))]
    public class PullableBlock : MonoBehaviour, IPullableBlock
    {
        [SerializeField] private GridMovementActorBehaviour _gridMovementActorBehaviour;
        [SerializeField] private InterfaceReference<IPullableBlockPullHandle, MonoBehaviour>[] _handles;
        private PullableBlockView _pullableBlockView;
        
        public bool IsMoving => _gridMovementActorBehaviour.IsMoving;
        
        private void Awake()
        {
            for (int i = 0; i < _handles.Length; ++i)
            {
                _handles[i].Value.Configure(this);
            }

            _pullableBlockView = new PullableBlockView();
        }

        private void OnEnable()
        {
            _gridMovementActorBehaviour.OnMoveStarted += OnMoveStarted;
            _gridMovementActorBehaviour.OnMoveFinished += OnMoveFinished;
            _gridMovementActorBehaviour.OnMoveFailed += OnMoveFailed;
        }
        private void OnDisable()
        {
            _gridMovementActorBehaviour.OnMoveStarted -= OnMoveStarted;
            _gridMovementActorBehaviour.OnMoveFinished -= OnMoveFinished;
            _gridMovementActorBehaviour.OnMoveFailed -= OnMoveFailed;
        }

        public void TryPullTowardsDirection(Vector2 pullDirection)
        {
            _gridMovementActorBehaviour.QueueMove(pullDirection);
        }

        private void OnMoveStarted(GridMovementActorBehaviour.MovementStep movementStep)
        {
            _pullableBlockView.PlayMoveStartedAnimation();
        }
        private void OnMoveFinished(GridMovementActorBehaviour.MovementStep movementStep)
        {
            _pullableBlockView.PlayMoveFinishedAnimation();
        }
        private void OnMoveFailed(GridMovementActorBehaviour.MovementStep movementStep)
        {
            if (!_pullableBlockView.PlayingMoveFailedAnimation)
            {
                _pullableBlockView.PlayMoveFailedAnimation(_gridMovementActorBehaviour, movementStep).Forget();
            }
        }

        
    }
}