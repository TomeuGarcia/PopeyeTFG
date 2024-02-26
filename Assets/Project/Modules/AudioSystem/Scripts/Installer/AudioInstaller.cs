using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class AudioInstaller : MonoBehaviour
    {
        [SerializeField] private LastingSoundsControllerConfig _lastingSoundsControllerConfig;
        [SerializeField] private Transform _lastingSoundsParent;


        public void Install(ServiceLocator serviceLocator)
        {
            serviceLocator.RegisterService<IFMODAudioManager>(
                new FMODAudioManager(_lastingSoundsParent, _lastingSoundsControllerConfig)
            );
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            serviceLocator.RemoveService<IFMODAudioManager>();
        }
    }
}