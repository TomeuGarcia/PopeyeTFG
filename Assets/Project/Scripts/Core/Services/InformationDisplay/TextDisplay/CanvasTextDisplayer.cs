using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Scripts.TextUtilities;
using Project.Scripts.TweenExtensions;
using TMPro;
using UnityEngine;

namespace Popeye.Core.Services.InformationDisplay
{
    public class CanvasTextDisplayer : MonoBehaviour, ITextDisplayer
    {
        [SerializeField] private CanvasGroup _backgroundFadeGroup;
        [SerializeField] private CanvasGroup _contentFadeGroup;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _contentText;

        private TextDisplayConfig _currentDisplay;

        private bool _isShowing = false;
        private bool _isHiding = false;
        
        private void Awake()
        {
            _backgroundFadeGroup.alpha = 0;
            _contentFadeGroup.alpha = 0;
            _currentDisplay = null;
        }

        private void OnDestroy()
        {
            _backgroundFadeGroup.DOKill();
            _contentFadeGroup.DOKill();
        }

        public async UniTask StartShowing(TextDisplayConfig textDisplayConfig)
        {
            if (DisplayWantsToOverwrite(textDisplayConfig))
            {
                await StopShowing(_currentDisplay);
            }
            await UniTask.WaitUntil(() => !_isHiding);


            _isShowing = true;
            _currentDisplay = textDisplayConfig;

            _headerText.SetContent(_currentDisplay.Header);
            _contentText.SetContent(_currentDisplay.Description);
            
            await _backgroundFadeGroup.Fade(_currentDisplay.BackgroundViewExtras.ShowFade)
                    .AsyncWaitForCompletion();
            await _contentFadeGroup.Fade(_currentDisplay.ContentViewExtras.ShowFade)
                .AsyncWaitForCompletion();

            _isShowing = false;
        }

        public async UniTask StopShowing(TextDisplayConfig textDisplayConfig)
        {
            if (_currentDisplay != textDisplayConfig) return;
            
            await UniTask.WaitUntil(() => !_isShowing);

            _isHiding = true;
            
            await _contentFadeGroup.Fade(_currentDisplay.ContentViewExtras.HideFade)
                .AsyncWaitForCompletion();
            await _backgroundFadeGroup.Fade(_currentDisplay.BackgroundViewExtras.HideFade)
                .AsyncWaitForCompletion();
            
            _currentDisplay = null;
            _isHiding = false;
        }


        private bool DisplayWantsToOverwrite(TextDisplayConfig textDisplayConfig)
        {
            if (_currentDisplay == null) return false;
            return _currentDisplay != textDisplayConfig;
        }
        
    }
}