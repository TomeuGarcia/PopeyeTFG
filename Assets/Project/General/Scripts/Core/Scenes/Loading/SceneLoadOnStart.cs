using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    public class SceneLoadOnStart : MonoBehaviour
    {
        [SerializeField] private bool _loadAdditively = false;
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _loadSceneGroup;
        
        private void Start()
        {
            if (_loadAdditively)
            {
                ServiceLocator.Instance.GetService<ISceneLoadManager>().LoadSceneAdditively(_loadSceneGroup);    
            }
            else
            {
                ServiceLocator.Instance.GetService<ISceneLoadManager>().LoadScene(_loadSceneGroup);
            }
        }
        
    }
}