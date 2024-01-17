using System;
using Popeye.Core.Pool;
using UnityEngine;

namespace Popeye.Modules.VFX.Generic.ParticleBehaviours
{
    public class CallbackRecycleParticle : RecyclableObject
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        internal override void Init()
        {
            _particleSystem.Play();
        }

        public void OnParticleSystemStopped()
        {
            Recycle();
        }

        internal override void Release() { }
    }
}