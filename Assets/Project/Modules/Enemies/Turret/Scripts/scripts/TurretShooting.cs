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
       private bool _playerInSight = false;

       [SerializeField] private float _randomDistance = 3;

       private bool _hiding =false;
       [SerializeField] private int _numberOfShots = 3;
       public void Configure(TurretMediator turetMediator, IHazardFactory hazardFactory,Transform playerTransform)
        {
            _mediator = turetMediator;
            _playerTransform = playerTransform;
            _hazardsFactory = hazardFactory;
            _squaredPlayerDistanceThresholdToAppear = _playerDistanceThresholdToAppear * _playerDistanceThresholdToAppear;
            _squaredPlayerDistanceThresholdToHide = _playerDistanceThresholdToHide * _playerDistanceThresholdToHide;
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint,_playerTransform,_playerDistanceThreshold,_playerDistanceThresholdToHide);
            _squaredPlayerDistanceThreshold = _playerDistanceThreshold * _playerDistanceThreshold;
           
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
                else if(!_playerInSight)
                {
                    _mediator.AppearAnimation();
                    _hiding = false;
                    _mediator.PlayerSeen();
                    _playerInSight = true;
                }
            }
            if(!IsPlayerAtCloseDistance() && !_hiding)
            {
                _hiding = true;
                _playerInSight = false;
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
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint, _playerTransform,_playerDistanceThreshold,_playerDistanceThresholdToHide);
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
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint, _playerTransform,_randomDistance,
                _randomDistance);
        }

        public void MultipleShoot()
        {
            Shoot();
            for (int i = 0; i < _numberOfShots-1; i++)
            {
                _currentProjectile.ShootRandom();
                _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint, _playerTransform,_randomDistance,0);
            }
        }
        private void OnDestroy()
        {
            transform.DOComplete();
        }
    }
}
