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
        [Expandable] [SerializeField] private OneShotFMODSound _objectContactSound; 
        [Expandable] [SerializeField] private OneShotFMODSound _lifetimeEndSound;
        [Expandable] [SerializeField] private OneShotFMODSound _dealDamageSound;
        

        public void PlayObjectContactSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_objectContactSound, source);
        }

        public void PlayLifetimeEndSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_lifetimeEndSound, source);
        }

        public void PlayDealDamageSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_dealDamageSound, source);
        }
    }
}