using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace Popeye.Modules.ValueStatSystem.StatBarsUI
{
    public class ValueStatSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField, Range(0.01f, 10.0f)] private float _totalFillDuration = 1.0f;
        [SerializeField] private Ease _fillEase = Ease.InOutSine;
        
        private AValueStat _valueStat;
        

        public void Init(AValueStat valueStat)
        {
            _valueStat = valueStat;

            UpdatePositionInstantly();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _valueStat.OnValueUpdate += UpdatePosition;
            _valueStat.OnMaxValueUpdate += UpdatePosition;
        }
        private void UnsubscribeToEvents()
        {
            _valueStat.OnValueUpdate -= UpdatePosition;
            _valueStat.OnMaxValueUpdate -= UpdatePosition;
        }


        private void UpdatePositionInstantly()
        {
            _slider.value = _valueStat.GetValuePer1Ratio();
        }
        private void UpdatePosition()
        {
            float currentValue = _slider.value;
            float newValue = _valueStat.GetValuePer1Ratio();
            float changeRatio = Mathf.Abs(newValue - currentValue);

            _slider.DOComplete();
            _slider.DOValue(newValue, _totalFillDuration * changeRatio)
                .SetEase(_fillEase);
        }
    }
}