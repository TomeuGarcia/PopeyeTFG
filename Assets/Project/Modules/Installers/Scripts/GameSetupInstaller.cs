using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Project.Modules.CombatSystem.KnockbackSystem;
using Project.PhysicsMovement;
using UnityEngine;


public class GameSetupInstaller : MonoBehaviour
{
    [SerializeField] private FactoriesInstaller _factoriesInstaller;
    [SerializeField] private PlayerAnchorInstaller _playerAnchorInstaller;

    [SerializeField] private CollisionProbingConfig _hitTargetCollisionProbingConfig;
    [SerializeField] private PhysicsTweenerBehaviour _physicsTweenerBehaviour;

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
        
        _factoriesInstaller.Install();
        _playerAnchorInstaller.Install();
    }
    
    private void Uninstall()
    {
        ServiceLocator.Instance.RemoveService<ICombatManager>();
        
        _factoriesInstaller.Uninstall();
        _playerAnchorInstaller.Uninstall();
    }
}
