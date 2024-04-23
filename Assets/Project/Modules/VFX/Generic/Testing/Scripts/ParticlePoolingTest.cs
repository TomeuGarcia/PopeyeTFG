using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

public class ParticlePoolingTest : MonoBehaviour
{
    [SerializeField] private ParticleTypes particleType;
    
    private IParticleFactory particleFactory;
    
    private void Start()
    {
        particleFactory = ServiceLocator.Instance.GetService<IParticleFactory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            particleFactory.Create(particleType, transform.position, transform.rotation);
        }
    }
}
