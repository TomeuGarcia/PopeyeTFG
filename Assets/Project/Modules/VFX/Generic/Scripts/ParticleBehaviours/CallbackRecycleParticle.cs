using System;
using System.Collections.Generic;
using Popeye.Core.Pool;
using UnityEngine;

namespace Popeye.Modules.VFX.Generic.ParticleBehaviours
{
    public class CallbackRecycleParticle : RecyclableObject
    {
        [SerializeField] private List<ParticleSystem> _particleSystems = new();
        private int _completedParticles;

        internal override void Init()
        {
            _completedParticles = 0;
            
            foreach (var particleSystem in _particleSystems)
            {
                particleSystem.Play();
            }
        }

        public void OnParticleSystemStopped()
        {
            _completedParticles++;

            if (_completedParticles >= _particleSystems.Count)
            {
                Recycle();
            }
        }

        internal override void Release() { }
    }
}