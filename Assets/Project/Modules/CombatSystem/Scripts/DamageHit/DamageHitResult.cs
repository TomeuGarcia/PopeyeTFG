using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Modules.CombatSystem
{
    public class DamageHitResult
    {
        public IDamageHitTarget DamageHitTarget { get; private set; }
        public GameObject DamageHitTargetGameObject { get; private set; }
        public float ReceivedDamage { get; private set; }

        public DamageHitResult(IDamageHitTarget damageHitTarget, GameObject damageHitTargetGameObject, 
            float receivedDamage)
        {
            DamageHitTarget = damageHitTarget;
            DamageHitTargetGameObject = damageHitTargetGameObject;
            ReceivedDamage = receivedDamage;
        }
    }

}