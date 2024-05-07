using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Project.General.Scripts.Core.Services.ScreenFade
{
    public class CanvasScreenFadeService : MonoBehaviour, IScreenFadeService
    {
        [Header("COMPONENTS")]
        [SerializeField] private CanvasGroup _fadeGroup;
        
        [Header("CONFIG")]
        [SerializeField] private TweenFadeConfig _defaultFadeIn = TweenFadeConfig.FadeIn();
        [SerializeField] private TweenFadeConfig _defaultFadeOut = TweenFadeConfig.FadeOut();

        private Queue<TweenFadeConfig> _queuedFades;
        private bool _isProcessingQueue;

        private void Awake()
        {
            _queuedFades = new Queue<TweenFadeConfig>(2);
            _isProcessingQueue = false;
            
            _fadeGroup.alpha = 0;
        }


        public void QueueFadeIn()
        {
            QueueFade(_defaultFadeIn);
        }

        public void QueueFadeOut()
        {
            QueueFade(_defaultFadeOut);
        }

        public void QueueFade(TweenFadeConfig fade)
        {
            _queuedFades.Enqueue(fade);

            if (!_isProcessingQueue)
            {
                ProcessFades().Forget();
            }
        }


        private async UniTaskVoid ProcessFades()
        {
            _isProcessingQueue = true;

            while (_queuedFades.Count > 0)
            {
                TweenFadeConfig fade = _queuedFades.Dequeue();

                await _fadeGroup.Fade(fade)
                    .AsyncWaitForCompletion();
            }
            
            _isProcessingQueue = false;
        }
        
    }
}