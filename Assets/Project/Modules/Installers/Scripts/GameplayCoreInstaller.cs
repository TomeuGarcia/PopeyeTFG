using Popeye.Core.Installers;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.InformationDisplay;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.GameDataEvents;
using Popeye.Modules.GameState;
using Popeye.Modules.PlayerAnchor;
using Popeye.Scripts.Collisions;
using Popeye.Scripts.Core.Scenes.PlayedScene;
using Project.Modules.CombatSystem.KnockbackSystem;
using Project.PhysicsMovement;
using Project.Scripts.Time.TimeFunctionalities;
using UnityEngine;

namespace Popeye.Modules.Installers
{

    public class GameplayCoreInstaller : MonoBehaviour
    {


        [Header("FACTORIES")] 
        [SerializeField] private FactoriesInstaller _factoriesInstaller;

        [Header("PLAYER ANCHOR")] 
        [SerializeField] private PlayerAnchorInstaller _playerAnchorInstaller;

        [Header("GAME REFERENCES")] 
        [SerializeField] private GameReferencesInstaller _gameReferencesInstaller;

        [Header("GAME EVENTS")] 
        [SerializeField] private GameDataEventsInstaller _gameDataEventsInstaller;

        [Header("INFORMATION DISPLAY")] 
        [SerializeField] private InformationDisplayInstaller _informationDisplayInstaller;

        [Header("OTHER")] 
        [SerializeField] private CollisionProbingConfig _hitTargetCollisionProbingConfig;
        [SerializeField] private CollisionProbingConfig _floorPlatformsProbingConfig;
        [SerializeField] private PhysicsTweenerBehaviour _physicsTweenerBehaviour;



        private LastLoadedSceneProvider _lastLoadedSceneProvider;
        
        private TimeManagerGameEventsListener _timeManagerGameEventsListener;



        void Awake()
        {
            Install();
        }

        private void OnDestroy()
        {
            Uninstall();
        }

        private void Install()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;

            IEventSystemService eventSystemService = serviceLocator.GetService<IEventSystemService>();
            ITimeFunctionalities timeFunctionalities = serviceLocator.GetService<ITimeFunctionalities>();
            IFMODAudioManager audioManager = ServiceLocator.Instance.GetService<IFMODAudioManager>();


            _lastLoadedSceneProvider = new LastLoadedSceneProvider(eventSystemService);
            _lastLoadedSceneProvider.StartListeningToSceneUpdates();
            

            CombatManagerService combatManagerService =
                new CombatManagerService(_hitTargetCollisionProbingConfig,
                    new KnockbackManager(_physicsTweenerBehaviour, _floorPlatformsProbingConfig));
            serviceLocator.RegisterService<ICombatManager>(combatManagerService);

            

            
            _informationDisplayInstaller.Install(serviceLocator);
            _factoriesInstaller.Install(serviceLocator, audioManager, eventSystemService, _lastLoadedSceneProvider);
            _playerAnchorInstaller.Install();
            _gameReferencesInstaller.Install(serviceLocator, _playerAnchorInstaller.PlayerMediator);
            _gameDataEventsInstaller.Install(eventSystemService, _lastLoadedSceneProvider);


            
            _timeManagerGameEventsListener = new TimeManagerGameEventsListener(eventSystemService, timeFunctionalities.TimeScaleManager);
            _timeManagerGameEventsListener.StartListening();
        }

        private void Uninstall()
        {
            _timeManagerGameEventsListener.StopListening();

            ServiceLocator serviceLocator = ServiceLocator.Instance;

            serviceLocator.RemoveService<ICombatManager>();
            

            _gameDataEventsInstaller.Uninstall();
            _gameReferencesInstaller.Uninstall(serviceLocator);

            _playerAnchorInstaller.Uninstall();
            _factoriesInstaller.Uninstall(serviceLocator);
            _informationDisplayInstaller.Uninstall(serviceLocator);
            
            _lastLoadedSceneProvider.StopListeningToSceneUpdates();
        }
    }

}