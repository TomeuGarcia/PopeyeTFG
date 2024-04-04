using System;
using Popeye.Core.Services.CommandQueue;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
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

            
            EventSystemService eventSystemService = new EventSystemService();
            serviceLocator.RegisterService<IEventSystemService>(eventSystemService);
            
            
            ITimeScaleManager timeScaleManager = new UnityTimeScaleManager();
            TimeFunctionalities timeFunctionalities =
                new TimeFunctionalities(timeScaleManager, new HitStopManager(_hitStopManagerConfig, timeScaleManager));
            serviceLocator.RegisterService<ITimeFunctionalities>(timeFunctionalities);


            CommandQueueServiceImpl commandQueueService = new CommandQueueServiceImpl();
            serviceLocator.RegisterService<ICommandQueueService>(commandQueueService);
            
            
            _audioInstaller.Install(serviceLocator);
            _sceneLoadingInstaller.Install(serviceLocator, commandQueueService);
        }

        private void Uninstall()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;
            
            
            _sceneLoadingInstaller.Uninstall(serviceLocator);
            _audioInstaller.Uninstall(serviceLocator);
            
            
            serviceLocator.RemoveService<ICommandQueueService>();
            serviceLocator.RemoveService<ITimeFunctionalities>();
            serviceLocator.RemoveService<IEventSystemService>();
        }
    }
}