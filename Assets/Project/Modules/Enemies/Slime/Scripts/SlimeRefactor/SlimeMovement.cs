using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;

namespace Popeye.Modules.Enemies.Components
{
    public class SlimeMovement : MonoBehaviour
    {
        [SerializeField] private Vector2 _speeedThreshold = new Vector2(5, 7);
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private int _spawnForce = 10;
        [SerializeField] private Rigidbody _rb;

        private Transform _playerTransform = null;
        private bool _followPlayer = false;

        protected IEnemyMediator _mediator;


        public void Configure(IEnemyMediator slimeMediator)
        {
            _mediator = slimeMediator;
        }

        private void Start()
        {
            _navMeshAgent.speed = Random.Range(_speeedThreshold.x, _speeedThreshold.y);
        }

        void Update()
        {
            if (_followPlayer && _playerTransform != null && _navMeshAgent.isActiveAndEnabled)
            {
                _navMeshAgent.SetDestination(_playerTransform.position);
            }

        }

        public void SetTarget(Transform transform)
        {
            _playerTransform = transform;
            if (_navMeshAgent.isActiveAndEnabled)
            {
                _followPlayer = true;
            }
        }

        public void ApplyExplosionForce(Vector3 explosionForceDir)
        {
            _rb.AddForce(explosionForceDir * _spawnForce, ForceMode.Impulse);
        }

        public void StopExplosionForce()
        {
            _rb.velocity = Vector3.zero;
        }

        public void DeactivateNavigation()
        {
            _navMeshAgent.enabled = false;
        }

        public void ActivateNavigation()
        {
            _navMeshAgent.enabled = true;
        }





    }
}

