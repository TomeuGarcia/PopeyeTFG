using UnityEngine;

namespace Popeye.Modules.VFX.Generic
{
    public class UnscaledTimeParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particles;

        protected void Awake()
        {
            foreach (ParticleSystem particle in _particles)
            {
                ParticleSystem.MainModule mainModule = particle.main;
                mainModule.useUnscaledTime = true;
            }
            
            Destroy(this);
        }
        
    }
}