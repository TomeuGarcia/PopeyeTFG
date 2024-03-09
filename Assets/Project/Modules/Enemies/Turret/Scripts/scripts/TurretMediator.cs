using System;
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

        public Transform PlayerTransform { get; private set; }
        
        private TurretMindEnemy _turretMind;
        [SerializeField] private ParabolicProjectile _parabolicProjectile;
        [SerializeField] private AreaDamageOverTime _damageableArea;
        [SerializeField] private TurretAnimatorController _turretAnimatorController;
        
        
        internal override void Init()
        {
            _turretShooting.Configure(this,_hazardsFactory,PlayerTransform);
            _enemyHealth.Configure(this);
            _enemyVisuals.Configure(ServiceLocator.Instance.GetService<IParticleFactory>());
            _turretAnimatorController.Configure(this);
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
            PlayerTransform = _playerTransform;
        }
        
        public override void OnPlayerClose()
        {
            throw new System.NotImplementedException();
        }

        public override void OnPlayerFar()
        {
            throw new System.NotImplementedException();
        }

        public override void DieFromOrder()
        {
            //  
        }

        public void StartShootingAnimation()
        {
            _turretAnimatorController.PlayShootingAnimation();
        }
        
        public void StopShootingAnimation()
        {
            _turretAnimatorController.StopShootingAnimation();
        }
        public override Vector3 Position { get; }
        
        public override void OnDeath(DamageHit damageHit)
        {
            _turretMind.Die();
            _enemyVisuals.PlayDeathEffects(damageHit);
        }
    }
}
