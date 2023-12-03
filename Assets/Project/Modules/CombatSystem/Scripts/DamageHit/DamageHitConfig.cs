using UnityEngine;

namespace Project.Modules.CombatSystem
{
    [CreateAssetMenu(fileName = "New_DamageHitConfig", 
        menuName = "Popeye/CombatSystem/DamageHitConfig")]
    public class DamageHitConfig : ScriptableObject
    {
        [SerializeField] private DamageHitTargetPreset _damageHitPreset;
        [SerializeField, Range(0, 100)] private int _damage = 10;
        [SerializeField, Range(0f, 10f)] private float _knockbackMagnitude = 0;
        [SerializeField, Range(0f, 10f)] private float _stunDuration = 0;
        
        
        public DamageHitTargetType DamageHitTargetTypeMask => _damageHitPreset.TargetMask;
        public float Damage => _damage;
        public float KnockbackMagnitude => _knockbackMagnitude;
        public float StunDuration => _stunDuration;
        
    }
}