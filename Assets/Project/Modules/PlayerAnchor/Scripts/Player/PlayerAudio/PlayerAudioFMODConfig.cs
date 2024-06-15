using System;
using NaughtyAttributes;
using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [CreateAssetMenu(fileName = "PlayerAudioFMODConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerAudioFMODConfig")]
    public class PlayerAudioFMODConfig : ScriptableObject
    {
        [Header("AUDIO MANAGER")] 
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        
        [Header("FOOTSTEPS")]
        [Expandable] [SerializeField] private OneShotFMODSound _leftFootstepSound;
        [Expandable] [SerializeField] private OneShotFMODSound _rightFootstepSound;
        [Expandable] [SerializeField] private LastingFMODSound _footstepsSound;
        
        [Header("DASH")]
        [Expandable] [SerializeField] private OneShotFMODSound _dashTowardsAnchorSound;
        [Expandable] [SerializeField] private OneShotFMODSound _dashDroppingAnchor;
        
        [Header("TAKE DAMAGE")]
        [Expandable] [SerializeField] private OneShotFMODSound _takeDamage;
        
        [Header("HEAL")]
        [Expandable] [SerializeField] private OneShotFMODSound _healingPerformed;

        
        private LastingFMODSound.SoundId _footstepsSoundId;

        private void OnEnable()
        {
            _footstepsSoundId = null;
        }

        public void StartPlayingStepsSounds(GameObject source)
        {
            _footstepsSoundId = _audioManager.PlayLastingSound(_footstepsSound, source);
        }

        public void StopPlayingStepsSounds()
        {
            if (_footstepsSoundId != null)
            {
                _audioManager.StopLastingSound(_footstepsSoundId);
            }
        }

        public void PlayDashTowardsAnchorSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_dashTowardsAnchorSound, source);
        }

        public void PlayDashDroppingAnchorSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_dashDroppingAnchor, source);
        }

        public void PlayTakeDamageSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_takeDamage, source);
        }

        public void PlayHealingPerformedSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_healingPerformed, source);
        }


        public void PlayLeftFootstepSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_leftFootstepSound, source);
        }

        public void PlayRightFootstepSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_rightFootstepSound, source);
        }
    }
}