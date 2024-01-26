using System.Globalization;
using Popeye.Modules.PlayerController.AutoAim;
using Project.Modules.PlayerController.Testing.AutoAim.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Modules.PlayerController.Testing.AutoAim
{
    public class AutoAimInteractableHUD_Test : MonoBehaviour
    {
        [Header("DATA CONFIG")]
        [SerializeField] private AutoAimTargetDataConfig _autoAimTargetDataConfig;

        [SerializeField] private Slider _sliderAngularSize;
        [SerializeField] private Slider _sliderAngularTargetRegion;
        [SerializeField] private Slider _sliderCenterFlattening;

        [SerializeField] private TextMeshProUGUI _textAngularSize;
        [SerializeField] private TextMeshProUGUI _textAngularTargetRegion;
        [SerializeField] private TextMeshProUGUI _textCenterFlattening;

        
        [Header("TARGETS")]        
        [SerializeField] private AutoAimWorld_Test _autoAimWorldTest;
        [SerializeField] private Button _buttonRandomizeTargets;

        
        private void OnEnable()
        {
            Setup();
            
            _sliderAngularSize.onValueChanged.AddListener(UpdateAngularSize);
            _sliderAngularTargetRegion.onValueChanged.AddListener(UpdateAngularTargetRegion);
            _sliderCenterFlattening.onValueChanged.AddListener(UpdateResetCenterFlattening);
            
            _buttonRandomizeTargets.onClick.AddListener(_autoAimWorldTest.RandomizeTargetPositions);
        }
        private void OnDisable()
        {
            _sliderAngularSize.onValueChanged.RemoveAllListeners();
            _sliderAngularTargetRegion.onValueChanged.RemoveAllListeners();
            _sliderCenterFlattening.onValueChanged.RemoveAllListeners();
            
            _buttonRandomizeTargets.onClick.RemoveAllListeners();
        }


        private void Setup()
        {
            _sliderAngularSize.minValue = 0f;
            _sliderAngularSize.maxValue = 90f;
            _sliderAngularSize.value = _autoAimTargetDataConfig.AngularSize;
            
            _sliderAngularTargetRegion.minValue = 0f;
            _sliderAngularTargetRegion.maxValue = 90f;
            _sliderAngularTargetRegion.value = _autoAimTargetDataConfig.AngularTargetRegion;
            
            _sliderCenterFlattening.minValue = 0f;
            _sliderCenterFlattening.maxValue = 1f;
            _sliderCenterFlattening.value = _autoAimTargetDataConfig.FlatCenterAngularTargetRegion;
            
            UpdateUIText(_textAngularSize, _sliderAngularSize.value, "0");
            UpdateUIText(_textAngularTargetRegion, _sliderAngularTargetRegion.value, "0");
            UpdateUIText(_textCenterFlattening, _sliderCenterFlattening.value, "0.00");
        }


        private void UpdateAngularSize(float value)
        {
            _autoAimTargetDataConfig.ResetAngularSize(value);
            UpdateUIText(_textAngularSize, value, "0");
        }
        private void UpdateAngularTargetRegion(float value)
        {
            _autoAimTargetDataConfig.ResetAngularTargetRegion(value);
            UpdateUIText(_textAngularTargetRegion, value, "0");
        }
        private void UpdateResetCenterFlattening(float value)
        {
            _autoAimTargetDataConfig.ResetCenterFlattening(value);
            UpdateUIText(_textCenterFlattening, value, "0.00");
        }

        private void UpdateUIText(TextMeshProUGUI UItext, float value, string format)
        {
            UItext.text = value.ToString(format);
        }
        
        
        
    }
}