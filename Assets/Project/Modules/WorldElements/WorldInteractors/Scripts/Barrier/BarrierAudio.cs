using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    [CreateAssetMenu(fileName = "BarrierAudio", 
        menuName = ScriptableObjectsHelper.WORLDELEMENTS_ASSETS_PATH + "BarrierAudio")]
    public class BarrierAudio : ScriptableObject
    {
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        [SerializeField] private OneShotFMODSound _activatedSound;
        [SerializeField] private OneShotFMODSound _deactivatedSound;


        public void PlayActivatedSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_activatedSound, source);
        }
        
        public void PlayDeactivatedSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_deactivatedSound, source);
        }
        
        
    }
}