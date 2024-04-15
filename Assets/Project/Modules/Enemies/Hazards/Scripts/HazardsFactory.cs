using Popeye.Core.Pool;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class HazardsFactory : IHazardFactory
    {
        private readonly ICombatManager _combatManager;
        private readonly IParticleFactory _particleFactory;
        
        private readonly ObjectPool _areaDamagePool;
        private readonly ObjectPool _parabolicProjectilePool;
        private readonly ObjectPool _flatStraightProjectilePool;
        private readonly ObjectPool _explosionProjectilePool;

        public HazardsFactory(HazardsFactoryConfig hazardsFactoryConfig, Transform parent, 
            ICombatManager combatManager, IParticleFactory particleFactory)
        {
            _combatManager = combatManager;
            _particleFactory = particleFactory;
                        
            _areaDamagePool = new ObjectPool(hazardsFactoryConfig.AreaDamagePoolData.Prefab, parent);
            _areaDamagePool.Init(hazardsFactoryConfig.AreaDamagePoolData.InitialInstances);
            
            _parabolicProjectilePool = new ObjectPool(hazardsFactoryConfig.ParabolicProjectilePoolData.Prefab, parent);
            _parabolicProjectilePool.Init(hazardsFactoryConfig.ParabolicProjectilePoolData.InitialInstances);

            _flatStraightProjectilePool = new ObjectPool(hazardsFactoryConfig.FlatStraightProjectilePoolData.Prefab, parent);
            _flatStraightProjectilePool.Init(hazardsFactoryConfig.FlatStraightProjectilePoolData.InitialInstances);
            
            _explosionProjectilePool = new ObjectPool(hazardsFactoryConfig.ExplosionProjectilePoolData.Prefab, parent);
            _explosionProjectilePool.Init(hazardsFactoryConfig.ExplosionProjectilePoolData.InitialInstances);
        }

        public AreaDamageOverTime CreateDamageArea(Vector3 position, Quaternion rotation)
        {
           return _areaDamagePool.Spawn<AreaDamageOverTime>(position, rotation);
        }
        
        public ParabolicProjectile CreateParabolicProjectile(Transform origin, Transform targetPosition, float maxDistance,float minDistance)
        {
            ParabolicProjectile projectile = _parabolicProjectilePool.Spawn<ParabolicProjectile>(origin.position, Quaternion.identity);
            projectile.SetParticleFactory(_particleFactory);
            projectile.PrepareShot(targetPosition,this,origin,maxDistance,minDistance);
            return projectile;
        }

        public FlatStraightProjectile CreateFlatStraightProjectile(Vector3 position, Quaternion rotation)
        {
            FlatStraightProjectile projectile = _flatStraightProjectilePool.Spawn<FlatStraightProjectile>(position, rotation);
            projectile.Configure(_combatManager, _particleFactory);
            return projectile;
        }

        public Explosion CreateExplosion(Vector3 position, Quaternion rotation, ExplosionSize size)
        {
            Explosion explosion = _explosionProjectilePool.Spawn<Explosion>(position, rotation);
            explosion.Configure(_combatManager, _particleFactory, size);
            return explosion;
        }
        
        
    }
}