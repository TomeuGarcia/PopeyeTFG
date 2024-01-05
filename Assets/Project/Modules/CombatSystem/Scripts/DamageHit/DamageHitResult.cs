using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    public class DamageHitResult
    {
        public IDamageHitTarget DamageHitTarget { get; private set; }
        public GameObject DamageHitTargetGameObject { get; private set; }
        public float ReceivedDamage { get; private set; }


        public Vector3 DamagedHitTargetPosition { get; private set; }
        public Vector3 ContactPosition { get; private set; }
        public Vector3 ContactNormal { get; private set; }
        public Vector3 ToHitOriginDirection { get; private set; }


        public DamageHitResult(IDamageHitTarget damageHitTarget, GameObject damageHitTargetGameObject,
            float receivedDamage, Vector3 damagedHitTargetPosition)
        {
            DamageHitTarget = damageHitTarget;
            DamageHitTargetGameObject = damageHitTargetGameObject;
            ReceivedDamage = receivedDamage;
            DamagedHitTargetPosition = damagedHitTargetPosition;
        }

        public void SetContactValues(Vector3 contactPosition, Vector3 contactNormal, Vector3 toHitOriginDirection)
        {
            ContactPosition = contactPosition;
            ContactNormal = contactNormal;
            ToHitOriginDirection = toHitOriginDirection;
        }
    }

}