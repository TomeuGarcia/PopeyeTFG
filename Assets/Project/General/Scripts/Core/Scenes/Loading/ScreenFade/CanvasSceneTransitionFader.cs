using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    public class CanvasSceneTransitionFader : MonoBehaviour, ISceneTransitionScreenFader
    {
        [Header("COMPONENTS")]
        [SerializeField] private CanvasGroup _fadeGroup;
        [SerializeField] private CanvasGroup _textFadeGroup;

        [Header("CONFIGURATION")] 
        [Expandable] [SerializeField] private SceneTransitionScreenFaderConfig _config;


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
            
            float timeBeforeLoading = Time.time;

            await _fadeGroup.Fade(_config.FadeIn)
                .AsyncWaitForCompletion();
    
            _textFadeGroup.Fade(_config.LoadingTextFadeIn);
            

            for (int i = 0; i < _sceneFinishedLoadingAwaits.Count; ++i)
            {
                await UniTask.WaitUntil(_sceneFinishedLoadingAwaits[i]);
            }


            float timeAfterLoading = Time.time;
            float remainingFadedInTime = Mathf.Max(0, _config.MinimumTimeFadedIn - (timeAfterLoading - timeBeforeLoading));
            remainingFadedInTime += _config.ExtraTimeFadedIn;
            
            await UniTask.Delay(TimeSpan.FromSeconds(remainingFadedInTime));

            _textFadeGroup.Fade(_config.LoadingTextFadeOut);
            await _fadeGroup.Fade(_config.FadeOut).AsyncWaitForCompletion();

            _isFading = false;
            _sceneFinishedLoadingAwaits.Clear();
        }
        
        
        
        
        
    }
}