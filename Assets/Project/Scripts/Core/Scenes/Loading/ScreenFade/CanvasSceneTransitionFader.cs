using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    public class CanvasSceneTransitionFader : MonoBehaviour, ISceneTransitionScreenFader
    {
        [SerializeField] private CanvasGroup _fadeGroup;
        [SerializeField] private TweenFadeConfig _fadeIn = TweenFadeConfig.FadeIn();
        [SerializeField] private TweenFadeConfig _fadeOut = TweenFadeConfig.FadeOut();

        private void Awake()
        {
            _fadeGroup.alpha = 0;
        }

        public async UniTaskVoid FadeScreen(Func<bool> sceneFinishedLoading)
        {
            await _fadeGroup.Fade(_fadeIn).AsyncWaitForCompletion();

            await UniTask.WaitUntil(sceneFinishedLoading);
            
            _fadeGroup.Fade(_fadeOut);
        }
        
    }
}