using System;
using Popeye.Core.Pool;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class Explosion : RecyclableObject
    {
        [SerializeField] private ExplosionHazardConfig _explosionHazardConfig;
        [SerializeField] private DamageTrigger _playerDamageTrigger;
        [SerializeField] private DamageTrigger _otherDamageTrigger;
        
        private ICombatManager _combatManager;
        private IParticleFactory _particleFactory;
        
        private ExplosionSize _size;

        private DamageHit PlayerDamage => _explosionHazardConfig.GetPlayerDamageHitBySize(_size);
        private DamageHit OtherDamage => _explosionHazardConfig.GetOtherDamageHitBySize(_size);
        

        private void Awake()
        {
            _playerDamageTrigger.Deactivate();
            _otherDamageTrigger.Deactivate();
        }

        internal override void Init()
        {
        }

        internal override void Release()
        {
            _playerDamageTrigger.Deactivate();
            _otherDamageTrigger.Deactivate();
        }


        public void Configure(ICombatManager combatManager, IParticleFactory particleFactory, ExplosionSize size)
        {
            _combatManager = combatManager;
            _particleFactory = particleFactory;
            _size = size;
            
            _playerDamageTrigger.Configure(_combatManager, PlayerDamage);
            _otherDamageTrigger.Configure(_combatManager, OtherDamage);
        }

        public void StartExplosion()
        {
            _playerDamageTrigger.Activate();
            _otherDamageTrigger.Activate();

            float size = _explosionHazardConfig.GetScaleBySize(_size);
            transform.localScale = Vector3.one * size;
            
            _explosionHazardConfig.ExplosionAudio.PlayExplosionSound(gameObject);
            
            Invoke("FinishExplosion", _explosionHazardConfig.LifeTime);
        }

        private void FinishExplosion()
        {
            Recycle();
        }
        
    }
}