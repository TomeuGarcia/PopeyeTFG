using System;
using NaughtyAttributes;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.GameMenus.Generic;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Modules.GameMenus.MainMenu.Scripts
{
    public class MainMenuGroupController : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField] private SmartButtonAndConfig _playButtonAndConfig;
        [SerializeField] private SmartButtonAndConfig _quitButtonAndConfig;

        [Header("GAME SCENE")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _coreGmeSceneLoadGroup;
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _gameSceneLoadGroup;

        private ISceneLoadManager _sceneLoadManager;
        
        
        private void Awake()
        {
            _sceneLoadManager = ServiceLocator.Instance.GetService<ISceneLoadManager>();
        }

        private void Start()
        {
            _playButtonAndConfig.SmartButton.Init(_playButtonAndConfig.Config, PlayGame);
            _quitButtonAndConfig.SmartButton.Init(_quitButtonAndConfig.Config, QuitGame);
        }


        private void PlayGame()
        {
            _sceneLoadManager.LoadScene(_coreGmeSceneLoadGroup);
            _sceneLoadManager.LoadSceneAdditively(_gameSceneLoadGroup);
        }
        
        private void QuitGame()
        {
            Application.Quit();
        }
        
    }
}