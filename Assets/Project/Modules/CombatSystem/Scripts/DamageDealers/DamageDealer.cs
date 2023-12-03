using UnityEngine;

namespace Project.Modules.CombatSystem
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