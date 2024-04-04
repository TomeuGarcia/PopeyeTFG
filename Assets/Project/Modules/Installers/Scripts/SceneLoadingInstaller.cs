using Popeye.Core.Services.CommandQueue;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;

namespace Popeye.Modules.Installers
{
    public class SceneLoadingInstaller : MonoBehaviour
    {
        [SerializeField] private CanvasSceneTransitionFader _screenFader;
        
        public void Install(ServiceLocator serviceLocator, ICommandQueueService commandQueueService)
        {
            SceneLoadManager sceneLoadManager = new SceneLoadManager(commandQueueService, _screenFader);

            serviceLocator.RegisterService<ISceneLoadManager>(sceneLoadManager);
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            serviceLocator.RemoveService<ISceneLoadManager>();
        }
        
    }
}