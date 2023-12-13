using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;

namespace Popeye.Modules.Enemies.Components
{
    public class SlimeMovement : ANavMeshMovement
    {
        [SerializeField] private Vector2 _speeedThreshold = new Vector2(5, 7);
        [SerializeField] private int _spawnForce = 10;
        [SerializeField] private Rigidbody _rb;
        
        private bool _followPlayer = false;

        private AEnemyMediator _mediator;


        public void Configure(AEnemyMediator slimeMediator)
        {
            _mediator = slimeMediator;
        }

        private new void Start()
        {
            _speed = Random.Range(_speeedThreshold.x, _speeedThreshold.y);
            _navMeshAgent.speed = _speed;
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
           /* if (_navMeshAgent.isActiveAndEnabled && _followPlayer)
            {
                StartChasing();
            }*/
        }

        public void ApplyExplosionForce(Vector3 explosionForceDir)
        {
            _rb.AddForce(explosionForceDir * _spawnForce, ForceMode.Impulse);
        }

        public void StopExplosionForce()
        {
            _rb.velocity = Vector3.zero;
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

