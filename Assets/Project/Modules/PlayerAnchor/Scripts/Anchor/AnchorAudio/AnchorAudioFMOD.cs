using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorAudioFMOD : IAnchorAudio
    {
        private readonly GameObject _anchorGameObject;
        private readonly IFMODAudioManager _fmodAudioManager;
        private readonly AnchorAudioFMODConfig _config;

        public AnchorAudioFMOD(GameObject anchorGameObject, IFMODAudioManager fmodAudioManager, AnchorAudioFMODConfig config)
        {
            _anchorGameObject = anchorGameObject;
            _fmodAudioManager = fmodAudioManager;
            _config = config;
        }
        
        
        public void PlayThrowSound()
        {
            PlayOneShotAttached(_config.Throw);
        }

        public void PlayPullSound()
        {
            PlayOneShotAttached(_config.Pull);
        }

        public void PlayPickedUpSound()
        {
            PlayOneShotAttached(_config.Grab);           
        }

        public void PlayDealDamageSound()
        {
            PlayOneShotAttached(_config.DealDamage);
        }

        public void PlayLandOnFloorSound()
        {
            PlayOneShotAttached(_config.LandOnFloor);
        }


        private void PlayOneShotAttached(OneShotFMODSound oneShotSound)
        {
            _fmodAudioManager.PlayOneShotAttached(oneShotSound, _anchorGameObject);
        }
        
    }
}