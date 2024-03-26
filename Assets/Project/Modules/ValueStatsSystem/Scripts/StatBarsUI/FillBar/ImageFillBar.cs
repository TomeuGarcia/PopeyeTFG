using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public class ImageFillBar : MonoBehaviour
    {
        [Header("COMPONENTS")]
        [Required] [SerializeField] private RectTransform _holder;
        [Required] [SerializeField] private SmartImage _backgroundImage;
        [Required] [SerializeField] private SmartImage _lazyBarFillImage;
        [Required] [SerializeField] private SmartImage _fillImage;
        
        private ImageFillBarConfig _viewConfig;
        


        public void Init(ImageFillBarConfig viewConfig)
        {
            _viewConfig = viewConfig;
            
            _fillImage.Init();
            _lazyBarFillImage.Init();
            _backgroundImage.Init();
            
            _fillImage.SetColor(_viewConfig.OriginalColor);
            _lazyBarFillImage.SetColor(_viewConfig.LazyColor);
            _backgroundImage.SetColor(_viewConfig.BackgroundColor);
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
            _fillImage.SetFillValue(value01);
            _lazyBarFillImage.SetFillValue(value01);
        }
    
        public async UniTask UpdateFill(float value01)
        {
            float changeAmount = value01 - _fillImage.FillValue;

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
            _fillImage.ToFillValue(newFillValue, fillDuration, _viewConfig.FillEase);
            _lazyBarFillImage.ToFillValue(newFillValue, lazyFillDuration, _viewConfig.LazyFillEase);

            PunchFillImageColor(isSubtracting ? _viewConfig.DecrementColor : _viewConfig.IncrementColor, fillDuration);

            if (_viewConfig.PunchScale)
            {
                _holder.PunchScale(isSubtracting 
                    ? _viewConfig.DecrementPunchScaleConfigAsset 
                    : _viewConfig.IncrementPunchScaleConfigAsset, 
                    true);
            }
            
        }
        

        private void PunchFillImageColor(Color punchColor, float duration)
        {
            duration = Mathf.Max(duration, _viewConfig.ColorPunchMinDuration);
            
            _fillImage.PunchColor(punchColor, _viewConfig.OriginalColor, duration);
        }
        
        public void KillAllUpdates()
        {
            _fillImage.DOKill();
            _lazyBarFillImage.DOKill();
            _holder.DOKill();
        }
        
        public void CompleteAllUpdates()
        {
            _fillImage.DOComplete();
            _lazyBarFillImage.DOComplete();
            _holder.DOComplete();
        }
        
        
    }
}