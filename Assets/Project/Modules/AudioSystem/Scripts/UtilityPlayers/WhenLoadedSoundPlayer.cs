using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class WhenLoadedSoundPlayer : MonoBehaviour
    {
        private IFMODAudioManager _fmodAudioManager;

        [Header("SOUNDS")]
        [SerializeField] private GameObject _soundSource;
        [SerializeField] private OneShotFMODSound[] _oneShotSounds;
        [SerializeField] private LastingFMODSound[] _lastingSounds;
        
        
        private void Start()
        {
            _fmodAudioManager = ServiceLocator.Instance.GetService<IFMODAudioManager>();
        }


        private void OnEnable()
        {
            _fmodAudioManager.PlayOneShotsAttached(_oneShotSounds, _soundSource);
            _fmodAudioManager.PlayLastingSounds(_lastingSounds, _soundSource);
        }

        private void OnDisable()
        {
            _fmodAudioManager.StopLastingSounds(_lastingSounds);
        }
        
        
        
    }
}