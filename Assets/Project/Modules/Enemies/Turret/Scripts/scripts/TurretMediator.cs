using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.Camera;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Modules.Enemies.Turret;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public class TurretMediator : AEnemyMediator
    {
        [Header("COMPONENTS")] 
        [SerializeField] private TurretShooting _turretShooting;

        public Transform PlayerTransform { get; private set; }
        public override Vector3 Position { get; }

        private TurretMindEnemy _turretMind;
        [SerializeField] private ParabolicProjectile _parabolicProjectile;
        [SerializeField] private AreaDamageOverTime _damageableArea;
        [SerializeField] private TurretAnimatorController _turretAnimatorController;
        [SerializeField] private TurretAnimationCallback _turretAnimatorCallback;
        [SerializeField] private TurretSpineRotator _turretSpineRotator;
        [SerializeField] private PowerBoostDropConfig _powerBoostDrop;
        private IPowerBoostDropFactory _powerBoostDropFactory;
        [SerializeField] private TurretSoundConfig _turretSounds;

        internal override void Init()
        {
            _turretShooting.Configure(this,_hazardsFactory,PlayerTransform);
            _enemyHealth.Configure(this);
            _enemyVisuals.Configure(ServiceLocator.Instance.GetService<IParticleFactory>(), ServiceLocator.Instance.GetService<ICameraFunctionalities>().CameraShaker);
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
        public void AppearAnimation(bool playerWasTooClose)
        {
            _turretSounds.PlayTurretDigUp(gameObject);
            _turretAnimatorController.AppearAnimation();

            if (!playerWasTooClose)
            {
                PlayerSeen();
            }            
        }
        public void HideAnimation(bool playerIsTooClose, bool playerIsTooFar)
        {
            _turretSounds.PlayTurretDigDown(gameObject);
            _turretAnimatorController.HideAnimation();

            if (playerIsTooFar)
            {
                InvokeEnemyStopsFightingPlayer();
            }            
        }
        
        public override void OnDeath(DamageHitResult damageHitResult)
        {
            _powerBoostDropFactory.Create(transform.position, Quaternion.identity, _powerBoostDrop);
            _turretMind.Die();
            _turretSounds.PlayTurretDeath(gameObject);
            _enemyVisuals.PlayDeathEffects(damageHitResult.DamageHit);
            InvokeEnemyStopsFightingPlayer();
        }

        public void LookAtPlayer(float delta)
        {
            _turretSpineRotator.LookAtPlayer(delta);
        }
        public void Shoot()
        {
            _turretSounds.PlayTurretShot(gameObject);
            _turretShooting.Shoot();
        }

        public void MultipleShoot()
        {
            _turretSounds.PlayTurretShot(gameObject);
            _turretShooting.MultipleShoot();
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

        public void PlayerSeen()
        {
            OnSeePlayer();
        }
        
        public void SetInvulnerable()
        {
            _enemyHealth.SetIsInvulnerable(true);
        }
    }
}
