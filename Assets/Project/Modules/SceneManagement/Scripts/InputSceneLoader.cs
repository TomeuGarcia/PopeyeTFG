using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Popeye.Modules.SceneManagement.Scripts
{
    public class InputSceneLoader : MonoBehaviour
    {
        [SerializeField] private InputSceneLoaderConfig _inputSceneLoaderConfig;
        private InputSceneLoaderConfig.SceneLoadData _lastLoadedSceneLoadData;
        

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
                    LoadScene(sceneLoadData);
                }
            }

            if (Input.GetKeyDown(_inputSceneLoaderConfig.ReloadCurrentSceneKeyCode))
            {
                ReloadCurrentScene();
            }
        }

        private void LoadScene(InputSceneLoaderConfig.SceneLoadData sceneLoadData)
        {
            SceneManager.LoadScene(sceneLoadData.BuiltInSceneIndex);
        }

        private void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}