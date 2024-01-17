using UnityEngine;
using Popeye.Modules.VFX.Generic;

namespace Popeye.Modules.VFX.ParticleFactories
{
    public interface IParticleFactory
    {
        Transform Create(ParticleTypes type, Vector3 position, Quaternion rotation, Transform parent = null);
    }
}