using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Popeye.Modules.Enemies.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime;
        private Coroutine _destroyBullet;

        private void OnCollisionEnter(Collision other)
        {
            StopCoroutine(_destroyBullet);
            Destroy(gameObject);
        }

        private void Start()
        {
            _destroyBullet = StartCoroutine(DestroyBullet());
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(_lifeTime);
            Destroy(gameObject);
        }
    }
}
