using System;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Scripts.Core.Scenes
{
    public class SceneLoadButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private ISceneLoadManager _sceneLoadManager;
        private ISceneLoadManager.SceneAdditiveLoadGroup _sceneLoadGroup;
        
        
        public void Configure(ISceneLoadManager sceneLoadManager, ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup)
        {
            _sceneLoadManager = sceneLoadManager;
            _sceneLoadGroup = sceneLoadGroup;
            
            _button.onClick.AddListener(LoadScene);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }


        private void LoadScene()
        {
            _sceneLoadManager.LoadSceneAdditively(_sceneLoadGroup);
        }
        
    }
}