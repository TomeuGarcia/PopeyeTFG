using System;
using System.Collections.Generic;
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

        private List<Func<bool>> _sceneFinishedLoadingAwaits;
        
        
        private void Awake()
        {
            _fadeGroup.alpha = 0;
            _isFading = false;

            _sceneFinishedLoadingAwaits = new List<Func<bool>>(2);
        }

        public async UniTaskVoid FadeScreen(Func<bool> sceneFinishedLoading)
        {
            _sceneFinishedLoadingAwaits.Add(sceneFinishedLoading);
            
            if (_isFading) return;


            await DoFadeScreen();
        }
        
        private async UniTask DoFadeScreen()
        {
            _isFading = true;
            
            
            _fadeGroup.Fade(_fadeIn);
            //await _fadeGroup.Fade(_fadeIn).AsyncWaitForCompletion();
    
            
            float timeBeforeLoading = Time.time;

            for (int i = 0; i < _sceneFinishedLoadingAwaits.Count; ++i)
            {
                await UniTask.WaitUntil(_sceneFinishedLoadingAwaits[i]);
                Debug.Log("waited " + i);
            }


            float timeAfterLoading = Time.time;
            float remainingFadedInTime = _minimumTimeFededIn - (timeAfterLoading - timeBeforeLoading);
            if (remainingFadedInTime > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(remainingFadedInTime));
            }

            
            await _fadeGroup.Fade(_fadeOut).AsyncWaitForCompletion();

            _isFading = false;
            _sceneFinishedLoadingAwaits.Clear();
        }
        
        
        
        
        
    }
}