using System;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class TriggerSoundPlayer : MonoBehaviour
    {
        private IFMODAudioManager _fmodAudioManager;


        [Header("TRIGGER MODE")]
        [SerializeField] private bool _playOnEnter = true;
        [SerializeField] private bool _stopOnExit = true;

        [Header("ACCEPT TYPES")]
        [SerializeField] private ObjectTypeAsset[] _acceptObjectTypes;

        [Header("SOUNDS")] 
        [SerializeField] private GameObject _soundSource;
        [SerializeField] private OneShotFMODSound[] _oneShotSounds;
        [SerializeField] private LastingFMODSound[] _lastingSounds;
        
        
        private void Start()
        {
            _fmodAudioManager = ServiceLocator.Instance.GetService<IFMODAudioManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_playOnEnter) return;
            if (!AcceptsOther(other)) return;
            
            _fmodAudioManager.PlayOneShotsAttached(_oneShotSounds, _soundSource);
            _fmodAudioManager.PlayLastingSounds(_lastingSounds, _soundSource);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_stopOnExit) return;
            if (!AcceptsOther(other)) return;
        
            _fmodAudioManager.StopLastingSounds(_lastingSounds);
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