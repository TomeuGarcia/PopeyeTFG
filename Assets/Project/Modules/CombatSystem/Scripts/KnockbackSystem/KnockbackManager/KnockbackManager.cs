using Popeye.Scripts.Collisions;
using Project.PhysicsMovement;
using UnityEngine;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    public class KnockbackManager : IKnockbackManager
    {
        private PhysicsTweenerBehaviour _physicsTweener;
        private KnockbackTweenObjectMaker _knockbackTweenObjectMaker;

        
        public KnockbackManager(PhysicsTweenerBehaviour physicsTweener, 
            KnockbackTweenObjectMaker knockbackTweenObjectMaker)
        {
            _physicsTweener = physicsTweener;
            _knockbackTweenObjectMaker = knockbackTweenObjectMaker;
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
                _knockbackTweenObjectMaker.CreatePhysicsTweenObject(knockbackHitTarget, knockbackHit)
            );
            return true;
        }
        
    }
}