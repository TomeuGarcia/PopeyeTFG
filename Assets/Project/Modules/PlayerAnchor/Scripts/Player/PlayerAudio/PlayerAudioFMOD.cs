using System.Collections.Generic;
using FMODUnity;
using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerAudioFMOD : MonoBehaviour, IPlayerAudio
    {
        private GameObject _playerGameObject;
        private IFMODAudioManager _fmodAudioManager;
        private PlayerAudioFMODConfig _config;
        private PlayerMovementChecker _playerMovementChecker;

        private LastingFMODSound.SoundId _footstepsSoundId;

        [SerializeField] private StudioEventEmitter _healingPreparationEmitter;
        

        public void Configure(GameObject playerGameObject,
            IFMODAudioManager fmodAudioManager, PlayerAudioFMODConfig config,
            PlayerMovementChecker playerMovementChecker)
        {
            _playerGameObject = playerGameObject;
            _fmodAudioManager = fmodAudioManager;
            _config = config;
            _playerMovementChecker = playerMovementChecker;
        }
        
        private void PlayOneShotAttached(OneShotFMODSound oneShotSound)
        {
            _fmodAudioManager.PlayOneShotAttached(oneShotSound, _playerGameObject);
        }
        
        public void StartPlayingStepsSounds()
        {
            _footstepsSoundId = _fmodAudioManager.PlayLastingSound(_config.FootstepsSound, _playerGameObject);
        }

        public void StopPlayingStepsSounds()
        {
            if (_footstepsSoundId != null)
            {
                _fmodAudioManager.StopLastingSound(_footstepsSoundId);
            }
        }

        public void PlayDashTowardsAnchorSound()
        {
            PlayOneShotAttached(_config.DashTowardsAnchorSound);
        }

        public void PlayDashDroppingAnchorSound()
        {
            PlayOneShotAttached(_config.DashDroppingAnchor);
        }

        public void PlayTakeDamageSound()
        {
            PlayOneShotAttached(_config.TakeDamage);
        }

        public void StartPlayingHealingPreparationSound()
        {
            //_fmodAudioManager.PlayOneShotAttached(_config.HealingPreparation, _playerGameObject);
            _healingPreparationEmitter.Play();
        }

        public void StopPlayingHealingPreparationSound()
        {
            _healingPreparationEmitter.Stop();
        }

        public void PlayHealingPerformedSound()
        {
            PlayOneShotAttached(_config.HealingPerformed);
        }


        public void OnLeftFootstep()
        {
            if (!_playerMovementChecker.IsMoving) return;
            
            PlayOneShotAttached(_config.LeftFootstepSound);
        }

        public void OnRightFootstep()
        {
            if (!_playerMovementChecker.IsMoving) return;
        
            PlayOneShotAttached(_config.RightFootstepSound);
        }
        
        
    }
}

