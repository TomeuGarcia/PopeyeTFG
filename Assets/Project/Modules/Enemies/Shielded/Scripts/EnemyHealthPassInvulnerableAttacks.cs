using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class EnemyHealthPassInvulnerableAttacks : EnemyHealth
    {
        [Header("Attacks that pass invulnerable")] 
        [SerializeField] private DamageHitConfig[] _passInvulnerableDamageHits;

      
        public override bool CanBeDamaged(DamageHit damageHit)
        {
            if (IsDead())
            {
                return false;
            }
            if (!HealthSystem.IsInvulnerable)
            {
                return true;
            }

            foreach (var passInvulnerableDamageHit in _passInvulnerableDamageHits)
            {
                if (passInvulnerableDamageHit == damageHit.DamageHitConfig)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
