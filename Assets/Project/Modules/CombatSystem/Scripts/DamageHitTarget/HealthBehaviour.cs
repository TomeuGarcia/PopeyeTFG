using Popeye.Modules.ValueStatSystem;
using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    public class HealthBehaviour : MonoBehaviour, IDamageHitTarget, IHealthTarget
    {
        private IHealthBehaviourListener _listener;
        private DamageHitTargetType _damageHitTargetType;
        public HealthSystem HealthSystem { get; private set; }
        
        
        public void Configure(IHealthBehaviourListener listener, int maxHealth, DamageHitTargetType damageHitTargetType)
        {
            _listener = listener;
            HealthSystem = new HealthSystem(maxHealth);
            _damageHitTargetType = damageHitTargetType;
        }
        

        public DamageHitTargetType GetDamageHitTargetType()
        {
            return _damageHitTargetType;
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            int receivedDamage = HealthSystem.TakeDamage(damageHit.Damage);
            DamageHitResult damageHitResult = new DamageHitResult(this, gameObject, receivedDamage);
            
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
    }
}