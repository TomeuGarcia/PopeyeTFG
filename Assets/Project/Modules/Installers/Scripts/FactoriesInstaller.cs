using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.VFX.ParticleFactories;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies;
using Popeye.Modules.Enemies.EnemyFactories;
using Popeye.Modules.Enemies.Hazards;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FactoriesInstaller : MonoBehaviour
{
    [SerializeField] private ParticleFactoryConfig _particleFactoryConfig;
    [SerializeField] private EnemyFactoryInstaller _enemyFactoryInstaller;
    [SerializeField] private HazardsFactoryConfig _hazardFactryConfig;
    [SerializeField] private Transform _hazardsParent;

    [SerializeField] private Transform _particleParent;
    
    public void Install(ServiceLocator serviceLocator)
    {
        serviceLocator.RegisterService<IParticleFactory>(new ParticleFactory(_particleFactoryConfig, _particleParent));
        serviceLocator.RegisterService<IHazardFactory>(new HazardsFactory(_hazardFactryConfig,_hazardsParent));
        _enemyFactoryInstaller.Install(serviceLocator);
    }

    public void Uninstall(ServiceLocator serviceLocator)
    {
        serviceLocator.RemoveService<IParticleFactory>();
        serviceLocator.RemoveService<IHazardFactory>();
        _enemyFactoryInstaller.Uninstall(serviceLocator);
    }
}
