using Popeye.Scripts.TextUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.GameMenus.Generic
{
    public class SmartButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        
        private SmartButtonConfig _config;

        public delegate void SmartButtonEvent();
        private SmartButtonEvent _onButtonClickedCallback;

        [SerializeField] private bool _selectOnEnable = false;
        
        
        public void Init(SmartButtonConfig config, SmartButtonEvent onButtonPressedCallback)
        {
            _config = config;
            _onButtonClickedCallback = onButtonPressedCallback;
            ApplyConfig();
        }


        private void ApplyConfig()
        {
            ColorBlock colorBlock = _button.colors;
            colorBlock.normalColor = _config.ViewConfig.NormalColor;
            colorBlock.highlightedColor = _config.ViewConfig.HighlightedColor;
            colorBlock.selectedColor = _config.ViewConfig.SelectedColor;
            
            _button.colors = colorBlock;
            
            _text.SetContent(_config.TextContent);
        }


        private void OnEnable()
        {
            _button.onClick.AddListener(InvokeOnButtonClicked);

            if (_selectOnEnable)
            {
                _button.Select();
            }
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
        
        private void InvokeOnButtonClicked()
        {
            _onButtonClickedCallback.Invoke();
        }
    }
}