using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.VFX.ParticleFactories;
using Popeye.Modules.Enemies.EnemyFactories;
using Popeye.Modules.Enemies.Hazards;
using UnityEngine;


namespace Popeye.Modules.Installers
{
    public class FactoriesInstaller : MonoBehaviour
    {
        [SerializeField] private ParticleFactoryConfig _particleFactoryConfig;
        [SerializeField] private EnemyFactoryInstaller _enemyFactoryInstaller;
        [SerializeField] private HazardsFactoryConfig _hazardFactryConfig;
        [SerializeField] private Transform _hazardsParent;

        [SerializeField] private Transform _particleParent;
    
        public void Install(ServiceLocator serviceLocator)
        {
            serviceLocator.RegisterService<IParticleFactory>(new ParticleFactory(_particleFactoryConfig, _particleParent));
            serviceLocator.RegisterService<IHazardFactory>(new HazardsFactory(_hazardFactryConfig,_hazardsParent, serviceLocator.GetService<IParticleFactory>()));
            _enemyFactoryInstaller.Install(serviceLocator, ServiceLocator.Instance.GetService<IFMODAudioManager>());
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            serviceLocator.RemoveService<IParticleFactory>();
            serviceLocator.RemoveService<IHazardFactory>();
            _enemyFactoryInstaller.Uninstall(serviceLocator);
        }
    }

}
