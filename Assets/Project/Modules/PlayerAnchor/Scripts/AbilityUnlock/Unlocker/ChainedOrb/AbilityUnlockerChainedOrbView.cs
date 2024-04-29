using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Popeye.Core.Services.GameReferences;
using Popeye.Modules.AudioSystem;
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
        }


        public async UniTaskVoid PlayIdleAnimation()
        {
            _movingChainsSoundId = _audio.StartPlayingMovingChainsSound(gameObject);
        }
        
        public async UniTask PlayUnlockAbilityAnimation()
        {
            _audio.PlayHitSound(gameObject);
            await _chainedOrb.PlayDisappearAnimation(_orbTargetTransform.position);
            
            _audio.StopPlayingMovingChainsSound(_movingChainsSoundId);
            _audio.PlayBreakSound(gameObject);
            foreach (ChainedOrbChainView chainGroup in _chains)
            {
                chainGroup.PlayDisappearAnimation(_finishChainTargetHolder).Forget();
            }
            
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