using Popeye.ProjectHelpers;
using Project.Modules.CombatSystem.KnockbackSystem;
using UnityEngine;

namespace Popeye.Modules.CombatSystem
{
    [CreateAssetMenu(fileName = "DamageHitConfig_ATTACK_NAME", 
        menuName = ScriptableObjectsHelper.COMBATSYSTEM_PATH + "DamageHitConfig")]
    public class DamageHitConfig : ScriptableObject
    {
        [SerializeField] private DamageHitTargetPreset _damageHitPreset;
        [SerializeField, Range(0, 200)] private int _damage = 10;
        [SerializeField, Range(-10f, 10f)] private float _knockbackMagnitude = 0;
        [SerializeField, Range(0f, 10f)] private float _stunDuration = 0;

        [SerializeField] private KnockbackHitConfig _knockbackHitConfig;
        
        
        public DamageHitTargetType DamageHitTargetTypeMask => _damageHitPreset.TargetMask;
        public int Damage => _damage;
        public float StunDuration => _stunDuration;

        public KnockbackHitConfig KnockbackHitConfig => _knockbackHitConfig;

    }
}