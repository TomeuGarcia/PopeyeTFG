using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    [CreateAssetMenu(fileName = "AbilityUnlockerChristalSounds", 
        menuName = ScriptableObjectsHelper.PLAYERABILITYUNLOCK_ASSETS_PATH + "ChristalSounds")]
    public class AbilityUnlockerChristalAudio : ScriptableObject, IAbilityUnlockerChristalAudio
    {
        [SerializeField] private OneShotFMODSound _hit;
        [SerializeField] private OneShotFMODSound _break;
        [SerializeField] private OneShotFMODSound _collected;

        private IFMODAudioManager _audioManager;
        
        
        public void Configure(IFMODAudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        
        public void PlayHitSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_hit, source);
        }
        
        public void PlayBreakSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_break, source);
        }
        
        public void PlayCollectedSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_collected, source);
        }
        
        
        
    }
}