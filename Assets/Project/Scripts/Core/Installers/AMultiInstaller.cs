using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Core.Installers
{
    public abstract class GeneralInstaller : MonoBehaviour
    {
        [SerializeField] private AInstaller[] _installers;

        private void Awake()
        {
            InstallDependencies();
        }

        private void Start()
        {
            DoStart();
        }
        private void OnDestroy()
        {
            UninstallDependencies();
        }

        protected abstract void DoStart();
        protected abstract void DoOnDestroy();

        private void InstallDependencies()
        {
            foreach (var installer in _installers)
            {
                installer.Install(ServiceLocator.Instance);
            }
            DoInstallDependencies();
        }

        protected abstract void DoInstallDependencies();

        private void UninstallDependencies()
        {
            foreach (var installer in _installers)
            {
                installer.Uninstall(ServiceLocator.Instance);
            }
            DoUninstallDependencies();
        }

        protected abstract void DoUninstallDependencies();
    }
}