using System;
using AYellowpaper;
using Popeye.Modules.GameMenus.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Popeye.Modules.GameMenus.OptionsMenu
{
    public class OptionsMenuController : AMenuController
    {
        [Header("AUDIO")]
        [SerializeField] private SmartButtonAndConfig _audioOptionsButtonAndConfig;
        [SerializeField] private InterfaceReference<AMenuController, MonoBehaviour> _audioOptionsMenu;

        [Header("LANGUAGE")] 
        [SerializeField] private OptionSelectorAndConfig _languageOptionSelectorAndConfig;
        
        private AMenuController AudioOptionsMenu => _audioOptionsMenu.Value;

        

        protected override void DoInit(InputAction goBackInput)
        {
            AudioOptionsMenu.Init(CloseAudioOptionsMenu, goBackInput);
            
            _audioOptionsButtonAndConfig.SmartButton.Init(
                _audioOptionsButtonAndConfig.Config, OpenAudioOptionsMenu);

            LanguageOptionController languageOptionController = new LanguageOptionController();
            _languageOptionSelectorAndConfig.OptionSelector.Init(_languageOptionSelectorAndConfig.Config, 
                languageOptionController, GameLocalizationState.GetGameLanguageIndex());

            CloseAudioOptionsMenu();
        }

        
        

        private void OpenAudioOptionsMenu()
        {
            AudioOptionsMenu.Show();
            Hide();
        }
        
        private void CloseAudioOptionsMenu()
        {
            AudioOptionsMenu.Hide();
            Show();
        }
    }
    
}