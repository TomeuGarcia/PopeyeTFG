using System;
using AYellowpaper;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem.GameAudiosManager;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class AudioInstaller : MonoBehaviour
    {
        [Header("LASTING SOUNDS")]
        [SerializeField] private LastingSoundsControllerConfig _lastingSoundsControllerConfig;
        [SerializeField] private Transform _lastingSoundsParent;

        [Header("GLOBAL PARAMETERS")] 
        [SerializeField] private GlobalParametersConfig _globalParametersConfig;

        [Header("GAME AUDIO MANAGERS")] 
        [SerializeField] private InterfaceReference<IGameAudiosManager, MonoBehaviour> _gameAudiosManager;
        
        
        public void Install(ServiceLocator serviceLocator)
        {
            OneShotSoundsController oneShotSoundsController = new OneShotSoundsController();
            LastingSoundsController lastingSoundsController = new LastingSoundsController(_lastingSoundsParent, _lastingSoundsControllerConfig);
            GlobalParametersController globalParametersController = new GlobalParametersController(_globalParametersConfig);

            IFMODAudioManager fmodAudioManager = new FMODAudioManager(oneShotSoundsController, lastingSoundsController, globalParametersController);
            
            fmodAudioManager.GlobalParametersController.StartListeningToParameters();
            
            serviceLocator.RegisterService<IFMODAudioManager>(fmodAudioManager);
            
            _gameAudiosManager.Value.Init(fmodAudioManager, serviceLocator.GetService<IEventSystemService>());
            _gameAudiosManager.Value.StartListeningToGameEvents();
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            IFMODAudioManager fmodAudioManager = serviceLocator.GetService<IFMODAudioManager>(); 
            
            fmodAudioManager.GlobalParametersController.StopListeningToParameters();
            fmodAudioManager.StopAllSounds();
            
            serviceLocator.RemoveService<IFMODAudioManager>();
        }
        
        private void OnDestroy()
        {
            _gameAudiosManager.Value.StopListeningToGameEvents();
        }
    }
}