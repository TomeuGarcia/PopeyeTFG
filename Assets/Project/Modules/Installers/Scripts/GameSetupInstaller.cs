using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Project.Modules.CombatSystem.KnockbackSystem;
using Project.PhysicsMovement;
using Project.Scripts.Time.TimeFunctionalities;
using Project.Scripts.Time.TimeHitStop;
using Project.Scripts.Time.TimeScale;
using UnityEngine;


public class GameSetupInstaller : MonoBehaviour
{
    [SerializeField] private FactoriesInstaller _factoriesInstaller;
    [SerializeField] private PlayerAnchorInstaller _playerAnchorInstaller;

    [SerializeField] private CollisionProbingConfig _hitTargetCollisionProbingConfig;
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
            new CombatManagerService(_hitTargetCollisionProbingConfig, new KnockbackManager(_physicsTweenerBehaviour));
        ServiceLocator.Instance.RegisterService<ICombatManager>(combatManagerService);

        ITimeScaleManager timeScaleManager = new UnityTimeScaleManager();
        TimeFunctionalities timeFunctionalities =
            new TimeFunctionalities(timeScaleManager, new HitStopManager(_hitStopManagerConfig, timeScaleManager));
        ServiceLocator.Instance.RegisterService<ITimeFunctionalities>(timeFunctionalities);
        
        _factoriesInstaller.Install();
        _playerAnchorInstaller.Install();
    }
    
    private void Uninstall()
    {
        ServiceLocator.Instance.RemoveService<ICombatManager>();
        ServiceLocator.Instance.RemoveService<ITimeFunctionalities>();
        
        _factoriesInstaller.Uninstall();
        _playerAnchorInstaller.Uninstall();
    }
}
