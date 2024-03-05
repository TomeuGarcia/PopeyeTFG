using Popeye.Core.Services.ServiceLocator;
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


        public void Install(ServiceLocator serviceLocator)
        {
            OneShotSoundsController oneShotSoundsController = new OneShotSoundsController();
            LastingSoundsController lastingSoundsController = new LastingSoundsController(_lastingSoundsParent, _lastingSoundsControllerConfig);
            GlobalParametersController globalParametersController = new GlobalParametersController(_globalParametersConfig);

            IFMODAudioManager fmodAudioManager = new FMODAudioManager(oneShotSoundsController, lastingSoundsController, globalParametersController);
            
            fmodAudioManager.GlobalParametersController.StartListeningToParameters();
            
            serviceLocator.RegisterService<IFMODAudioManager>(fmodAudioManager);
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            IFMODAudioManager fmodAudioManager = serviceLocator.GetService<IFMODAudioManager>(); 
            
            fmodAudioManager.GlobalParametersController.StopListeningToParameters();
            fmodAudioManager.StopAllSounds();
            
            serviceLocator.RemoveService<IFMODAudioManager>();
        }
    }
}