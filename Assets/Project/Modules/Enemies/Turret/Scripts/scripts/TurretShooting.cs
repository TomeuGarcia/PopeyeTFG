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
        private AEnemyMediator _mediator;
        private Transform _playerTransform;
        private IHazardFactory _hazardsFactory;
        [SerializeField] private float timeBetweenShots;
       [SerializeField] private Transform _firePoint;
       private float timer = 0;
       private ParabolicProjectile _currentProjectile;
       
        
       [SerializeField] private float _squashAmountY = 2.6f;
       [SerializeField] private float _squashAmountXZ = 2.6f;
       [SerializeField] private float _stretchAmountY = 2.6f;
       [SerializeField] private float _stretchAmountXZ = 2.8f;
       [SerializeField] private float _squashAndStretchTime = 0.5f;

       private bool _animationOn = false;
       [SerializeField] private float _playerDistanceThreshold;
       private float _squaredPlayerDistanceThreshold;

       public void Configure(AEnemyMediator turetMediator, IHazardFactory hazardFactory,Transform playerTransform)
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
                if (timer >= timeBetweenShots - 1)
                {

                    if (!_animationOn)
                    {
                        SquashAndStretch().Forget();
                    }

                    if (timer >= timeBetweenShots)
                    {
                        _hazardsFactory.CreateParabolicProjectile(_firePoint,_playerTransform).Shoot();
                        timer = 0;
                    }
                }

                timer += Time.deltaTime;
            }
            else
            {
                if (_animationOn)
                {
                    _animationOn = false;
                    transform.DOComplete();
                    transform.localScale = Vector3.one;
                }
                timer = 0;
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
        private async UniTaskVoid SquashAndStretch()
        {
            _animationOn = true;
            await transform.DOScale(new Vector3(_squashAmountXZ, _squashAmountY, _squashAmountXZ), 0.9f)
                .AsyncWaitForCompletion();
            if (!_animationOn)
            {
                return;
            }
            await transform.DOScale(new Vector3(_stretchAmountXZ, _stretchAmountY, _stretchAmountXZ), 0.1f)
                .AsyncWaitForCompletion();
            
            _animationOn = false;

        }

        private void OnDestroy()
        {
            transform.DOComplete();
        }
    }
}
