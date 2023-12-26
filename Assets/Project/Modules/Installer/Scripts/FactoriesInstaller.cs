using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Modules.CombatSystem;
using UnityEngine;

public class FactoriesInstaller : MonoBehaviour
{
    [SerializeField] private ParticleFactoryConfig _particleFactoryConfig;
    
    private void Awake()
    {
        Install();
    }

    public void Install()
    {
        ServiceLocator.Instance.RegisterService<IParticleFactory>(new ParticleFactory(_particleFactoryConfig));
    }

    private void OnDestroy()
    {
        Uninstall();
    }

    public void Uninstall()
    {
        ServiceLocator.Instance.RemoveService<IParticleFactory>();
    }
}
