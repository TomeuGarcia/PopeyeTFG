using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor;
using UnityEngine;
using UnityEngine.Serialization;

public class GameSetupInstaller : MonoBehaviour
{
    [SerializeField] private FactoriesInstaller _factoriesInstaller;
    [SerializeField] private PlayerAnchorInstaller _playerAnchorInstaller;

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
        ServiceLocator.Instance.RegisterService<ICombatManager>(new CombatManagerService());
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
