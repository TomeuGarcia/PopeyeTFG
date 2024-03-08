using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem
{
    public abstract class AValueStatBar : MonoBehaviour
    {
        [Header("COMPONENTS")]
        [SerializeField] private RectTransform _mainTransform;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _lazyBarFillImage;
        
        [Header("CONFIGURATION")]
        [Expandable] [SerializeField] private ValueStatBarViewConfig _viewConfig;
        
        
        private float FullFillDuration => _viewConfig.FullFillDuration;
        private float LazyFullFillDuration =>_viewConfig.LazyFullFillDuration;
        private float ColorPunchMinDuration => _viewConfig.ColorPunchMinDuration;
    
        private Color OriginalColor => _viewConfig.OriginalColor;
        private Color LazyColor => _viewConfig.LazyColor;
        private Color IncrementColor => _viewConfig.IncrementColor;
        private Color DecrementColor => _viewConfig.DecrementColor;

        private Ease FillEase => _viewConfig.FillEase;
        private Ease LazyFillEase => _viewConfig.LazyFillEase;

        
        private bool _isSubscribed;
        
        protected abstract AValueStat ValueStat { get; }

        protected float LazyExtraDuration => LazyFullFillDuration - FullFillDuration;

        
    
        private void OnValidate()
        {
            if (_fillImage != null)
            {
                _fillImage.color = OriginalColor;
            }
    
            if (_lazyBarFillImage != null)
            {
                _lazyBarFillImage.color = LazyColor;
            }
        }
    
        private void OnEnable()
        {
            if (HasSubscriptionReferences())
            {
                SubscribeToEvents();
            }
        }
    
        private void OnDisable()
        {
            if (HasSubscriptionReferences())
            {
                UnsubscribeToEvents();
            }
        }


        protected void BaseInit()
        {
            _isSubscribed = false;
    
            OnValidate();
    
            SubscribeToEvents();
            InstantUpdateFillImage();
        }


        protected abstract bool HasSubscriptionReferences();
        protected abstract void DoSubscribeToEvents();
        protected abstract void DoUnsubscribeToEvents();

        private void SubscribeToEvents()
        {
            if (_isSubscribed) return;
            _isSubscribed = true;
    
            DoSubscribeToEvents();
        }
        private void UnsubscribeToEvents()
        {
            if (!_isSubscribed) return;
            _isSubscribed = false;
    
            DoUnsubscribeToEvents();
        }
    
    
        private void InstantUpdateFillImage()
        {
            _fillImage.fillAmount = _lazyBarFillImage.fillAmount = ValueStat.GetValuePer1Ratio();
        }
    
        protected void UpdateFillImage()
        {
            float newFillValue = ValueStat.GetValuePer1Ratio();        
            float changeAmount = newFillValue - _fillImage.fillAmount;

            bool isSubtracting = changeAmount < 0;
            changeAmount = Mathf.Abs(changeAmount);

            float fillDuration = changeAmount * FullFillDuration;
            float lazyFillDuration = changeAmount * LazyFullFillDuration;

            DoUpdateFillImage(newFillValue, fillDuration, lazyFillDuration, isSubtracting);
        }
        
        protected void DoUpdateFillImage(float newFillValue, float fillDuration, float lazyFillDuration,
            bool isSubtracting)
        {
            FillBar(_fillImage, newFillValue, fillDuration, FillEase);
            FillBar(_lazyBarFillImage, newFillValue, lazyFillDuration, LazyFillEase);

            PunchFillImageColor(isSubtracting ? DecrementColor : IncrementColor, fillDuration);
        }
        
        
    
        private void FillBar(Image fillImage, float newFillValue, float duration, Ease ease)
        {
            if (duration < 0.01f)
            {
                fillImage.fillAmount = newFillValue;
                return;
            }
            
            fillImage.DOKill();
            fillImage.DOComplete();
            fillImage.DOFillAmount(newFillValue, duration)
                .SetEase(ease);
        }
        
        private void PunchFillImageColor(Color punchColor, float duration)
        {
            duration = Mathf.Max(duration, ColorPunchMinDuration);
            duration /= 2;
            _fillImage.DOColor(punchColor, duration)
                .OnComplete(() =>
                {
                    _fillImage.DOColor(OriginalColor, duration);
                });
        }
    
        public void PlayErrorAnimation()
        {
            _mainTransform.DOComplete();
            _mainTransform.DOPunchPosition(Vector3.right * 20.0f, 0.5f);
        }

        protected void KillAllUpdates()
        {
            _mainTransform.DOComplete();
            _fillImage.DOKill();
            _lazyBarFillImage.DOKill();
        }
    }
}