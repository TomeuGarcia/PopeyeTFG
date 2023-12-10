using UnityEngine;

namespace Project.Modules.CombatSystem
{
    public interface ICombatManager
    {
        bool TryDealDamage(GameObject hitObject, DamageHit damageHit, out DamageHitResult damageHitResult);
    }
}