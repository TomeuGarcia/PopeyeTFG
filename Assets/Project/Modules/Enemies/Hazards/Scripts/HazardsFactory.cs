using Popeye.Core.Pool;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class HazardsFactory : IHazardFactory
    {
        private readonly IParticleFactory _particleFactory;
        
        private readonly ObjectPool _areaDamagePool;
        private readonly ObjectPool _parabolicProjectilePool;
        private readonly ObjectPool _flatStraightProjectilePool;

        public HazardsFactory(HazardsFactoryConfig hazardsFactoryConfig, Transform parent, IParticleFactory particleFactory)
        {
            _particleFactory = particleFactory;
                        
            _areaDamagePool = new ObjectPool(hazardsFactoryConfig.AreaDamagePoolData.Prefab, parent);
            _areaDamagePool.Init(hazardsFactoryConfig.AreaDamagePoolData.InitialInstances);
            
            _parabolicProjectilePool = new ObjectPool(hazardsFactoryConfig.ParabolicProjectilePoolData.Prefab, parent);
            _parabolicProjectilePool.Init(hazardsFactoryConfig.ParabolicProjectilePoolData.InitialInstances);

            _flatStraightProjectilePool = new ObjectPool(hazardsFactoryConfig.FlatStraightProjectilePoolData.Prefab, parent);
            _flatStraightProjectilePool.Init(hazardsFactoryConfig.FlatStraightProjectilePoolData.InitialInstances);
            
            _flatStraightProjectilePool = new ObjectPool(hazardsFactoryConfig.ExplosionProjectilePoolData.Prefab, parent);
            _flatStraightProjectilePool.Init(hazardsFactoryConfig.ExplosionProjectilePoolData.InitialInstances);
        }

        public AreaDamageOverTime CreateDamageArea(Vector3 position, Quaternion rotation)
        {
           return _areaDamagePool.Spawn<AreaDamageOverTime>(position, rotation);
        }
        
        public ParabolicProjectile CreateParabolicProjectile(Transform origin, Transform targetPosition)
        {
            ParabolicProjectile projectile = _parabolicProjectilePool.Spawn<ParabolicProjectile>(origin.position, Quaternion.identity);
            projectile.SetParticleFactory(_particleFactory);
            projectile.PrepareShot(targetPosition,this,origin);
            return projectile;
        }

        public FlatStraightProjectile CreateFlatStraightProjectile(Vector3 position, Quaternion rotation)
        {
            FlatStraightProjectile projectile = _flatStraightProjectilePool.Spawn<FlatStraightProjectile>(position, rotation);
            projectile.Configure(_particleFactory);
            return projectile;
        }

        public Explosion CreateExplosion(Vector3 position, Quaternion rotation, ExplosionSize size)
        {
            Explosion explosion = _parabolicProjectilePool.Spawn<Explosion>(position, rotation);
            explosion.Configure(_particleFactory, size);
            return explosion;
        }
        
        
    }
}