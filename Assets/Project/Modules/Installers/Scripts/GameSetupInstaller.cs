using Popeye.Core.Services.ServiceLocator;
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
    
    [Header("FACTORIES")]
    [SerializeField] private FactoriesInstaller _factoriesInstaller;
    
    [Header("PLAYER ANCHOR")]
    [SerializeField] private PlayerAnchorInstaller _playerAnchorInstaller;

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
        CombatManagerService combatManagerService = 
            new CombatManagerService(_hitTargetCollisionProbingConfig, 
                new KnockbackManager(_physicsTweenerBehaviour, _floorPlatformsProbingConfig));
        ServiceLocator.Instance.RegisterService<ICombatManager>(combatManagerService);

        ITimeScaleManager timeScaleManager = new UnityTimeScaleManager();
        TimeFunctionalities timeFunctionalities =
            new TimeFunctionalities(timeScaleManager, new HitStopManager(_hitStopManagerConfig, timeScaleManager));
        ServiceLocator.Instance.RegisterService<ITimeFunctionalities>(timeFunctionalities);
        
        _objectTypesInstaller.Install();
        _factoriesInstaller.Install(ServiceLocator.Instance);
        _playerAnchorInstaller.Install();
    }
    
    private void Uninstall()
    {
        ServiceLocator.Instance.RemoveService<ICombatManager>();
        ServiceLocator.Instance.RemoveService<ITimeFunctionalities>();
        
        _playerAnchorInstaller.Uninstall();
        _factoriesInstaller.Uninstall(ServiceLocator.Instance);
        _objectTypesInstaller.Uninstall();
    }
}
