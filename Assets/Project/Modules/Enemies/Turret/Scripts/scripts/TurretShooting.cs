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
        
       
       private bool _animationOn = false;
       [SerializeField] private float _playerDistanceThreshold;
       [SerializeField] private float _playerDistanceThresholdToHide;
       [SerializeField] private float _playerDistanceThresholdToAppear;
       private float _squaredPlayerDistanceThreshold;
       private float _squaredPlayerDistanceThresholdToHide;
       private float _squaredPlayerDistanceThresholdToAppear;
       private bool _playerInSight = false;
       private bool _playerWasTooFar = false;

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
            
            DoHide(false, false);
        }

       private void OnEnable()
       {
           _playerWasTooFar = true;
       }

       private void Update()
        {
            if (IsPlayerAtValidDistance())
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
                else if (!_playerInSight)
                {
                    _mediator.AppearAnimation(_playerWasTooFar);
                    _playerInSight = true;
                }
                _hiding = false;
            }
            
            if(IsPlayerAtNotValidDistance(out bool playerIsTooClose, out bool playerIsTooFar) && !_hiding)
            {
                DoHide(playerIsTooClose, playerIsTooFar);
            }

            _playerWasTooFar = playerIsTooFar;
        }

       private void DoHide(bool playerIsTooClose, bool playerIsTooFar)
       {
           _hiding = true;
           _playerInSight = false;
           _mediator.HideAnimation(playerIsTooClose, playerIsTooFar);
           _timer = 0;
       }
       
        
        private float GetPlayerSqrMagnitude()
        {
            return (_playerTransform.position - transform.position).sqrMagnitude;
        }
        
        private bool IsPlayerAtNotValidDistance(out bool playerIsTooClose, out bool playerIsTooFar)
        {
            float playerDistanceSqr = GetPlayerSqrMagnitude();

            playerIsTooFar = playerDistanceSqr > _squaredPlayerDistanceThreshold;
            playerIsTooClose = playerDistanceSqr < _squaredPlayerDistanceThresholdToHide;
            return playerIsTooFar || playerIsTooClose;
        }
        
        private bool IsPlayerAtValidDistance()
        {
            float playerDistanceSqr = GetPlayerSqrMagnitude();
            
            bool playerIsCloseEnough = playerDistanceSqr < _squaredPlayerDistanceThreshold;
            bool playerIsFarEnough = playerDistanceSqr > _squaredPlayerDistanceThresholdToAppear;
            
            return playerIsCloseEnough && playerIsFarEnough;
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
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint, _playerTransform,_playerDistanceThreshold,
                _playerDistanceThresholdToHide);
        }

        public void MultipleShoot()
        {
            for (int i = 0; i < _numberOfShots; i++)
            {
                _currentProjectile.ShootRandom();
                _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint, _playerTransform,_playerDistanceThreshold,_playerDistanceThresholdToHide);
            }
        }
        private void OnDestroy()
        {
            transform.DOComplete();
        }
    }
}
