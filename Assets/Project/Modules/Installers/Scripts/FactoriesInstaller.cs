using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.VFX.ParticleFactories;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FactoriesInstaller : MonoBehaviour
{
    [SerializeField] private ParticleFactoryConfig _particleFactoryConfig;
    [SerializeField] private EnemyFactoryConfiguration _enemyFactoryConfiguration;

    [SerializeField] private Transform _particleParent;
    
    public void Install()
    {
        ServiceLocator.Instance.RegisterService<IParticleFactory>(new ParticleFactory(_particleFactoryConfig, _particleParent));
        ServiceLocator.Instance.RegisterService<EnemyFactory>(new EnemyFactory(Instantiate(_enemyFactoryConfiguration)));
    }

    public void Uninstall()
    {
        ServiceLocator.Instance.RemoveService<IParticleFactory>();
        ServiceLocator.Instance.RemoveService<EnemyFactory>();
    }
}
