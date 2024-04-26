using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.ParticleFactories;
using Unity.Mathematics;
using UnityEngine;

public class VFXTester : MonoBehaviour
{
    [SerializeField] private ParticleTypes _testParticleType1;
    [SerializeField] private Transform _particleParent;
    
    private void Start()
    {
        StartCoroutine(TestVFX());
    }

    private IEnumerator TestVFX()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            ServiceLocator.Instance.GetService<IParticleFactory>().Create(_testParticleType1, Vector3.zero, quaternion.identity, _particleParent);
        }
    }
}
