using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Pool;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor.Anchor;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spikes
{
    public class ChainSpike : RecyclableObject
    {
        public class SpikePositioning
        {
            public Vector3 position;
            public Vector3 normal;
        }
        
        [Header("DAMAGE")]
        [SerializeField] private DamageTrigger _damageTrigger;
        [SerializeField] private DamageHitConfig _damageHitConfig;

        [Header("VIEW")]
        [SerializeField] private Transform _meshHolder;
        [SerializeField] private ParticleSystem _spawnParticles;

        [Header("CONFIG")]
        [SerializeField] private ChainSpikeConfig _config;
        private ChainSpikeViewConfig.SpikeSpawnAnimation SpawnAnimation => _config.ViewConfig.SpawnAnimation;
        

        private SpikePositioning _spikePositioning;
        private IAnchorMediator _anchorMediator;

        private void Awake()
        {
            ICombatManager combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
            _damageTrigger.Configure(combatManager, new DamageHit(_damageHitConfig));
        }
        
        internal override void Init() { }

        internal override void Release()
        {
            _damageTrigger.OnDamageDealt -= _anchorMediator.OnDamageDealt;
        }

        public void InitBeforeAttack(SpikePositioning spikePositioning, IAnchorMediator anchorMediator)
        {
            _spikePositioning = spikePositioning;
            _anchorMediator = anchorMediator;
            _meshHolder.localScale = Vector3.zero;
            
            _damageTrigger.OnDamageDealt += _anchorMediator.OnDamageDealt;
            _damageTrigger.Deactivate();
        }

        private void LateUpdate()
        {
            if (_spikePositioning == null) return;
            
            transform.position = _spikePositioning.position;
            transform.forward = _spikePositioning.normal;
        }

        public async UniTaskVoid PlaySpawnAnimation()
        {            
            await _meshHolder.Scale(SpawnAnimation.PreScaleUp)
                .AsyncWaitForCompletion();            
            _spawnParticles.Play();
            await _meshHolder.Scale(SpawnAnimation.ScaleUp)
                .AsyncWaitForCompletion();

            _damageTrigger.Activate();            

            await _meshHolder.LocalRotateBy(SpawnAnimation.ScaledUpRotation)
                .AsyncWaitForCompletion();
            await _meshHolder.Scale(SpawnAnimation.ScaleDown)
                .AsyncWaitForCompletion();
            await _meshHolder.Scale(SpawnAnimation.PostScaleDown)
                .AsyncWaitForCompletion();
            
            Recycle();
        }


    }
}