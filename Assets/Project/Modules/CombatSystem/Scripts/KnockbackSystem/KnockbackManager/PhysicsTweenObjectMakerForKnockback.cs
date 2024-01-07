using System;
using System.Collections.Generic;
using Project.PhysicsMovement;
using UnityEngine;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    public class PhysicsTweenObjectMakerForKnockback
    {
        private delegate PhysicsTweenObject CreateFunction(
            IKnockbackHitTarget knockbackHitTarget, KnockbackHit knockbackHit);
        
        private Dictionary<KnockbackHitType, CreateFunction> _knockbackTypeToCreateFunction;
        


        public PhysicsTweenObjectMakerForKnockback()
        {
            _knockbackTypeToCreateFunction = new Dictionary<KnockbackHitType, CreateFunction>
            {
                { KnockbackHitType.Push, CreatePhysicsTweenObject_Push },
                { KnockbackHitType.MoveToPosition, CreatePhysicsTweenObject_MoveToPosition }
            };
        }

        public PhysicsTweenObject CreatePhysicsTweenObject(IKnockbackHitTarget knockbackHitTarget,
            KnockbackHit knockbackHit)
        {
            return _knockbackTypeToCreateFunction[knockbackHit.KnockbackType]
                (knockbackHitTarget, knockbackHit);
        }


        private PhysicsTweenObject CreatePhysicsTweenObject_Push(IKnockbackHitTarget knockbackHitTarget,
            KnockbackHit knockbackHit)
        {
            Rigidbody rigidbody = knockbackHitTarget.GetRigidbodyToKnockback();
            Vector3 startPosition = rigidbody.position;
            Vector3 endPosition = startPosition + 
                                  (knockbackHit.PushDisplacement * knockbackHitTarget.GetKnockbackEffectiveness());

            return DoCreatePhysicsTweenObject(rigidbody, knockbackHit.Duration, startPosition, endPosition);
        }
        
        private PhysicsTweenObject CreatePhysicsTweenObject_MoveToPosition(IKnockbackHitTarget knockbackHitTarget,
            KnockbackHit knockbackHit)
        {
            Rigidbody rigidbody = knockbackHitTarget.GetRigidbodyToKnockback();
            Vector3 startPosition = rigidbody.position;
            Vector3 endPosition = knockbackHit.EndPosition;

            return DoCreatePhysicsTweenObject(rigidbody, knockbackHit.Duration, startPosition, endPosition);
        }

        private PhysicsTweenObject DoCreatePhysicsTweenObject(Rigidbody rigidbody, float duration, 
            Vector3 startPosition, Vector3 endPosition)
        {
            return new PhysicsTweenObject(rigidbody, duration, startPosition, endPosition);
        }
    }
}