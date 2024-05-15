using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Project.Modules.WorldElements.DestructiblePlatforms
{
    [System.Serializable]
    public class DestructiblePlatformAudio
    {
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        [SerializeField] private OneShotFMODSound _startBreakingSound;
        [SerializeField] private OneShotFMODSound _breakingSound;
        [SerializeField] private OneShotFMODSound _regeneratingSound;
        
        
        public void PlayStartBreakingOverTimeSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_startBreakingSound, source);
        }
        
        public void PlayBreakingSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_breakingSound, source);
        }

        public void PlayRegeneratingSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_regeneratingSound, source);
        }
    }
}