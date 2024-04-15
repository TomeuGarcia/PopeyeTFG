using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using Popeye.Core.Pool;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using Popeye.Timers;
using Project.PhysicsMovement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.Enemies.Hazards
{
    public class FlatStraightProjectile : RecyclableObject
    {
        [SerializeField] private FlatStraightProjectileConfig _config;
        [SerializeField] private DamageTrigger _damageTrigger;
        [SerializeField] private InterfaceReference<IFlatStraightProjectileView, MonoBehaviour> _view;
        [SerializeField] private PhysicsMovementBehaviour _physicsMovement;
        private IFlatStraightProjectileView View => _view.Value;
        private IFlatStraightProjectileAudio Audio => _config.Audio;

        private Lifetime _lifetime;
        
        internal override void Init() { }

        internal override void Release()
        {
            _damageTrigger.Deactivate();
            _damageTrigger.OnDamageDealt -= OnDamageDealtEvent;
            _damageTrigger.OnEnterFinish -= OnTriggerEnterFinishEvent;
            
            Audio.StopMovingSound();
        }

        public void Configure(ICombatManager combatManager, IParticleFactory particleFactory)
        {
            SetupDamageTrigger(combatManager);
            _lifetime = new Lifetime(_config.MaximumLifetime, OnLifetimeFinish);
            
            View.Configure(particleFactory, _config.ViewConfig);
            View.ResetView();
            View.PlayStartShootAnimation();
            
            _physicsMovement.UseGravity(false);
            _physicsMovement.MovementSpeed = _config.MovementSpeed;
            _physicsMovement.MovementDirection = transform.forward;
            
            Audio.Configure();
            Audio.PlayMovingSound(gameObject);
        }

        private void SetupDamageTrigger(ICombatManager combatManager)
        {
            _damageTrigger.Configure(combatManager, new DamageHit(_config.DamageHitConfig));
            _damageTrigger.Activate();
            _damageTrigger.OnDamageDealt += OnDamageDealtEvent;
            _damageTrigger.OnEnterFinish += OnTriggerEnterFinishEvent;
        }
        

        private void OnDamageDealtEvent(DamageHitResult damageHitResult)
        {
            
        }
        
        private void OnTriggerEnterFinishEvent()
        {
            _damageTrigger.Deactivate();
            _lifetime.Cancel();
            Audio.PlayObjectContactSound(gameObject);
            
            DoHitObjectSequence().Forget();
        }

        private async UniTaskVoid DoHitObjectSequence()
        {
            await View.PlayObjectContactAnimation();
            StartDisappearing();
        }

        private void OnLifetimeFinish()
        {
            Audio.PlayLifetimeEndSound(gameObject);
            StartDisappearing();
        }

        private void StartDisappearing()
        {
            View.PlayDisappearAnimation(_config.DisappearDuration);
            Disappear().Forget();
        }
        private async UniTaskVoid Disappear()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.DisappearDuration));
            Recycle();
        }
        
        
    }
}