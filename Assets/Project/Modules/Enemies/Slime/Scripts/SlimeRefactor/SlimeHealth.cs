using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHealth : ISlimeComponent,IDamageHitTarget
{
        private HealthSystem _healthSystem;
        [SerializeField, Range(0.0f, 100.0f)] private float _maxHealth = 50.0f;
        
        

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
                _mediator.Divide();
            }
    
            return new DamageHitResult(damageHit.Damage);
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
