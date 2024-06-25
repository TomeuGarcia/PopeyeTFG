using System;
using Popeye.Scripts.TextUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Popeye.Modules.GameMenus.Generic
{
    public class OptionSelector : Selectable
    {
        private OptionSelectorConfig _config;
        private IOptionSelectorListener _listener;
        private int _currentOptionIndex;
        
        [Header("CONFIGURATION")]
        [SerializeField] private bool _optionsLoop = false;
        [SerializeField] private bool _selectOnEnable = false;

        [Header("COMPONENTS")] 
        [SerializeField] private Button _previousOptionButton;
        [SerializeField] private Button _nextOptionButton;
        [SerializeField] private TextInitializer _optionNameText;
        [SerializeField] private TextInitializer _currentOptionText;
        
        private int NumberOfOptions => _config.OptionTextContents.Length;
        

        public void Init(OptionSelectorConfig config, IOptionSelectorListener listener, int startOptionIndex = 0)
        {
            _config = config;
            _listener = listener;
            _currentOptionIndex = startOptionIndex;

            ApplyConfig();
            ValidateButtons();
            UpdateOption();
        }
        
        private void ApplyConfig()
        {
            ColorBlock colorBlock = colors;
            colorBlock.normalColor = _config.ViewConfig.NormalColor;
            colorBlock.highlightedColor = _config.ViewConfig.HighlightedColor;
            colorBlock.selectedColor = _config.ViewConfig.SelectedColor;
            
            colors = colorBlock;
            
            _optionNameText.SetTextContent(_config.OptionNameTextContent);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _previousOptionButton.onClick.AddListener(OnPreviousOptionButtonPressed);
            _nextOptionButton.onClick.AddListener(OnNextOptionButtonPressed);
            
            if (_selectOnEnable)
            {
                Select();
            }
        }
        protected override void OnDisable()
        {
            _previousOptionButton.onClick.RemoveAllListeners();
            _nextOptionButton.onClick.RemoveAllListeners();
        }


        private void OnPreviousOptionButtonPressed()
        {
            bool optionChanged = true;
            
            if (_optionsLoop)
            {
                _currentOptionIndex = (_currentOptionIndex - 1 + NumberOfOptions) % NumberOfOptions;
            }
            else
            {
                optionChanged = _currentOptionIndex > 0;                
                if (optionChanged) _currentOptionIndex -= 1;
                ValidateButtons();
            }
            
                        
            DoButtonPressed(optionChanged);
        }
        
        private void OnNextOptionButtonPressed()
        {
            bool optionChanged = true;
            
            if (_optionsLoop)
            {
                _currentOptionIndex = (_currentOptionIndex + 1) % NumberOfOptions;
            }
            else
            {
                optionChanged = _currentOptionIndex < NumberOfOptions - 1;
                if (optionChanged) _currentOptionIndex += 1;
                ValidateButtons();
            }


            DoButtonPressed(optionChanged);
        }

        private void DoButtonPressed(bool optionChanged)
        {
            if (optionChanged)
            {
                UpdateOption();
                _config.OptionChangedAudio.PlaySound();
            }
        }

        private void UpdateOption()
        {
            _currentOptionText.SetTextContent(_config.OptionTextContents[_currentOptionIndex]);
            _listener.OnOptionUpdated(_currentOptionIndex);
        }

        private void ValidateButtons()
        {
            _previousOptionButton.interactable = _currentOptionIndex > 0;
            _nextOptionButton.interactable = _currentOptionIndex < NumberOfOptions - 1;
        }
        
        
        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    OnPreviousOptionButtonPressed();
                    break;

                case MoveDirection.Right:
                    OnNextOptionButtonPressed();
                    break;
            }
        }
        
    }
}