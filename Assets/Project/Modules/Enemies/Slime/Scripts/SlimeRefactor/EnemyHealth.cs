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
        [SerializeField, Range(0, 100)] private int _maxHealth = 50;

        private AEnemyMediator _mediator;


        public void Configure(AEnemyMediator slimeMediator)
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
            int receivedDamage = _healthSystem.TakeDamage(damageHit.Damage);
            
            if (IsDead())
            {
                _mediator.OnDeath();
            }
            else
            {
                _mediator.OnHit();
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

        public float GetValuePer1Ratio()
        {
            return _healthSystem.GetValuePer1Ratio();
        }
    }
}
