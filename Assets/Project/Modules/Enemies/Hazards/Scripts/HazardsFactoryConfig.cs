using Popeye.Core.Pool;
using Popeye.Modules.Enemies.Components;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards 
{
    [CreateAssetMenu(fileName = "HazardsFactoryConfig", 
        menuName = ScriptableObjectsHelper.HAZARDS_ASSET_PATH + "HazardsFactoryConfig")]
    public class HazardsFactoryConfig : ScriptableObject
    {
        [SerializeField] private ObjectPoolData<AreaDamageOverTime> _areaDamagePoolData;
        [Space(10)]
        [SerializeField] private ObjectPoolData<ParabolicProjectile> _parabolicProjectilePoolData;
        [Space(10)]
        [SerializeField] private ObjectPoolData<FlatStraightProjectile> _flatStraightProjectilePoolData;
        [Space(10)]
        [SerializeField] private ObjectPoolData<Explosion> _explosionProjectilePoolData;
        
        
        public ObjectPoolData<AreaDamageOverTime> AreaDamagePoolData => _areaDamagePoolData;
        public ObjectPoolData<ParabolicProjectile> ParabolicProjectilePoolData => _parabolicProjectilePoolData;
        public ObjectPoolData<FlatStraightProjectile> FlatStraightProjectilePoolData => _flatStraightProjectilePoolData;
        public ObjectPoolData<Explosion> ExplosionProjectilePoolData => _explosionProjectilePoolData;
        

    }
}