using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.AudioSystem.Testing
{
    public class LastingSoundInterface : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _stopButton;

        private IFMODAudioManager _audioManager;
        private LastingFMODSound _sound;
        private GameObject _attachedGameObject;

        public void Init(IFMODAudioManager audioManager, LastingFMODSound sound, GameObject attachedGameObject)
        {
            _audioManager = audioManager;
            _sound = sound;
            _attachedGameObject = attachedGameObject;

            _titleText.text = sound.name.Split('_')[^1];
        }
        
        
        private void OnEnable()
        {
            _playButton.onClick.AddListener(PlaySound);
            _stopButton.onClick.AddListener(StopSound);
        }
        
        private void OnDisable()
        {
            _playButton.onClick.RemoveAllListeners();
            _stopButton.onClick.RemoveAllListeners();
        }
        
        private void PlaySound()
        {
            _audioManager.PlayLastingSound(_sound, _attachedGameObject);
        }
        private void StopSound()
        {
            _audioManager.StopLastingSound(_sound);
        }
    }
}