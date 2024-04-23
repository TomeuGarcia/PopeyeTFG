using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    public interface ICombatManager
    {
        bool TryDealDamage(GameObject hitObject, DamageHit damageHit, out DamageHitResult damageHitResult);
    }
}