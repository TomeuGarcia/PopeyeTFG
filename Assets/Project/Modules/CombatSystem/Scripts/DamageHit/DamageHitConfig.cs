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

        [SerializeField] private KnockbackHitConfig _knockbackHitConfig;
        
        
        public DamageHitTargetType DamageHitTargetTypeMask => _damageHitPreset.TargetMask;
        public int Damage => _damage;

        public KnockbackHitConfig KnockbackHitConfig => _knockbackHitConfig;

    }
}