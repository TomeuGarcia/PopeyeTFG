using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.Enemies
{
    public class LaunchProjectileBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _projectilePrefab = null;
        [SerializeField] private Transform _spawnPoint = null;
        [SerializeField] private float _relativeSpeed;


        public void Launch()
        {
            var projectileInstance = Instantiate(_projectilePrefab, _spawnPoint.position,
                Quaternion.LookRotation(_spawnPoint.transform.forward));


            if (projectileInstance.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = rb.transform.TransformDirection(_spawnPoint.transform.forward * _relativeSpeed);
            }
        }
    }
}
