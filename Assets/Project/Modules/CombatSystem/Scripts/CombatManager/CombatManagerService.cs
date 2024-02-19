using Popeye.Scripts.Collisions;
using Project.Modules.CombatSystem.KnockbackSystem;
using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    public class CombatManagerService : ICombatManager
    {
        private CollisionProbingConfig _hitTargetProbingConfig;
        private IKnockbackManager _knockbackManager;
        
        public CombatManagerService(CollisionProbingConfig hitTargetProbingConfig,
            IKnockbackManager knockbackManager)
        {
            _hitTargetProbingConfig = hitTargetProbingConfig;
            _knockbackManager = knockbackManager;
        }
        
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
            SetDamageHitResultContactValues(damageHit, damageHitResult);

            _knockbackManager.TryApplyKnockback(hitObject, damageHit.KnockbackHit);

            return true;
        }


        private bool DamageHitIgnoresDamageTarget(DamageHit damageHit, IDamageHitTarget hitTarget)
        {
            DamageHitTargetType damageHitTypeMask = damageHit.DamageHitTargetTypeMask;
            DamageHitTargetType damageTargetType = hitTarget.GetDamageHitTargetType();

            return !damageHitTypeMask.HasFlag(damageTargetType);
        }

        private void SetDamageHitResultContactValues(DamageHit damageHit, DamageHitResult damageHitResult)
        {
            Vector3 hitOrigin = damageHit.DamageSourcePosition;
            Vector3 hitEnd = damageHitResult.DamagedHitTargetPosition;
            Vector3 originToEnd = hitEnd - hitOrigin;
            float originToEndDistance = originToEnd.magnitude;
            Vector3 originToEndDirection = originToEnd / originToEndDistance;

            Vector3 contactPosition;
            Vector3 contactNormal;
            
            if (Physics.Raycast(hitOrigin, originToEndDirection, out RaycastHit hit, originToEndDistance,
                    _hitTargetProbingConfig.CollisionLayerMask, _hitTargetProbingConfig.QueryTriggerInteraction))
            {
                contactPosition = hit.point;
                contactNormal = hit.normal;
            }
            else
            {
                contactPosition = hitEnd;
                contactNormal = -originToEndDirection;
            }
            
            damageHitResult.SetContactValues(contactPosition, contactNormal, -originToEndDirection);
        }
        
    }
}