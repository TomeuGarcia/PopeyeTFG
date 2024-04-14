using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Timers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class AbilityUnlockerChristalView : MonoBehaviour, IPlayerAbilityUnlockerView
    {        
        [Header("COMPONENTS")]
        [SerializeField] private Transform _christalTransform;
        [SerializeField] private Transform _chainTransform;
        [SerializeField] private Transform _coreHolderTransform;
        [SerializeField] private GameObject _coreSphere;
        [SerializeField] private GameObject _chainsHolder;
        [SerializeField] private GameObject _christalHolder;
        [SerializeField] private ParticleSystem _chainExplosionPS;
        [SerializeField] private ParticleSystem _coreContactPS;
        
        [Header("CONFIG")]
        [Expandable] [SerializeField] private AbilityUnlockerChristalViewConfig _viewConfig;

        private IAbilityUnlockerChristalAudio _audio;
        
        private bool _playIdleAnimation;
        private Transform _coreTargetTransform;

        public void Configure(IGameReferences gameReferences, IAbilityUnlockerChristalAudio audio)
        {
            _coreTargetTransform = gameReferences.GetPlayerPositionTransform();
            _audio = audio;
                        
            ResetViewState();
        }


        [Button()]
        public async UniTaskVoid PlayIdleAnimation()
        {
            if (_playIdleAnimation) return;
            
            _playIdleAnimation = true;
            while (_playIdleAnimation)
            {

                await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.IdleDelay));

                if (!_playIdleAnimation) return;
                
                _chainTransform.PunchRotation(_viewConfig.IdleChainPunch, true);

            }
        }
        
        [Button()]
        public async UniTask PlayUnlockAbilityAnimation()
        {
            _playIdleAnimation = false;

            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.UnlockDelay));
            
            _audio.PlayHitSound(gameObject);
            _christalTransform.PunchScale(_viewConfig.UnlockScalePunch);
            await _christalTransform.PunchRotation(_viewConfig.UnlockRotationPunch)
                .AsyncWaitForCompletion();

            _audio.PlayBreakSound(gameObject);
            _viewConfig.ExplodingChainSharedMaterial.SetFloat(_viewConfig.ExplodeStartTimePropertyId, Time.time);
            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.ExplodeDelay));
            
            _chainsHolder.SetActive(false);
            _christalHolder.SetActive(false);
            _chainExplosionPS.Play();
            await CoreMoveToTarget();
            
            //ResetViewState(1.0f).Forget(); // Debug to test animation with button
        }

        private async UniTask CoreMoveToTarget()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.CoreMoveDelay));

            _coreSphere.transform.PunchScale(_viewConfig.CoreScalePunch);
            Vector3 startPosition = _coreHolderTransform.position;
            
            Timer moveToTargetTimer = new Timer(_viewConfig.CoreMoveDuration);
            while (!moveToTargetTimer.HasFinished())
            {
                float t = _viewConfig.CoreMoveEase.Evaluate(moveToTargetTimer.GetCounterRatio01());
                Vector3 endPosition = _coreTargetTransform.position;

                Vector3 currentPosition = Vector3.LerpUnclamped(startPosition, endPosition, t);
                _coreHolderTransform.position = currentPosition;

                Vector3 currentToEnd = endPosition - currentPosition;
                float currentToEndDistance = currentToEnd.magnitude;
                if (currentToEndDistance > 0.001f)
                {
                    _coreHolderTransform.forward = currentToEnd / currentToEndDistance;
                }

                moveToTargetTimer.Update(Time.deltaTime);
                await UniTask.Yield();
            }
            
            _audio.PlayCollectedSound(gameObject);
            _coreSphere.SetActive(false);
            _coreContactPS.Play();

            await UniTask.WaitUntil(() => !_coreContactPS.isEmitting);
        }


        private async UniTaskVoid ResetViewState(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            ResetViewState();
            
            PlayIdleAnimation().Forget();
        }

        private void ResetViewState()
        {
            _chainsHolder.SetActive(true);
            _christalHolder.SetActive(true);
            _coreSphere.SetActive(true);
            _coreHolderTransform.localPosition = Vector3.zero;
            
            _viewConfig.ExplodingChainSharedMaterial.SetFloat(_viewConfig.ExplodeStartTimePropertyId, -10);
        }
    }
}