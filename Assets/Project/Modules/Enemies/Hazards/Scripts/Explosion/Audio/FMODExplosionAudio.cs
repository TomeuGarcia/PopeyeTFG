using NaughtyAttributes;
using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [System.Serializable]
    public class FMODExplosionAudio : IExplosionAudio
    {
        [Header("AUDIO MANAGER")]
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        
        [Header("SOUNDS")]
        [Expandable] [SerializeField] private OneShotFMODSound _explosionSound; 
        [Expandable] [SerializeField] private OneShotFMODSound _dealDamageSound; 
        
        
        public void PlayExplosionSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_explosionSound, source);
        }
        public void PlayDealDamageSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_dealDamageSound, source);
        }
    }
}