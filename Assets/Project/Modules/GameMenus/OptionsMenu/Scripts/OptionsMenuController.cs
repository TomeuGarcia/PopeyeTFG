using System;
using AYellowpaper;
using Popeye.Modules.GameMenus.AudioMenu;
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
        private AMenuController AudioOptionsMenu => _audioOptionsMenu.Value;

        

        protected override void DoInit(InputAction goBackInput)
        {
            AudioOptionsMenu.Init(CloseAudioOptionsMenu, goBackInput);
            
            _audioOptionsButtonAndConfig.SmartButton.Init(
                _audioOptionsButtonAndConfig.Config, OpenAudioOptionsMenu);

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