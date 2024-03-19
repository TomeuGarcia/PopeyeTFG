using System;
using AYellowpaper;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.GameMenus.Generic;
using Popeye.Modules.GameState;
using UnityEngine;

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
        
        
        private void Start()
        {
            _gameStateEventsDispatcher = ServiceLocator.Instance.GetService<IGameStateEventsDispatcher>();
            
            PauseMenu.Init(Close);
            Close();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Open();
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