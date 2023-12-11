using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.ServiceLocator;
using Project.Modules.CombatSystem;
using Unity.VisualScripting;
using UnityEngine;
using Debug = FMOD.Debug;

namespace Popeye.Modules.Enemies.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        private Coroutine _destroyBullet;
        private CancellationTokenSource _cancellationTokenSource;
        
        private DamageHit _contactDamageHit;
        [SerializeField] private DamageHitConfig _contactDamageConfig;

        private ICombatManager _combatManager;
        
        
        private void Start()
        {
            _contactDamageHit = new DamageHit(_contactDamageConfig);

            _combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
            
            _cancellationTokenSource = new CancellationTokenSource();
            DestroyBullet();
        }

        private void OnCollisionEnter(Collision other)
        {
            _cancellationTokenSource.Cancel();
            
            _contactDamageHit.Position = _transform.position;
            _contactDamageHit.KnockbackDirection = 
                PositioningHelper.Instance.GetDirectionAlignedWithFloor(_transform.position, other.transform.position);
            _combatManager.TryDealDamage(other.gameObject, _contactDamageHit, out DamageHitResult damageHitResult);
            
            Destroy(gameObject);
        }


        private async UniTaskVoid DestroyBullet()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime),
                cancellationToken: _cancellationTokenSource.Token);
            
            Destroy(gameObject);
        }
    }
}
