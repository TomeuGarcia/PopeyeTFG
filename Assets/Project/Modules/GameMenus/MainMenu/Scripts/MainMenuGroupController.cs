using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.GameMenus.Generic;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;

namespace Project.Modules.GameMenus.MainMenu
{
    public class MainMenuGroupController : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField] private SmartButtonAndConfig _playButtonAndConfig;
        [SerializeField] private SmartButtonAndConfig _creditsButtonAndConfig;
        [SerializeField] private SmartButtonAndConfig _quitButtonAndConfig;
        [SerializeField] private SmartButtonAndConfig _debugSceneHubButtonAndConfig;

        [Header("GAME SCENE")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _coreGameSceneLoadGroup;
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _gameSceneLoadGroup;
        
        [Header("CREDITS")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _creditsSceneLoadGroup;
        
        [Header("DEBUG SCENES HUB")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _scenesHubLoadGroup;

        private ISceneLoadManager _sceneLoadManager;
        
        
        private void Awake()
        {
            _sceneLoadManager = ServiceLocator.Instance.GetService<ISceneLoadManager>();
        }

        private void Start()
        {
            _playButtonAndConfig.SmartButton.Init(_playButtonAndConfig.Config, PlayGame);
            _creditsButtonAndConfig.SmartButton.Init(_creditsButtonAndConfig.Config, LoadCredits);
            _quitButtonAndConfig.SmartButton.Init(_quitButtonAndConfig.Config, QuitGame);
            _debugSceneHubButtonAndConfig.SmartButton.Init(_debugSceneHubButtonAndConfig.Config, LoadScenesHub);
        }

        private void PlayGame()
        {
            _sceneLoadManager.LoadSceneAdditively(_coreGameSceneLoadGroup);
            _sceneLoadManager.LoadSceneAdditively(_gameSceneLoadGroup);
        }
        private void LoadCredits()
        {
            _sceneLoadManager.LoadSceneAdditively(_creditsSceneLoadGroup);
        }
        private void LoadScenesHub()
        {
            _sceneLoadManager.LoadSceneAdditively(_scenesHubLoadGroup);
        }
        
        private void QuitGame()
        {
            Application.Quit();
        }
        
        
    }
}