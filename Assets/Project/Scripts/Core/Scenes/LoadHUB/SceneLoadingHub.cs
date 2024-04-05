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
        
        [Header("GAMEPLAY SCENE")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _gameplayScene;
        
        [Header("SCENES")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup[] _scenesToLoad;
        
        
        private ISceneLoadManager _sceneLoadManager;


        private void Awake()
        {
            _sceneLoadManager = ServiceLocator.Instance.GetService<ISceneLoadManager>();
            
            _buttons = new SceneLoadButton[_scenesToLoad.Length];
            
            for (int i = 0; i < _scenesToLoad.Length; ++i)
            {
                ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup = _scenesToLoad[i];
            
                SceneLoadButton sceneLoadButton = Instantiate(_sceneLoadButtonPrefab, _buttonsHolder);
                sceneLoadButton.Configure(_sceneLoadManager, sceneLoadGroup, OnSceneButtonPressed);

                _buttons[i] = sceneLoadButton;
            }
        }

        private void OnSceneButtonPressed()
        {
            // Load gameplay scene too
            //_sceneLoadManager.LoadSceneAdditively(_gameplayScene);

            foreach (SceneLoadButton sceneLoadButton in _buttons)
            {
                sceneLoadButton.DisableClicking();
            }
        }
        
    }
}