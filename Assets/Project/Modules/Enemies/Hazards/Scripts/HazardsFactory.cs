using Popeye.Core.Pool;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class HazardsFactory : IHazardFactory
    {
        private ObjectPool _projectilePool;
        private ObjectPool _areaDamagePool;
        private HazardsFactoryConfig _hazardsFactoryConfig;
        public HazardsFactory(HazardsFactoryConfig hazardsFactoryConfig, Transform parent)
        {
            _hazardsFactoryConfig = hazardsFactoryConfig;
            _projectilePool = new ObjectPool(_hazardsFactoryConfig.ParabolicProjectilePrefab,parent);
            _projectilePool.Init(_hazardsFactoryConfig.ParabolicProjectilesInitialInstances);
            _areaDamagePool = new ObjectPool(_hazardsFactoryConfig.AreaDamageOverTimePrefab,parent);
            _areaDamagePool.Init(_hazardsFactoryConfig.AreaDamageOverTimeInitialInstances);
        }
        public ParabolicProjectile CreateParabolicProjectile(Transform origin, Transform targetPosition)
        {
            var projectile = _projectilePool.Spawn<ParabolicProjectile>(origin.position, Quaternion.identity);
            projectile.PrepareShot(targetPosition,this,origin);
            return projectile;
        }

        public AreaDamageOverTime CreateDamageArea(Vector3 position, Quaternion rotation)
        {
           return _areaDamagePool.Spawn<AreaDamageOverTime>(position, rotation);
        }
    }
}