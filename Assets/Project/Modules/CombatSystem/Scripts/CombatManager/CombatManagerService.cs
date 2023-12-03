using UnityEngine;

namespace Project.Modules.CombatSystem
{
    public class CombatManagerService : ICombatManager
    {

        public bool TryDealDamage(GameObject hitObject, DamageHit damageHit, out DamageHitResult damageHitResult)
        {
            damageHitResult = null;
            if (!hitObject.TryGetComponent<IDamageHitTarget>(out IDamageHitTarget hitTarget))
            {
                return false;
            }

            if (DamageHitIgnoresDamageTarget(damageHit, hitTarget))
            {
                return false;
            }


            if (!hitTarget.CanBeDamaged(damageHit))
            {
                return false;
            }

            damageHitResult = hitTarget.TakeHitDamage(damageHit);
            return true;
        }


        private bool DamageHitIgnoresDamageTarget(DamageHit damageHit, IDamageHitTarget hitTarget)
        {
            DamageHitTargetType damageHitTypeMask = damageHit.DamageHitTargetTypeMask;
            DamageHitTargetType damageTargetType = hitTarget.GetDamageHitTargetType();

            return !damageHitTypeMask.HasFlag(damageTargetType);
        }
    }
}