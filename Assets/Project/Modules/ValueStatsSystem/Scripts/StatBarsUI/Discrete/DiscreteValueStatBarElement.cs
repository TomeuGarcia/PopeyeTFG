using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem
{
    public class DiscreteValueStatBarElement : MonoBehaviour
    {
        [Header("COMPONENTS")]
        [SerializeField] private RectTransform _mainTransform;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _lazyBarFillImage;
        
        private ValueStatBarViewConfig _viewConfig;

        
        private float FullFillDuration => _viewConfig.FullFillDuration;
        private float LazyFullFillDuration =>_viewConfig.LazyFullFillDuration;
        private float ColorPunchMinDuration => _viewConfig.ColorPunchMinDuration;
    
        private Color OriginalColor => _viewConfig.OriginalColor;
        private Color LazyColor => _viewConfig.LazyColor;
        private Color IncrementColor => _viewConfig.IncrementColor;
        private Color DecrementColor => _viewConfig.DecrementColor;

        private Ease FillEase => _viewConfig.FillEase;
        private Ease LazyFillEase => _viewConfig.LazyFillEase;
        
        
        public void Init(ValueStatBarViewConfig viewConfig)
        {
            _viewConfig = viewConfig;
        }


        public void ToMax()
        {
            DoUpdateFillImage(1, FullFillDuration, LazyFullFillDuration, false);
        }
        public void ToMin()
        {
            DoUpdateFillImage(0, FullFillDuration, LazyFullFillDuration, false);
        }

        private void DoUpdateFillImage(float newFillValue, float fillDuration, float lazyFillDuration,
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
        
        
    }
}