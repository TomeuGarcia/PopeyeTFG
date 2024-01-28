using System;
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
        [Header("REFERNCES")]
        [SerializeField] private AutoAimControllerGeneralConfig _autoAimControllerGeneralConfig;
        [SerializeField] private AutoAimTargetDataConfig _autoAimTargetDataConfig;
        [SerializeField] private AutoAimWorld_Test _autoAimWorldTest;
        [SerializeField] private AutoAimFunction_Test _autoAimFunctionTest;
        [SerializeField] private AutoAimCreator _autoAimCreator;
        [SerializeField] private AutoAimCreator_Test _autoAimCreatorTest;
        
        [Header("DATA CONFIG")]
        [SerializeField] private Slider _sliderAngularSize;
        [SerializeField] private Slider _sliderAngularTargetRegion;
        [SerializeField] private Slider _sliderCenterFlattening;
        [SerializeField] private Slider _sliderBlendWithIdentity;

        [SerializeField] private TextMeshProUGUI _textAngularSize;
        [SerializeField] private TextMeshProUGUI _textAngularTargetRegion;
        [SerializeField] private TextMeshProUGUI _textCenterFlattening;
        [SerializeField] private TextMeshProUGUI _textBlendWithIdentity;

        
        [Header("TARGETS")]        
        [SerializeField] private Button _buttonRandomizeTargets;
        
        [Header("ORIGINAL LOOK")]        
        [SerializeField] private Button _buttonOriginalLookTargets;
        [SerializeField] private TextMeshProUGUI _textOriginalLookTargets;

        private void Awake()
        {
            Setup();
        }

        private void OnEnable()
        {
            _sliderAngularSize.onValueChanged.AddListener(UpdateAngularSize);
            _sliderAngularTargetRegion.onValueChanged.AddListener(UpdateAngularTargetRegion);
            _sliderCenterFlattening.onValueChanged.AddListener(UpdateResetCenterFlattening);
            
            _sliderBlendWithIdentity.onValueChanged.AddListener(UpdateResetBlendWithIdentity);
            
            _buttonRandomizeTargets.onClick.AddListener(_autoAimWorldTest.RandomizeTargetPositions);
            
            _buttonOriginalLookTargets.onClick.AddListener(ToggleShowHideDefaultLook);
        }
        private void OnDisable()
        {
            _sliderAngularSize.onValueChanged.RemoveAllListeners();
            _sliderAngularTargetRegion.onValueChanged.RemoveAllListeners();
            _sliderCenterFlattening.onValueChanged.RemoveAllListeners();
            
            _sliderBlendWithIdentity.onValueChanged.RemoveAllListeners();
            
            _buttonRandomizeTargets.onClick.RemoveAllListeners();
            
            _buttonOriginalLookTargets.onClick.RemoveAllListeners();
        }


        private void Setup()
        {
            _sliderAngularSize.minValue = 0f;
            _sliderAngularSize.maxValue = 90f;
            _sliderAngularSize.value = _autoAimTargetDataConfig.AngularSize;
            _sliderAngularSize.wholeNumbers = true;
            
            _sliderAngularTargetRegion.minValue = 0f;
            _sliderAngularTargetRegion.maxValue = 90f;
            _sliderAngularTargetRegion.value = _autoAimTargetDataConfig.AngularTargetRegion;
            _sliderAngularTargetRegion.wholeNumbers = true;
            
            _sliderCenterFlattening.minValue = 0f;
            _sliderCenterFlattening.maxValue = 1f;
            _sliderCenterFlattening.value = _autoAimTargetDataConfig.FlatCenterAngularTargetRegion;
            _sliderCenterFlattening.wholeNumbers = false;
            
            _sliderBlendWithIdentity.minValue = 0f;
            _sliderBlendWithIdentity.maxValue = 1f;
            _sliderBlendWithIdentity.value = _autoAimControllerGeneralConfig.FunctionConfig.BlendWithIdentity;
            _sliderBlendWithIdentity.wholeNumbers = false;
            
            UpdateUIText(_textAngularSize, _sliderAngularSize.value, "0");
            UpdateUIText(_textAngularTargetRegion, _sliderAngularTargetRegion.value, "0");
            UpdateUIText(_textCenterFlattening, _sliderCenterFlattening.value, "0.00");
            
            UpdateUIText(_textBlendWithIdentity, _sliderBlendWithIdentity.value, "0.00");

            _autoAimWorldTest.SetTargetsAutoAimTargetDataConfig(_autoAimTargetDataConfig);
            UpdateShowHideDefaultLookButton(_autoAimWorldTest.DefaultLookIsVisible);
            
            
            _autoAimCreator.SetAutoAimControllerGeneralConfig(_autoAimControllerGeneralConfig);
            _autoAimCreatorTest.SetAutoAimControllerGeneralConfig(_autoAimControllerGeneralConfig);
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
        
        private void UpdateResetBlendWithIdentity(float value)
        {
            _autoAimControllerGeneralConfig.FunctionConfig.ResetBlendWithIdentity(value);
            UpdateUIText(_textBlendWithIdentity, value, "0.00");
        }

        private void UpdateUIText(TextMeshProUGUI UItext, float value, string format)
        {
            UItext.text = value.ToString(format);
        }



        private void ToggleShowHideDefaultLook()
        {
            bool isVisible = !_autoAimWorldTest.DefaultLookIsVisible;
            _autoAimWorldTest.SetDefaultLookIsVisible(isVisible);
            _autoAimFunctionTest.SetDefaultLookIsVisible(isVisible);

            UpdateShowHideDefaultLookButton(isVisible);
        }
        
        private void UpdateShowHideDefaultLookButton(bool isVisible)
        {
            _textOriginalLookTargets.text = isVisible ? "Hide" : "Show";
        }
        
        
    }
}