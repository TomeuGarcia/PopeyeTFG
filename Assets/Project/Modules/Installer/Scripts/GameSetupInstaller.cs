using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies;
using Project.Modules.CombatSystem;
using Project.Modules.PlayerAnchor;
using UnityEngine;

public class GameSetupInstaller : MonoBehaviour
{
    
    [Header("Enemy")]
    private EnemyFactory _enemyFactory;
    [SerializeField] private EnemyConfiguration _enemyConfiguration;
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
        ServiceLocator.Instance.RegisterService<EnemyFactory>(new EnemyFactory(Instantiate(_enemyConfiguration)));

        _playerAnchorInstaller.Install();
    }
    
    private void Uninstall()
    {
        ServiceLocator.Instance.RemoveService<ICombatManager>();
        ServiceLocator.Instance.RemoveService<EnemyFactory>();

        _playerAnchorInstaller.Uninstall();
    }
}
