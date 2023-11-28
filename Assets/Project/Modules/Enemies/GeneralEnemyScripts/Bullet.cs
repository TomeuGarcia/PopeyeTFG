using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Popeye.Modules.Enemies.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime;
        private Coroutine _destroyBullet;
        private CancellationTokenSource _cancellationTokenSource;
        private void OnCollisionEnter(Collision other)
        {
            _cancellationTokenSource.Cancel();
            Destroy(gameObject);
        }

        private void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            DestroyBullet();
        }

        private async UniTaskVoid DestroyBullet()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime),
                cancellationToken: _cancellationTokenSource.Token);
            
            Destroy(gameObject);
        }
    }
}
