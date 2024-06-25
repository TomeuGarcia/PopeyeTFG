using System;
using NaughtyAttributes;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.GameMenus.OptionsMenu;
using TMPro;
using UnityEngine;

namespace Popeye.Scripts.TextUtilities
{
    public class TextInitializer : MonoBehaviour
    {
        [Required()] [SerializeField] private TMP_Text _text;
        [Required()] [Expandable] [SerializeField] private TextContent _textContent;
        private IEventSystemService _eventSystemService;

        private void OnValidate()
        {
            if (_text && _textContent)
            {
                UpdateText();    
            }            
        }
        private void Start()
        {
            UpdateText();
            _eventSystemService = ServiceLocator.Instance.GetService<IEventSystemService>();
            _eventSystemService.Subscribe<GameLocalizationState.OnGameLanguageUpdatedEvent>(OnGameLanguageUpdated);
        }
        private void OnDestroy()
        {
            _eventSystemService?.Unsubscribe<GameLocalizationState.OnGameLanguageUpdatedEvent>(OnGameLanguageUpdated);
        }
        

        private void OnGameLanguageUpdated(GameLocalizationState.OnGameLanguageUpdatedEvent eventData)
        {
            UpdateText();
        }
        
        
        [Button()]
        private void UpdateText()
        {
            _text.SetContent(_textContent);
        }

        public void SetTextContent(TextContent textContent)
        {
            _textContent = textContent;
            UpdateText();
        }
        
    }
}