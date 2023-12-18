using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Popeye.Modules.SceneManagment.Scripts
{
    public class InputSceneLoader : MonoBehaviour
    {
        [SerializeField] private InputSceneLoaderConfig _inputSceneLoaderConfig;


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
        }

        private void LoadScene(InputSceneLoaderConfig.SceneLoadData sceneLoadData)
        {
            SceneManager.LoadScene(sceneLoadData.BuiltInSceneIndex);
        }
        
    }
}