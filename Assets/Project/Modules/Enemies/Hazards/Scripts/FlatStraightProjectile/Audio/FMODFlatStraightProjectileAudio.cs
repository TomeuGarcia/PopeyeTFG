using NaughtyAttributes;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [System.Serializable]
    public class FMODFlatStraightProjectileAudio : IFlatStraightProjectileAudio
    {
        private IFMODAudioManager _audioManager; 
        [Expandable] [SerializeField] private LastingFMODSound _movingSound; 
        [Expandable] [SerializeField] private OneShotFMODSound _objectContactSound; 
        [Expandable] [SerializeField] private OneShotFMODSound _lifetimeEndSound; 
        
        
        public void Configure()
        {
            _audioManager = ServiceLocator.Instance.GetService<IFMODAudioManager>();
        }

        public void PlayMovingSound(GameObject source)
        {
            _audioManager.PlayLastingSound(_movingSound, source);
        }

        public void StopMovingSound()
        {
            _audioManager.StopLastingSound(_movingSound);
        }

        public void PlayObjectContactSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_objectContactSound, source);
        }

        public void PlayLifetimeEndSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_lifetimeEndSound, source);
        }
    }
}