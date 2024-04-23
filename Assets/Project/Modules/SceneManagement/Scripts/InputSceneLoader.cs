using System;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;

namespace Popeye.Modules.SceneManagement.Scripts
{
    public class InputSceneLoader : MonoBehaviour
    {
        [SerializeField] private InputSceneLoaderConfig _inputSceneLoaderConfig;

        private ISceneLoadManager _sceneLoadManager;

        private void Start()
        {
            _sceneLoadManager = ServiceLocator.Instance.GetService<ISceneLoadManager>();
        }

        private void Update()
        {
            UpdateLoadScene();
        }


        private void UpdateLoadScene()
        {
            foreach (InputSceneLoaderConfig.SceneLoadData sceneLoadData in _inputSceneLoaderConfig.ScenesData)
            {
                if (Input.GetKeyDown(sceneLoadData.LoadKeyCode))
                {
                    LoadScene(sceneLoadData.SceneLoadGroup);
                }
            }

            if (Input.GetKeyDown(_inputSceneLoaderConfig.ReloadCurrentSceneKeyCode))
            {
                ReloadCurrentScene();
            }
        }

        private void LoadScene(ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup)
        {
            _sceneLoadManager.LoadSceneAdditively(sceneLoadGroup);
        }

        private void ReloadCurrentScene()
        {
            _sceneLoadManager.ReloadCurrentScene(_inputSceneLoaderConfig.ReloadCurrentLoadOptions);
        }
        
    }
}