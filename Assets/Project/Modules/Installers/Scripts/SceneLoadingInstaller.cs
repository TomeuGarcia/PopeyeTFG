using Popeye.Core.Services.CommandQueue;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.GameState;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;

namespace Popeye.Modules.Installers
{
    public class SceneLoadingInstaller : MonoBehaviour
    {
        [SerializeField] private CanvasSceneTransitionFader _screenFader;
        
        public void Install(
            ServiceLocator serviceLocator, 
            ICommandQueueService commandQueueService, 
            IGameStateEventsDispatcher gameStateEventsDispatcher
        )
        {
            SceneLoadManager sceneLoadManager = 
                new SceneLoadManager(commandQueueService, _screenFader, gameStateEventsDispatcher);

            serviceLocator.RegisterService<ISceneLoadManager>(sceneLoadManager);
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            serviceLocator.RemoveService<ISceneLoadManager>();
        }
        
    }
}