using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor.Anchor;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spikes
{
    public class ChainSpike : MonoBehaviour
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
        [SerializeField] private TweenConfig _preScaleUpTween;
        [SerializeField] private TweenConfig _scaleUpTween;
        [SerializeField] private float _delayBeforeScaleDown = 0.2f;
        [SerializeField] private TweenConfig _scaleDownTween;
        [SerializeField] private TweenConfig _postScaleDownTween;

        private SpikePositioning _spikePositioning;
        private IAnchorMediator _anchorMediator;

        private void Awake()
        {
            ICombatManager combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
            _damageTrigger.Configure(combatManager, new DamageHit(_damageHitConfig));
            _damageTrigger.Deactivate();
        }
        
        private void OnDestroy()
        {
            _damageTrigger.OnDamageDealt -= _anchorMediator.OnDamageDealt;
        }

        private void LateUpdate()
        {
            if (_spikePositioning == null) return;
            
            transform.position = _spikePositioning.position;
            transform.forward = _spikePositioning.normal;
        }

        public void Init(SpikePositioning spikePositioning, IAnchorMediator anchorMediator)
        {
            _spikePositioning = spikePositioning;
            _anchorMediator = anchorMediator;
            _meshHolder.localScale = Vector3.zero;
            
            _damageTrigger.OnDamageDealt += _anchorMediator.OnDamageDealt;
        }

        public async UniTaskVoid PlaySpawnAnimation()
        {            
            await _meshHolder.Scale(_preScaleUpTween)
                .AsyncWaitForCompletion();
            await _meshHolder.Scale(_scaleUpTween)
                .AsyncWaitForCompletion();
            
            _damageTrigger.Activate();            

            _meshHolder.DOBlendableLocalRotateBy(Vector3.forward * 180f, _delayBeforeScaleDown).SetEase(Ease.InOutSine);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_delayBeforeScaleDown));
            await _meshHolder.Scale(_scaleDownTween)
                .AsyncWaitForCompletion();
            await _meshHolder.Scale(_postScaleDownTween)
                .AsyncWaitForCompletion();
            
            Destroy(gameObject);
        }
    }
}