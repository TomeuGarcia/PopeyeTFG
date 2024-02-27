using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;


namespace Popeye.Modules.Enemies.Components
{
    public class TurretShooting : MonoBehaviour
    {
        private AEnemyMediator _mediator;
        private Transform _playerTransform;
        private Core.Pool.ObjectPool _projectilePool;
        private Core.Pool.ObjectPool _damageAreaPool;

       [SerializeField] private float timeBetweenShots;
       [SerializeField] private Transform _firePoint;
       private float timer = 0;
       private ParabolicProjectile _currentProjectile;
       
        
       [SerializeField] private float _squashAmountY = 2.6f;
       [SerializeField] private float _squashAmountXZ = 2.6f;
       [SerializeField] private float _stretchAmountY = 2.6f;
       [SerializeField] private float _stretchAmountXZ = 2.8f;
       [SerializeField] private float _squashAndStretchTime = 0.5f;

       private bool animationOn = false;
       [SerializeField] private float _playerDistanceThreshold;
       private float _squaredPlayerDistanceThreshold;

        public void Configure(AEnemyMediator turetMediator,Core.Pool.ObjectPool projectilePool,Core.Pool.ObjectPool damageAreaPool,Transform transform)
        {
            _mediator = turetMediator;
            _projectilePool = projectilePool;
            _damageAreaPool = damageAreaPool;
            _playerTransform = transform;
            _currentProjectile = _projectilePool.Spawn<ParabolicProjectile>(_firePoint.position, quaternion.identity);
            _currentProjectile.PrepareShot(_firePoint,_playerTransform,_damageAreaPool);
            _squaredPlayerDistanceThreshold = _playerDistanceThreshold * _playerDistanceThreshold;
        }

        private void Update()
        {
            if (IsPlayerAtCloseDistance())
            {
                if (timer >= timeBetweenShots - 1)
                {
                    _currentProjectile.StartAiming();

                    if (!animationOn)
                    {
                        SquashAndStretch();
                    }

                    if (timer >= timeBetweenShots)
                    {
                        _currentProjectile.Shoot();
                        timer = 0;
                        _currentProjectile = _projectilePool.Spawn<ParabolicProjectile>(_firePoint.position, quaternion.identity);
                        _currentProjectile.PrepareShot(_firePoint, _playerTransform,_damageAreaPool);
                    }
                }

                timer += Time.deltaTime;
            }
            else
            {
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
            animationOn = true;
            // Squash
            await transform.DOScale(new Vector3(_squashAmountXZ, _squashAmountY, _squashAmountXZ), 0.9f)
                .AsyncWaitForCompletion();
            //Stretch
            await transform.DOScale(new Vector3(_stretchAmountXZ, _stretchAmountY, _stretchAmountXZ), 0.2f)
                .AsyncWaitForCompletion();
            animationOn = false;

        }
    }
}
