using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.ValueStatSystem;
using Project.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class EnemyHealth : MonoBehaviour, IDamageHitTarget
    {
        private HealthSystem _healthSystem;
        [SerializeField, Range(0.0f, 100.0f)] private float _maxHealth = 50.0f;
        [SerializeField] private EnemyVisuals _enemyVisuals;

        private IEnemyMediator _mediator;


        public void Configure(IEnemyMediator slimeMediator)
        {
            _mediator = slimeMediator;
        }

        private void Awake()
        {
            _healthSystem = new HealthSystem(_maxHealth);
        }

        public DamageHitTargetType GetDamageHitTargetType()
        {
            return DamageHitTargetType.Enemy;
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            _healthSystem.TakeDamage(damageHit.Damage);
            
            if (IsDead())
            {
                _mediator.OnDeath();
                _enemyVisuals.OnDeathEffects();
            }
            else
            {
                _enemyVisuals.OnHitEffects(_healthSystem.CurrentHealth / _healthSystem.MaxHealth);
            }

            return new DamageHitResult(this, gameObject, receivedDamage);
        }

        public bool CanBeDamaged(DamageHit damageHit)
        {
            return !_healthSystem.IsDead() && !_healthSystem.IsInvulnerable;
        }

        public bool IsDead()
        {
            return _healthSystem.IsDead();
        }

        public void SetIsInvulnerable(bool isInvulnerable)
        {
            _healthSystem.IsInvulnerable = isInvulnerable;
        }


    }
}
