using Popeye.Core.Pool;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Components;
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
        private readonly ObjectPool _explosionPool;

        public HazardsFactory(HazardsFactoryConfig hazardsFactoryConfig, Transform parent, 
            ICombatManager combatManager, IParticleFactory particleFactory)
        {
            _combatManager = combatManager;
            _particleFactory = particleFactory;

            _areaDamagePool = hazardsFactoryConfig.AreaDamagePoolData.ToObjectPool(parent);
            _parabolicProjectilePool = hazardsFactoryConfig.ParabolicProjectilePoolData.ToObjectPool(parent);
            _flatStraightProjectilePool = hazardsFactoryConfig.FlatStraightProjectilePoolData.ToObjectPool(parent);
            _explosionPool = hazardsFactoryConfig.ExplosionProjectilePoolData.ToObjectPool(parent);
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
            Explosion explosion = _explosionPool.Spawn<Explosion>(position, rotation);
            explosion.Configure(_combatManager, _particleFactory, size);
            explosion.StartExplosion();
            return explosion;
        }
        
        
    }
}