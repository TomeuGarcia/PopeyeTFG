using System;
using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    public class SceneLoadingHub : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField] private RectTransform _buttonsHolder;
        [SerializeField] private SceneLoadButton _sceneLoadButtonPrefab;
        private SceneLoadButton[] _buttons;

        [Header("CONFIGURATION")] 
        [SerializeField] private SceneLoadingHubConfig _config;
        
        
        private ISceneLoadManager _sceneLoadManager;


        private void Awake()
        {
            _sceneLoadManager = ServiceLocator.Instance.GetService<ISceneLoadManager>();

            ISceneLoadManager.SceneAdditiveLoadGroup[] scenesToLoad = _config.ScenesToLoad;
            
            _buttons = new SceneLoadButton[scenesToLoad.Length];
            
            for (int i = 0; i < scenesToLoad.Length; ++i)
            {
                ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup = scenesToLoad[i];
            
                SceneLoadButton sceneLoadButton = Instantiate(_sceneLoadButtonPrefab, _buttonsHolder);
                sceneLoadButton.Configure(sceneLoadGroup, OnSceneButtonPressed);

                _buttons[i] = sceneLoadButton;
            }
        }

        private void OnSceneButtonPressed(ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup)
        {
            if (NeedsToLoadGameplayCore(sceneLoadGroup))
            {
                _sceneLoadManager.LoadSceneAdditively(_config.GameplayCoreSceneGroup);
            }
            _sceneLoadManager.LoadSceneAdditively(sceneLoadGroup);


            foreach (SceneLoadButton sceneLoadButton in _buttons)
            {
                sceneLoadButton.DisableClicking();
            }
        }
        
        private bool NeedsToLoadGameplayCore(ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup)
        {
            return sceneLoadGroup.sceneReference != _config.MainMenuScene;
        }
    }
}