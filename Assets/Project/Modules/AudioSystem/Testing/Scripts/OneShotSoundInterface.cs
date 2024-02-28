using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.AudioSystem.Testing
{
    public class OneShotSoundInterface : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _playAtPositionButton;

        private IFMODAudioManager _audioManager;
        private OneShotFMODSound _sound;
        private GameObject _attachedGameObject;

        public void Init(IFMODAudioManager audioManager, OneShotFMODSound sound, GameObject attachedGameObject)
        {
            _audioManager = audioManager;
            _sound = sound;
            _attachedGameObject = attachedGameObject;
            
            _titleText.text = sound.name.Split('_')[^1];
        }
        
        
        private void OnEnable()
        {
            _playButton.onClick.AddListener(PlaySound);
            _playAtPositionButton.onClick.AddListener(PlaySoundAtPosition);
        }
        
        private void OnDisable()
        {
            _playButton.onClick.RemoveAllListeners();
            _playAtPositionButton.onClick.RemoveAllListeners();
        }
        
        private void PlaySound()
        {
            _audioManager.PlayOneShot(_sound);
        }
        private void PlaySoundAtPosition()
        {
            _audioManager.PlayOneShotAttached(_sound, _attachedGameObject);
        }
    }
}