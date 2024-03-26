using NaughtyAttributes;
using Popeye.Scripts.TextUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.GameMenus.Generic
{
    public class SmartSlider : MonoBehaviour
    {
        [Required()] [SerializeField] private Slider _slider;
        [Required()] [SerializeField] private Image _backgroundImage;
        [Required()] [SerializeField] private Image _fillImage;
        [Required()] [SerializeField] private Image _knobImage;
        [Required()] [SerializeField] private TextMeshProUGUI _text;
        
        private SmartSliderConfig _config;

        public delegate void SmartSliderValueEvent(float sliderValue);
        private SmartSliderValueEvent _onValueChangedCallback;

        [SerializeField] private bool _selectOnEnable = false;
        
        
        public void Init(SmartSliderConfig config, float startValue01, SmartSliderValueEvent onValueChangedCallback)
        {
            _config = config;
            _onValueChangedCallback = onValueChangedCallback;
            ApplyConfig();
            SetSliderValue(startValue01);
        }


        private void ApplyConfig()
        {
            _slider.wholeNumbers = _config.ViewConfig.WholeNumbers;
            _slider.minValue = _config.ViewConfig.MinValue;
            _slider.maxValue = _config.ViewConfig.MaxValue;

            _backgroundImage.color = _config.ViewConfig.BackgroundColor;
            _fillImage.color = _config.ViewConfig.FillColor;
            _knobImage.color = _config.ViewConfig.KnobColor;
            
            _text.SetContent(_config.TextContent);
        }


        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(InvokeOnValueChanged);

            if (_selectOnEnable)
            {
                _slider.Select();
            }
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveAllListeners();
        }
        
        private void InvokeOnValueChanged(float sliderValue)
        {
            _onValueChangedCallback.Invoke(SliderValueToValue01(sliderValue));
        }

        private float SliderValueToValue01(float sliderValue)
        {
            if (_config.ViewConfig.ProcessValueTo01Range)
            {
                float maxPositive = _config.ViewConfig.MaxValue - _config.ViewConfig.MinValue;
                sliderValue -= _config.ViewConfig.MinValue;
                sliderValue /= maxPositive;
            }
            
            return sliderValue;
        }

        private void SetSliderValue(float value01)
        {
            _slider.value = Value01ToSliderValue(value01);
        }
        
        private float Value01ToSliderValue(float value01)
        {
            if (_config.ViewConfig.ProcessValueTo01Range)
            {
                value01 = Mathf.LerpUnclamped(_config.ViewConfig.MinValue, _config.ViewConfig.MaxValue, value01);
            }
            
            return value01;
        }


    }
    
}