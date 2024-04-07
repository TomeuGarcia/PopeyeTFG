using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.VFX.ParticleFactories;
using Popeye.Modules.Enemies.EnemyFactories;
using Popeye.Modules.Enemies.Hazards;
using Popeye.Scripts.Core.Scenes.PlayedScene;
using UnityEngine;


namespace Popeye.Modules.Installers
{
    public class FactoriesInstaller : MonoBehaviour
    {
        [Header("PARTICLES")]
        [SerializeField] private ParticleFactoryConfig _particleFactoryConfig;
        [SerializeField] private Transform _particleParent;
        
        [Header("ENEMIES")]
        [SerializeField] private EnemyFactoryInstaller _enemyFactoryInstaller;
        [SerializeField] private HazardsFactoryConfig _hazardFactryConfig;
        [SerializeField] private Transform _hazardsParent;

    
        public void Install(ServiceLocator serviceLocator, 
            IFMODAudioManager audioManager, IEventSystemService eventSystemService,
            ICurrentlyPlayedSceneProvider currentlyPlayedSceneProvider)
        {
            ParticleFactory particleFactory = new ParticleFactory(_particleFactoryConfig, _particleParent);
            HazardsFactory hazardsFactory = new HazardsFactory(_hazardFactryConfig, _hazardsParent, particleFactory);
            
            serviceLocator.RegisterService<IParticleFactory>(particleFactory);
            serviceLocator.RegisterService<IHazardFactory>(hazardsFactory);
            
            _enemyFactoryInstaller.Install(serviceLocator, audioManager, eventSystemService, currentlyPlayedSceneProvider);
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            _enemyFactoryInstaller.Uninstall(serviceLocator);
            
            serviceLocator.RemoveService<IHazardFactory>();
            serviceLocator.RemoveService<IParticleFactory>();
        }
    }

}
