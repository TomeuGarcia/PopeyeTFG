using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies;
using Project.Modules.CombatSystem;
using UnityEngine;

public class GameSetupInstaller : MonoBehaviour
{
    
    [Header("Enemy")]
    private EnemyFactory _enemyFactory;
    [SerializeField] private EnemyConfiguration _enemyConfiguration;
    void Awake()
    {
        Install();
    }

    private void Install()
    {
        ServiceLocator.Instance.RegisterService<ICombatManager>(new CombatManagerService());
        ServiceLocator.Instance.RegisterService<EnemyFactory>(new EnemyFactory(Instantiate(_enemyConfiguration)));
    }
}
