using Popeye.Core.Pool;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards 
{
    [CreateAssetMenu(fileName = "HazardsFactoryConfig", 
        menuName = ScriptableObjectsHelper.HAZARDS_ASSET_PATH + "HazardsFactoryConfig")]
    public class HazardsFactoryConfig : ScriptableObject
    {
        [System.Serializable]
        public class PoolData<T> where T : RecyclableObject
        {
            [SerializeField] private T _prefab;
            [SerializeField] private int _initialInstances = 15;
            
            public ObjectPool ToObjectPool(Transform parent)
            {
                ObjectPool objectPool = new ObjectPool(_prefab, parent);
                objectPool.Init(_initialInstances);
                return objectPool;
            }
        }

        
        [SerializeField] private PoolData<AreaDamageOverTime> _areaDamagePoolData;
        [Space(10)]
        [SerializeField] private PoolData<ParabolicProjectile> _parabolicProjectilePoolData;
        [Space(10)]
        [SerializeField] private PoolData<FlatStraightProjectile> _flatStraightProjectilePoolData;
        [Space(10)]
        [SerializeField] private PoolData<Explosion> _explosionProjectilePoolData;
        
        
        public PoolData<AreaDamageOverTime> AreaDamagePoolData => _areaDamagePoolData;
        public PoolData<ParabolicProjectile> ParabolicProjectilePoolData => _parabolicProjectilePoolData;
        public PoolData<FlatStraightProjectile> FlatStraightProjectilePoolData => _flatStraightProjectilePoolData;
        public PoolData<Explosion> ExplosionProjectilePoolData => _explosionProjectilePoolData;
        

    }
}