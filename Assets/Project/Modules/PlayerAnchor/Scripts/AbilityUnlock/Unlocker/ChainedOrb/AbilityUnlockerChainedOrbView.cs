using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using Unity.Mathematics;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class AbilityUnlockerChainedOrbView : MonoBehaviour, IPlayerAbilityUnlockerView
    {
        [Header("VIEW CONFIG")]
        [Expandable] [SerializeField] private ChainedOrbViewConfig _viewConfig;
        
        [Header("HOLDERS")]
        [SerializeField] private Transform _originalChainTargetHolder;
        [SerializeField] private Transform _finishChainTargetHolder;
        [SerializeField] private List<Transform> _orbitalChains = new();
        [SerializeField] private Transform _orbitalChains0;
        [SerializeField] private Transform _orbitalChains1;
        [SerializeField] private Transform _orbitalChains2;
        
        [Header("SUB-VIEWS")]
        [SerializeField] private ChainedOrbView _chainedOrb;
        [SerializeField] private ChainedOrbChainView[] _chains;

        [Header("PARTICLES")] 
        [SerializeField] private Transform _orbHitParticlesHolder;
        [SerializeField] private Transform _pickAbilityParticlesHolder;
        private ParticleSystem _orbHitPS;
        private ParticleSystem _pickAbilityPS;
        
        private Transform _orbTargetTransform;
        private IAbilityUnlockerChristalAudio _audio;
        private LastingFMODSound.SoundId _movingChainsSoundId;

        private IParticleFactory _particleFactory;

        private void OnDestroy()
        {
            _audio.StopPlayingMovingChainsSound(_movingChainsSoundId);            
        }

        
        public void Configure(IGameReferences gameReferences, IAbilityUnlockerChristalAudio audio, 
            GeneralInitializePlayerAbilityUnlockerConfig.Ability upgradeType)
        {
            _orbTargetTransform = gameReferences.GetPlayerPositionTransform();
            _audio = audio;

            ChainedOrbViewConfig.UpgradeTypeToViewData typeViewData =
                _viewConfig.GetViewDataFromUpgradeType(upgradeType);
            
            _chainedOrb.Init(_viewConfig, typeViewData);
            foreach (ChainedOrbChainView chainGroup in _chains)
            {
                chainGroup.Init(_viewConfig, typeViewData);
            }

            _orbHitPS = Instantiate(typeViewData.OrbHitParticlesPrefab, _orbHitParticlesHolder);
            _pickAbilityPS = Instantiate(typeViewData.PickAbilityParticlesPrefab, _pickAbilityParticlesHolder);

            _particleFactory = ServiceLocator.Instance.GetService<IParticleFactory>();
            _movingChainsSoundId = _audio.StartPlayingMovingChainsSound(gameObject);
        }


        public async UniTaskVoid PlayIdleAnimation()
        {
            for (int i = 0; i < _orbitalChains.Count; i++)
            {
                _orbitalChains[i].DOBlendableLocalRotateBy(Vector3.up, _viewConfig.OrbitalChainRotationSpeeds[i]).SetLoops(-1);
            }
        }
        
        public async UniTask PlayUnlockAbilityAnimation()
        {
            _audio.PlayHitSound(gameObject);
            Transform sparks = _particleFactory.Create(_viewConfig.OnHitSparklesParticleType, Vector3.zero, quaternion.identity, transform);
            sparks.LookAt(_orbTargetTransform.position);
            sparks.transform.position += _viewConfig.SparkOffset;
            
            foreach (ChainedOrbChainView chainGroup in _chains)
            {
                chainGroup.PlayDisappearAnimation(_finishChainTargetHolder).Forget();
            }
            await _chainedOrb.PlayDisappearAnimation(_orbTargetTransform.position);
            
            _audio.StopPlayingMovingChainsSound(_movingChainsSoundId);
            _audio.PlayBreakSound(gameObject);
            
            _orbHitPS.Play();

            await _chainedOrb.MoveToTarget(_orbTargetTransform);
            _audio.PlayCollectedSound(gameObject);
            
            _pickAbilityPS.Play();
            await UniTask.WaitUntil(() => !_pickAbilityPS.isEmitting);
        }


        [Button]
        private void DebugReset()
        {
            _chainedOrb.ResetState();
        
            foreach (ChainedOrbChainView chainGroup in _chains)
            {
                chainGroup.ResetState(_originalChainTargetHolder);
            }
        }
        [Button]
        private void DebugPlay()
        {
            PlayUnlockAbilityAnimation().Forget();
        }

    }
}