using System.Collections.Generic;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.AudioSystem;
using Popeye.Scripts.TextUtilities;
using Project.Scripts.TweenExtensions;
using TMPro;
using UnityEngine;

namespace Popeye.Core.Services.InformationDisplay
{
    public class CanvasTextDisplayer : MonoBehaviour, ITextDisplayer, IDisplayQueueDelegate
    {
        [Header("AUDIO")]
        [SerializeField] private AFMODAudioManagerReference _audioManager;
    
        [Header("COMPONENTS")]
        [SerializeField] private InterfaceReference<ITextDisplayViewEffects, MonoBehaviour> _displayViewEffects;
        [SerializeField] private CanvasGroup _backgroundFadeGroup;
        [SerializeField] private CanvasGroup _contentFadeGroup;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _contentText;

        DisplayQueue<TextDisplayConfig> _displayQueue;

        private void Awake()
        {
            _backgroundFadeGroup.alpha = 0;
            _contentFadeGroup.alpha = 0;

            _displayQueue = new DisplayQueue<TextDisplayConfig>(this);
        }

        private void OnDestroy()
        {
            _backgroundFadeGroup.DOKill();
            _contentFadeGroup.DOKill();
        }

        public void StartShowing(TextDisplayConfig textDisplayConfig)
        {
            _displayQueue.StartShowing(textDisplayConfig);
        }

        public async UniTask DoStartShowing()
        {
            TextDisplayConfig currentDisplay = _displayQueue.CurrentDisplay;
            SetTextContents(currentDisplay);
            
            await _backgroundFadeGroup.Fade(currentDisplay.BackgroundViewExtras.ShowFade)
                .AsyncWaitForCompletion();
            await _contentFadeGroup.Fade(currentDisplay.ContentViewExtras.ShowFade)
                .AsyncWaitForCompletion();
        }


        public void StopShowing(TextDisplayConfig textDisplayConfig)
        {
            _displayQueue.StopShowing(textDisplayConfig);
        }
        
        public async UniTask DoStopShowing()
        {
            TextDisplayConfig currentDisplay = _displayQueue.CurrentDisplay;
            
            await _contentFadeGroup.Fade(currentDisplay.ContentViewExtras.HideFade)
                .AsyncWaitForCompletion();
            await _backgroundFadeGroup.Fade(currentDisplay.BackgroundViewExtras.HideFade)
                .AsyncWaitForCompletion();
        }
        
        
        private void SetTextContents(TextDisplayConfig displayConfig)
        {
            _headerText.SetContent(displayConfig.Header);
            _headerText.color = displayConfig.TextDisplaySettings.HeaderColor;
            
            _contentText.SetContent(displayConfig.Description);
            _contentText.color = displayConfig.TextDisplaySettings.DescriptionColor;
            
            _audioManager.PlayOneShot(displayConfig.TextDisplaySettings.Sound);
            
            _displayViewEffects.Value.UpdateView(displayConfig.TextDisplaySettings);
        }


    }
}