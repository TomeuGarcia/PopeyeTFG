using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public class StaticEnemyHealth : MonoBehaviour, IDamageHitTarget
    {
        private HealthSystem _healthSystem;
        [SerializeField, Range(0.0f, 100)] private int _maxHealth = 50;

        void Awake()
        {
            _healthSystem = new HealthSystem(_maxHealth);
        }

        public DamageHitTargetType GetDamageHitTargetType()
        {
            throw new System.NotImplementedException();
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            throw new System.NotImplementedException();
        }

        public bool CanBeDamaged(DamageHit damageHit)
        {
            return !_healthSystem.IsDead() && !_healthSystem.IsInvulnerable;
        }

        public bool IsDead()
        {
            return _healthSystem.IsDead();
        }
    }

}