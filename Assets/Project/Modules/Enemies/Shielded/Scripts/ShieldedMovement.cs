using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class ShieldedMovement : ANavMeshMovement
    {


        private bool _followPlayer = false;
        private AEnemyMediator _mediator;


        private new void Start()
        {
            _navMeshAgent.speed = _speed;
        }

        public void Configure(AEnemyMediator mediator)
        {
            _mediator = mediator;
        }

        void Update()
        {
            if (_followPlayer && _playerTransform != null && _navMeshAgent.isActiveAndEnabled)
            {
                SetDestination(_playerTransform.position);
            }
        }

        public void SetTarget(Transform transform)
        {
            _playerTransform = transform;
        }
        
        public void StopChasing()
        {
            _followPlayer = false;
        }

        public void StartChasing()
        {
            _followPlayer = true;
        }

    }
}
