using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.CombatSystem;
using Project.Modules.CombatSystem.KnockbackSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class EnemyHealth : MonoBehaviour, IDamageHitTarget, IKnockbackHitTarget
    {
        private HealthSystem _healthSystem;
        [SerializeField, Range(0, 100)] private int _maxHealth = 50;
        
        [SerializeField] private Rigidbody _knockbackRigidbody;
        [SerializeField, Range(0f, 1f)] private float _knockbackEffectiveness = 1f;

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
                _mediator.OnDeath(damageHit);
            }
            else
            {
                _mediator.OnHit(damageHit);
            }

            return new DamageHitResult(this, gameObject, receivedDamage, _mediator.Position);
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

        public Rigidbody GetRigidbodyToKnockback()
        {
            return _knockbackRigidbody;
        }

        public bool CanBeKnockbacked()
        {
            return true;
        }

        public float GetKnockbackEffectivenessMultiplier()
        {
            return _knockbackEffectiveness;
        }
    }
}
