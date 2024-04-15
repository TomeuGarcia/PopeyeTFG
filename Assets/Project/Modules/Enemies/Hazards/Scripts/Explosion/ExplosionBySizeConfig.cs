using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [System.Serializable]
    public struct ExplosionBySizeConfig
    {
        public ExplosionSize size;
        public DamageHit damageHitConfig;
        public float scale;

    }
}
