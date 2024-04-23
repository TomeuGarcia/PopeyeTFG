using System;
using AYellowpaper;
using NaughtyAttributes;
using Popeye.Modules.WorldElements.MovableBlocks.GridMovement;
using UnityEngine;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    [RequireComponent(typeof(GridMovementActorBehaviour))]
    public class PullableBlock : MonoBehaviour, IPullableBlock
    {
        [Header("HANDLES")]
        [SerializeField] private InterfaceReference<IPullableBlockPullHandle, MonoBehaviour>[] _handles;
        private GridMovementActorBehaviour _gridMovementActorBehaviour;


        [Header("CONFIG")] 
        [Expandable] [SerializeField] private PullableBlockConfig _config;
        private PullableBlockView _pullableBlockView;

        
        public bool IsMoving => _gridMovementActorBehaviour.IsMoving;
        
        private void Awake()
        {
            _gridMovementActorBehaviour = GetComponent<GridMovementActorBehaviour>();
            
            for (int i = 0; i < _handles.Length; ++i)
            {
                _handles[i].Value.Configure(this);
            }

            _pullableBlockView = new PullableBlockView(_config.ViewConfig);
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

        public void TryPullTowardsDirectionUntilEnd(Vector2 pullDirection)
        {
            _gridMovementActorBehaviour.QueueMoveUntilEnd(pullDirection);
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