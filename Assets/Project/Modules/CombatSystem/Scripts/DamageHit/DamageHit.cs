using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    public class DamageHit 
    {
        public int Damage { get; set; }
        public Vector3 DamageSourcePosition  { get; set; }
    
        public Vector3 ContactPosition => DamageSourcePosition;
        public Vector3 ContactNormal => -KnockbackDirection;
    
        private float _knockbackMagnitude;
        public float KnockbackMagnitude
        {
            get { return _knockbackMagnitude; }
            set { 
                _knockbackMagnitude = value;
                KnockbackForce = _knockbackMagnitude * _knockbackDirection;
            }
        }
        
        private Vector3 _knockbackDirection;
        public Vector3 KnockbackDirection
        {
            get { return _knockbackDirection; }
            set { 
                _knockbackDirection = value;
                KnockbackForce = _knockbackMagnitude * _knockbackDirection;
            }
        }
    
        public Vector3 KnockbackForce { get; private set; }
    
    
    
        public float StunDuration  { get; set; }
    
    
        private DamageHitTargetType _damageHitTargetTypeMask;
        public DamageHitTargetType DamageHitTargetTypeMask
        {
            get { return _damageHitTargetTypeMask; }
        }
    
    
        public DamageHit(DamageHitConfig config)
        {
            _damageHitTargetTypeMask = config.DamageHitTargetTypeMask;
            Damage = config.Damage;
            DamageSourcePosition = Vector3.zero;
            StunDuration = config.StunDuration;
    
            KnockbackMagnitude = config.KnockbackMagnitude;
            KnockbackDirection = Vector3.zero;
        }
        
        public DamageHit(DamageHitTargetType damageHitTargetTypeMask, int damage, float knockbackMagnitude, float stunDuration)
        {
            _damageHitTargetTypeMask = damageHitTargetTypeMask;
            Damage = damage;
            DamageSourcePosition = Vector3.zero;
            StunDuration = stunDuration;
    
            KnockbackMagnitude = knockbackMagnitude;
            KnockbackDirection = Vector3.zero;
        }
        
        public DamageHit(DamageHitTargetType damageHitTargetTypeMask, int damage, float knockbackMagnitude, float stunDuration, Vector3 damageSourcePosition)
        {
            _damageHitTargetTypeMask = damageHitTargetTypeMask;
            Damage = damage;
            DamageSourcePosition = damageSourcePosition;
            StunDuration = stunDuration;
    
            KnockbackMagnitude = knockbackMagnitude;
            KnockbackDirection = Vector3.zero;
        }
        
    }
    
}


