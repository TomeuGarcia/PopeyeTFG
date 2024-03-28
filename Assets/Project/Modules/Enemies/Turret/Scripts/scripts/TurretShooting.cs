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
       [SerializeField] private float _playerDistanceThresholdToHide;
       [SerializeField] private float _playerDistanceThresholdToAppear;
       private float _squaredPlayerDistanceThreshold;
       private float _squaredPlayerDistanceThresholdToHide;
       private float _squaredPlayerDistanceThresholdToAppear;

       public void Configure(TurretMediator turetMediator, IHazardFactory hazardFactory,Transform playerTransform)
        {
            _mediator = turetMediator;
            _playerTransform = playerTransform;
            _hazardsFactory = hazardFactory;
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint,_playerTransform);
            _squaredPlayerDistanceThreshold = _playerDistanceThreshold * _playerDistanceThreshold;
            _squaredPlayerDistanceThresholdToAppear = _playerDistanceThresholdToAppear * _playerDistanceThresholdToAppear;
            _squaredPlayerDistanceThresholdToHide = _playerDistanceThresholdToHide * _playerDistanceThresholdToHide;
        }



        private void Update()
        {
            if (IsPlayerFarEnoughToAppear())
            {
                
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
                else
                {
                    _mediator.AppearAnimation();
                }
            }
            if(!IsPlayerAtCloseDistance())
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
            return GetPlayerSqrMagnitude() < _squaredPlayerDistanceThreshold && GetPlayerSqrMagnitude() > _squaredPlayerDistanceThresholdToHide;
        }
        
        private bool IsPlayerFarEnoughToAppear()
        {
            return GetPlayerSqrMagnitude() < _squaredPlayerDistanceThreshold && GetPlayerSqrMagnitude() > _squaredPlayerDistanceThresholdToAppear;
        }

        private bool IsPlayerTooClose()
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
            _currentProjectile.Recycle();
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
