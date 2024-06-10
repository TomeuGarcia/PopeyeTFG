using System;
using Popeye.Core.Services.CommandQueue;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.GameState;
using Project.General.Scripts.Core.Services.ScreenFade;
using Project.Scripts.Time.TimeFunctionalities;
using Project.Scripts.Time.TimeHitStop;
using Project.Scripts.Time.TimeScale;
using UnityEngine;

namespace Popeye.Modules.Installers
{
    public class GameCoreInstaller : MonoBehaviour
    {
        [Header("TIME")]
        [SerializeField] private HitStopManagerConfig _hitStopManagerConfig;
        
        [Header("AUDIO")] 
        [SerializeField] private AudioInstaller _audioInstaller;
        
        [Header("SCENES")] 
        [SerializeField] private SceneLoadingInstaller _sceneLoadingInstaller;
        
        [Header("SCREEN FADE")]
        [SerializeField] private CanvasScreenFadeService _canvasScreenFadeService;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Install();
        }

        private void OnDestroy()
        {
            Uninstall();
        }

        private void Install()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;


            serviceLocator.RegisterService<IScreenFadeService>(_canvasScreenFadeService);

            
            EventSystemService eventSystemService = new EventSystemService();
            serviceLocator.RegisterService<IEventSystemService>(eventSystemService);
            
            
            ITimeScaleManager timeScaleManager = new UnityTimeScaleManager();
            TimeFunctionalities timeFunctionalities =
                new TimeFunctionalities(timeScaleManager, new HitStopManager(_hitStopManagerConfig, timeScaleManager));
            serviceLocator.RegisterService<ITimeFunctionalities>(timeFunctionalities);


            CommandQueueServiceImpl commandQueueService = new CommandQueueServiceImpl();
            serviceLocator.RegisterService<ICommandQueueService>(commandQueueService);
            
            
            IGameStateEventsDispatcher gameStateEventsDispatcher = new GameStateEventsDispatcher(eventSystemService);
            serviceLocator.RegisterService<IGameStateEventsDispatcher>(gameStateEventsDispatcher);
            
            
            _audioInstaller.Install(serviceLocator);
            _sceneLoadingInstaller.Install(serviceLocator, commandQueueService, gameStateEventsDispatcher);
        }

        private void Uninstall()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;
            
            
            _sceneLoadingInstaller.Uninstall(serviceLocator);
            _audioInstaller.Uninstall(serviceLocator);
            
            serviceLocator.RemoveService<IGameStateEventsDispatcher>();
            serviceLocator.RemoveService<ICommandQueueService>();
            serviceLocator.RemoveService<ITimeFunctionalities>();
            serviceLocator.RemoveService<IEventSystemService>();
            serviceLocator.RemoveService<IScreenFadeService>();
        }
    }
}