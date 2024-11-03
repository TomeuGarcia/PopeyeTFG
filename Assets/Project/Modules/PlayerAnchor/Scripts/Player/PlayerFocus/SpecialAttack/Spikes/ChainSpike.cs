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
        [SerializeField] private Transform _meshRotateHolder;
        [SerializeField] private ParticleSystem _spawnParticles;

        [Header("CONFIG")]
        [SerializeField] private ChainSpikeConfig _config;
        private ChainSpikeViewConfig.SpikeSpawnAnimation SpawnAnimation => _config.ViewConfig.SpawnAnimation;
        

        private SpikePositioning _spikePositioning;
        private IAnchorDamageDealerListener _damageDealerListener;


        private void Awake()
        {
            ICombatManager combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
            _damageTrigger.Configure(new DamageDealer(combatManager), new DamageHit(_damageHitConfig));
        }
        
        internal override void Init() { }

        internal override void Release()
        {
            
        }

        public void InitBeforeAttack(SpikePositioning spikePositioning, IAnchorDamageDealerListener damageDealerListener)
        {
            _spikePositioning = spikePositioning;
            _damageDealerListener = damageDealerListener;
            _meshHolder.localScale = Vector3.zero;
            
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

            _damageTrigger.OnDamageDealt += _damageDealerListener.OnDamageDealt;
            _damageTrigger.Activate();            

            await _meshRotateHolder.LocalRotateBy(SpawnAnimation.ScaledUpRotation)
                .AsyncWaitForCompletion();
            await _meshHolder.Scale(SpawnAnimation.ScaleDown)
                .AsyncWaitForCompletion();
            await _meshHolder.Scale(SpawnAnimation.PostScaleDown)
                .AsyncWaitForCompletion();
            
            _damageTrigger.OnDamageDealt -= _damageDealerListener.OnDamageDealt;
            
            Recycle();
        }


    }
}