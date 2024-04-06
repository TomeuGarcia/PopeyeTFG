using System;
using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Modules.AudioSystem.Testing
{
    public class AudioSystemTester : MonoBehaviour
    {
        [Header("SOUND SOURCE")]
        [SerializeField] private GameObject _soundSource;
        
        [Header("ONE SHOT SOUNDS")]
        [SerializeField] private RectTransform _oneShotInterfaceHolder;
        [SerializeField] private OneShotSoundInterface _oneShotInterfacePrefab;
        [SerializeField] private OneShotFMODSound[] _oneShotSounds;
        
        [Header("LASTING SOUNDS")]
        [SerializeField] private RectTransform _lastingInterfaceHolder;
        [SerializeField] private LastingSoundInterface _lastingInterfacePrefab;
        [SerializeField] private LastingFMODSound[] _lastingSounds;



        private IFMODAudioManager _fmodAudioManager;
        
        private void Start()
        {
            _fmodAudioManager = ServiceLocator.Instance.GetService<IFMODAudioManager>();
            
            
            foreach (var onShotSound in _oneShotSounds)
            {
                OneShotSoundInterface oneShotSoundInterface = Instantiate(_oneShotInterfacePrefab, _oneShotInterfaceHolder);
                oneShotSoundInterface.Init(_fmodAudioManager, onShotSound, _soundSource);
            }
            
            foreach (var lastingSound in _lastingSounds)
            {
                LastingSoundInterface oneShotSoundInterface = Instantiate(_lastingInterfacePrefab, _lastingInterfaceHolder);
                oneShotSoundInterface.Init(_fmodAudioManager, lastingSound, _soundSource);
            }
            
        }


        
    }
    
    
    
}