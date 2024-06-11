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
        [SerializeField] private DamageTrigger _playerDamageTrigger;
        [SerializeField] private DamageTrigger _othersDamageTrigger;
        [SerializeField] private InterfaceReference<IFlatStraightProjectileView, MonoBehaviour> _view;
        [SerializeField] private PhysicsMovementBehaviour _physicsMovement;
        private IFlatStraightProjectileView View => _view.Value;
        private IFlatStraightProjectileAudio Audio => _config.Audio;

        private Lifetime _lifetime;

        private void Awake()
        {
            _othersDamageTrigger.OnDamageDealt += OnDamageDealtToOther;
        }
        private void OnDestroy()
        {
            _othersDamageTrigger.OnDamageDealt -= OnDamageDealtToOther;
        }

        internal override void Init() { }

        internal override void Release()
        {
            _playerDamageTrigger.Deactivate();
            _othersDamageTrigger.Deactivate();
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
        }

        private void SetupDamageTrigger(ICombatManager combatManager)
        {
            _playerDamageTrigger.Configure(new DamageDealer(combatManager), new DamageHit(_config.PlayerDamageHitConfig));
            _playerDamageTrigger.Activate();
            
            _othersDamageTrigger.Configure(new DamageDealer(combatManager), new DamageHit(_config.OthersDamageHitConfig));
            _othersDamageTrigger.Activate();            
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

        private void OnDamageDealtToOther(DamageHitResult damageHitResult)
        {
            _config.Audio.PlayDealDamageSound(damageHitResult.DamageHitTargetGameObject);
        }
    }
}