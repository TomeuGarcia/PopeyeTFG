using Popeye.Core.Installers;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor;
using Popeye.Scripts.Collisions;
using Project.Modules.CombatSystem.KnockbackSystem;
using Project.Modules.Installers.Scripts;
using Project.PhysicsMovement;
using Project.Scripts.Time.TimeFunctionalities;
using Project.Scripts.Time.TimeHitStop;
using Project.Scripts.Time.TimeScale;
using UnityEngine;


public class GameSetupInstaller : MonoBehaviour
{
    [Header("OBJECT TYPES")]
    [SerializeField] private ObjectTypesInstaller _objectTypesInstaller;

    [Header("AUDIO")] 
    [SerializeField] private AudioInstaller _audioInstaller;
    
    [Header("FACTORIES")]
    [SerializeField] private FactoriesInstaller _factoriesInstaller;
    
    [Header("PLAYER ANCHOR")]
    [SerializeField] private PlayerAnchorInstaller _playerAnchorInstaller;

    [Header("GAME REFERENCES")] 
    [SerializeField] private GameReferencesInstaller _gameReferencesInstaller;
    
    [Header("OTHER")]
    [SerializeField] private CollisionProbingConfig _hitTargetCollisionProbingConfig;
    [SerializeField] private CollisionProbingConfig _floorPlatformsProbingConfig;
    [SerializeField] private PhysicsTweenerBehaviour _physicsTweenerBehaviour;
    
    [SerializeField] private HitStopManagerConfig _hitStopManagerConfig;

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
        
        CombatManagerService combatManagerService = 
            new CombatManagerService(_hitTargetCollisionProbingConfig, 
                new KnockbackManager(_physicsTweenerBehaviour, _floorPlatformsProbingConfig));
        serviceLocator.RegisterService<ICombatManager>(combatManagerService);

        ITimeScaleManager timeScaleManager = new UnityTimeScaleManager();
        TimeFunctionalities timeFunctionalities =
            new TimeFunctionalities(timeScaleManager, new HitStopManager(_hitStopManagerConfig, timeScaleManager));
        serviceLocator.RegisterService<ITimeFunctionalities>(timeFunctionalities);
        
        _objectTypesInstaller.Install();
        _factoriesInstaller.Install(serviceLocator);
        _audioInstaller.Install(serviceLocator);
        _playerAnchorInstaller.Install();
        
        _gameReferencesInstaller.Install(serviceLocator, _playerAnchorInstaller.PlayerMediator);
    }
    
    private void Uninstall()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;
        
        serviceLocator.RemoveService<ICombatManager>();
        serviceLocator.RemoveService<ITimeFunctionalities>();
        
        _gameReferencesInstaller.Uninstall(serviceLocator);
        
        _playerAnchorInstaller.Uninstall();
        _factoriesInstaller.Uninstall(serviceLocator);
        _audioInstaller.Uninstall(serviceLocator);
        _objectTypesInstaller.Uninstall();
        
    }
}
