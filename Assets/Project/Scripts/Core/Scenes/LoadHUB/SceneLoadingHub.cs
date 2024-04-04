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
        
        [Header("GAMEPLAY SCENE")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _gameplayScene;
        
        [Header("SCENES")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup[] _scenesToLoad;
        
        
        private ISceneLoadManager _sceneLoadManager;


        private void Awake()
        {
            _sceneLoadManager = ServiceLocator.Instance.GetService<ISceneLoadManager>();
            
            foreach (ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup in _scenesToLoad)
            {
                SceneLoadButton sceneLoadButton = Instantiate(_sceneLoadButtonPrefab, _buttonsHolder);
                sceneLoadButton.Configure(_sceneLoadManager, sceneLoadGroup, OnSceneButtonPressed);
            }
        }

        private void OnSceneButtonPressed()
        {
            // Load gameplay scene too
            //_sceneLoadManager.LoadSceneAdditively(_gameplayScene);
        }
        
    }
}