using System;
using AYellowpaper;
using Popeye.Modules.GameMenus.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Popeye.Modules.GameMenus.PauseMenu
{
    public class PauseMenuController : AMenuController
    {
        [Header("OPTIONS")]
        [SerializeField] private SmartButtonAndConfig _optionsButtonAndConfig;
        [SerializeField] private InterfaceReference<AMenuController, MonoBehaviour> _optionsMenu;
        private AMenuController OptionsMenu => _optionsMenu.Value;

        

        protected override void DoInit(InputAction goBackInput)
        {
            OptionsMenu.Init(CloseOptionsMenu, goBackInput);
            
            _optionsButtonAndConfig.SmartButton.Init(
                _optionsButtonAndConfig.Config, OpenOptionsMenu);

            CloseOptionsMenu();
        }


        private void OpenOptionsMenu()
        {
            OptionsMenu.Show();
            Hide();
        }
        
        private void CloseOptionsMenu()
        {
            OptionsMenu.Hide();
            Show();
        }
    
        
    }
}