using System.Collections;
using System.Collections.Generic;
using Project.Modules.CombatSystem.KnockbackSystem;
using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    public class DamageHit
    {
        private readonly DamageHitConfig _config;
        public int Damage => _config.Damage;
        public Vector3 DamageSourcePosition  { get; set; }


        public float StunDuration => _config.StunDuration;
    
    
        public DamageHitTargetType DamageHitTargetTypeMask => _config.DamageHitTargetTypeMask;


        public KnockbackHit KnockbackHit { get; private set; }
        
    
        public DamageHit(DamageHitConfig config)
        {
            _config = config;
            DamageSourcePosition = Vector3.zero;

            KnockbackHit = new KnockbackHit(config.KnockbackHitConfig);
        }
        
        public void UpdateKnockbackPushDirection(Vector3 pushDirection)
        {
            KnockbackHit.UpdatePushDirection(pushDirection);
        }
        
        public void UpdateKnockbackEndPosition(Vector3 endPosition)
        {
            KnockbackHit.UpdateEndPosition(endPosition);
        }

        public string GetName()
        {
            return _config.name;
        }
    }
    
}


