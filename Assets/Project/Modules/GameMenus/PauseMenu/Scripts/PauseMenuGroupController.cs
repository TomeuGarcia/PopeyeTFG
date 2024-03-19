using System;
using AYellowpaper;
using InputSystem;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.GameMenus.Generic;
using Popeye.Modules.GameState;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Popeye.Modules.GameMenus.PauseMenu
{
    public class PauseMenuGroupController : MonoBehaviour, IPauseMenuGroupController
    {
        [Header("CANVAS")]
        [SerializeField] private GameObject _menusCanvasHolder;

        
        [Header("PAUSE MENU")]
        [SerializeField] private InterfaceReference<AMenuController, MonoBehaviour> _pauseMenu;
        private AMenuController PauseMenu => _pauseMenu.Value;


        private IGameStateEventsDispatcher _gameStateEventsDispatcher;
        
        private PlayerAnchorInputControls _inputUIActions;
        private InputAction _goBackInput;
        private InputAction _openMenuInput;


        private bool IsBeingShown => _menusCanvasHolder.activeInHierarchy;
        
        private void Start()
        {
            _gameStateEventsDispatcher = ServiceLocator.Instance.GetService<IGameStateEventsDispatcher>();

            _inputUIActions = new InputSystem.PlayerAnchorInputControls();
            _inputUIActions.Enable();
            _goBackInput = _inputUIActions.UI.Back;
            _openMenuInput = _inputUIActions.UI.OpenMenu;
            
            PauseMenu.Init(Close, _goBackInput);
            Close();
        }

        private void OnDestroy()
        {
            _inputUIActions.Disable();
        }

        private void Update()
        {
            if (_openMenuInput.WasPressedThisFrame())
            {
                if (IsBeingShown)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
        }

        public void Open()
        {
            _menusCanvasHolder.SetActive(true);
            PauseMenu.Show();
            
            _gameStateEventsDispatcher.InvokeOnGamePaused();
        }

        public void Close()
        {
            PauseMenu.Hide();
            _menusCanvasHolder.SetActive(false);
            
            _gameStateEventsDispatcher.InvokeOnGameResumed();
        }
        
        
    }
}