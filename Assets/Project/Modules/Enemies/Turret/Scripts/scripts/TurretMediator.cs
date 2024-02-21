using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public class TurretMediator : AEnemyMediator
    {
        [Header("COMPONENTS")] 
        [SerializeField] private TurretShooting _turretShooting;

        public Transform playerTransform { get; private set; }
        
        private Core.Pool.ObjectPool _projectilePool;
        private TurretMindEnemy _turretMind;
        [SerializeField] private ParabolicProjectile _parabolicProjectile;
        internal override void Init()
        {
            _projectilePool = new ObjectPool(_parabolicProjectile);
            _projectilePool.Init(15);
            _turretShooting.Configure(this,_projectilePool,playerTransform);
            _enemyHealth.Configure(this);
            _enemyVisuals.Configure(ServiceLocator.Instance.GetService<IParticleFactory>());
        }

        internal override void Release()
        {
            
        }

        public void SetTurretMind(TurretMindEnemy turretMind)
        {
            _turretMind = turretMind;
        }
        public void SetPlayerTransform(Transform _playerTransform)
        {
            playerTransform = _playerTransform;
        }
        
        public void SetObjectPool(Core.Pool.ObjectPool objectPool)
        {
            _projectilePool = objectPool;
        }
        public override void OnPlayerClose()
        {
            throw new System.NotImplementedException();
        }

        public override void OnPlayerFar()
        {
            throw new System.NotImplementedException();
        }

        public override Vector3 Position { get; }

        public override void OnDeath(DamageHit damageHit)
        {
            _turretMind.Die();
            _enemyVisuals.PlayDeathEffects(damageHit);
        }
    }
}
