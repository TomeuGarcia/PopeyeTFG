using UnityEngine;

namespace Project.Modules.CombatSystem
{
    [CreateAssetMenu(fileName = "New_PresetDamageHit", 
        menuName = "Popeye/CombatSystem/DamageHitTargetPreset")]
    public class DamageHitTargetPreset : ScriptableObject
    {
        [SerializeField] private DamageHitTargetType _targetMask;
        public DamageHitTargetType TargetMask => _targetMask;
    }
}