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
        [SerializeField] private GameObject _background;

        
        
        [Header("PAUSE MENU")]
        [SerializeField] private InterfaceReference<AMenuController, MonoBehaviour> _pauseMenu;
        private AMenuController PauseMenu => _pauseMenu.Value;


        private IGameStateEventsDispatcher _gameStateEventsDispatcher;
        private IEventSystemService _eventSystemService;
        
        private PlayerAnchorInputControls _inputUIActions;
        private InputAction _goBackInput;
        private InputAction _openMenuInput;


        private bool IsBeingShown => _menusCanvasHolder.activeInHierarchy;
        
        private void Start()
        {
            _gameStateEventsDispatcher = ServiceLocator.Instance.GetService<IGameStateEventsDispatcher>();
            _eventSystemService = ServiceLocator.Instance.GetService<IEventSystemService>();
            StartListeningToEvents();
            
            _background.SetActive(true);
            
            _inputUIActions = new InputSystem.PlayerAnchorInputControls();
            EnableInputs();
            _goBackInput = _inputUIActions.UI.Back;
            _openMenuInput = _inputUIActions.UI.OpenMenu;
            
            PauseMenu.Init(Close, _goBackInput);
            Close();
        }

        private void OnDestroy()
        {
            StopListeningToEvents();
            DisableInputs();
        }

        private void StartListeningToEvents()
        {
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartLoadingAdditiveScene>(OnStartLoadingSceneEvent);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnFinishLoadingScenes>(OnFinishLoadingScenesEvent);
            
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartCameraAnimation>(OnStartCameraAnimationEvent);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnFinishCameraAnimation>(OnFinishCameraAnimationEvent);
        }
        
        private void StopListeningToEvents()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartLoadingAdditiveScene>(OnStartLoadingSceneEvent);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnFinishLoadingScenes>(OnFinishLoadingScenesEvent);
            
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartCameraAnimation>(OnStartCameraAnimationEvent);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnFinishCameraAnimation>(OnFinishCameraAnimationEvent);
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


        private void EnableInputs()
        {
            _inputUIActions.Enable();
        }
        private void DisableInputs()
        {
            _inputUIActions.Disable();
        }


        private void OnStartLoadingSceneEvent(IGameStateEventsDispatcher.OnStartLoadingAdditiveScene eventData)
        {
            DisableInputs();
        }
        private void OnFinishLoadingScenesEvent(IGameStateEventsDispatcher.OnFinishLoadingScenes eventData)
        {
            EnableInputs();
        }
        
        private void OnStartCameraAnimationEvent(IGameStateEventsDispatcher.OnStartCameraAnimation eventData)
        {
            DisableInputs();
        }
        private void OnFinishCameraAnimationEvent(IGameStateEventsDispatcher.OnFinishCameraAnimation eventData)
        {
            EnableInputs();
        }
        
    }
}