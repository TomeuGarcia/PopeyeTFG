using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Popeye.Modules.Enemies.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        private Coroutine _destroyBullet;

        private DamageHit _contactDamageHit;
        [SerializeField] private DamageHitConfig _contactDamageConfig;

        private ICombatManager _combatManager;
        
        [SerializeField] private PooledBullets _bullet;

        private void Start()
        {
            _contactDamageHit = new DamageHit(_contactDamageConfig);

            _combatManager = ServiceLocator.Instance.GetService<ICombatManager>();

        }

        private void OnCollisionEnter(Collision other)
        {

            _contactDamageHit.DamageSourcePosition = _transform.position;
            _contactDamageHit.KnockbackDirection =
                PositioningHelper.Instance.GetDirectionAlignedWithFloor(_transform.position, other.transform.position);
            _combatManager.TryDealDamage(other.gameObject, _contactDamageHit, out DamageHitResult damageHitResult);
            _bullet.Recycle();
        }


    }
}
