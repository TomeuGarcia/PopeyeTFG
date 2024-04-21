using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using Popeye.Modules.Enemies.EnemyFactories;
using Popeye.Modules.Enemies.Hazards;
using Popeye.Scripts.Core.Scenes.ObjectTracking;
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


        private SceneObjectsTracker _createdParticlesRecycler;
        private SceneObjectsTracker _createdEnemiesRecycler;
        
    
        public void Install(ServiceLocator serviceLocator, 
            IEventSystemService eventSystemService, ICombatManager combatManager,
            ICurrentlyPlayedSceneProvider currentlyPlayedSceneProvider)
        {
            _createdParticlesRecycler = new SceneObjectsTracker(eventSystemService, currentlyPlayedSceneProvider); 
            
            _createdEnemiesRecycler = new SceneObjectsTracker(eventSystemService, currentlyPlayedSceneProvider);
            
            ParticleFactory particleFactory = new ParticleFactory(_particleFactoryConfig, _particleParent, _createdParticlesRecycler);
            HazardsFactory hazardsFactory = new HazardsFactory(_hazardFactryConfig, _hazardsParent, combatManager, particleFactory);
            
            serviceLocator.RegisterService<IParticleFactory>(particleFactory);
            serviceLocator.RegisterService<IHazardFactory>(hazardsFactory);
            
            _enemyFactoryInstaller.Install(serviceLocator, _createdEnemiesRecycler);
            
            _createdParticlesRecycler.StartListeningToSceneUpdates();
            _createdEnemiesRecycler.StartListeningToSceneUpdates();
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            _createdEnemiesRecycler.StopListeningToSceneUpdates();
            _createdParticlesRecycler.StopListeningToSceneUpdates();
        
            _enemyFactoryInstaller.Uninstall(serviceLocator);
            
            serviceLocator.RemoveService<IHazardFactory>();
            serviceLocator.RemoveService<IParticleFactory>();
        }
    }

}
