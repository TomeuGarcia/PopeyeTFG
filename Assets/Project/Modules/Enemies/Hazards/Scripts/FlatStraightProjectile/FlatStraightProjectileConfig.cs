using Popeye.Modules.CombatSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [CreateAssetMenu(fileName = "FlatStraightProjectileConfig", 
        menuName = ScriptableObjectsHelper.HAZARDS_ASSET_PATH + "FlatStraightProjectileConfig")]
    public class FlatStraightProjectileConfig : ScriptableObject
    {
        [SerializeField] private DamageHitConfig _damageHitConfig;
        
        
        public DamageHitConfig DamageHitConfig => _damageHitConfig;
    }
}