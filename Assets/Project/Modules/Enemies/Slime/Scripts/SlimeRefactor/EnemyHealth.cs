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
        public HealthSystem HealthSystem { get; private set; }
        [SerializeField, Range(0, 100)] private int _maxHealth = 50;
        
        [SerializeField] private Rigidbody _knockbackRigidbody;
        [SerializeField, Range(0f, 1f)] private float _knockbackEffectiveness = 1f;

        private AEnemyMediator _mediator;


        public void Configure(AEnemyMediator slimeMediator)
        {
            _mediator = slimeMediator;
            HealToMax();
            SetIsInvulnerable(false);
        }

        private void Awake()
        {
            HealthSystem = new HealthSystem(_maxHealth);
        }

        public DamageHitTargetType GetDamageHitTargetType()
        {
            return DamageHitTargetType.Enemy;
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            int receivedDamage = HealthSystem.TakeDamage(damageHit.Damage);
            DamageHitResult hitResult = new DamageHitResult(this, gameObject, damageHit, receivedDamage, _mediator.Position);
            _mediator.OnHit(hitResult);
            if (IsDead())
            {
                _mediator.OnDeath(hitResult);
            }


            return hitResult;
        }

        public bool CanBeDamaged(DamageHit damageHit)
        {
            return !HealthSystem.IsDead() && !HealthSystem.IsInvulnerable;
        }

        public bool IsDead()
        {
            return HealthSystem.IsDead();
        }

        public void SetIsInvulnerable(bool isInvulnerable)
        {
            HealthSystem.IsInvulnerable = isInvulnerable;
        }

        public float GetValuePer1Ratio()
        {
            return HealthSystem.GetValuePer1Ratio();
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

        public void HealToMax()
        {
            HealthSystem.HealToMax();
        }
    }
}
