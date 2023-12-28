using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Popeye.Modules.Enemies.Components
{
    public abstract class ANavMeshMovement : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent _navMeshAgent;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _targetDistanceThreshold = 0.5f;
        private float _squaredTargetDistanceThreshold;
        protected Transform _playerTransform = null;

        private void Awake()
        {
            _squaredTargetDistanceThreshold = _targetDistanceThreshold * _targetDistanceThreshold;
        }

        private void OnValidate()
        {
            _squaredTargetDistanceThreshold = _targetDistanceThreshold * _targetDistanceThreshold;
        }

        public virtual void SetPlayerTransform(Transform transform)
        {
            _playerTransform = transform;
        }

        public virtual void DeactivateNavigation()
        {
            _navMeshAgent.enabled = false;
        }

        public virtual void ActivateNavigation()
        {
                _navMeshAgent.enabled = true;
        }

        protected virtual void SetDestination(Vector3 destination)
        {
            float sqrDistance = (destination - _navMeshAgent.destination).sqrMagnitude;
                if (sqrDistance > _squaredTargetDistanceThreshold)
                {
                    _navMeshAgent.SetDestination(destination);
                }
        }
        }

    }

