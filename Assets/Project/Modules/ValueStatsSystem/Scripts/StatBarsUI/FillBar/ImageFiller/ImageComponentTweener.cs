using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem
{
    public class ImageComponentTweener : IImageTweener
    {
        private readonly Image _fillImage;

        public ImageComponentTweener(Image fillImage)
        {
            _fillImage = fillImage;
        }

        public float FillValue => _fillImage.fillAmount;

        public void SetFillValue(float value01)
        {
            _fillImage.fillAmount = value01;
        }

        public void ToFillValue(float value01, float duration, Ease ease)
        {
            if (duration < SmartImage.ALMOST_ZERO_DURATION)
            {
                SetFillValue(value01);
                return;
            }
            
            _fillImage.DOComplete();
            _fillImage.DOFillAmount(value01, duration)
                .SetEase(ease);
        }

        public void SetColor(Color color)
        {
            _fillImage.color = color;
        }

        public void PunchColor(Color toColor, Color backColor, float duration)
        {
            duration /= 2;
            _fillImage.DOColor(toColor, duration)
                .OnComplete(() =>
                {
                    _fillImage.DOColor(backColor, duration);
                });
        }
    }
}