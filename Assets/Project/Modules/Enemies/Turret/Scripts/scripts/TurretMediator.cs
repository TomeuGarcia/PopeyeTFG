using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops;
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
        [SerializeField] private TurretAnimationCallback _turretAnimatorCallback;
        [SerializeField] private TurretSpineRotator _turretSpineRotator;
        [SerializeField] private PowerBoostDropConfig _powerBoostDrop;
        private IPowerBoostDropFactory _powerBoostDropFactory;
        
        internal override void Init()
        {
            _turretShooting.Configure(this,_hazardsFactory,PlayerTransform);
            _enemyHealth.Configure(this);
            _enemyVisuals.Configure(ServiceLocator.Instance.GetService<IParticleFactory>());
            _turretAnimatorController.Configure(this);
            _turretAnimatorCallback.Configure(this);
            _turretSpineRotator.Configure(this,PlayerTransform);
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
        public void SetBoostDropFactory(IPowerBoostDropFactory powerBoostDropFactory)
        {
            _powerBoostDropFactory = powerBoostDropFactory;
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

        public void StartIdleAnimation()
        {
            _turretAnimatorController.PlayIdleAnimation();
        }
        public void StoptIdleAnimation()
        {
            _turretAnimatorController.StopIdleAnimation();
        }
        
        public void StopShootingAnimation()
        {
            _turretAnimatorController.StopShootingAnimation();
        }
        public void AppearAnimation()
        {
            _turretAnimatorController.AppearAnimation();
        }
        public void HideAnimation()
        {
            _turretAnimatorController.HideAnimation();
        }
        public override Vector3 Position { get; }
        
        public override void OnDeath(DamageHit damageHit)
        {
            _powerBoostDropFactory.Create(transform.position, Quaternion.identity, _powerBoostDrop);
            _turretMind.Die();
            _enemyVisuals.PlayDeathEffects(damageHit);
        }

        public void LookAtPlayer(float delta)
        {
            _turretSpineRotator.LookAtPlayer(delta);
        }
        public void Shoot()
        {
            _turretShooting.Shoot();
        }

        public void SetOutOfGround()
        {
            _turretShooting.SetOutOfGround();
        }
        public void SetInsideGround()
        {
            _turretShooting.InsideGround();
        }

        public void SetVulnerable()
        {
            _enemyHealth.SetIsInvulnerable(false);
        }
        
        public void SetInvulnerable()
        {
            _enemyHealth.SetIsInvulnerable(true);
        }
    }
}
