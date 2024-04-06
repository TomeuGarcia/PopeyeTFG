using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    public class CanvasSceneTransitionFader : MonoBehaviour, ISceneTransitionScreenFader
    {
        [Header("COMPONENTS")]
        [SerializeField] private CanvasGroup _fadeGroup;
        
        [Header("CONFIGURATION")]
        [SerializeField] private TweenFadeConfig _fadeIn = TweenFadeConfig.FadeIn();
        [SerializeField] private TweenFadeConfig _fadeOut = TweenFadeConfig.FadeOut();
        [SerializeField, Range(0.0f, 10.0f)] private float _minimumTimeFededIn = 1.0f;

        private bool _isFading;
        
        
        private void Awake()
        {
            _fadeGroup.alpha = 0;
            _isFading = false;
        }

        public async UniTaskVoid FadeScreen(Func<bool> sceneFinishedLoading)
        {
            if (_isFading) return;
            _isFading = true;
            
            
            _fadeGroup.Fade(_fadeIn);
            //await _fadeGroup.Fade(_fadeIn).AsyncWaitForCompletion();
    
            
            float timeBeforeLoading = Time.time;
            await UniTask.WaitUntil(sceneFinishedLoading);

            
            float timeAfterLoading = Time.time;
            float remainingFadedInTime = _minimumTimeFededIn - (timeAfterLoading - timeBeforeLoading);
            if (remainingFadedInTime > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(remainingFadedInTime));
            }

            
            await _fadeGroup.Fade(_fadeOut).AsyncWaitForCompletion();

            _isFading = false;
        }
        
    }
}