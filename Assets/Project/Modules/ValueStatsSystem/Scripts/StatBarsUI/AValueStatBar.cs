using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem
{
    public abstract class AValueStatBar : MonoBehaviour
    {
        [Header("COMPONENTS")]
        [SerializeField] private RectTransform _mainTransform;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _lazyBarFillImage;
        [SerializeField, Range(0.0f, 10.0f)] private float _fullFillDuration = 1.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _lazyFullFillDuration = 2.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _colorPunchMinDuration = 0.4f;
    
        [SerializeField] private Color _originalColor = Color.blue;
        [SerializeField] private Color _lazyColor = Color.black;
        [SerializeField] private Color _incrementColor = Color.green;
        [SerializeField] private Color _decrementColor = Color.red;

        [SerializeField] private Ease _fillEase = Ease.InOutQuad;
        [SerializeField] private Ease _lazyFillEase = Ease.InOutQuad;
        
        private bool _isSubscribed;
        
        protected abstract AValueStat ValueStat { get; }

        protected float LazyExtraDuration => _lazyFullFillDuration - _fullFillDuration;

        
    
        private void OnValidate()
        {
            if (_fillImage != null)
            {
                _fillImage.color = _originalColor;
            }
    
            if (_lazyBarFillImage != null)
            {
                _lazyBarFillImage.color = _lazyColor;
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

            float fillDuration = changeAmount * _fullFillDuration;
            float lazyFillDuration = changeAmount * _lazyFullFillDuration;

            DoUpdateFillImage(newFillValue, fillDuration, lazyFillDuration, isSubtracting);
        }
        
        protected void DoUpdateFillImage(float newFillValue, float fillDuration, float lazyFillDuration,
            bool isSubtracting)
        {
            FillBar(_fillImage, newFillValue, fillDuration, _fillEase);
            FillBar(_lazyBarFillImage, newFillValue, lazyFillDuration, _lazyFillEase);

            PunchFillImageColor(isSubtracting ? _decrementColor : _incrementColor, fillDuration);
        }
        
        
    
        private void FillBar(Image fillImage, float newFillValue, float duration, Ease ease)
        {
            fillImage.DOComplete();
            fillImage.DOFillAmount(newFillValue, duration)
                .SetEase(ease);
        }
        
        private void PunchFillImageColor(Color punchColor, float duration)
        {
            duration = Mathf.Max(duration, _colorPunchMinDuration);
            duration /= 2;
            _fillImage.DOColor(punchColor, duration)
                .OnComplete(() =>
                {
                    _fillImage.DOColor(_originalColor, duration);
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