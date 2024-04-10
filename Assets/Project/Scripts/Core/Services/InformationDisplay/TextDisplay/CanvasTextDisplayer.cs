using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.AudioSystem;
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
        private IFMODAudioManager _audioManager;
        
        private Queue<TextDisplayConfig> _queuedDisplays;
        private bool _processingQueuedDisplays;

        private bool _isShowing;
        private bool _isHiding;
        
        private void Awake()
        {
            _backgroundFadeGroup.alpha = 0;
            _contentFadeGroup.alpha = 0;
            
            _currentDisplay = null;
            
            _queuedDisplays = new Queue<TextDisplayConfig>(2);
            _processingQueuedDisplays = false;
            
            _isShowing = false;
            _isHiding = false;
        }

        private void Start()
        {
            _audioManager = ServiceLocator.ServiceLocator.Instance.GetService<IFMODAudioManager>();
        }

        private void OnDestroy()
        {
            _backgroundFadeGroup.DOKill();
            _contentFadeGroup.DOKill();
        }

        public void StartShowing(TextDisplayConfig textDisplayConfig)
        {
            _queuedDisplays.Enqueue(textDisplayConfig);
            if (_processingQueuedDisplays)
            {
                return;
            }

            TransitionToNext().Forget();
        }

        private async UniTask TransitionToNext()
        {
            _processingQueuedDisplays = true;

            
            if (_queuedDisplays.Count == 1)
            {
                _currentDisplay = _queuedDisplays.Peek();
                await StartShowingCurrent();
                _queuedDisplays.Dequeue();
            }
            
            
            while (_queuedDisplays.Count > 0)
            {
                await StopShowingCurrent();
                _currentDisplay = _queuedDisplays.Peek();
                await StartShowingCurrent();
                _queuedDisplays.Dequeue();
            }
            
            _processingQueuedDisplays = false;
        } 
        
        private async UniTask StartShowingCurrent()
        {
            await UniTask.WaitUntil(() => !_isHiding);
            
            _isShowing = true;

            SetTextContents(_currentDisplay);
            
            await _backgroundFadeGroup.Fade(_currentDisplay.BackgroundViewExtras.ShowFade)
                    .AsyncWaitForCompletion();
            await _contentFadeGroup.Fade(_currentDisplay.ContentViewExtras.ShowFade)
                .AsyncWaitForCompletion();

            _isShowing = false;
        }

        public void StopShowing(TextDisplayConfig textDisplayConfig)
        {
            if (_currentDisplay != textDisplayConfig) return;

            StopShowingCurrent().Forget();
        }
        
        private async UniTask StopShowingCurrent()
        {
            await UniTask.WaitUntil(() => !_isShowing);

            _isHiding = true;
            
            await _contentFadeGroup.Fade(_currentDisplay.ContentViewExtras.HideFade)
                .AsyncWaitForCompletion();
            await _backgroundFadeGroup.Fade(_currentDisplay.BackgroundViewExtras.HideFade)
                .AsyncWaitForCompletion();
            
            _currentDisplay = null;
            _isHiding = false;
        }


        private void SetTextContents(TextDisplayConfig displayConfig)
        {
            _headerText.SetContent(displayConfig.Header);
            _headerText.color = displayConfig.TextDisplaySettings.HeaderColor;
            
            _contentText.SetContent(displayConfig.Description);
            _contentText.color = displayConfig.TextDisplaySettings.DescriptionColor;
            
            _audioManager.PlayOneShot(displayConfig.TextDisplaySettings.Sound);
        }
        
    }
}