using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    public class DamageDealer
    {
        private ICombatManager _combatManager;
        private DamageHit _damageHit;

        public void Configure(ICombatManager combatManager, DamageHit damageHit)
        {
            _combatManager = combatManager;
            _damageHit = damageHit;
        }

        public void SetDamageHit(DamageHit damageHit)
        {
            _damageHit = damageHit;
        }

        public void UpdatePosition(Vector3 position)
        {
            _damageHit.DamageSourcePosition = position;
        }
        public void UpdateKnockbackDirection(Vector3 knockbackDirection)
        {
            _damageHit.UpdateKnockbackPushDirection(knockbackDirection);
        }

        public bool TryDealDamage(GameObject gameObject, out DamageHitResult damageHitResult)
        {
            if (_combatManager.TryDealDamage(gameObject, _damageHit, out damageHitResult))
            {
                return true;
            }

            return false;
        }
    }
    
    
}