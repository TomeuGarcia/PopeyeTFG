using Popeye.Scripts.Collisions;
using Project.PhysicsMovement;
using UnityEngine;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    public class KnockbackManager : IKnockbackManager
    {
        private PhysicsTweenerBehaviour _physicsTweener;
        private PhysicsTweenObjectMakerForKnockback _physicsTweenObjectMakerForKnockback;

        
        public KnockbackManager(PhysicsTweenerBehaviour physicsTweener, CollisionProbingConfig floorPlatformsProbingConfig)
        {
            _physicsTweener = physicsTweener;
            _physicsTweenObjectMakerForKnockback = new PhysicsTweenObjectMakerForKnockback(floorPlatformsProbingConfig);
        }


        public bool TryApplyKnockback(GameObject hitObject, KnockbackHit knockbackHit)
        {
            if (knockbackHit.KnockbackType == KnockbackHitType.None)
            {
                return false;
            }
            
            if (!hitObject.TryGetComponent<IKnockbackHitTarget>(out IKnockbackHitTarget knockbackHitTarget))
            {
                return false;
            }

            if (!knockbackHitTarget.CanBeKnockbacked())
            {
                return false;
            }


            _physicsTweener.AddObject(
                _physicsTweenObjectMakerForKnockback.CreatePhysicsTweenObject(knockbackHitTarget, knockbackHit)
            );
            return true;
        }
        
    }
}