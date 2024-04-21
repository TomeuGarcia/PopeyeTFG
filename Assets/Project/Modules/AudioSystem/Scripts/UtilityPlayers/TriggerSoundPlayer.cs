using System;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class TriggerSoundPlayer : MonoBehaviour
    {
        [Header("TRIGGER MODE")]
        [SerializeField] private bool _playOnEnter = true;
        [SerializeField] private bool _stopOnExit = true;

        [Header("ACCEPT TYPES")]
        [SerializeField] private ObjectTypeAsset[] _acceptObjectTypes;

        [Header("AUDIO MANAGER")]
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        
        [Header("SOUNDS")]
        [SerializeField] private GameObject _soundSource;
        [SerializeField] private OneShotFMODSound[] _oneShotSounds;
        [SerializeField] private LastingFMODSound[] _lastingSounds;

        private LastingFMODSound.SoundId[] _lastingSoundIds;
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (!_playOnEnter) return;
            if (!AcceptsOther(other)) return;
            
            _audioManager.PlayOneShotsAttached(_oneShotSounds, _soundSource);
            _lastingSoundIds = _audioManager.PlayLastingSounds(_lastingSounds, _soundSource);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_stopOnExit) return;
            if (!AcceptsOther(other)) return;
        
            _audioManager.StopLastingSounds(_lastingSoundIds);
        }

        private bool AcceptsOther(Collider other)
        {
            if (!other.TryGetComponent(out ObjectTypeBehaviour objectTypeBehaviour))
            {
                return false;
            }

            return objectTypeBehaviour.IsOfAnyType(_acceptObjectTypes);
        }
        
    }
}