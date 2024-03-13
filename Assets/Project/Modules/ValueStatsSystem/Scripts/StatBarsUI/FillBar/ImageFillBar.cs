using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Project.Scripts.TweenExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem
{
    public class ImageFillBar : MonoBehaviour
    {
        [Header("COMPONENTS")]
        [Required] [SerializeField] private RectTransform _holder;
        [Required] [SerializeField] private Image _backgroundImage;
        [Required] [SerializeField] private Image _lazyBarFillImage;
        [Required] [SerializeField] private Image _fillImage;
        
        private ImageFillBarConfig _viewConfig;
        
        private const float ALMOST_ZERO_DURATION = 0.01f;


        public void Init(ImageFillBarConfig viewConfig)
        {
            _viewConfig = viewConfig;
            _fillImage.color = _viewConfig.OriginalColor;
            _lazyBarFillImage.color = _viewConfig.LazyColor;
            _backgroundImage.color = _viewConfig.BackgroundColor;
        }

        private void OnDisable()
        {
            CompleteAllUpdates();
        }
        private void OnDestroy()
        {
            KillAllUpdates();
        }


        public void InstantUpdateFill(float value01)
        {
            _fillImage.fillAmount = _lazyBarFillImage.fillAmount = value01;
        }
    
        public async UniTask UpdateFill(float value01)
        {
            float changeAmount = value01 - _fillImage.fillAmount;

            bool isSubtracting = changeAmount < 0;
            changeAmount = Mathf.Abs(changeAmount);

            float fillDuration = changeAmount * _viewConfig.FullFillDuration;
            float lazyFillDuration = changeAmount * _viewConfig.LazyFullFillDuration;

            DoUpdateFill(value01, fillDuration, lazyFillDuration, isSubtracting);

            await UniTask.Delay(TimeSpan.FromSeconds(fillDuration));
        }
        
        public async UniTask UpdateFillToMax(float fillDuration)
        {
            float lazyFillDuration = fillDuration + _viewConfig.LazyExtraDuration;
            
            DoUpdateFill(1.0f, fillDuration, lazyFillDuration, false);
            
            await UniTask.Delay(TimeSpan.FromSeconds(fillDuration));
        }
        
        
        private void DoUpdateFill(float newFillValue, float fillDuration, float lazyFillDuration,
            bool isSubtracting)
        {
            FillBar(_fillImage, newFillValue, fillDuration, _viewConfig.FillEase);
            FillBar(_lazyBarFillImage, newFillValue, lazyFillDuration, _viewConfig.LazyFillEase);

            PunchFillImageColor(isSubtracting ? _viewConfig.DecrementColor : _viewConfig.IncrementColor, fillDuration);

            if (_viewConfig.PunchScale)
            {
                _holder.PunchScale(isSubtracting ? _viewConfig.DecrementPunchScaleConfig : _viewConfig.IncrementPunchScaleConfig, true);
            }
            
        }
        
        
    
        private void FillBar(Image fillImage, float newFillValue, float duration, Ease ease)
        {
            if (duration < ALMOST_ZERO_DURATION)
            {
                fillImage.fillAmount = newFillValue;
                return;
            }
            
            fillImage.DOComplete();
            fillImage.DOFillAmount(newFillValue, duration)
                .SetEase(ease);
        }
        
        private void PunchFillImageColor(Color punchColor, float duration)
        {
            duration = Mathf.Max(duration, _viewConfig.ColorPunchMinDuration);
            duration /= 2;
            _fillImage.DOColor(punchColor, duration)
                .OnComplete(() =>
                {
                    _fillImage.DOColor(_viewConfig.OriginalColor, duration);
                });
        }
        
        public void KillAllUpdates()
        {
            _fillImage.DOKill();
            _lazyBarFillImage.DOKill();
        }
        public void CompleteAllUpdates()
        {
            _fillImage.DOComplete();
            _lazyBarFillImage.DOComplete();
        }
        
        
    }
}