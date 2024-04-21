using NaughtyAttributes;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [System.Serializable]
    public class FMODFlatStraightProjectileAudio : IFlatStraightProjectileAudio
    {
        [Header("AUDIO MANAGER")]
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        
        [Header("SOUNDS")]
        [Expandable] [SerializeField] private LastingFMODSound _movingSound; 
        [Expandable] [SerializeField] private OneShotFMODSound _objectContactSound; 
        [Expandable] [SerializeField] private OneShotFMODSound _lifetimeEndSound;


        private LastingFMODSound.SoundId _movingSoundId;
        
        public void PlayMovingSound(GameObject source)
        {
            _movingSoundId = _audioManager.PlayLastingSound(_movingSound, source);
        }

        public void StopMovingSound()
        {
            _audioManager.StopLastingSound(_movingSoundId);
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