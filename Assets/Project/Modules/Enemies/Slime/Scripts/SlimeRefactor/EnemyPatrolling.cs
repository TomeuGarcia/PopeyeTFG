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
        [SerializeField] private float _playerDistanceThresholdToStartFollowing;
        [SerializeField] private float _wayPointDistanceThreshold;
        private float _squaredWayPointDistanceThreshold;
        private float _squaredPlayerDistanceThreshold;
        private float _squaredPlayerDistanceThresholdToStartFollowing;
        private AEnemyMediator _mediator;
        private Vector3 _target;
        private bool _patrolling;
        
        public enum PatrolType
        {
            None,
            FixedWaypoints,
            Random
        }
        
        public void Configure(AEnemyMediator slimeMediator)
        {
            _mediator = slimeMediator;
        }
        
        public void Start()
        {
            _squaredPlayerDistanceThreshold = _playerDistanceThreshold * _playerDistanceThreshold;
            _squaredPlayerDistanceThresholdToStartFollowing = _playerDistanceThresholdToStartFollowing * _playerDistanceThresholdToStartFollowing;
        }

        private void Update()
        {
            if (_patrolType == PatrolType.FixedWaypoints)
            {
                
                if (_patrolling)
                {
                    if (IsPlayerAtCloseDistance())
                    {
                        _mediator.OnPlayerClose();
                        return;
                    }
                    if (IsCloseToWayPoint())
                    {
                        UpdateWaypointDestination();
                    }
                }
                else
                {
                    if (IsPlayerAtFarDistance())
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
            _squaredWayPointDistanceThreshold = _wayPointDistanceThreshold * _wayPointDistanceThreshold;
        }

        public void ResetPatrolling()
        {
            _patrolType = PatrolType.None;
            _patrolling = false;
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

        private float GetPlayerSqrMagnitude()
        {
            return (_playerTransform.position - _navMeshAgent.transform.position).sqrMagnitude;
        }
        
        private bool IsPlayerAtCloseDistance()
        {
            return GetPlayerSqrMagnitude() < _squaredPlayerDistanceThresholdToStartFollowing;
        }
        private bool IsPlayerAtFarDistance()
        {
            return GetPlayerSqrMagnitude() > _squaredPlayerDistanceThreshold;
        }
        
        private float GetWayPointSqrMagnitude()
        {
            return (_target - _navMeshAgent.transform.position).sqrMagnitude;
        }
        
        private bool IsCloseToWayPoint()
        {
            return GetWayPointSqrMagnitude() < _squaredWayPointDistanceThreshold;
        }
    }
}
