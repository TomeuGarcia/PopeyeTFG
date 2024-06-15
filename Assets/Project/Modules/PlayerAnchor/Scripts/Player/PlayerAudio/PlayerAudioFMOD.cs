using System.Collections.Generic;
using FMODUnity;
using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerAudioFMOD : MonoBehaviour, IPlayerAudio
    {
        private GameObject _playerGameObject;
        private PlayerAudioFMODConfig _config;
        private PlayerMovementChecker _playerMovementChecker;
        
        [SerializeField] private StudioEventEmitter _healingPreparationEmitter;
        

        public void Configure(GameObject playerGameObject,
            PlayerAudioFMODConfig config,
            PlayerMovementChecker playerMovementChecker)
        {
            _playerGameObject = playerGameObject;
            _config = config;
            _playerMovementChecker = playerMovementChecker;
        }
        
        public void StartPlayingStepsSounds()
        {
            _config.StartPlayingStepsSounds(_playerGameObject);
        }

        public void StopPlayingStepsSounds()
        {
            _config.StopPlayingStepsSounds();
        }

        public void PlayDashTowardsAnchorSound()
        {
            _config.PlayDashTowardsAnchorSound(_playerGameObject);
        }

        public void PlayDashDroppingAnchorSound()
        {
            _config.PlayDashDroppingAnchorSound(_playerGameObject);
        }

        public void PlayTakeDamageSound()
        {
            _config.PlayTakeDamageSound(_playerGameObject);
        }

        public void StartPlayingHealingPreparationSound()
        {
            _healingPreparationEmitter.Play();
        }

        public void StopPlayingHealingPreparationSound()
        {
            _healingPreparationEmitter.Stop();
        }

        public void PlayHealingPerformedSound()
        {
            _config.PlayHealingPerformedSound(_playerGameObject);
        }


        public void OnLeftFootstep()
        {
            if (!_playerMovementChecker.IsMoving) return;
            
            _config.PlayLeftFootstepSound(_playerGameObject);
        }

        public void OnRightFootstep()
        {
            if (!_playerMovementChecker.IsMoving) return;
        
            _config.PlayRightFootstepSound(_playerGameObject);
        }
        
        
    }
}

