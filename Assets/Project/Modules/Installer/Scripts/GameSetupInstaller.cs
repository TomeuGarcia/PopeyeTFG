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
    
    [Header("Enemy")]
    private EnemyFactory _enemyFactory;
    [FormerlySerializedAs("_enemyConfiguration")] [SerializeField] private EnemyFactoryConfiguration enemyFactoryConfiguration;
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
        ServiceLocator.Instance.RegisterService<EnemyFactory>(new EnemyFactory(Instantiate(enemyFactoryConfiguration)));

        _playerAnchorInstaller.Install();
    }
    
    private void Uninstall()
    {
        ServiceLocator.Instance.RemoveService<ICombatManager>();
        ServiceLocator.Instance.RemoveService<EnemyFactory>();

        _playerAnchorInstaller.Uninstall();
    }
}
