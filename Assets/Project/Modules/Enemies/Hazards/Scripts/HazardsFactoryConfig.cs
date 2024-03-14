using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards 
{
    [CreateAssetMenu(fileName = "HazardsFactoryConfig", 
        menuName = ScriptableObjectsHelper.HAZARDS_ASSET_PATH + "HazardsFactoryConfig")]
    public class HazardsFactoryConfig : ScriptableObject
    {
        [Header("Area damage over time")]
        [SerializeField] private AreaDamageOverTime _areaDamageOverTimePrefab;
        [SerializeField] private int _areaDamageOverTimeInitialInstances;
        
        [Header("Parabolic projectile")]
        [SerializeField] private ParabolicProjectile _parabolicProjectilePrefab;
        [SerializeField] private int _parabolicProjectilesInitialInstances;

        public AreaDamageOverTime AreaDamageOverTimePrefab => _areaDamageOverTimePrefab;
        public ParabolicProjectile ParabolicProjectilePrefab => _parabolicProjectilePrefab;
        public int AreaDamageOverTimeInitialInstances => _areaDamageOverTimeInitialInstances;
        public int ParabolicProjectilesInitialInstances => _parabolicProjectilesInitialInstances;

    }
}