using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Modules.CombatSystem
{
    public class DamageHitResult
    {
        public float ReceivedDamage { get; set; }

        public DamageHitResult(float receivedDamage)
        {
            ReceivedDamage = receivedDamage;
        }
    }

}