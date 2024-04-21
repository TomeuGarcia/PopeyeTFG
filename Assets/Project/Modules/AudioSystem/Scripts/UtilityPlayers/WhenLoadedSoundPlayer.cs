using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class WhenLoadedSoundPlayer : MonoBehaviour
    {
        [Header("AUDIO MANAGER")]
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        
        [Header("SOUNDS")]
        [SerializeField] private GameObject _soundSource;
        [SerializeField] private OneShotFMODSound[] _oneShotSounds;
        [SerializeField] private LastingFMODSound[] _lastingSounds;
        
        private LastingFMODSound.SoundId[] _lastingSoundIds;

        
        private void OnEnable()
        {
            _audioManager.PlayOneShotsAttached(_oneShotSounds, _soundSource);
            _lastingSoundIds = _audioManager.PlayLastingSounds(_lastingSounds, _soundSource);
        }

        private void OnDisable()
        {
            _audioManager.StopLastingSounds(_lastingSoundIds);
        }
        
        
        
    }
}