using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerAudioFMOD : MonoBehaviour, IPlayerAudio
    {
        private GameObject _playerGameObject;
        private IFMODAudioManager _fmodAudioManager;
        
        [SerializeField] private LastingFMODSound _footstepsSound;
        
        
        public void Configure(GameObject playerGameObject, IFMODAudioManager fmodAudioManager)
        {
            _playerGameObject = playerGameObject;
            _fmodAudioManager = fmodAudioManager;
        }
        
        public void StartPlayingStepsSounds()
        {
            _fmodAudioManager.PlayLastingSound(_footstepsSound, _playerGameObject);
        }

        public void StopPlayingStepsSounds()
        {
            _fmodAudioManager.StopLastingSound(_footstepsSound);
        }
        
        
    }
}

