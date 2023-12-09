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
        [SerializeField] private Transform _transform;
        private Coroutine _destroyBullet;
        private CancellationTokenSource _cancellationTokenSource;
        
        private DamageHit _contactDamageHit;
        [SerializeField] private float _contactHitDamageAmount;
        [SerializeField] private float _contactHitStunDuration;
        [SerializeField] private float _contactHitKnockbackForce;
        private void OnCollisionEnter(Collision other)
        {
            _cancellationTokenSource.Cancel();
            
            _contactDamageHit.Position = _transform.position;
            _contactDamageHit.KnockbackDirection = PositioningHelper.Instance.GetDirectionAlignedWithFloor(_transform.position, other.transform.position);
            CombatManager.Instance.TryDealDamage(other.gameObject, _contactDamageHit, out DamageHitResult damageHitResult);
            
            Destroy(gameObject);
        }

        private void Start()
        {
            _contactDamageHit = new DamageHit(CombatManager.Instance.DamageOnlyPlayerPreset,
                _contactHitDamageAmount, _contactHitKnockbackForce, _contactHitStunDuration);
            
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
