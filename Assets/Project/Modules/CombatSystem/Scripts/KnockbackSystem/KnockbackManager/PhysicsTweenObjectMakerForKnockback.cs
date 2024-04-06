using System;
using System.Collections.Generic;
using Popeye.Modules.Utilities.Scripts.Collisions;
using Popeye.Scripts.Collisions;
using Project.PhysicsMovement;
using UnityEngine;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    public class PhysicsTweenObjectMakerForKnockback
    {
        private delegate PhysicsTweenObject CreateFunction(
            IKnockbackHitTarget knockbackHitTarget, KnockbackHit knockbackHit);
        
        private Dictionary<KnockbackHitType, CreateFunction> _knockbackTypeToCreateFunction;

        private QuickMotionFloorPlatformChecker _floorPlatformChecker;
        
        

        public PhysicsTweenObjectMakerForKnockback(CollisionProbingConfig floorPlatformsProbingConfig)
        {
            _knockbackTypeToCreateFunction = new Dictionary<KnockbackHitType, CreateFunction>
            {
                { KnockbackHitType.Push, CreatePhysicsTweenObject_Push },
                { KnockbackHitType.MoveToPosition, CreatePhysicsTweenObject_MoveToPosition }
            };

            _floorPlatformChecker = new QuickMotionFloorPlatformChecker(floorPlatformsProbingConfig);
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
                                  (knockbackHit.PushDisplacement * knockbackHitTarget.GetKnockbackEffectivenessMultiplier());

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
            ApplyEndPositionCorrections(startPosition, ref endPosition, ref duration);
            
            return new PhysicsTweenObject(rigidbody, duration, startPosition, endPosition);
        }

        private void ApplyEndPositionCorrections(Vector3 startPosition, ref Vector3 endPosition, ref float duration)
        {
            endPosition = _floorPlatformChecker.ComputeEndPosition_Rear(startPosition, endPosition, 
                out float distanceChangeRatio01_NoFloor);

            duration *= Mathf.Max(distanceChangeRatio01_NoFloor, 0.05f);
        }
    }
}