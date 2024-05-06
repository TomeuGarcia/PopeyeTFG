using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerAudioFMOD : IPlayerAudio
    {
        private readonly GameObject _playerGameObject;
        private readonly IFMODAudioManager _fmodAudioManager;
        private readonly PlayerAudioFMODConfig _config;
        private readonly PlayerMovementChecker _playerMovementChecker;

        private LastingFMODSound.SoundId _footstepsSoundId;
        private LastingFMODSound.SoundId _healingPreparationSoundId;


        public PlayerAudioFMOD(GameObject playerGameObject,
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
            _healingPreparationSoundId = 
                _fmodAudioManager.PlayLastingSound(_config.HealingPreparation, _playerGameObject);
        }

        public void StopPlayingHealingPreparationSound()
        {
            _fmodAudioManager.StopLastingSound(_healingPreparationSoundId);
        }

        public void PlayHealingPerformedSound()
        {
            PlayOneShotAttached(_config.HealingPerformed);
            _fmodAudioManager.StopLastingSound(_healingPreparationSoundId);
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

