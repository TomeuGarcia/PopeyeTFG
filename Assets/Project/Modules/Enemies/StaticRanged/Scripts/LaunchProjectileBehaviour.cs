using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.Enemies
{
    public class LaunchProjectileBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab = null;
        [SerializeField] private Transform _spawnPoint = null;
        [SerializeField] private float _relativeSpeed;
        [SerializeField] private Transform _transform;
        
        private Core.Pool.ObjectPool _objectPool;
        [SerializeField]private PooledBullets _bullets;

        private void Awake()
        {
            _objectPool = new ObjectPool(_bullets, null);
            _objectPool.Init(20);
        }

        public void Launch()
        {
             var projectileInstance = _objectPool.Spawn<PooledBullets>(_spawnPoint.position,Quaternion.LookRotation(_spawnPoint.transform.forward));
             

            if (projectileInstance.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = rb.transform.TransformDirection(-_spawnPoint.transform.forward * _relativeSpeed);
            }
        }
    }
}
