using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class AbilityUnlockerChristalView : MonoBehaviour, IPlayerAbilityUnlockerView
    {        
        [Header("COMPONENTS")]
        [SerializeField] private Transform _christalTransform;
        [SerializeField] private Transform _chainTransform;
        [SerializeField] private GameObject _chainsHolder;
        [SerializeField] private GameObject _christalHolder;
        [SerializeField] private ParticleSystem _particleSystem;
        
        [Header("CONFIG")]
        [Expandable] [SerializeField] private AbilityUnlockerChristalViewConfig _viewConfig;


        private bool _playIdleAnimation;




        [Button()]
        public async UniTaskVoid PlayIdleAnimation()
        {
            _playIdleAnimation = true;
            while (_playIdleAnimation)
            {

                await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.IdleDelay));

                if (!_playIdleAnimation) return;
                
                _chainTransform.PunchRotation(_viewConfig.IdleChainPunch, true);

            }
        }
        
        [Button()]
        public async UniTaskVoid PlayUnlockAbilityAnimation()
        {
            _playIdleAnimation = false;
            _chainsHolder.SetActive(true);
            _christalHolder.SetActive(true);
            
            _christalTransform.PunchScale(_viewConfig.UnlockScalePunch);
            await _christalTransform.PunchRotation(_viewConfig.UnlockRotationPunch)
                .AsyncWaitForCompletion();

            _viewConfig.ExplodingChainSharedMaterial.SetFloat(_viewConfig.ExplodeStartTimePropertyId, Time.time);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.ExplodeDelay));
            _chainsHolder.SetActive(false);
            _christalHolder.SetActive(false);
            
            _particleSystem.Play();
        }
        
    }
}