using System;
using AYellowpaper;
using NaughtyAttributes;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.GameMenus.Generic;
using Popeye.Modules.GameState;
using Popeye.Scripts.Core.Scenes;
using Project.Modules.GameMenus.Generic.Scripts.Audio;
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
        
        [Header("CONTROLS")]
        [SerializeField] private SmartButtonAndConfig _controlsButtonAndConfig;
        [SerializeField] private InterfaceReference<AMenuController, MonoBehaviour> _controlsMenu;
        private AMenuController ControlsMenu => _controlsMenu.Value;

        [Header("QUIT")]
        [SerializeField] private SmartButtonAndConfig _quitButtonAndConfig;
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _mainMenuSceneLoadGroup;

        [Header("AUDIO")] 
        [SerializeField] private UIAudioInteractionConfig _openMenuAudio;
        
        private IGameStateEventsDispatcher _gameStateEventsDispatcher;

        protected override void DoInit(InputAction goBackInput)
        {
            OptionsMenu.Init(CloseOptionsMenu, goBackInput);
            _optionsButtonAndConfig.SmartButton.Init(
                _optionsButtonAndConfig.Config, OpenOptionsMenu);
            
            ControlsMenu.Init(CloseControlsMenu, goBackInput);
            _controlsButtonAndConfig.SmartButton.Init(
                _controlsButtonAndConfig.Config, OpenControlsMenu);
            
            _quitButtonAndConfig.SmartButton.Init(
                _quitButtonAndConfig.Config, QuitToMainMenu);

            CloseOptionsMenu();
            
            _gameStateEventsDispatcher = ServiceLocator.Instance.GetService<IGameStateEventsDispatcher>();
        }


        private void OpenOptionsMenu()
        {
            OptionsMenu.Show();
            Hide();            
            _openMenuAudio.PlaySound();
        }
        
        private void CloseOptionsMenu()
        {
            OptionsMenu.Hide();
            Show();
        }
        
        private void OpenControlsMenu()
        {
            ControlsMenu.Show();
            Hide();
        }
        
        private void CloseControlsMenu()
        {
            ControlsMenu.Hide();
            Show();
        }


        private void QuitToMainMenu()
        {
            CloseMenu();
            _gameStateEventsDispatcher.InvokeOnExitToMainMenu();
            ServiceLocator.Instance.GetService<ISceneLoadManager>().LoadScene(_mainMenuSceneLoadGroup);
        }
        
    }
}