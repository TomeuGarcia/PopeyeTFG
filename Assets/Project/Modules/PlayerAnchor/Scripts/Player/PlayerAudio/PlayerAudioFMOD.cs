using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerAudioFMOD : IPlayerAudio
    {
        private readonly GameObject _playerGameObject;
        private readonly IFMODAudioManager _fmodAudioManager;
        private readonly PlayerAudioFMODConfig _config;


        public PlayerAudioFMOD(GameObject playerGameObject, IFMODAudioManager fmodAudioManager, PlayerAudioFMODConfig config)
        {
            _playerGameObject = playerGameObject;
            _fmodAudioManager = fmodAudioManager;
            _config = config;
        }
        
        public void StartPlayingStepsSounds()
        {
            _fmodAudioManager.PlayLastingSound(_config.FootstepsSound, _playerGameObject);
        }

        public void StopPlayingStepsSounds()
        {
            _fmodAudioManager.StopLastingSound(_config.FootstepsSound);
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

        private void PlayOneShotAttached(OneShotFMODSound oneShotSound)
        {
            _fmodAudioManager.PlayOneShotAttached(oneShotSound, _playerGameObject);
        }
    }
}

