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
        [Header("SHOOTING")]
        [SerializeField] private float timeBetweenShots;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private int _numberOfShots = 3;
        
        [Header("THRESHOLD DISTANCES")]
        [SerializeField] private float _playerDistanceThreshold;
        [SerializeField] private float _playerDistanceThresholdToHide;
        [SerializeField] private float _playerDistanceThresholdToAppear;

        private TurretMediator _mediator;
        private Transform _playerTransform;
        private IHazardFactory _hazardsFactory;
        private float _timer = 0;
        private ParabolicProjectile _currentProjectile;
        private bool _outOfGround = true;

        private bool _animationOn = false;

        private bool _isHidden = false;

        public void Configure(TurretMediator turetMediator, IHazardFactory hazardFactory,Transform playerTransform)
        {
            _mediator = turetMediator;
            _playerTransform = playerTransform;
            _hazardsFactory = hazardFactory;
            
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(
                _firePoint,_playerTransform,
                _playerDistanceThreshold,_playerDistanceThresholdToHide);

            PlayerIsTooClose = false;
            PlayerIsTooFar = false;
            DoHide();
        }
       

        private void Update()
        {
            UpdateDistances();
           
            if (PlayerIsAtValidDistance)
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
                else if (_isHidden)
                {
                    DoAppear();
                }
            }
            else if ((PlayerIsTooClose || PlayerIsTooFar) && !_isHidden)
            {
                DoHide();
            }
        }

        private void DoAppear()
        {
           _isHidden = false;
           _mediator.AppearAnimation(PlayerIsAtValidDistanceFromClose);
        }
        private void DoHide()
        {
           _isHidden = true;
           _mediator.HideAnimation(PlayerIsTooClose, PlayerIsTooFar);
           _timer = 0;
        }
       
        
        private float GetPlayerDistance()
        {
            return Vector3.Distance(_playerTransform.position, transform.position);
        }


        public bool PlayerIsTooClose { get; private set; }
        public bool PlayerIsTooFar { get; private set; }
        public bool PlayerIsAtValidDistance { get; private set; }
        public bool PlayerIsAtValidDistanceFromClose { get; private set; }
        private void UpdateDistances()
        {
            float playerDistance = GetPlayerDistance();
            
            PlayerIsAtValidDistance = playerDistance < _playerDistanceThreshold &&
                                      playerDistance > _playerDistanceThresholdToAppear;

            PlayerIsAtValidDistanceFromClose = PlayerIsAtValidDistance && 
                                               playerDistance < (_playerDistanceThresholdToAppear + 1);
            
            PlayerIsTooFar = playerDistance > _playerDistanceThreshold;
            PlayerIsTooClose = playerDistance < _playerDistanceThresholdToHide;
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
