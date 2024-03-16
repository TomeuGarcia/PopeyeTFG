using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.Enemies.Hazards;
using Unity.Mathematics;
using UnityEngine;


namespace Popeye.Modules.Enemies.Components
{
    public class TurretShooting : MonoBehaviour
    {
        private TurretMediator _mediator;
        private Transform _playerTransform;
        private IHazardFactory _hazardsFactory;
        [SerializeField] private float timeBetweenShots;
        [SerializeField] private Transform _firePoint;
        private float _timer = 0;
        private ParabolicProjectile _currentProjectile;
        private bool _outOfGround = true;
        
       [SerializeField] private float _squashAmountY = 2.6f;
       [SerializeField] private float _squashAmountXZ = 2.6f;
       [SerializeField] private float _stretchAmountY = 2.6f;
       [SerializeField] private float _stretchAmountXZ = 2.8f;
       [SerializeField] private float _squashAndStretchTime = 0.5f;

       
       private bool _animationOn = false;
       [SerializeField] private float _playerDistanceThreshold;
       [SerializeField] private float _playerDistanceThresholdToAppear;
       private float _squaredPlayerDistanceThreshold;

       public void Configure(TurretMediator turetMediator, IHazardFactory hazardFactory,Transform playerTransform)
        {
            _mediator = turetMediator;
            _playerTransform = playerTransform;
            _hazardsFactory = hazardFactory;
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint,_playerTransform);
            _squaredPlayerDistanceThreshold = _playerDistanceThreshold * _playerDistanceThreshold;
        }



        private void Update()
        {
            if (IsPlayerAtCloseDistance())
            {
                _mediator.AppearAnimation();
                if (_outOfGround)
                {
                    _mediator.LookAtPlayer(Time.deltaTime);

                    if (_timer >= timeBetweenShots)
                    {

                        _mediator.StartShootingAnimation();
                        _mediator.StoptIdleAnimation();
                        _timer = 0;
                    }

                    _timer += Time.deltaTime;
                }
            }
            else
            {
                _mediator.HideAnimation();
                _timer = 0;
            }
        }
        
        private float GetPlayerSqrMagnitude()
        {
            return (_playerTransform.position - transform.position).sqrMagnitude;
        }
        
        private bool IsPlayerAtCloseDistance()
        {
            return GetPlayerSqrMagnitude() < _squaredPlayerDistanceThreshold;
        }

        public void SetOutOfGround()
        {
            
            _timer = 0;
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint, _playerTransform);
            _outOfGround = true;
        }
        public void InsideGround()
        {
            _outOfGround = false;
        }
        public void Shoot()
        {
            _currentProjectile.Shoot();
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint, _playerTransform);
        }
        private void OnDestroy()
        {
            transform.DOComplete();
        }
    }
}
