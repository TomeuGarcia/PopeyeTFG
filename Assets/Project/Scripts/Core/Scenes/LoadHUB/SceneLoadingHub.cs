using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    public class SceneLoadingHub : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField] private RectTransform _buttonsHolder;
        [SerializeField] private SceneLoadButton _sceneLoadButtonPrefab;
        
        [Header("SCENES")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup[] _scenesToLoad;
        
        
        private ISceneLoadManager _sceneLoadManager;
        
        
        public void Configure(ISceneLoadManager sceneLoadManager)
        {
            foreach (ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup in _scenesToLoad)
            {
                SceneLoadButton sceneLoadButton = Instantiate(_sceneLoadButtonPrefab, _buttonsHolder);
                sceneLoadButton.Configure(sceneLoadManager, sceneLoadGroup);
            }
        }
        
    }
}