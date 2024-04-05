using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    public class SceneLoadOnStart : MonoBehaviour
    {
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _loadSceneGroup;
        
        private void Start()
        {
            ServiceLocator.Instance.GetService<ISceneLoadManager>().LoadScene(_loadSceneGroup);
        }
        
    }
}