using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class EnemyPatrolling : ANavMeshMovement
    {
        private PatrolType _patrolType = PatrolType.None;
        private Transform[] _wayPoints;
        private int _wayPointIndex;
        [SerializeField] private float _playerDistanceThreshold;
        [SerializeField] private float _wayPointDistanceThreshold;
        private float _squaredWayPointDistanceThreshold;
        private float _squaredPlayerDistanceThreshold;
        private IEnemyMediator _mediator;
        private Vector3 _target;
        private bool _patrolling;
        
        public enum PatrolType
        {
            None,
            FixedWaypoints,
            Random
        }
        
        public void Configure(IEnemyMediator slimeMediator)
        {
            _mediator = slimeMediator;
        }
        
        public void Start()
        {
            _squaredPlayerDistanceThreshold = _playerDistanceThreshold * _playerDistanceThreshold;

            if (_patrolType == PatrolType.None)
            {
                return;
            }
            else if (_patrolType == PatrolType.FixedWaypoints)
            {
                
                
            }
            else if (_patrolType == PatrolType.Random)
            {
                
            }
        }

        private void Update()
        {
            if (_patrolType == PatrolType.FixedWaypoints)
            {
                float playerSqrMagnitude = (_playerTransform.position - _navMeshAgent.transform.position).sqrMagnitude;
                if (_patrolling)
                {
                    if (playerSqrMagnitude < _squaredPlayerDistanceThreshold)
                    {
                        _mediator.OnPlayerClose();
                        return;
                    }

                    float wayPointSqrMagnitude = (_target - _navMeshAgent.transform.position).sqrMagnitude;
                    if (wayPointSqrMagnitude < _squaredWayPointDistanceThreshold)
                    {
                        UpdateWaypointDestination();
                    }
                }
                else
                {
                    if (playerSqrMagnitude > _squaredPlayerDistanceThreshold)
                    {
                        _patrolling = true;
                        _mediator.OnPlayerFar();
                        

                    }
                }
            }
        }

        public void SetWayPoints(Transform[] wayPoints)
        {
            _wayPoints = wayPoints;

            _patrolType = PatrolType.FixedWaypoints;
            UpdateWaypointDestination();
            _squaredWayPointDistanceThreshold = _wayPointDistanceThreshold * _wayPointDistanceThreshold;
            SetPatrolling(true);
        }
        public void SetPatrolling(bool patrolling)
        {
            _patrolling = patrolling;
            if (_patrolling)
            {
                UpdateWaypointDestination();
            }
            
        }
        private void UpdateWaypointDestination()
        {
            
            IterateWayPoints();
            _target = _wayPoints[_wayPointIndex].position;
            SetDestination(_target);
        }

        private void IterateWayPoints()
        {
            _wayPointIndex = (_wayPointIndex + 1) % _wayPoints.Length;
        }

        public PatrolType GetPatrolType()
        {
            return _patrolType;
        }

        public Transform[] GetWaypoints()
        {
            return _wayPoints;
        }
        
    }
}
