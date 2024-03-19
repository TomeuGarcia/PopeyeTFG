using System;
using AYellowpaper;
using Popeye.Modules.GameMenus.AudioMenu;
using Popeye.Modules.GameMenus.Generic;
using UnityEngine;

namespace Popeye.Modules.GameMenus.OptionsMenu
{
    public class OptionsMenuController : AMenuController
    {
        [Header("AUDIO")]
        [SerializeField] private SmartButtonAndConfig _audioOptionsButtonAndConfig;
        [SerializeField] private InterfaceReference<AMenuController, MonoBehaviour> _audioOptionsMenu;
        private AMenuController AudioOptionsMenu => _audioOptionsMenu.Value;


        private void Start()
        {
            
        }


        protected override void DoInit()
        {
            AudioOptionsMenu.Init(CloseAudioOptionsMenu);
            
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