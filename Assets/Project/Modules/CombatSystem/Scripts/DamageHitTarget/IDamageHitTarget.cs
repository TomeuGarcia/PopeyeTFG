using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    public interface IDamageHitTarget
    {
        public DamageHitTargetType GetDamageHitTargetType();
        public DamageHitResult TakeHitDamage(DamageHit damageHit);
        public bool CanBeDamaged(DamageHit damageHit);
        public bool IsDead();
    }
}
