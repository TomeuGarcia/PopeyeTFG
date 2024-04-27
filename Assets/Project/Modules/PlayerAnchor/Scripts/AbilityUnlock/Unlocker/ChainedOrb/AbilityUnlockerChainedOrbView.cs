using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Popeye.Core.Services.GameReferences;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class AbilityUnlockerChainedOrbView : MonoBehaviour, IPlayerAbilityUnlockerView
    {

        [SerializeField] private Transform _originalChainTargetHolder;
        [SerializeField] private Transform _finishChainTargetHolder;
        [SerializeField] private ChainedOrbView _chainedOrb;
        [SerializeField] private ChainedOrbChainView[] _chains;
        [SerializeField] private ParticleSystem _freedBurstPS;
        [SerializeField] private ParticleSystem _contactPS;
        
        private Transform _orbTargetTransform;
        private IAbilityUnlockerChristalAudio _audio;

        private void Awake()
        {
            _chainedOrb.Init();
            foreach (ChainedOrbChainView chainGroup in _chains)
            {
                chainGroup.Init();
            }
        }
        
        public void Configure(IGameReferences gameReferences, IAbilityUnlockerChristalAudio audio)
        {
            _orbTargetTransform = gameReferences.GetPlayerPositionTransform();
            _audio = audio;
        }


        public async UniTaskVoid PlayIdleAnimation()
        {
            
        }
        public async UniTask PlayUnlockAbilityAnimation()
        {
            _audio.PlayHitSound(gameObject);
            await _chainedOrb.PlayDisappearAnimation(_orbTargetTransform.position);
            
            _audio.PlayBreakSound(gameObject);
            foreach (ChainedOrbChainView chainGroup in _chains)
            {
                chainGroup.PlayDisappearAnimation(_finishChainTargetHolder).Forget();
            }
            
            _freedBurstPS.Play();

            await _chainedOrb.MoveToTarget(_orbTargetTransform);
            _audio.PlayCollectedSound(gameObject);
            
            _contactPS.Play();
            await UniTask.WaitUntil(() => !_contactPS.isEmitting);
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