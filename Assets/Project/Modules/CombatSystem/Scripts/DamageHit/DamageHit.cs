using System.Collections;
using System.Collections.Generic;
using Project.Modules.CombatSystem.KnockbackSystem;
using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    public class DamageHit 
    {
        public int Damage { get; set; }
        public Vector3 DamageSourcePosition  { get; set; }
    

        public float StunDuration  { get; set; }
    
    
        private DamageHitTargetType _damageHitTargetTypeMask;
        public DamageHitTargetType DamageHitTargetTypeMask
        {
            get { return _damageHitTargetTypeMask; }
        }


        public KnockbackHit KnockbackHit { get; private set; }
        
    
        public DamageHit(DamageHitConfig config)
        {
            _damageHitTargetTypeMask = config.DamageHitTargetTypeMask;
            Damage = config.Damage;
            DamageSourcePosition = Vector3.zero;
            StunDuration = config.StunDuration;

            
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

    }
    
}


