using System;
using AYellowpaper;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem.GameAudiosManager;
using Popeye.Modules.AudioSystem.SoundVolume;
using Project.Modules.AudioSystem.Scripts.SoundVolume;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class AudioInstaller : MonoBehaviour
    {
        [Header("REFERENCE")] 
        [SerializeField] private FMODAudioManagerReference _audioManagerReference;
        
        [Header("LASTING SOUNDS")]
        [SerializeField] private LastingSoundsControllerConfig _lastingSoundsControllerConfig;
        [SerializeField] private Transform _lastingSoundsParent;

        [Header("GLOBAL PARAMETERS")] 
        [SerializeField] private GlobalParametersConfig _globalParametersConfig;

        [Header("GAME AUDIO MANAGERS")] 
        [SerializeField] private InterfaceReference<IGameAudiosManager, MonoBehaviour> _gameAudiosManager;

        [Header("SOUND VOLUME CONTROLLER")] 
        [SerializeField] private FMODSoundSoundVolumeController _masterSoundVolumeController;
        [SerializeField] private FMODSoundSoundVolumeController _musicSoundVolumeController;
        [SerializeField] private FMODSoundSoundVolumeController _ambientSoundVolumeController;
        [SerializeField] private FMODSoundSoundVolumeController _sfxSoundVolumeController;
        
        public void Install(ServiceLocator serviceLocator)
        {
            OneShotSoundsController oneShotSoundsController = new OneShotSoundsController();
            LastingSoundsController lastingSoundsController = new LastingSoundsController(_lastingSoundsParent, _lastingSoundsControllerConfig);
            GlobalParametersController globalParametersController = new GlobalParametersController(_globalParametersConfig);


            _masterSoundVolumeController.Init(1.0f);
            _musicSoundVolumeController.Init(1.0f);
            _ambientSoundVolumeController.Init(1.0f);
            _sfxSoundVolumeController.Init(1.0f);

            SoundVolumeControllersGroup soundVolumeControllersGroup = new SoundVolumeControllersGroup(
                _masterSoundVolumeController, _musicSoundVolumeController,
                _ambientSoundVolumeController, _sfxSoundVolumeController);
            
            
            IFMODAudioManager fmodAudioManager = 
                new FMODAudioManager(oneShotSoundsController, lastingSoundsController, globalParametersController,
                    soundVolumeControllersGroup);            
            _audioManagerReference.Configure(fmodAudioManager);
            
            _audioManagerReference.GlobalParametersController.StartListeningToParameters();
            
            _gameAudiosManager.Value.Init(_audioManagerReference, serviceLocator.GetService<IEventSystemService>());
            _gameAudiosManager.Value.StartListeningToGameEvents();
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            _audioManagerReference.GlobalParametersController.StopListeningToParameters();
            _audioManagerReference.StopAllSounds();
        }
        
        private void OnDestroy()
        {
            _gameAudiosManager.Value.StopListeningToGameEvents();
        }
    }
}