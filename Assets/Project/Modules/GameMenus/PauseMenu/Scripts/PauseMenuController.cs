using System;
using AYellowpaper;
using NaughtyAttributes;
using Popeye.Modules.GameMenus.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Popeye.Modules.GameMenus.PauseMenu
{
    public class PauseMenuController : AMenuController
    {
        [Header("OPTIONS")]
        [SerializeField] private SmartButtonAndConfig _optionsButtonAndConfig;
        [SerializeField] private InterfaceReference<AMenuController, MonoBehaviour> _optionsMenu;
        private AMenuController OptionsMenu => _optionsMenu.Value;

        [Header("QUIT")]
        [SerializeField] private SmartButtonAndConfig _quitButtonAndConfig;
        [Scene] [SerializeField] private int _mainMenuScene; 
        
        

        protected override void DoInit(InputAction goBackInput)
        {
            OptionsMenu.Init(CloseOptionsMenu, goBackInput);
            
            _optionsButtonAndConfig.SmartButton.Init(
                _optionsButtonAndConfig.Config, OpenOptionsMenu);

            
            _quitButtonAndConfig.SmartButton.Init(
                _quitButtonAndConfig.Config, QuitToMainMenu);

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


        private void QuitToMainMenu()
        {
            SceneManager.LoadScene(_mainMenuScene);
        }
        
    }
}