using System.Collections;
using System.Collections.Generic;
using Project.Modules.CombatSystem;
using UnityEngine;

namespace Project.Modules.CombatSystem
{
    public interface IDamageHitTarget
    {
        public DamageHitTargetType GetDamageHitTargetType();
        public DamageHitResult TakeHitDamage(DamageHit damageHit);
        public bool CanBeDamaged(DamageHit damageHit);
        public bool IsDead();
    }
}
