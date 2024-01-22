using Popeye.Modules.ValueStatSystem;
using Project.Modules.CombatSystem.KnockbackSystem;
using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    public class HealthBehaviour : MonoBehaviour, IDamageHitTarget, IHealthTarget, IKnockbackHitTarget
    {
        private IHealthBehaviourListener _listener;
        private DamageHitTargetType _damageHitTargetType;
        private Rigidbody _knockbackRigidbody;
        [SerializeField, Range(0.0f, 1.0f)] private float _knockbackEffectiveness = 1.0f;
        
        public HealthSystem HealthSystem { get; private set; }
        
        private Vector3 Position => transform.position;
        
        public void Configure(IHealthBehaviourListener listener, int maxHealth, DamageHitTargetType damageHitTargetType,
            Rigidbody knockbackRigidbody)
        {
            _listener = listener;
            HealthSystem = new HealthSystem(maxHealth);
            _damageHitTargetType = damageHitTargetType;

            _knockbackRigidbody = knockbackRigidbody;
        }
        

        public DamageHitTargetType GetDamageHitTargetType()
        {
            return _damageHitTargetType;
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            int receivedDamage = HealthSystem.TakeDamage(damageHit.Damage);
            DamageHitResult damageHitResult = new DamageHitResult(this, gameObject, receivedDamage,
                Position);
            
            if (IsDead())
            {
                _listener.OnKilledByDamageTaken(damageHitResult);
            }
            else
            {
                _listener.OnDamageTaken(damageHitResult);
            }

            return damageHitResult;
        }
        

        public bool CanBeDamaged(DamageHit damageHit)
        {
            return !HealthSystem.IsDead() && !HealthSystem.IsInvulnerable;
        }

        public bool IsDead()
        {
            return HealthSystem.IsDead();
        }
        public bool IsMaxHealth()
        {
            return HealthSystem.IsMaxHealth();
        }
        

        public void Heal(int healAmount)
        {
            HealthSystem.Heal(healAmount);
            _listener.OnHealed();
        }

        public void HealToMax()
        {
            HealthSystem.HealToMax();
            _listener.OnHealed();
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