using System;
using AYellowpaper;
using Popeye.Modules.WorldElements.MovableBlocks.GridMovement;
using UnityEngine;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    [RequireComponent(typeof(GridMovementActorBehaviour))]
    public class PullableBlock : MonoBehaviour, IPullableBlock
    {
        [SerializeField] private GridMovementActorBehaviour _gridMovementActorBehaviour;
        [SerializeField] private InterfaceReference<IPullableBlockPullHandle, MonoBehaviour>[] _handles;

        private void Awake()
        {
            for (int i = 0; i < _handles.Length; ++i)
            {
                _handles[i].Value.Configure(this);
            }
        }

        private void OnEnable()
        {
            _gridMovementActorBehaviour.OnMoveFinished += OnMoveFinished;
            _gridMovementActorBehaviour.OnMoveFailed += OnMoveFailed;
        }
        private void OnDisable()
        {
            _gridMovementActorBehaviour.OnMoveFinished -= OnMoveFinished;
            _gridMovementActorBehaviour.OnMoveFailed -= OnMoveFailed;
        }

        public void TryPullTowardsDirection(Vector2 pullDirection)
        {
            _gridMovementActorBehaviour.QueueMove(pullDirection);
        }

        private void OnMoveFinished()
        {
            
        }
        private void OnMoveFailed()
        {
            
        }

        
    }
}